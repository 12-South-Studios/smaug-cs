using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmallDBConnectivity;
using SmaugCS.Common.Database;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    public sealed class BanManager : GameSingleton, IBanManager
    {
        private static BanManager _instance;
        private static readonly object Padlock = new object();

        private readonly List<BanData> _bans;
        private readonly SqlRepository _repository;
        private ILogManager _logManager;
        private ISmallDb _smallDb;
        private IDbConnection _connection;

        [ExcludeFromCodeCoverage]
        private BanManager()
        {
            _repository = new SqlRepository();
            _bans = new List<BanData>();
        }

        /// <summary>
        ///
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static BanManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new BanManager());
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public void Initialize(ILogManager logManager, ISmallDb smallDb, IDbConnection connection)
        {
            _repository.AddSql(DbCommands.BanAdd.ToString(), SqlProcedureStatics.BanAdd);
            _repository.AddSql(DbCommands.BanRemove.ToString(), SqlProcedureStatics.BanRemove);
            _repository.AddSql(DbCommands.BanGetByName.ToString(), SqlProcedureStatics.BanGetByName);

            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
        }

        [ExcludeFromCodeCoverage]
        public void LoadBans()
        {
            try
            {
                List<BanData> bans = _smallDb.ExecuteQuery(_connection,
                                                            _repository.GetSql(DbCommands.BanGetAll.ToString()),
                                                            TranslateBanData);

                bans.ForEach(ban => _bans.Add(ban));
                _logManager.BootLog("Loaded {0} Bans", _bans.Count);
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private bool SaveBans()
        {
            try 
            {
                foreach (BanData ban in _bans)
                {
                    _smallDb.ExecuteNonQuery(_connection, _repository.GetSql(DbCommands.BanAdd.ToString()),
                                            CreateSqlParameters(ban));
                }
                return true;
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
                return false;
            }
        }

        [ExcludeFromCodeCoverage]
        private bool SaveBan(BanData ban)
        {
            try
            {
                _smallDb.ExecuteNonQuery(_connection, _repository.GetSql(DbCommands.BanAdd.ToString()),
                                         CreateSqlParameters(ban));
                return true;
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
                return false;
            }
        }

        internal static IEnumerable<SqlParameter> CreateSqlParameters(BanData ban)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@BanTypeId", ban.Type.GetValue()));
            parameters.Add(new SqlParameter("@Name", ban.Name));
            parameters.Add(new SqlParameter("@Note", ban.Note));
            parameters.Add(new SqlParameter("@BannedBy", ban.BannedBy));
            parameters.Add(new SqlParameter("@BannedOn", ban.BannedOn));
            parameters.Add(new SqlParameter("@Duration", ban.Duration));

            return parameters;
        }

        [ExcludeFromCodeCoverage]
        private bool DeleteBan(int id)
        {
            try 
            {
                _smallDb.ExecuteNonQuery(_connection, _repository.GetSql(DbCommands.BanRemove.ToString()),
                                        new List<IDataParameter> {new SqlParameter("@BanId", id)});
                return true;
            }
            catch (Exception ex)
            {
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

                foreach (DataRow row in dt.Rows)
                {
                    BanData ban = new BanData(Convert.ToInt32(row["BanId"]),
                                              EnumerationExtensions.GetEnumByName<BanTypes>(row["BanType"].ToString()));
                    ban.Name = row["Name"].ToString();
                    ban.Note = row["Note"].IsNullOrDBNull() ? string.Empty : row["Note"].ToString();
                    ban.BannedBy = row["BannedBy"].ToString();
                    ban.BannedOn = Convert.ToDateTime(row["BannedOn"]);
                    ban.Duration = Convert.ToInt32(row["Duration"]);

                    bans.Add(ban);
                }
            }

            return bans;
        }
    }
}
