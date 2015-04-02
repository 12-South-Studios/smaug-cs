using Ninject;
using SmaugCS.Logging;

namespace SmaugCS.News
{
    public sealed class NewsManager : INewsManager
    {
        private readonly ILogManager _logManager;
        private static IKernel _kernel;

        public INewsRepository Repository { get; private set; }

        public NewsManager(ILogManager logManager, IKernel kernel, INewsRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
            _kernel = kernel;
        }

        public static INewsManager Instance
        {
            get { return _kernel.Get<INewsManager>(); }
        }

        public void Initialize()
        {
        }
    }
}
