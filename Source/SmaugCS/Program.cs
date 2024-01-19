using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Realm.Library.Common.Logging;
using Realm.Library.Network;
using SmaugCS.Auction;
using SmaugCS.Ban;
using SmaugCS.Board;
using SmaugCS.Clans;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;
using SmaugCS.News;
using SmaugCS.Repository;
using SmaugCS.Weather;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS
{
    internal class Program
    {
        private static IContainer _container;
        public static IContainer Container => _container;

        public static INetworkServer NetworkManager => Container.Resolve<INetworkServer>();
        public static ILogManager LogManager => Container.Resolve<ILogManager>();
        public static ILookupManager LookupManager => Container.Resolve<ILookupManager>();
        public static ILuaManager LuaManager => Container.Resolve<ILuaManager>();
        public static IRepositoryManager RepositoryManager => Container.Resolve<IRepositoryManager>();
        public static IGameManager GameManager => Container.Resolve<IGameManager>();
        public static IBanManager BanManager => Container.Resolve<IBanManager>();
        public static IBoardManager BoardManager => Container.Resolve<IBoardManager>();
        public static ICalendarManager CalendarManager => Container.Resolve<ICalendarManager>();
        public static IWeatherManager WeatherManager => Container.Resolve<IWeatherManager>();
        public static INewsManager NewsManager => Container.Resolve<INewsManager>();
        public static IAuctionManager AuctionManager => Container.Resolve<IAuctionManager>();
        public static IClanManager ClanManager => Container.Resolve<IClanManager>();
        public static int SessionId { get; private set; }

        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += Application_OnUnhandledException;

            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddTransient<ILogWrapper, LogWrapper>();
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddLogging(builder =>
                {
                    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                });
                ConfigureServices(services);
                IServiceProvider serviceProvider = services.BuildServiceProvider();

                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                NLog.LogManager.LoadConfiguration("nlog.config");

                var logger = serviceProvider.GetRequiredService<ILogWrapper>();
                logger.Debug("Initializing Application");

                var settings = serviceProvider.GetService<Config.Configuration.Settings>();
                var constants = serviceProvider.GetService<Config.Configuration.Constants>();
                var vnums = serviceProvider.GetService<Config.Configuration.Vnums>();
                var statics = serviceProvider.GetService<Config.Configuration.Statics>();

                serviceProvider = ConfigureAutofacServices(services, settings, constants);

                var app = serviceProvider.GetService<Application>();
                Task.Run(() => app.Start(settings, constants)).Wait();

                logger.Debug("Ending Application");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception in Application");
                Console.Write(ex.Message);
            }
        }

        private static void Application_OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            LogManager.Bug((Exception)unhandledExceptionEventArgs.ExceptionObject);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            var configuration = GetConfiguration();
            services.AddSingleton(configuration);

            services.AddOptions();

            var settings = new Config.Configuration.Settings();
            configuration.GetSection("Settings").Bind(settings);
            services.AddSingleton(settings);

            var constants = new Config.Configuration.Constants();
            configuration.GetSection("Constants").Bind(constants);
            services.AddSingleton(constants);

            var vnums = new Config.Configuration.Vnums();
            configuration.GetSection("Vnums").Bind(vnums);
            services.AddSingleton(vnums);

            var statics = new Config.Configuration.Statics();
            configuration.GetSection("Statics").Bind(statics);
            services.AddSingleton(statics);

            services.AddTransient<Application>();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private static IServiceProvider ConfigureAutofacServices(IServiceCollection services, Config.Configuration.Settings settings,
            Config.Configuration.Constants constants)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            Application.RegisterModules(containerBuilder, settings, constants);
            Application.ConfigureServices(containerBuilder);

            _container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(_container);
            return serviceProvider;
        }

   
        #region Old Constants
        public static int BERR = 255;

        public static bool EXA_prog_trigger = true;
        public static int RESTORE_INTERVAL = 21600;

        public static int VOTE_NONE = 0;
        public static int VOTE_OPEN = 1;
        public static int VOTE_CLOSED = 2;

        public static int MAX_TZONE = 25;

        public static int DUR_SCMN = 60;
        public static int DUR_MNHR = 60;
        public static int DUR_HRDY = 24;
        public static int DUR_DYWK = 7;

        public static int MAX_CLAN_NEST = 100;

        public static int TRW_MAXHEAP = 50;

        #region Star map

        public static int NUM_DAYS = 35;
        public static int NUM_MONTHS = 17;
        public static int MAP_WIDTH = 72;
        public static int MAP_HEIGHT = 8;

        #endregion

        #region 32bit bitvector defines
        public static int BV00 = 1 << 0;
        public static int BV01 = 1 << 1;
        public static int BV02 = 1 << 2;
        public static int BV03 = 1 << 3;
        public static int BV04 = 1 << 4;
        public static int BV05 = 1 << 5;
        public static int BV06 = 1 << 6;
        public static int BV07 = 1 << 7;
        public static int BV08 = 1 << 8;
        public static int BV09 = 1 << 9;
        public static int BV10 = 1 << 10;
        public static int BV11 = 1 << 11;
        public static int BV12 = 1 << 12;
        public static int BV13 = 1 << 13;
        public static int BV14 = 1 << 14;
        public static int BV15 = 1 << 15;
        public static int BV16 = 1 << 16;
        public static int BV17 = 1 << 17;
        public static int BV18 = 1 << 18;
        public static int BV19 = 1 << 19;
        public static int BV20 = 1 << 20;
        public static int BV21 = 1 << 21;
        public static int BV22 = 1 << 22;
        public static int BV23 = 1 << 23;
        public static int BV24 = 1 << 24;
        public static int BV25 = 1 << 25;
        public static int BV26 = 1 << 26;
        public static int BV27 = 1 << 27;
        public static int BV28 = 1 << 28;
        public static int BV29 = 1 << 29;
        public static int BV30 = 1 << 30;
        public static int BV31 = 1 << 31;
        // 32 USED! DO NOT ADD MORE! SB 
        #endregion

        #region String and Memory Management Parameters
        public static int MAX_KEY_HASH = 2048;
        public static int MAX_STRING_LENGTH = 4096;
        public static int MAX_INPUT_LENGTH = 1024;
        public static int MAX_INBUF_SIZE = 1024;
        public static int MSL = MAX_STRING_LENGTH;
        public static int MIL = MAX_INPUT_LENGTH;
        public static int MAX_NEST = 100;   // Max Container Nesting

        public static int MAX_KILLTRACK = 25;          // Track Mob Vnums killed
        #endregion

        #region Game Parameters



        public static int MAX_FIGHT = 8;

        public static int MAX_VNUM = 100000;  // Game can hold up to 2 billion
        public static int MAX_RGRID_ROOMs = 30000;
        public static int MAX_REXITS = 20;  // Max Exits in 1 room
        public static int MAX_SKILL = 500;
        public static string SPELL_SILENT_MARKER = "silent";
        public static int MAX_CLASS = 20;
        public static int MAX_NPC_CLASS = 26;
        public static int MAX_RACE = 20;
        public static int MAX_NPC_RACE = 91;
        public static int MAX_MSG = 18;
        public static int MAX_OINVOKE_QUANTITY = 20;

        public static int MAX_PC_RACE { get; set; }
        public static int MAX_PC_CLASS { get; set; }
        public static bool mud_down { get; set; }



        public static int MAX_CLAN = 50;
        public static int MAX_DEITY = 50;
        public static int MAX_CPD = 4;  // Max council power level difference
        public static int MAX_HERB = 20;
        public static int MAX_DISEASE = 20;
        public static int MAX_PERSONAL = 5; // Max personal skills
        public static int MAX_WHERE_NAME = 29;


        public static bool DONT_UPPER { get; set; }

        /*public static int SECONDS_PER_TICK = SystemData.SecondsPerTick;

        public static int PULSE_PER_SECOND = SystemData.PulsesPerSecond;
        public static int PULSE_VIOLENCE = SystemData.PulseViolence;
        public static int PULSE_MOBILE = SystemData.PulseMobile;
        public static int PULSE_TICK = SystemData.PulseTick;
        public static int PULSE_AREA = (60 * SystemData.PulsesPerSecond);
        public static int PULSE_AUCTION = (9 * SystemData.PulsesPerSecond);*/
        #endregion

        #region Smaug Version
        public static string SMAUG_VERSION_MAJOR = "1";
        public static string SMAUG_VERSION_MINOR = "0 CS";
        #endregion

        #region Old Stuff
        public static int HAS_SPELL_INDEX = -1;
        public static int AREA_VERSION_WRITE = 1;
        #endregion

        #region CurrentMorph structs
        public static int ONLY_PKILL = 1;
        public static int ONLY_PEACEFULL = 2;
        #endregion

        public static int MAX_NUISANCE_STAGE = 10;


        public static int MAX_ITEM_IMPACT = 30;
        public static int MAX_TRADE = 5;
        public static int MAX_FIX = 3;
        public static int SHOP_FIX = 1;
        public static int SHOP_RECHARGE = 2;

        public static int MAX_IFS = 20;
        public static int IN_IF = 0;
        public static int IN_ELSE = 1;
        public static int DO_IF = 2;
        public static int DO_ELSE = 3;
        public static int MAX_PROG_NEST = 20;

        public static bool MOBtrigger { get; set; }


        public static int AREA_DELETED = BV00;
        public static int AREA_LOADED = BV01;

        public static int STRING_NONE = 0;
        public static readonly int STRING_IMM = BV01;

        public static int OLD_SF_SAVE_HALF_DAMAGE = BV18;
        public static int OLD_SF_SAVE_NEGATES = BV19;

        public static int ALL_BITS = int.MaxValue;
        public static readonly int SDAM_MASK = ALL_BITS & ~(BV00 | BV01 | BV02);
        public static int SACT_MASK = ALL_BITS & ~(BV03 | BV04 | BV05);
        public static int SCLA_MASK = ALL_BITS & ~(BV06 | BV07 | BV08);
        public static int SPOW_MASK = ALL_BITS & ~(BV09 | BV10);
        public static int SSAV_MASK = ALL_BITS & ~(BV11 | BV12 | BV13);



        public static int MAX_ITEM_TYPE = (int)ItemTypes.DrinkMixture;

        public static int REVERSE_APPLY = 1000;



        public static int MAX_DIR = (int)DirectionTypes.Southwest;
        public static int DIR_PORTAL = (int)DirectionTypes.Somewhere;


        public static int FLAG_WRAUTH = 1;
        public static int FLAG_AUTH = 2;


        public static int MAX_IGN = 6;

        public static int AUCTION_MEM = 3;

        // Externs - Lines 2877 to 2986


        public static int RIS_000 = BV00;
        public static int RIS_R00 = BV01;
        public static int RIS_0I0 = BV02;
        public static int RIS_RIO = BV03;
        public static int RIS_00S = BV04;
        public static int RIS_R0S = BV05;
        public static int RIS_0IS = BV06;
        public static int RIS_RIS = BV07;

        public static int GA_AFFECTED = BV09;
        public static int GA_RESISTANT = BV10;
        public static int GA_IMMUNE = BV11;
        public static int GA_SUSCEPTIBLE = BV12;
        public static int GA_RIS = BV30;

        public static int MAX_LIQUIDS = 100;
        public static int MAX_COND_VALUE = 100;


        #region News

        public static string NEWS_TOP = "\r\n";
        public static string NEWS_HEADER = "\r\n";
        public static string NEWS_HEADER_ALL = "&g( &W#&g)                          (&WSubject&g)\r\n";
        public static string NEWS_HEADER_READ = "&g( &W#&g)                          (&WSubject&g)\r\n";
        public static int NEWS_VIEW = 15;
        public static int NEWS_MAX_TYPES = 10;
        #endregion

        #region MCCP

        public static int TELOPT_COMPRESS = 86;
        public static int COMPRESS_BUF_SIZE = MAX_STRING_LENGTH;
        #endregion

        #region Mapper
        /* Defines for ASCII Automapper */
        public static int MAPX = 10;
        public static int MAPY = 8;

        /* You can change MAXDEPTH to 1 if the diagonal directions are confusing */
        public static int MAXDEPTH = 2;

        public static bool BOUNDARY(int x, int y)
        {
            return (x < 0) || (y < 0) || (x > MAPX) || (y > MAPY);
        }

        #endregion

        #region Chess

        public static int NO_PIECE = 0;

        public static int WHITE_PAWN = 1;
        public static int WHITE_ROOK = 2;
        public static int WHITE_KNIGHT = 3;
        public static int WHITE_BISHOP = 4;
        public static int WHITE_QUEEN = 5;
        public static int WHITE_KING = 6;

        public static int BLACK_PAWN = 7;
        public static int BLACK_ROOK = 8;
        public static int BLACK_KNIGHT = 9;
        public static int BLACK_BISHOP = 10;
        public static int BLACK_QUEEN = 11;
        public static int BLACK_KING = 12;

        public static int MAX_PIECES = 13;

        public static bool IS_WHITE(int x)
        {
            return x >= WHITE_PAWN && x <= WHITE_KING;
        }

        public static bool IS_BLACK(int x)
        {
            return x >= BLACK_PAWN && x <= BLACK_KING;
        }

        public static int MOVE_OK = 0;
        public static int MOVE_INVALID = 1;
        public static int MOVE_BLOCKED = 2;
        public static int MOVE_TAKEN = 3;
        public static int MOVE_CHECKMATE = 4;
        public static int MOVE_OFFBOARD = 5;
        public static int MOVE_SAMECOLOR = 6;
        public static int MOVE_CHECK = 8;
        public static int MOVE_WRONGCOLOR = 9;
        public static int MOVE_INCHECK = 10;

        public static int TYPE_UNDEFINED = -1;
        public static int TYPE_LOCAL = 1;
        public static int TYPE_IMC = 2;
        public static int TYPE_HIT = 1000;
        #endregion

        #region Calendar

        // Hunger/Thirst modifiers 
        public static int WINTER_HUNGER = 1;
        public static int SUMMER_THIRST = 1;
        public static int SUMMER_THIRST_DESERT = 2;

        #endregion

        #region Automated Auction
        public static int advatoi(string s)
        {
            var number = 0;
            var multiplier = 0;

            var sb = new StringBuilder(s);

            var i = 0;
            while (char.IsDigit(sb[i]))
            {
                number = number * 10 + Convert.ToInt32(sb[i]);
                i++;
            }

            switch (char.ToUpper(sb[0]))
            {
                case 'K':
                    number *= multiplier = 1000;
                    break;
                case 'M':
                    number *= multiplier = 1000000;
                    break;
                default:
                    return 0;
            }

            i = 0;
            while (char.IsDigit(sb[i]) && multiplier > 1)
            {
                multiplier /= 10;
                number = number + Convert.ToInt32(sb[i] * multiplier);
            }

            if (!char.IsDigit(sb[0]))
                return 0;

            return number;
        }

        public static int parsebet(int currentbet, string s)
        {
            var sb = new StringBuilder(s);
            if (char.IsDigit(sb[0]))
                return advatoi(s);
            switch (sb[0])
            {
                case '+':
                    if (sb.Length == 1)
                        return currentbet * 125 / 100;
                    return currentbet * (100 + Convert.ToInt32(s)) / 100;
                case '*':
                case 'x':
                    if (sb.Length == 1)
                        return currentbet * 2;
                    return currentbet * Convert.ToInt32(s);
            }

            return 0;
        }

        #endregion
        #endregion

    }
}
