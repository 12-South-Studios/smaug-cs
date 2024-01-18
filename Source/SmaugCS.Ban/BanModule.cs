using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.DAL;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    public class BanModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("BanExpireTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("BanExpireFrequencyMS"));

            Kernel.Bind<IBanRepository>().To<BanRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("dbContext", Kernel.Get<IDbContext>());

            Kernel.Bind<IBanManager>().To<BanManager>().InSingletonScope()
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("BanExpireTimer"))
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("repository", Kernel.Get<IBanRepository>())
                .OnActivation(x => x.Initialize());
        }
    }
}
