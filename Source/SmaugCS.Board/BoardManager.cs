using SmaugCS.Logging;

namespace SmaugCS.Board
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class BoardManager : IBoardManager
    {
        private readonly ILogManager _logManager;

        public IBoardRepository Repository { get; private set; }

        public BoardManager(ILogManager logManager, IBoardRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
        }

        public void Initialize()
        {
            Repository.Load();
        }
    }
}
