using Ninject;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class BoardManager : IBoardManager
    {
        private readonly ILogManager _logManager;
        private static IKernel _kernel;

        public IBoardRepository Repository { get; private set; }

        public BoardManager(ILogManager logManager, IKernel kernel, IBoardRepository repository)
        {
            _logManager = logManager;
            Repository = repository;
            _kernel = kernel;
        }

        public static IBoardManager Instance => _kernel.Get<IBoardManager>();

        public void Initialize()
        {
            Repository.Load();
        }
    }
}
