using Autofac;
using Realm.Library.Network.Tcp;
using Realm.Library.Network;
using SmaugCS.Auction;
using SmaugCS.Ban;
using SmaugCS.Board;
using SmaugCS.Clans;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.News;
using SmaugCS.Repository;
using SmaugCS.Time;
using SmaugCS.Weather;
using System;
using System.Linq;
using System.Threading.Tasks;
using Realm.Library.Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using SmaugCS.DAL;

namespace SmaugCS
{
    public class Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogWrapper _logger;
        private readonly INetworkServer _server;

        public Application(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _server = serviceProvider.GetService<INetworkServer>();
            _logger = serviceProvider.GetService<ILogWrapper>();

            (_server as ITcpServer).OnTcpUserStatusChanged += NetworkManager_OnOnTcpUserStatusChanged;
        }

        public static void RegisterModules(ContainerBuilder containerBuilder, Config.Configuration.Settings settings,
            Config.Configuration.Constants constants)
        {
            containerBuilder.RegisterModule(new DbContextModule(settings.Database));
            containerBuilder.RegisterModule(new LoggingModule(constants));
            containerBuilder.RegisterModule(new RepositoryModule());
            containerBuilder.RegisterModule(new SmaugModule(settings, constants));
            containerBuilder.RegisterModule(new AuctionModule(constants));
            containerBuilder.RegisterModule(new BanModule(constants));
            containerBuilder.RegisterModule(new BoardModule());
            containerBuilder.RegisterModule(new LuaModule());
            containerBuilder.RegisterModule(new NewsModule());
            containerBuilder.RegisterModule(new TimeModule());
            containerBuilder.RegisterModule(new WeatherModule(constants));
            containerBuilder.RegisterModule(new ClanModule());
        }

        public static void ConfigureServices(ContainerBuilder containerBuilder)
        {

        }

