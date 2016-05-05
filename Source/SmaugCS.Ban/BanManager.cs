using System.Linq;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Data.Instances;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    public sealed class BanManager : IBanManager
    {
        private readonly ILogManager _logManager;
        private readonly ITimer _timer;
        private static IKernel _kernel;

        public IBanRepository Repository { get; }

        public BanManager(IKernel kernel, ITimer timer, ILogManager logManager, IBanRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
            _kernel = kernel;

            _timer = timer;

            if (_timer.Interval <= 0)
                _timer.Interval = 60000;

            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        ~BanManager()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer.Dispose();
        }

        public static IBanManager Instance => _kernel.Get<IBanManager>();

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            //if (!_bans.Any()) return;

            //List<BanData> toRemove = new List<BanData>(_bans.Where(b => b.IsExpired()));

            //foreach (BanData ban in toRemove)
            //{
            //    // TODO Do something

            //    DeleteBan(ban.Id);
            //}

            //toRemove.ForEach(b => _bans.Remove(b));
        }

        public void Initialize() => Repository.Load();

        public void Save() => Repository.Save();


        public void ClearBans() => Repository.Bans.ToList().Clear();

        public bool CheckTotalBans(string host, int supremeLevel)
        {
            foreach (BanData ban in Repository.Bans.Where(b => b.Level == supremeLevel))
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

        internal static bool CheckBanExpiration(BanData ban) => !ban.IsExpired();

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

        public bool CheckBans(PlayerInstance ch, int type)
        {
            switch (type)
            {
                case (int)BanTypes.Race:
                    return CheckRaceBans(ch);
                case (int)BanTypes.Class:
                    return CheckClassBans(ch);
                case (int)BanTypes.Site:
                    return CheckSiteBans(ch);
                default:
                    _logManager.Bug("Invalid ban type {0}", type);
                    return false;
            }
        }

        private bool CheckSiteBans(PlayerInstance ch)
        {
            string host = ch.Descriptor.host.ToLower();
            bool match = false;

            foreach (BanData ban in Repository.Bans.Where(x => x.Type == BanTypes.Site))
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
            return false;
        }

        private bool CheckClassBans(PlayerInstance ch) => Repository.Bans.Where(x => x.Type == BanTypes.Class)
            .Where(ban => ban.Flag == (int) ch.CurrentClass)
            .Select(ban => CheckBanExpireAndLevel(ban, ch.Level))
            .FirstOrDefault();

        private bool CheckRaceBans(PlayerInstance ch) => Repository.Bans.Where(x => x.Type == BanTypes.Race)
            .Where(ban => ban.Flag == (int) ch.CurrentRace)
            .Select(ban => CheckBanExpireAndLevel(ban, ch.Level))
            .FirstOrDefault();
    }
}
