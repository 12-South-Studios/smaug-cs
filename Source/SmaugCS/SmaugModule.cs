using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Network;
using SmaugCS.Constants.Constants;
using SmaugCS.Data;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using SmaugCS.Repository;
using SmaugCS.SpecFuns;

namespace SmaugCS
{
    public class SmaugModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("GameLoopTimer")
                .OnActivation(x => x.Interval = (1000f / GameConstants.GetSystemValue<int>("PulsesPerSecond")));

            Kernel.Bind<ILookupManager>().To<LookupManager>().InSingletonScope();

            Kernel.Bind<ITcpUserRepository>().To<TcpUserRepository>();
            Kernel.Bind<ITcpServer>().To<TcpServer>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("repository", Kernel.Get<ITcpUserRepository>());

            Kernel.Bind<IGameManager>().To<GameManager>().InSingletonScope()
                .WithConstructorArgument("databaseManager", Kernel.Get<IRepositoryManager>())
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("GameLoopTimer"));

            Kernel.Bind<ICalendarManager>().To<CalendarManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("gameManager", Kernel.Get<IGameManager>())
                .WithConstructorArgument("dbContext", Kernel.Get<ISmaugDbContext>())
                .OnActivation(x => x.Initialize());

            Kernel.Bind<IInitializer>().To<LuaInitializer>().InSingletonScope()
                .Named("LuaInitializer")
                .OnActivation(x => x.InitializeLuaInjections(GameConstants.DataPath))
                .OnActivation(x => x.InitializeLuaFunctions());

            Kernel.Bind<ISpecFunHandler>().To<SpecFunHandler>();
        }
    }
}
