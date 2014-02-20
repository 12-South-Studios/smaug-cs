using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Organizations;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Extensions;
using SmaugCS.Language;
using SmaugCS.Loaders;
using SmaugCS.Logging;
using SmaugCS.Lookup;
using SmaugCS.Repositories;
using SmaugCS.Weather;

namespace SmaugCS.Managers
{
    public sealed class DatabaseManager : GameSingleton, IDatabaseManager
    {
        private static DatabaseManager _instance;
        private static readonly object Padlock = new object();

        private readonly List<ListLoader> _loaders = new List<ListLoader>();

        private DatabaseManager()
        {
            ROOMS = new RoomRepository();
            AREAS = new AreaRepository();
            OBJECT_INDEXES = new ObjectRepository();
            MOBILE_INDEXES = new MobileRepository();
            CHARACTERS = new CharacterRepository();
            //OBJECTS = new ObjInstanceRepository(Program.MaximumWearLocations, Program.MaximumWearLayers);
            LIQUIDS = new List<LiquidData>();
            HERBS = new List<SkillData>();
            SKILLS = new List<SkillData>();
            SPEC_FUNS = new List<SpecialFunction>();
            COMMANDS = new List<CommandData>();
            SOCIALS = new List<SocialData>();
            RACES = new List<RaceData>();
            CLASSES = new List<ClassData>();
            DEITIES = new List<DeityData>();
            LANGUAGES = new List<LanguageData>();
            CLANS = new List<ClanData>();
            COUNCILS = new List<CouncilData>();
        }

