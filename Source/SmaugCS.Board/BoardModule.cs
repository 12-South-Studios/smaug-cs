using Ninject;
using Ninject.Modules;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public class BoardModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IBoardRepository>().To<BoardRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("dbContext", Kernel.Get<ISmaugDbContext>());

            Kernel.Bind<IBoardManager>().To<BoardManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("repository", Kernel.Get<IBoardRepository>())
                .OnActivation(x => x.Initialize());
        }
    }
}
