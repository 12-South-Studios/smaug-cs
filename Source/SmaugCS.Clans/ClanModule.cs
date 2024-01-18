using Ninject;
using Ninject.Modules;
using SmaugCS.DAL;
using SmaugCS.Logging;

namespace SmaugCS.Clans
{
    public class ClanModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IClanRepository>().To<ClanRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("dbContext", Kernel.Get<IDbContext>());

            Kernel.Bind<IClanManager>().To<ClanManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("repository", Kernel.Get<IClanRepository>())
                .OnActivation(x => x.Initialize());
        }
    }
}
