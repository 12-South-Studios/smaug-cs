using System;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Net;
using System.Text;
using Ninject;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using Realm.Library.Network;
using SmallDBConnectivity;
using SmaugCS.Ban;
using SmaugCS.Board;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using SmaugCS.Weather;
using log4net;
using LogManager = SmaugCS.Logging.LogManager;

namespace SmaugCS
{
    public class Program
    {
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
        public static int BV00 = (1 << 0);
        public static int BV01 = (1 << 1);
        public static int BV02 = (1 << 2);
        public static int BV03 = (1 << 3);
        public static int BV04 = (1 << 4);
        public static int BV05 = (1 << 5);
        public static int BV06 = (1 << 6);
        public static int BV07 = (1 << 7);
        public static int BV08 = (1 << 8);
        public static int BV09 = (1 << 9);
        public static int BV10 = (1 << 10);
        public static int BV11 = (1 << 11);
        public static int BV12 = (1 << 12);
        public static int BV13 = (1 << 13);
        public static int BV14 = (1 << 14);
        public static int BV15 = (1 << 15);
        public static int BV16 = (1 << 16);
        public static int BV17 = (1 << 17);
        public static int BV18 = (1 << 18);
        public static int BV19 = (1 << 19);
        public static int BV20 = (1 << 20);
        public static int BV21 = (1 << 21);
        public static int BV22 = (1 << 22);
        public static int BV23 = (1 << 23);
        public static int BV24 = (1 << 24);
        public static int BV25 = (1 << 25);
        public static int BV26 = (1 << 26);
        public static int BV27 = (1 << 27);
        public static int BV28 = (1 << 28);
        public static int BV29 = (1 << 29);
        public static int BV30 = (1 << 30);
        public static int BV31 = (1 << 31);
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

        public int MAX_PC_RACE { get; set; }
        public int MAX_PC_CLASS { get; set; }
        public bool mud_down { get; set; }



        public static int MAX_CLAN = 50;
        public static int MAX_DEITY = 50;
        public static int MAX_CPD = 4;  // Max council power level difference
        public static int MAX_HERB = 20;
        public static int MAX_DISEASE = 20;
        public static int MAX_PERSONAL = 5; // Max personal skills
        public static int MAX_WHERE_NAME = 29;

 
        public bool DONT_UPPER { get; set; }

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


        public static int INIT_WEAPON_CONDITION = 12;
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

        public bool MOBtrigger { get; set; }


        public static int AREA_DELETED = BV00;
        public static int AREA_LOADED = BV01;

        public static int STRING_NONE = 0;
        public static int STRING_IMM = BV01;

        public static int OLD_SF_SAVE_HALF_DAMAGE = BV18;
        public static int OLD_SF_SAVE_NEGATES = BV19;

        public static int ALL_BITS = Int32.MaxValue;
        public static int SDAM_MASK = ALL_BITS & ~(BV00 | BV01 | BV02);
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

        //Change these values to expand or contract your weather map according to your world size.
        public static int WEATHER_SIZE_X = 3;   // width
        public static int WEATHER_SIZE_Y = 3;   // height


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
            return ((x < 0) || (y < 0) || (x > MAPX) || (y > MAPY));
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
            return (x >= WHITE_PAWN && x <= WHITE_KING);
        }

        public static bool IS_BLACK(int x)
        {
            return (x >= BLACK_PAWN && x <= BLACK_KING);
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
            int number = 0;
            int multiplier = 0;

            StringBuilder sb = new StringBuilder(s);

            int i = 0;
            while (Char.IsDigit(sb[i]))
            {
                number = (number * 10) + Convert.ToInt32(sb[i]);
                i++;
            }

            switch (Char.ToUpper(sb[0]))
            {
                case 'K':
                    number *= (multiplier = 1000);
                    break;
                case 'M':
                    number *= (multiplier = 1000000);
                    break;
                default:
                    return 0;
            }

            i = 0;
            while (Char.IsDigit(sb[i]) && multiplier > 1)
            {
                multiplier /= 10;
                number = number + Convert.ToInt32(sb[i] * multiplier);
            }

            if (!Char.IsDigit(sb[0]))
                return 0;

            return number;
        }

