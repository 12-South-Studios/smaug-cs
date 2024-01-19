using SmaugCS.Logging;

namespace SmaugCS.News
{
    public sealed class NewsManager : INewsManager
    {
        private readonly ILogManager _logManager;

        public INewsRepository Repository { get; private set; }

        public NewsManager(ILogManager logManager, INewsRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
        }

        public void Initialize()
        {
        }
    }
}