        /// <summary>
        ///
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new DatabaseManager());
                }
            }
        }

        public static bool BootDb { get; set; }
        public ITemplateRepository<RoomTemplate> ROOMS { get; private set; }
        public IRepository<long, AreaData> AREAS { get; private set; }
        public ITemplateRepository<ObjectTemplate> OBJECT_INDEXES { get; private set; }
        
        public ITemplateRepository<MobTemplate> MOBILE_INDEXES { get; private set; }
        public MobTemplate GetMobTemplate(int vnum)
        {
            MobileRepository repo = (MobileRepository)MOBILE_INDEXES;

            MobTemplate found = repo.Get(vnum);
            if (found == null)
                throw new EntryNotFoundException("Missing MobTemplate {0}", vnum);

            return found;
        }
        
        public IInstanceRepository<CharacterInstance> CHARACTERS { get; private set; }
        public IInstanceRepository<ObjectInstance> OBJECTS { get; private set; }

        public void Initialize(bool fCopyOver)
        {
            LogManager.Instance.BootLog("Initializing the Database");
            db.SystemData = new SystemData();

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Commands));
            LogManager.Instance.BootLog("{0} Commands loaded.", COMMANDS.Count());
            CommandLookupTable.UpdateCommandFunctionReferences(COMMANDS);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.SpecFuns));
            LogManager.Instance.BootLog("{0} SpecFuns loaded.", SPEC_FUNS.Count());
            // TODO: Update function references

            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadAllMail, LevelConstants.GetLevel("demi"));
            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadMailFree, LevelConstants.GetLevel("immortal"));
            // TODO Do the rest of the system data

            if (!db.load_systemdata(db.SystemData))
            {
                LogManager.Instance.BootLog("SystemData not found. Creating new configuration.");
                db.SystemData.alltimemax = 0;
                db.SystemData.MudTitle = "(Name not set)";
                act_wiz.update_timers();
                //act_wiz.update_calendar();
                db.save_sysdata(db.SystemData);
            }

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Socials));
            LogManager.Instance.BootLog("{0} Socials loaded.", SOCIALS.Count());

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Skills));
            LogManager.Instance.BootLog("{0} Skills loaded.", SKILLS.Count());
            SkillLookupTable.UpdateSkillFunctionReferences(SKILLS);
            SpellLookupTable.UpdateSpellFunctionReferences(SKILLS);

            ClassLoader classLoader = new ClassLoader();
            _loaders.Add(classLoader);
            classLoader.Load();
            LogManager.Instance.BootLog("{0} Classes loaded.", CLASSES.Count());

            RaceLoader raceLoader = new RaceLoader();
            _loaders.Add(raceLoader);
            raceLoader.Load();
            LogManager.Instance.BootLog("{0} Races loaded.", RACES.Count());

            LogManager.Instance.BootLog("Loading news data");
            news.load_news();

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Liquids));
            LogManager.Instance.BootLog("{0} Liquids loaded.", LIQUIDS.Count());

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Mixtures));
            LogManager.Instance.BootLog("{0} Mixtures loaded.", db.MIXTURES.Count);
            // TODO: Update function references

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Herbs));
            LogManager.Instance.BootLog("{0} Herbs loaded.", HERBS.Count());
            SkillLookupTable.UpdateSkillFunctionReferences(HERBS);     // Maps

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Tongues));
            LogManager.Instance.BootLog("{0} Tongues loaded.", LANGUAGES.Count());

            LogManager.Instance.BootLog("Making wizlist");
            db.make_wizlist();

            // TODO Had auction stuff, not sure why it was needed

            // TODO Save equipment is inside each object now

            LogManager.Instance.BootLog("Setting time and weather.");
            TimeLoader timeLoader = new TimeLoader();
            TimeInfoData timeInfo = timeLoader.LoadTimeInfo();
            if (timeInfo != null)
            {
                db.GameTime = timeInfo;

                LogManager.Instance.BootLog("Resetting mud time based on current system time.");
                long lhour = (DateTime.Now.ToFileTimeUtc() - 650336715) / (db.SystemData.PulseTick / db.SystemData.PulsesPerSecond);
                db.GameTime.Hour = (int)(lhour % db.SystemData.HoursPerDay);

                long lday = lhour / db.SystemData.HoursPerDay;
                db.GameTime.Day = (int)(lday % db.SystemData.DaysPerMonth);

                long lmonth = lday / db.SystemData.DaysPerMonth;
                db.GameTime.Month = (int)(lmonth % db.SystemData.MonthsPerYear);

                db.GameTime.Year = (int)(lmonth % db.SystemData.MonthsPerYear);
            }

            db.GameTime.SetTimeOfDay(db.SystemData);

            WeatherManager.Instance.InitializeWeatherMap(Program.WEATHER_SIZE_X, Program.WEATHER_SIZE_Y);
            /*if (!WeatherManager.Instance.Weather.Load())
            {
                LogManager.Instance.BootLog("Failed to load WeatherMap");
                // TODO Fatal
            }*/

            //HolidayLoader hLoader = new HolidayLoader();
            //hLoader.Load();

            // TODO DNS Cache

            // TODO Assign GSNs
            LogManager.Instance.BootLog("Assigning GSN's");
            Macros.ASSIGN_GSN("evasive style");
            // TODO Assign remainder

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Planes));
            LogManager.Instance.BootLog("{0} Planes loaded.", 0);

            // Pre-Tests the module_Area to catch any errors early before area load
            LuaManager.Instance.DoLuaScript(GameConstants.GetDataPath() + "//modules//module_area.lua");

            AreaListLoader aLoader = new AreaListLoader();
            _loaders.Add(aLoader);
            aLoader.Load();
            LogManager.Instance.BootLog("{0} Areas loaded.", AREAS.Count);

            // TODO init_supermob();

            LogManager.Instance.BootLog("Fixing exits");
            db.FixExits();

            LogManager.Instance.BootLog("Initializing economy");
            db.initialize_economy();

            if (fCopyOver)
            {
                LogManager.Instance.BootLog("Loading world state...");
                hotboot.load_world();
            }

            LogManager.Instance.BootLog("Resetting areas");
            db.area_update();

            LogManager.Instance.BootLog("Loading buildlist");
            db.load_buildlist();

            LogManager.Instance.BootLog("Loading boards");
            boards.load_boards();

            ClanLoader clLoader = new ClanLoader();
            _loaders.Add(clLoader);
            clLoader.Load();
            LogManager.Instance.BootLog("{0} Clans loaded.", CLANS.Count());

            CouncilLoader coLoader = new CouncilLoader();
            _loaders.Add(coLoader);
            coLoader.Load();
            LogManager.Instance.BootLog("{0} Councils loaded.", COUNCILS.Count());

            DeityListLoader dLoader = new DeityListLoader();
            _loaders.Add(dLoader);
            dLoader.Load();
            LogManager.Instance.BootLog("{0} Deities loaded.", DEITIES.Count());

            WatchListLoader wLoader = new WatchListLoader();
            _loaders.Add(wLoader);
            wLoader.Load();
            LogManager.Instance.BootLog("{0} Watches loaded.", db.WATCHES.Count);

            //ban.load_banlist();
            //LogManager.Instance.BootLog("{0} Bans loaded.", db.BANS.Count);

            db.load_reserved();
            LogManager.Instance.BootLog("{0} Reserved Names loaded.", db.ReservedNames.Count);

            LogManager.Instance.BootLog("Loading corpses");
            save.load_corpses();

            LogManager.Instance.BootLog("Loading Immortal Hosts");
            // TODO load_imm_host();

            LogManager.Instance.BootLog("Loading projects");
            db.load_projects();

            LogManager.Instance.BootLog("Loading morphs");
            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Morphs));

            // TODO MOBTrigger = true;

            // TODO init_chess();

        }

        
        public IEnumerable<LiquidData> LIQUIDS { get; private set; }
        public IEnumerable<SkillData> HERBS { get; private set; }
        public IEnumerable<SkillData> SKILLS { get; private set; }
        public IEnumerable<SpecialFunction> SPEC_FUNS { get; private set; }

        #region Liquids
        public LiquidData GetLiquid(string str)
        {
            return str.IsNumber()
                ? LIQUIDS.FirstOrDefault(x => x.Vnum == str.ToInt32())
                : LIQUIDS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(str));
        }
        public LiquidData GetLiquid(int vnum)
        {
            return LIQUIDS.FirstOrDefault(x => x.Vnum == vnum);
        }
        #endregion

        #region Herbs
        public SkillData GetHerb(string name)
        {
            return HERBS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }
        public bool IsValidHerb(int sn)
        {
            return sn >= 0 && sn < HERBS.Count() && HERBS.ToList()[sn] != null && !HERBS.ToList()[sn].Name.IsNullOrEmpty();
        }
        #endregion

        #region Skills

        public SkillData GetSkill(string name)
        {
            return SKILLS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public SkillData GetSkill(int skillNumber)
        {
            return SKILLS.FirstOrDefault(x => x.ID == skillNumber);
        }

        public IEnumerable<SkillData> GetSkills(SkillTypes type)
        {
            return SKILLS.Where(x => x.Type == type);
        }

        /// <summary>
        /// Lookup a skill by name (or partial)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int LookupSkill(string name)
        {
            // Try to find an exact match for this skill
            SkillData skill = SKILLS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
            if (skill == null)
            {
                // Try to find a prefix match
                IEnumerable<SkillData> skills = SKILLS.Where(x => x.Name.StartsWithIgnoreCase(name));
                if (!skills.Any())
                {
                    LogManager.Instance.Bug("Skill entry {0} not found", name);
                    return -1;
                }

                skill = skills.First();
            }

            return skill.ID;
        }

        public int AddSkill(string name)
        {
            if (LookupSkill(name) > -1)
                return -1;

            int newId = SKILLS.Max(x => x.ID) + 1;
            SKILLS.ToList().Add(new SkillData(Program.MAX_CLASS, Program.MAX_RACE) { Name = name, ID = newId });
            return newId;
        }

        #endregion

        #region SpecFuns
        public SpecialFunction GetSpecFun(string name)
        {
            return SPEC_FUNS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }
        #endregion

        public IEnumerable<CommandData> COMMANDS { get; private set; }
        public CommandData GetCommand(string command)
        {
            return COMMANDS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(command));
        }

        public IEnumerable<SocialData> SOCIALS { get; private set; } 
        public SocialData GetSocial(string command)
        {
            return SOCIALS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(command));
        }


        #region Languages
        public IEnumerable<LanguageData> LANGUAGES { get; private set; }
        public LanguageData GetLanguage(string name)
        {
            return LANGUAGES.FirstOrDefault(x => x.Name.Equals(name));
        }
        public int GetLanguageCount(int languages)
        {
            return GameConstants.LanguageTable.Keys.ToList()
                     .Where(x => x != (int)LanguageTypes.Clan
                         && x != (int)LanguageTypes.Unknown)
                     .Select(x => (languages & x) > 0).Count();
        }
        #endregion

        #region Races
        public IEnumerable<RaceData> RACES { get; private set; }
        public RaceData GetRace(RaceTypes type)
        {
            return RACES.FirstOrDefault(x => x.Type == type);
        }
        public RaceData GetRace(string name)
        {
            return RACES.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public RaceData GetRace(int id)
        {
            return RACES.FirstOrDefault(x => (int)x.Type == id);
        }
        #endregion

        #region Classes

        public IEnumerable<ClassData> CLASSES { get; private set; }
        public ClassData GetClass(ClassTypes type)
        {
            return CLASSES.FirstOrDefault(x => x.Type == type);
        }
        public ClassData GetClass(string name)
        {
            return CLASSES.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public ClassData GetClass(int id)
        {
            return CLASSES.FirstOrDefault(x => (int)x.Type == id);
        }
        #endregion

        public IEnumerable<DeityData> DEITIES { get; private set; }
        public DeityData GetDeity(string name)
        {
            return DEITIES.Single(x => x.Name.EqualsIgnoreCase(name));
        }

                #region Organizations
        public IEnumerable<ClanData> CLANS { get; private set; }
        public ClanData GetClan(string name)
        {
            return CLANS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<CouncilData> COUNCILS { get; private set; }
        public CouncilData GetCouncil(string name)
        {
            return COUNCILS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}
