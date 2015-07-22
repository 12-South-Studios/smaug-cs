using Infrastructure.Data;
using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using SmaugCS.Constants.Constants;

namespace SmaugCS.Logging
{
    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("LogDumpTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("LogDumpFrequencyMS"));

            Kernel.Bind<ILogManager>().To<LogManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("LogDumpTimer"))
                .WithConstructorArgument("repository", Kernel.Get<IRepository>());
        }
    }
}
