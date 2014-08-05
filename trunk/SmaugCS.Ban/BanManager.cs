using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using SmallDBConnectivity;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    public sealed class BanManager : IBanManager
    {
        private readonly List<BanData> _bans;
        private readonly ILogManager _logManager;
        private readonly ISmallDb _smallDb;
        private readonly IDbConnection _connection;
        private readonly ITimer _timer;
        private static IKernel _kernel;

        public BanManager(ILogManager logManager, ISmallDb smallDb, IDbConnection connection, IKernel kernel,
            ITimer timer)
        {
            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
            _kernel = kernel;

            _bans = new List<BanData>();
            _timer = timer;

            if (_timer.Interval <= 0)
                _timer.Interval = 60000;

            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        ~BanManager()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            if (_connection != null)
                _connection.Dispose();
        }

        public static IBanManager Instance
        {
            get { return _kernel.Get<IBanManager>(); }
        }

        public void ClearBans()
        {
            _bans.Clear();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!_bans.Any()) return;

            List<BanData> toRemove = new List<BanData>(_bans.Where(b => b.IsExpired()));

            foreach (BanData ban in toRemove)
            {
                // TODO Do something

                DeleteBan(ban.Id);
            }

            toRemove.ForEach(b => _bans.Remove(b));
        }

        public void Initialize()
        {
            try
            {
                List<BanData> bans = _smallDb.ExecuteQuery(_connection, SqlProcedureStatics.BanGetAll, TranslateBanData);

                bans.ForEach(ban => _bans.Add(ban));
                _logManager.Boot("Loaded {0} Bans", _bans.Count);
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private bool SaveBans()
        {
            IDbTransaction transaction = null;
            try
            {
                transaction = _connection.BeginTransaction();
                foreach (BanData ban in _bans)
                {
                    _smallDb.ExecuteNonQuery(_connection, SqlProcedureStatics.BanSave, CreateSqlParameters(ban));
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                _logManager.Error(ex);
                return false;
            }
        }

        [ExcludeFromCodeCoverage]
        private bool SaveBan(BanData ban, bool isNew = false)
        {
            IDbTransaction transaction = null;
            try
            {
                IEnumerable<SqlParameter> parameters = CreateSqlParameters(ban);
                parameters.ToList().Add(new SqlParameter("@BanId", ban.Id));

                transaction = _connection.BeginTransaction();
                _smallDb.ExecuteNonQuery(_connection,
                                         isNew ? SqlProcedureStatics.BanSave : SqlProcedureStatics.BanUpdate, parameters);
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                _logManager.Error(ex);
                return false;
            }
        }

        internal static IEnumerable<SqlParameter> CreateSqlParameters(BanData ban)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@BanTypeId", ban.Type.GetValue()),
                    new SqlParameter("@Name", ban.Name),
                    new SqlParameter("@Note", ban.Note),
                    new SqlParameter("@BannedBy", ban.BannedBy),
                    new SqlParameter("@BannedOn", ban.BannedOn),
                    new SqlParameter("@Duration", ban.Duration),
                    new SqlParameter("@Level", ban.Level),
                    new SqlParameter("@Warn", ban.Warn),
                    new SqlParameter("@Prefix", ban.Prefix),
                    new SqlParameter("@Suffix", ban.Suffix)
                };

            return parameters;
        }

        [ExcludeFromCodeCoverage]
        private bool DeleteBan(int id)
        {
            IDbTransaction transaction = null;
            try
            {
                transaction = _connection.BeginTransaction();
                _smallDb.ExecuteNonQuery(_connection, SqlProcedureStatics.BanDelete,
                                         new List<IDataParameter> {new SqlParameter("@BanId", id)});
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                _logManager.Error(ex);
                return false;
            }
        }

        public bool AddBan(BanData ban)
        {
            if (_bans.Contains(ban))
                return false;

            _bans.Add(ban);
            return SaveBan(ban);
        }

        public bool RemoveBan(int id)
        {
            BanData ban = _bans.FirstOrDefault(x => x.Id == id);
            if (ban == null)
                return false;

            _bans.Remove(ban);
            return DeleteBan(id);
        }

        public BanData GetBan(string name)
        {
            return _bans.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }

        public BanData GetBan(int id)
        {
            return _bans.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<BanData> GetBans(string bannedBy)
        {
            return _bans.Where(x => x.BannedBy.EqualsIgnoreCase(bannedBy));
        }

        public IEnumerable<BanData> GetBans()
        {
            return _bans;
        }

        [ExcludeFromCodeCoverage]
        private static List<BanData> TranslateBanData(IDataReader reader)
        {
            List<BanData> bans = new List<BanData>();
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                bans.AddRange(from DataRow row in dt.Rows select BanData.Translate(row));
            }

            return bans;
        }

        public bool CheckTotalBans(string host, int supremeLevel)
        {
            foreach (BanData ban in _bans.Where(b => b.Level == supremeLevel))
            {
                if (ban.Prefix && ban.Suffix && host.Contains(ban.Name))
                    return CheckBanExpiration(ban);
                if (ban.Suffix && host.StartsWith(ban.Name))
                    return CheckBanExpiration(ban);
                if (ban.Prefix && host.EndsWith(ban.Name))
                    return CheckBanExpiration(ban);
                if (host.EqualsIgnoreCase(ban.Name))
                    return CheckBanExpiration(ban);
            }
            return false;
        }

        internal static bool CheckBanExpiration(BanData ban)
        {
            return !ban.IsExpired();
        }

        internal static bool CheckBanExpireAndLevel(BanData ban, int characterLevel)
        {
            if (!ban.IsExpired())
                return false;
            if (ban.Level == characterLevel)
            {
                if (ban.Warn)
                {
                    // TODO what?
                }
                return false;
            }
            return true;
        }

        public bool CheckBans(CharacterInstance ch, int type)
        {
            switch (type)
            {
                case (int)BanTypes.Race:
                    foreach (BanData ban in _bans.Where(x => x.Type == BanTypes.Race)
                        .Where(ban => ban.Flag == (int)ch.CurrentRace))
                    {
                        return CheckBanExpireAndLevel(ban, ch.Level);
                    }
                    break;
                case (int)BanTypes.Class:
                    foreach (BanData ban in _bans.Where(x => x.Type == BanTypes.Class)
                        .Where(ban => ban.Flag == (int)ch.CurrentClass))
                    {
                        return CheckBanExpireAndLevel(ban, ch.Level);
                    }
                    break;
                case (int)BanTypes.Site:
                    string host = ch.Descriptor.host.ToLower();
                    bool match = false;

                    foreach (BanData ban in _bans.Where(x => x.Type == BanTypes.Site))
                    {
                        if (ban.Prefix && ban.Suffix && host.Contains(ban.Name))
                            match = true;
                        else if (ban.Suffix && host.StartsWith(ban.Name))
                            match = true;
                        else if (ban.Prefix && host.EndsWith(ban.Name))
                            match = true;
                        else if (host.EqualsIgnoreCase(ban.Name))
                            match = true;

                        if (match)
                            return CheckBanExpireAndLevel(ban, ch.Level);
                    }
                    break;
                default:
                    _logManager.Bug("Invalid ban type {0}", type);
                    return false;
            }

            return false;
        }
    }
}
