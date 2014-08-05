using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Network;
using SmallDBConnectivity;
using SmaugCS.Ban;
using SmaugCS.Board;
using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using SmaugCS.News;
using SmaugCS.Weather;

namespace SmaugCS
{
    public class SmaugModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISmallDb>().To<SmallDb>();

            Kernel.Bind<ITimer>().To<CommonTimer>().Named("LogDumpTimer")
                .OnActivation(x => x.Interval = GameConstants.GetIntegerConstant("LogDumpFrequencyMS"));
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("BanExpireTimer")
                .OnActivation(x => x.Interval = GameConstants.GetIntegerConstant("BanExpireFrequencyMS"));

            Kernel.Bind<ILogManager>().To<LogManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.GetConnection())
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("LogDumpTimer"));

            Kernel.Bind<ILookupManager>().To<LookupManager>().InSingletonScope();

            Kernel.Bind<ILuaManager>().To<LuaManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("path", GameConstants.GetDataPath());

            Kernel.Bind<ITcpUserRepository>().To<TcpUserRepository>();
            Kernel.Bind<ITcpServer>().To<TcpServer>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("repository", Kernel.Get<ITcpUserRepository>());

            Kernel.Bind<IDatabaseManager>().To<DatabaseManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>());

            Kernel.Bind<IBanManager>().To<BanManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.GetConnection())
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("BanExpireTimer"))
                .OnActivation(x => x.Initialize());

            Kernel.Bind<IBoardManager>().To<BoardManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.GetConnection())
                .OnActivation(x => x.Initialize());

            Kernel.Bind<IGameManager>().To<GameManager>().InSingletonScope();

            Kernel.Bind<ICalendarManager>().To<CalendarManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.GetConnection())
                .WithConstructorArgument("gameManager", Kernel.Get<IGameManager>())
                .OnActivation(x => x.Initialize());

            Kernel.Bind<IWeatherManager>().To<WeatherManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.GetConnection())
                .OnActivation(x => x.Initialize(Kernel.Get<IGameManager>().GameTime,
                    GameConstants.GetIntegerConstant("WeatherWidth"),
                    GameConstants.GetIntegerConstant("WeatherHeight")));

            Kernel.Bind<INewsManager>().To<NewsManager>().InSingletonScope();

            Kernel.Bind<IInitializer>().To<LuaInitializer>().InSingletonScope()
                .Named("LuaInitializer")
                .OnActivation(x => x.InitializeLuaInjections(GameConstants.GetDataPath()))
                .OnActivation(x => x.InitializeLuaFunctions());

            Kernel.Bind<ITimerManager>().To<TimerManager>().InSingletonScope();
        }
    }
}
