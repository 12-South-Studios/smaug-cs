using SmaugCS.Logging;

namespace SmaugCS.Clans
{
    public sealed class ClanManager : IClanManager
    {
        private readonly ILogManager _logManager;

        public IClanRepository Repository { get; private set; }

        public ClanManager(ILogManager logManager,  IClanRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
        }

        public void Initialize()
        {
        }
    }
}
