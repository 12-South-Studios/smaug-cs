using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using SmallDBConnectivity;
using SmaugCS.Constants;

namespace SmaugCS.Logging
{
    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ILogManager>().To<LogManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.Connection)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("LogDumpTimer"));
        }
    }
}
