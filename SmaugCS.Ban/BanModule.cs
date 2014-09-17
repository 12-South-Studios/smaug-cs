using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using SmallDBConnectivity;
using SmaugCS.Constants;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    public class BanModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("BanExpireTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("BanExpireFrequencyMS"));

            Kernel.Bind<IBanManager>().To<BanManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.Connection)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("BanExpireTimer"))
                .OnActivation(x => x.Initialize());
        }
    }
}
