using Ninject;
using Ninject.Modules;
using Realm.Library.SmallDb;
using SmaugCS.Constants;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public class BoardModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IBoardManager>().To<BoardManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.Connection)
                .OnActivation(x => x.Initialize());
        }
    }
}
