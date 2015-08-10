using System.Linq;
using log4net;
using Ninject.Modules;
using Realm.Library.Common.Logging;

namespace Integration.Tests
{
    public class IntegrationModule : NinjectModule
    {
        public override void Load()
        {
            if (!Kernel.GetBindings(typeof(ILogWrapper)).Any())
                Bind<ILogWrapper>()
                    .To<LogWrapper>()
                    .WithConstructorArgument("log", LogManager.GetLogger("Integration"))
                    .WithConstructorArgument("level", LogLevel.Error);
        }
    }
}