        public async Task Start(Config.Configuration.Settings settings, Config.Configuration.Constants constants)
        {
            try
            {
                InitializeManagersAndGameSettings(constants, settings);
                InitializeStaticGameData(constants, settings);

                _server.Startup();

                while(true)
                {
                    // ?
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                Shutdown();
            }
            finally
            {
                // ?
            }
        }

        private static void InitializeManagersAndGameSettings(Config.Configuration.Constants constants, Config.Configuration.Settings settings)
        {
            Program.LogManager.Boot("-----------------------[ Boot Log ]----------------------");

            //var loaded = SystemConstants.LoadSystemDirectoriesFromConfig($"{constants.AppPath}/data/", settings);
            //Program.LogManager.Boot("{0} SystemDirectories loaded.", loaded);

            //loaded = SystemConstants.LoadSystemFilesFromConfig();
            //Program.LogManager.Boot("{0} SystemFiles loaded.", loaded);

            var luaInitializer = Program.Container.ResolveNamed<IInitializer>("LuaInitializer");
            if (luaInitializer == null)
                throw new ApplicationException("LuaInitializer failed to start");

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/Lookups.lua");
            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/StatModLookups.lua");

            Program.GameManager.SetGameTime(Program.CalendarManager.GameTime);
            Program.GameManager.GameTime.SetTimeOfDay(settings.Values.HourOfSunrise, settings.Values.HourOfDayBegin, 
                settings.Values.HourOfSunset, settings.Values.HourOfNightBegin);

            InitializeStaticGameData(constants, settings);
        }

        private static void InitializeStaticGameData(Config.Configuration.Constants constants, Config.Configuration.Settings settings)
        {
            Program.LogManager.Boot("Initializing Game Data");
            LoadSystemDataFromLuaScripts(constants, settings);
            var loaderInitializer = Program.Container.ResolveNamed<IInitializer>("LoaderInitializer");

            //// Pre-Tests the module_Area to catch any errors early before area load
            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/modules/module_area.lua");
            // TODO Load Areas,Clans,Classes,Councils,Deities,Languages,Races (lua scripts)
            // TODO Incomplete - Comments, Help Areas, Hints, Holidays, Mixtures, Reserved, Time, Watch List
        }

        private static void LoadSystemDataFromLuaScripts(Config.Configuration.Constants constants, Config.Configuration.Settings settings)
        {
            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/commands.lua");
            Program.LogManager.Boot("{0} Commands loaded.", Program.RepositoryManager.COMMANDS.Count);
            Program.LookupManager.CommandLookup.UpdateFunctionReferences(Program.RepositoryManager.COMMANDS.Values);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/specfuns.lua");
            Program.LogManager.Boot("{0} SpecFuns loaded.", Program.RepositoryManager.SPEC_FUNS.Count);
            //SpecFunLookupTable.UpdateCommandFunctionReferences(Program.RepositoryManager.SPEC_FUNS.Values);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/socials.lua");
            Program.LogManager.Boot("{0} Socials loaded.", Program.RepositoryManager.SOCIALS.Count);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/skills.lua");
            Program.LogManager.Boot("{0} Skills loaded.", Program.RepositoryManager.SKILLS.Count);
            Program.LookupManager.SkillLookup.UpdateFunctionReferences(Program.RepositoryManager.SKILLS.Values);
            Program.LookupManager.SpellLookup.UpdateFunctionReferences(Program.RepositoryManager.SKILLS.Values);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/liquids.lua");
            Program.LogManager.Boot("{0} Liquids loaded.", Program.RepositoryManager.LIQUIDS.Count);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/mixtures.lua");
            Program.LogManager.Boot("{0} Mixtures loaded.",
                Program.RepositoryManager.GetRepository<MixtureData>(RepositoryTypes.Mixtures).Count);
            // TODO: Update function references

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/herbs.lua");
            Program.LogManager.Boot("{0} Herbs loaded.", Program.RepositoryManager.HERBS.Count);
            Program.LookupManager.SkillLookup.UpdateFunctionReferences(Program.RepositoryManager.HERBS.Values);

            foreach(var language in settings.Languages.Split(","))
            {
                Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/languages/{language}.lua");
            }            
            Program.LogManager.Boot("{0} Tongues loaded.", Program.RepositoryManager.LANGUAGES.Count);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/planes.lua");
            Program.LogManager.Boot("{0} Planes loaded.", Program.RepositoryManager.PLANES.Count);

            Program.LuaManager.DoLuaScript($"{constants.AppPath}/data/system/morphs.lua");
            Program.LogManager.Boot("{0} Morphs loaded.", Program.RepositoryManager.MORPHS.Count);
        }

        public void Shutdown()
        {

        }

        private static void NetworkManager_OnOnTcpUserStatusChanged(object sender, NetworkEventArgs networkEventArgs)
        {
            var user = (INetworkUser)sender;
            var eventArgs = (TcpNetworkEventArgs)networkEventArgs;

            if (eventArgs.SocketStatus == TcpSocketStatus.Disconnected)
                DisconnectUser(user);
            else
                ConnectUser(user);
        }

        private static void ConnectUser(INetworkUser user)
        {
            var descrip = new DescriptorData(9999, 9999, 9999) { User = user };
            db.DESCRIPTORS.Add(descrip);
        }

        private static void DisconnectUser(INetworkUser user)
        {
            var character = Program.RepositoryManager.CHARACTERS.Values.OfType<PlayerInstance>().FirstOrDefault(x => x.Descriptor.User == user);
            if (character == null)
            {
                var descrip = db.DESCRIPTORS.FirstOrDefault(x => x.User == user);
                if (descrip == null)
                    throw new ObjectNotFoundException($"Character not found matching user {user.IpAddress}");

                db.DESCRIPTORS.Remove(descrip);
                return;
            }

            Program.RepositoryManager.CHARACTERS.Delete(character.ID);
        }
    }
}
