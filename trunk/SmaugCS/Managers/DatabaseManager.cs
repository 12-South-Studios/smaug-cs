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
        private ILogManager _logManager;

        private DatabaseManager()
        {
            ROOMS = new RoomRepository();
            AREAS = new AreaRepository();
            OBJECT_INDEXES = new ObjectRepository();
            MOBILE_INDEXES = new MobileRepository();
            CHARACTERS = new CharacterRepository();
            //OBJECTS = new ObjInstanceRepository(Program.MaximumWearLocations, Program.MaximumWearLayers);
            _liquids = new List<LiquidData>();
            _herbs = new List<SkillData>();
            _skills = new List<SkillData>();
            _specfuns = new List<SpecialFunction>();
            _commands = new List<CommandData>();
            _socials = new List<SocialData>();
            _races = new List<RaceData>();
            _classes = new List<ClassData>();
            _deities = new List<DeityData>();
            _languages = new List<LanguageData>();
            _clans = new List<ClanData>();
            _councils = new List<CouncilData>();
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

        public void Initialize(ILogManager logManager)
        {
            _logManager = logManager;
        }
        
        public void InitializeDatabase(bool fCopyOver)
        {
            _logManager.Boot("Initializing the Database");
            db.SystemData = new SystemData();

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.Commands));
            _logManager.Boot("{0} Commands loaded.", COMMANDS.Count());
            CommandLookupTable.UpdateCommandFunctionReferences(COMMANDS);

            LuaManager.Instance.DoLuaScript(SystemConstants.GetSystemFile(SystemFileTypes.SpecFuns));
            LogManager.Instance.Boot("{0} SpecFuns loaded.", SPEC_FUNS.Count());
            // TODO: Update function references

            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadAllMail, LevelConstants.GetLevel("demi"));
            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadMailFree, LevelConstants.GetLevel("immortal"));
            // TODO Do the rest of the system data

            if (!db.load_systemdata(db.SystemData))
            {
                LogManager.Instance.Boot("SystemData not found. Creating new configuration.");
                db.SystemData.alltimemax = 0;
                db.SystemData.MudTitle = "(Name not set)";
                act_wiz.update_timers();
                //act_wiz.update_calendar();
                db.save_sysdata(db.SystemData);
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
                db.GameTime = timeInfo;

                LogManager.Instance.Boot("Resetting mud time based on current system time.");
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
                LogManager.Instance.Boot("Failed to load WeatherMap");
                // TODO Fatal
            }*/

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

        }

        #region Liquids
        private readonly List<LiquidData> _liquids; 
        public IEnumerable<LiquidData> LIQUIDS { get { return _liquids; } }
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

        public void AddLiquid(LiquidData liquid)
        {
            if (!LIQUIDS.Contains(liquid))
            {
                _liquids.Add(liquid);
                _logManager.Boot("Liquid {0} added", liquid.Name);
            }
        }
        #endregion

        #region Herbs
        private readonly List<SkillData> _herbs;
        public IEnumerable<SkillData> HERBS { get { return _herbs; } }
        public SkillData GetHerb(string name)
        {
            return HERBS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }
        public bool IsValidHerb(int sn)
        {
            return sn >= 0 && sn < HERBS.Count() && HERBS.ToList()[sn] != null && !HERBS.ToList()[sn].Name.IsNullOrEmpty();
        }
        public void AddHerb(SkillData herb)
        {
            if (!HERBS.Contains(herb))
            {
                _herbs.Add(herb);
                _logManager.Boot("Herb {0} added", herb.Name);
            }
        }
        #endregion

        #region Skills

        private readonly List<SkillData> _skills;
        public IEnumerable<SkillData> SKILLS { get { return _skills; } }
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
            AddSkill(new SkillData(Program.MAX_CLASS, Program.MAX_RACE) { Name = name, ID = newId });
            return newId;
        }

        public void AddSkill(SkillData skill)
        {
            if (!SKILLS.Contains(skill))
            {
                _skills.Add(skill);
                _logManager.Boot("Skill {0} added", skill.Name);
            }
        }
        #endregion

        #region SpecFuns
        private readonly List<SpecialFunction> _specfuns;
        public IEnumerable<SpecialFunction> SPEC_FUNS { get { return _specfuns; } }
        public SpecialFunction GetSpecFun(string name)
        {
            return SPEC_FUNS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }
        public void AddSpecFun(SpecialFunction specfun)
        {
            if (!SPEC_FUNS.Contains(specfun))
            {
                _specfuns.Add(specfun);
                _logManager.Boot("SpecFun {0} added", specfun.Name);
            }
        }
        #endregion

        #region Commands
        private readonly List<CommandData> _commands;
        public IEnumerable<CommandData> COMMANDS { get { return _commands; } }
        public CommandData GetCommand(string command)
        {
            return COMMANDS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(command));
        }
        public void AddCommand(CommandData command)
        {
            if (!COMMANDS.Contains(command))
            {
                _commands.Add(command);
                _logManager.Boot("Command {0} added", command.Name);
            }
        }
        #endregion

        #region Socials
        private readonly List<SocialData> _socials;
        public IEnumerable<SocialData> SOCIALS { get { return _socials; } } 
        public SocialData GetSocial(string command)
        {
            return SOCIALS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(command));
        }
        public void AddSocial(SocialData social)
        {
            if (!SOCIALS.Contains(social))
            {
                _socials.Add(social);
                _logManager.Boot("Social {0} added", social.Name);
            }
        }
        #endregion

        #region Languages
        private readonly List<LanguageData> _languages;
        public IEnumerable<LanguageData> LANGUAGES { get { return _languages; } }
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
        public void AddLanguage(LanguageData lang)
        {
            if (!LANGUAGES.Contains(lang))
            {
                _languages.Add(lang);
                _logManager.Boot("Language {0} added", lang.Name);
            }
        }
        #endregion

        #region Races
        private readonly List<RaceData> _races;
        public IEnumerable<RaceData> RACES { get { return _races; } }
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
        public void AddRace(RaceData race)
        {
            if (!RACES.Contains(race))
            {
                _races.Add(race);
                _logManager.Boot("Race {0} added", race.Name);
            }
        }
        #endregion

        #region Classes
        private readonly List<ClassData> _classes;
        public IEnumerable<ClassData> CLASSES { get { return _classes; } } 
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
        public void AddClass(ClassData cls)
        {
            if (!_classes.Contains(cls))
            {
                _classes.Add(cls);
                _logManager.Boot("Class {0} added", cls.Name);
            }
        }
        #endregion

        #region Deities
        private readonly List<DeityData> _deities; 
        public IEnumerable<DeityData> DEITIES { get { return _deities; } }
        public DeityData GetDeity(string name)
        {
            return DEITIES.Single(x => x.Name.EqualsIgnoreCase(name));
        }

        public void AddDeity(DeityData deity)
        {
            if (!DEITIES.Contains(deity))
            {
                _deities.Add(deity);
                _logManager.Boot("Deity {0} added", deity.Name);
            }
        }
        #endregion
        
        #region Organizations
        private readonly List<ClanData> _clans;
        public IEnumerable<ClanData> CLANS { get { return _clans; } }
        public ClanData GetClan(string name)
        {
            return CLANS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public void AddClan(ClanData clan)
        {
            if (!CLANS.Contains(clan))
            {
                _clans.Add(clan);
                _logManager.Boot("Clan {0} added", clan.Name);
            }
        }

        private readonly List<CouncilData> _councils;
        public IEnumerable<CouncilData> COUNCILS { get { return _councils; } }
        public CouncilData GetCouncil(string name)
        {
            return COUNCILS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public void AddCouncil(CouncilData council)
        {
            if (!COUNCILS.Contains(council))
            {
                _councils.Add(council);
                _logManager.Boot("Council {0} added", council.Name);
            }
        }
        #endregion
    }
}
