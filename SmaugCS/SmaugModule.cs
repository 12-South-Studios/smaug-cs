using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Network;
using SmallDBConnectivity;
using SmaugCS.Auction;
using SmaugCS.Ban;
using SmaugCS.Board;
using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using SmaugCS.News;
using SmaugCS.Repositories;
using SmaugCS.Weather;

namespace SmaugCS
{
    public class SmaugModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISmallDb>().To<SmallDb>();

            Kernel.Bind<ITimer>().To<CommonTimer>().Named("LogDumpTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("LogDumpFrequencyMS"));
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("BanExpireTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("BanExpireFrequencyMS"));
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("AuctionPulseTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("AuctionPulseSeconds"));

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
                    GameConstants.GetConstant<int>("WeatherWidth"),
                    GameConstants.GetConstant<int>("WeatherHeight")));

            Kernel.Bind<INewsManager>().To<NewsManager>().InSingletonScope();

            Kernel.Bind<IInitializer>().To<LuaInitializer>().InSingletonScope()
                .Named("LuaInitializer")
                .OnActivation(x => x.InitializeLuaInjections(GameConstants.GetDataPath()))
                .OnActivation(x => x.InitializeLuaFunctions());

            Kernel.Bind<ITimerManager>().To<TimerManager>().InSingletonScope();

            Kernel.Bind<IAuctionRepository>().To<AuctionRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.GetConnection());

            Kernel.Bind<IAuctionManager>().To<AuctionManager>().InSingletonScope()
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("AuctionPulseTimer"))
                .WithConstructorArgument("repository", Kernel.Get<IAuctionRepository>())
                .OnActivation(x => x.Initialize());

            Kernel.Bind<IInstanceRepository<ObjectInstance>>().To<ObjInstanceRepository>();
            Kernel.Bind<IInstanceRepository<CharacterInstance>>().To<CharacterRepository>();
            Kernel.Bind<ITemplateRepository<MobTemplate>>().To<MobileRepository>();
            Kernel.Bind<ITemplateRepository<ObjectTemplate>>().To<ObjectRepository>();
            Kernel.Bind<ITemplateRepository<RoomTemplate>>().To<RoomRepository>();

        }
    }
}