        public static int parsebet(int currentbet, string s)
        {
            StringBuilder sb = new StringBuilder(s);
            if (Char.IsDigit(sb[0]))
                return advatoi(s);
            if (sb[0] == '+')
            {
                if (sb.Length == 1)
                    return (currentbet * 125) / 100;
                return (currentbet * (100 + Convert.ToInt32(s))) / 100;
            }

            if (sb[0] == '*' || sb[0] == 'x')
            {
                if (sb.Length == 1)
                    return currentbet * 2;
                return currentbet * Convert.ToInt32(s);
            }
            return 0;
        }

        #endregion
        #endregion

        private static readonly ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TcpServer _networkMgr;
        public static IKernel Kernel { get; private set; }

        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            
            try
            {
                ConfigureLogging();
                InitializeNinjectKernel();

                OnServerStart();
                GameManager.Instance.DoLoop();
                OnServerStop();
            }
            catch (Exception ex)
            {
                LogManager.Instance.Boot(ex);
                Environment.Exit(0);
            }
        }

        private static void InitializeNinjectKernel()
        {
            Kernel = new StandardKernel();

            Kernel.Load("SmaugCS.*.dll");
        }

        private static void ConfigureLogging()
        {
            GlobalContext.Properties["BootLogName"] = string.Format("{0}\\{1}_{2}.log",
                                                                    GameConstants.GetLogPath(), "BootLog",
                                                                    DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            GlobalContext.Properties["BugsLogName"] = string.Format("{0}\\{1}.log",
                                                                    GameConstants.GetLogPath(), "Bugs");
            GlobalContext.Properties["SmaugLogName"] = string.Format("{0}\\{1}.log",
                                                                     GameConstants.GetLogPath(), "Smaug");

            log4net.Config.XmlConfigurator.Configure();
        }

        private static void OnServerStart()
        {
            LogWrapper logWrapper = new LogWrapper(Logger, LogLevel.Debug);
            LogManager.Instance.Initialize(logWrapper, GameConstants.GetDataPath());
            LogManager.Instance.Boot("---------------------[ Boot Log ]--------------------");

            var loaded = SystemConstants.LoadSystemDirectoriesFromConfig(GameConstants.GetDataPath());
            LogManager.Instance.Boot("{0} SystemDirectories loaded.", loaded);

            loaded = SystemConstants.LoadSystemFilesFromConfig(GameConstants.GetDataPath());
            LogManager.Instance.Boot("{0} SystemFiles loaded.", loaded);

            LookupManager.Instance.Initialize();

            LuaManager.Instance.Initialize(LogManager.Instance, GameConstants.GetDataPath());
            InitializeLuaInjections();
            InitializeLuaFunctions();

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Lookups));

            _networkMgr = new TcpServer(logWrapper, new TcpUserRepository());
            _networkMgr.Startup(Convert.ToInt32(ConfigurationManager.AppSettings["port"]),
                           IPAddress.Parse(ConfigurationManager.AppSettings["host"]));
            _networkMgr.OnTcpUserStatusChanged += NetworkMgrOnOnTcpUserStatusChanged;

            DatabaseManager.Instance.Initialize(LogManager.Instance);
            //DatabaseManager.Instance.InitializeDatabase(false);

            string connectionString = ConfigurationManager.ConnectionStrings["SmaugDB"]
                .ConnectionString.Replace("|DataPath|", GameConstants.GetDataPath());

            IDbConnection connection = new SqlCeConnection(connectionString);

            BanManager.Instance.Initialize(LogManager.Instance, new SmallDb(), connection);
            BanManager.Instance.LoadBans();

            BoardManager.Instance.Initialize(LogManager.Instance, new SmallDb(), connection);
            BoardManager.Instance.LoadBoards();
            // TODO: Load Projects

            GameManager.Instance.Initialize(false);
            CalendarManager.Instance.Initialize(LogManager.Instance, GameManager.Instance);
            GameManager.Instance.GameTime.SetTimeOfDay(GameManager.Instance.SystemData);
            WeatherManager.Instance.InitializeWeatherMap(WEATHER_SIZE_X, WEATHER_SIZE_Y);

            InitializeGameData();           
        }

        private static void NetworkMgrOnOnTcpUserStatusChanged(object sender, NetworkEventArgs networkEventArgs)
        {
            ITcpUser user = (ITcpUser) sender;
            
            // TODO: if disconnected, Remove from CharacterRepository
            // TODO: if connected, create new instance and add to CharacterRepository
        }

        private static void OnServerStop()
        {
            _networkMgr.Shutdown("Shutting down MUD");

            // TODO: Shutdown Managers
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            LogManager.Instance.Bug((Exception)unhandledExceptionEventArgs.ExceptionObject);
        }

        private static void InitializeLuaInjections()
        {
            LuaAreaFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance);
            LuaCreateFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance, LogManager.Instance);
            LuaGetFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance, GameConstants.GetDataPath());
            LuaMobFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance);
            LuaObjectFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance);
            LuaRoomFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance);
            LuaLookupFunctions.InitializeReferences(LookupManager.Instance, LogManager.Instance);
        }

        private static void InitializeLuaFunctions()
        {
            var proxy = new LuaInterfaceProxy();
            var luaFuncRepo = new LuaFunctionRepository();
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaAreaFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaCreateFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaGetFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaMobFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaObjectFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof (LuaRoomFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof (LuaLookupFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof (LogManager));
            proxy.RegisterFunctions(luaFuncRepo);
            LuaManager.Instance.InitializeLuaProxy(proxy);  
        }

        private static void InitializeGameData()
        {
            LogManager.Instance.Boot("Initializing Game Data");

            ExecuteLuaScripts();
            LoaderInitializer.Initialize();

            //// Pre-Tests the module_Area to catch any errors early before area load
            LuaManager.Instance.DoLuaScript(GameConstants.GetDataPath() + "//modules//module_area.lua");

            LoaderInitializer.Load();
        }

        private static void ExecuteLuaScripts()
        {
            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Commands));
            LogManager.Instance.Boot("{0} Commands loaded.", DatabaseManager.Instance.COMMANDS.Count);

            LookupManager.Instance.CommandLookup.UpdateFunctionReferences(DatabaseManager.Instance.COMMANDS.Values);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.SpecFuns));
            LogManager.Instance.Boot("{0} SpecFuns loaded.", DatabaseManager.Instance.SPEC_FUNS.Count);
            //SpecFunLookupTable.UpdateCommandFunctionReferences(DatabaseManager.Instance.SPEC_FUNS.Values);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Socials));
            LogManager.Instance.Boot("{0} Socials loaded.", DatabaseManager.Instance.SOCIALS.Count);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Skills));
            LogManager.Instance.Boot("{0} Skills loaded.", DatabaseManager.Instance.SKILLS.Count);

            LookupManager.Instance.SkillLookup.UpdateFunctionReferences(DatabaseManager.Instance.SKILLS.Values);
            LookupManager.Instance.SpellLookup.UpdateFunctionReferences(DatabaseManager.Instance.SKILLS.Values);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Liquids));
            LogManager.Instance.Boot("{0} Liquids loaded.", DatabaseManager.Instance.LIQUIDS.Count);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Mixtures));
            LogManager.Instance.Boot("{0} Mixtures loaded.",
                                     DatabaseManager.Instance.GetRepository<MixtureData>(RepositoryTypes.Mixtures).Count);
            // TODO: Update function references

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Herbs));
            LogManager.Instance.Boot("{0} Herbs loaded.", DatabaseManager.Instance.GetSkills(SkillTypes.Herb).Count());
            LookupManager.Instance.SkillLookup.UpdateFunctionReferences(DatabaseManager.Instance.GetSkills(SkillTypes.Herb));

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Tongues));
            LogManager.Instance.Boot("{0} Tongues loaded.", DatabaseManager.Instance.LANGUAGES.Count);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Planes));
            LogManager.Instance.Boot("{0} Planes loaded.",
                                     DatabaseManager.Instance.GetRepository<PlaneData>(RepositoryTypes.Planes).Count);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Morphs));
            LogManager.Instance.Boot("{0} Morphs loaded.",
                                     DatabaseManager.Instance.GetRepository<MorphData>(RepositoryTypes.Morphs).Count);

        }

    }
}
