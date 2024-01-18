using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using SmaugCS.Constants.Constants;
using SmaugCS.DAL;

namespace SmaugCS.Logging
{
    public class LoggingModule : NinjectModule
    {
        private readonly int _sessionId;

        public LoggingModule(int sessionId)
        {
            _sessionId = sessionId;
        }

        public override void Load()
        {
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("LogDumpTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("LogDumpFrequencyMS"));

            Kernel.Bind<ILogManager>().To<LogManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("LogDumpTimer"))
                .WithConstructorArgument("dbContext", Kernel.Get<IDbContext>())
                .WithConstructorArgument("sessionId", _sessionId);
        }
    }
}
