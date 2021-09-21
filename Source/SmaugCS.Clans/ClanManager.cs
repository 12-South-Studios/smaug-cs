using Ninject;
using SmaugCS.Logging;

namespace SmaugCS.Clans
{
    public sealed class ClanManager : IClanManager
    {
        private readonly ILogManager _logManager;
        private static IKernel _kernel;

        public IClanRepository Repository { get; private set; }

        public ClanManager(ILogManager logManager, IKernel kernel, IClanRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
            _kernel = kernel;
        }

        public static IClanManager Instance => _kernel.Get<IClanManager>();

        public void Initialize()
        {
        }
    }
}
