using System.Linq;
using log4net;
using Ninject.Modules;
using Realm.Library.Common.Logging;
using SmaugCS.DAL.Interfaces;

namespace SmaugCS.DAL.DependencyModules
{
    public class SmaugDbContextModule : NinjectModule
    {
        public override void Load()
        {
            if (!Kernel.GetBindings(typeof(ILogWrapper)).Any())
                Bind<ILogWrapper>()
                    .To<LogWrapper>()
                    .WithConstructorArgument("log", LogManager.GetLogger(typeof(SmaugDbContext)))
                    .WithConstructorArgument("level", LogLevel.Error);
            if (!Kernel.GetBindings(typeof(ISmaugDbContext)).Any())
                Bind<ISmaugDbContext>().To<SmaugDbContext>();
        }
    }
}
