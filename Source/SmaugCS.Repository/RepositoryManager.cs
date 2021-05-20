using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Language;
using SmaugCS.Logging;
using EnumerationExtensions = SmaugCS.Common.EnumerationExtensions;

namespace SmaugCS.Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Dictionary<RepositoryTypes, object> _repositories;
        public ILogManager LogManager { get; }
        public static IKernel Kernel { get; private set; }

        public RepositoryManager(IKernel kernel, ILogManager logManager)
        {
            LogManager = logManager;
            Kernel = kernel;

            _repositories = new Dictionary<RepositoryTypes, object>();
            foreach (var repoType in EnumerationFunctions.GetAllEnumValues<RepositoryTypes>())
            {
                var attrib = EnumerationExtensions.GetAttribute<TypeMapAttribute>(repoType);
                if (attrib?.Repository == null)
                    continue;

                _repositories.Add(repoType, Activator.CreateInstance(attrib.Repository));
            }
        }

        public static IRepositoryManager Instance => Kernel.Get<IRepositoryManager>();

        /// <summary>
        /// Adds the given object to the repository of the matching entity type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void AddToRepository<T>(T obj) where T : Entity
        {
            var repo = GetRepository<T>(ObjectTypeToRepositoryType(typeof(T)));
            if (repo.Contains(obj.ID)) return;

            repo.Add(obj.ID, obj);
            LogManager.Boot("{0} {1} added to repository", typeof(T), obj.ID);
        }

        /// <summary>
        /// Generates a new ID using the highest value currently in the repository 
        /// for the given entity type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public long GenerateNewId<T>() where T : Entity
        {
            var repo = GetRepository<T>(ObjectTypeToRepositoryType(typeof(T)));
            return repo.Count == 0 ? 1 : repo.Values.Max(x => x.ID) + 1;
        }

        /// <summary>
        /// Matches the given type to an equivalent enumerated repository type
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private static RepositoryTypes ObjectTypeToRepositoryType(Type objectType)
        {
            foreach (var repoType in EnumerationFunctions.GetAllEnumValues<RepositoryTypes>())
            {
                var attrib = EnumerationExtensions.GetAttribute<TypeMapAttribute>(repoType);
                if (attrib?.Object == null)
                    continue;
                if (attrib.Object == objectType)
                    return repoType;
            }

            throw new ArgumentException($"{objectType} is not a valid Repository Type", nameof(objectType));
        }

        #region Properties
        public RoomRepository ROOMS => _repositories[RepositoryTypes.Rooms].CastAs<RoomRepository>();

        public AreaRepository AREAS => _repositories[RepositoryTypes.Areas].CastAs<AreaRepository>();

        public ObjectRepository OBJECTTEMPLATES => _repositories[RepositoryTypes.ObjectTemplates].CastAs<ObjectRepository>();

        public MobileRepository MOBILETEMPLATES => _repositories[RepositoryTypes.MobileTemplates].CastAs<MobileRepository>();

        public CharacterRepository CHARACTERS => _repositories[RepositoryTypes.Characters].CastAs<CharacterRepository>();

        public ObjInstanceRepository OBJECTS => _repositories[RepositoryTypes.ObjectInstances].CastAs<ObjInstanceRepository>();

        public GenericRepository<T> GetRepository<T>(RepositoryTypes type) where T : class
        {
            return _repositories.ContainsKey(type) ? _repositories[type].CastAs<GenericRepository<T>>() : null;
        }

        public GenericRepository<LiquidData> LIQUIDS => GetRepository<LiquidData>(RepositoryTypes.Liquids);

        public GenericRepository<SkillData> SKILLS => GetRepository<SkillData>(RepositoryTypes.Skills);

        public GenericRepository<HerbData> HERBS => GetRepository<HerbData>(RepositoryTypes.Herbs);

        public GenericRepository<SpecialFunction> SPEC_FUNS => GetRepository<SpecialFunction>(RepositoryTypes.SpecFuns);

        public GenericRepository<CommandData> COMMANDS => GetRepository<CommandData>(RepositoryTypes.Commands);

        public GenericRepository<SocialData> SOCIALS => GetRepository<SocialData>(RepositoryTypes.Socials);

        public GenericRepository<LanguageData> LANGUAGES => GetRepository<LanguageData>(RepositoryTypes.Languages);

        public GenericRepository<RaceData> RACES => GetRepository<RaceData>(RepositoryTypes.Races);

        public GenericRepository<ClassData> CLASSES => GetRepository<ClassData>(RepositoryTypes.Classes);

        public GenericRepository<DeityData> DEITIES => GetRepository<DeityData>(RepositoryTypes.Deities);

        public GenericRepository<ClanData> CLANS => GetRepository<ClanData>(RepositoryTypes.Clans);

        public GenericRepository<CouncilData> COUNCILS => GetRepository<CouncilData>(RepositoryTypes.Councils);

        public GenericRepository<PlaneData> PLANES => GetRepository<PlaneData>(RepositoryTypes.Planes);

        public GenericRepository<MorphData> MORPHS => GetRepository<MorphData>(RepositoryTypes.Morphs);

        public GenericRepository<HintData> HINTS => GetRepository<HintData>(RepositoryTypes.Hints);

        public GenericRepository<MixtureData> MIXTURES => GetRepository<MixtureData>(RepositoryTypes.Mixtures);

        #endregion

        #region Get Methods
        /// <summary>
        /// Gets an entity by name from the repository of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetEntity<T>(string name) where T : class
        {
            return
                GetRepository<T>(ObjectTypeToRepositoryType(typeof (T)))
                    .Values.FirstOrDefault(x => x.CastAs<Entity>().Name.EqualsIgnoreCase(name));
        }

        /// <summary>
        /// Gets an entity by ID from the repository of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity<T>(long id) where T : class
        {
            return
                GetRepository<T>(ObjectTypeToRepositoryType(typeof (T)))
                    .Values.FirstOrDefault(x => x.CastAs<Entity>().ID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity<T>(int id) where T : class
        {
            return GetEntity<T>(id);
        }
        #endregion

        /*public void InitializeDatabase(bool fCopyOver)
        {

            GameManager.Instance.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadAllMail, LevelConstants.GetLevel("demi"));
            GameManager.Instance.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadMailFree, LevelConstants.ImmortalLevel);
            // TODO Do the rest of the system data

            if (!db.load_systemdata(GameManager.Instance.SystemData))
            {
                LogManager.Instance.Boot("SystemData not found. Creating new configuration.");
                GameManager.Instance.SystemData.alltimemax = 0;
                GameManager.Instance.SystemData.MudTitle = "(Name not set)";
                act_wiz.update_timers();
                //act_wiz.update_calendar();
                db.save_sysdata(GameManager.Instance.SystemData);
            }

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Socials));
            LogManager.Instance.Boot("{0} Socials loaded.", SOCIALS.Count());

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Skills));
            LogManager.Instance.Boot("{0} Skills loaded.", SKILLS.Count());

            SkillLookupTable.UpdateSkillFunctionReferences(SKILLS);
            SpellLookupTable.UpdateSpellFunctionReferences(SKILLS);

            ClassLoader classLoader = new ClassLoader();
            _loaders.Add(classLoader);
            classLoader.Load();
            LogManager.Instance.Boot("{0} Classes loaded.", CLASSES.Count());

            RaceLoader raceLoader = new RaceLoader();
            _loaders.Add(raceLoader);
            raceLoader.Load();
            LogManager.Instance.Boot("{0} Races loaded.", RACES.Count());

            LogManager.Instance.Boot("Loading news data");
            //news.load_news();

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Liquids));
            LogManager.Instance.Boot("{0} Liquids loaded.", LIQUIDS.Count());

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Mixtures));
            LogManager.Instance.Boot("{0} Mixtures loaded.", db.MIXTURES.Count);
            // TODO: Update function references

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Herbs));
            LogManager.Instance.Boot("{0} Herbs loaded.", HERBS.Count());
            SkillLookupTable.UpdateSkillFunctionReferences(HERBS);     // Maps

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Tongues));
            LogManager.Instance.Boot("{0} Tongues loaded.", LANGUAGES.Count());

            LogManager.Instance.Boot("Making wizlist");
            db.make_wizlist();

            // TODO Had auction stuff, not sure why it was needed

            // TODO Save equipment is inside each object now

            LogManager.Instance.Boot("Setting time and weather.");
            TimeLoader timeLoader = new TimeLoader();
            TimeInfoData timeInfo = timeLoader.LoadTimeInfo();
            if (timeInfo != null)
            {
                GameManager.Instance.SetGameTime(timeInfo);

                LogManager.Instance.Boot("Resetting mud time based on current system time.");
                long lhour = (DateTime.Now.ToFileTimeUtc() - 650336715) / (GameManager.Instance.SystemData.PulseTick / GameManager.Instance.SystemData.PulsesPerSecond);
                GameManager.Instance.GameTime.Hour = (int)(lhour % GameManager.Instance.SystemData.HoursPerDay);

                long lday = lhour / GameManager.Instance.SystemData.HoursPerDay;
                GameManager.Instance.GameTime.Day = (int)(lday % GameManager.Instance.SystemData.DaysPerMonth);

                long lmonth = lday / GameManager.Instance.SystemData.DaysPerMonth;
                GameManager.Instance.GameTime.Month = (int)(lmonth % GameManager.Instance.SystemData.MonthsPerYear);

                GameManager.Instance.GameTime.Year = (int)(lmonth % GameManager.Instance.SystemData.MonthsPerYear);
            }

            GameManager.Instance.GameTime.SetTimeOfDay(GameManager.Instance.SystemData);

            WeatherManager.Instance.InitializeWeatherMap(Program.WEATHER_SIZE_X, Program.WEATHER_SIZE_Y);
            //if (!WeatherManager.Instance.Weather.Load())
            //{
            //    LogManager.Instance.Boot("Failed to load WeatherMap");
            //    // TODO Fatal
            //}

            //HolidayLoader hLoader = new HolidayLoader();
            //hLoader.Load();

            // TODO DNS Cache

            // TODO Assign GSNs
            LogManager.Instance.Boot("Assigning GSN's");
            Macros.ASSIGN_GSN("evasive style");
            // TODO Assign remainder

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Planes));
            LogManager.Instance.Boot("{0} Planes loaded.", 0);

            // Pre-Tests the module_Area to catch any errors early before area load
            LuaManager.Instance.DoLuaScript(GameConstants.GetDataPath() + "//modules//module_area.lua");

            AreaListLoader aLoader = new AreaListLoader();
            _loaders.Add(aLoader);
            aLoader.Load();
            LogManager.Instance.Boot("{0} Areas loaded.", AREAS.Count);

            // TODO init_supermob();

            LogManager.Instance.Boot("Fixing exits");
            db.FixExits();

            LogManager.Instance.Boot("Initializing economy");
            db.initialize_economy();

            if (fCopyOver)
            {
                LogManager.Instance.Boot("Loading world state...");
                hotboot.load_world();
            }

            LogManager.Instance.Boot("Resetting areas");
            db.area_update();

            LogManager.Instance.Boot("Loading buildlist");
            db.load_buildlist();

            LogManager.Instance.Boot("Loading boards");
            //boards.load_boards();

            ClanLoader clLoader = new ClanLoader();
            _loaders.Add(clLoader);
            clLoader.Load();
            LogManager.Instance.Boot("{0} Clans loaded.", CLANS.Count());

            CouncilLoader coLoader = new CouncilLoader();
            _loaders.Add(coLoader);
            coLoader.Load();
            LogManager.Instance.Boot("{0} Councils loaded.", COUNCILS.Count());

            DeityListLoader dLoader = new DeityListLoader();
            _loaders.Add(dLoader);
            dLoader.Load();
            LogManager.Instance.Boot("{0} Deities loaded.", DEITIES.Count());

            WatchListLoader wLoader = new WatchListLoader();
            _loaders.Add(wLoader);
            wLoader.Load();
            LogManager.Instance.Boot("{0} Watches loaded.", db.WATCHES.Count);

            //ban.load_banlist();
            //LogManager.Instance.Boot("{0} Bans loaded.", db.BANS.Count);

            db.load_reserved();
            LogManager.Instance.Boot("{0} Reserved Names loaded.", db.ReservedNames.Count);

            LogManager.Instance.Boot("Loading corpses");
            save.load_corpses();

            LogManager.Instance.Boot("Loading Immortal Hosts");
            // TODO load_imm_host();

            LogManager.Instance.Boot("Loading projects");
            db.load_projects();

            LogManager.Instance.Boot("Loading morphs");
            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Morphs));

            // TODO MOBTrigger = true;

            // TODO init_chess();

        }*/

    }
}
