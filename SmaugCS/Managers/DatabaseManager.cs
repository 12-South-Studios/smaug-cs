﻿using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Database;
using SmaugCS.Extensions;
using SmaugCS.Loaders;
using SmaugCS.Weather;

namespace SmaugCS.Managers
{
    public sealed class DatabaseManager : GameSingleton
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
            //OBJECTS = new ObjInstanceRepository(Program.MAX_WEAR, Program.MAX_LAYERS);
            LIQUIDS = new List<LiquidData>();
            HERBS = new List<SkillData>();
            SKILLS = new List<SkillData>();
            SPEC_FUNS = new List<SpecialFunction>();
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
        public RoomRepository ROOMS { get; private set; }
        public AreaRepository AREAS { get; private set; }
        public ObjectRepository OBJECT_INDEXES { get; private set; }
        public MobileRepository MOBILE_INDEXES { get; private set; }
        public CharacterRepository CHARACTERS { get; private set; }
        public ObjInstanceRepository OBJECTS { get; private set; }

        private void DoLuaScript(string file)
        {
            LuaManager.Instance.Proxy.DoFile(SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + file);
        }

        public void Initialize(bool fCopyOver)
        {
            LogManager.BootLog("---------------------[ Boot Log ]--------------------");

            db.SystemData = new SystemData();

            LogManager.BootLog("Loading commands...");
            DoLuaScript(Program.GetAppSetting("CommandsFile"));

            LogManager.BootLog("Loading spec funs...");
            DoLuaScript(Program.GetAppSetting("SpecFunsFile"));

            LogManager.BootLog("Loading sysdata configuration...");
            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadAllMail, Program.GetLevel("demi"));
            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadMailFree, Program.GetLevel("immortal"));
            // TODO Do the rest of the system data

            if (db.load_systemdata(db.SystemData))
            {
                LogManager.BootLog("Not found. Creating new configuration.");
                db.SystemData.alltimemax = 0;
                db.SystemData.MudTitle = "(Name not set)";
                act_wiz.update_timers();
                act_wiz.update_calendar();
                db.save_sysdata(db.SystemData);
            }

            LogManager.BootLog("Loading socials");
            DoLuaScript(Program.GetAppSetting("SocialsFile"));

            LogManager.BootLog("Loading skill table");
            //tables.load_skill_table();
            //tables.remap_slot_numbers();
            //db.NumberSortedSkills = db.SKILLS.Count;
            DoLuaScript(Program.GetAppSetting("SkillsFile"));

            LogManager.BootLog("Loading classes");
            tables.load_classes();

            LogManager.BootLog("Loading races");
            tables.load_races();

            LogManager.BootLog("Loading news data");
            news.load_news();

            LogManager.BootLog("Loading liquids");
            DoLuaScript(Program.GetAppSetting("LiquidsFile"));

            LogManager.BootLog("Loading mixtures");
            DoLuaScript(Program.GetAppSetting("MixturesFile"));

            LogManager.BootLog("Loading herbs");
            DoLuaScript(Program.GetAppSetting("HerbsFile"));

            LogManager.BootLog("Loading tongues");
            tables.load_tongues();

            LogManager.BootLog("Making wizlist");
            db.make_wizlist();

            // TODO Had auction stuff, not sure why it was needed

            // TODO Save equipment is inside each object now

            LogManager.BootLog("Setting time and weather.");
            TimeLoader timeLoader = new TimeLoader();
            TimeInfoData timeInfo = timeLoader.LoadTimeInfo();
            if (timeInfo != null)
            {
                db.GameTime = timeInfo;

                LogManager.BootLog("Resetting mud time based on current system time.");
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
                LogManager.BootLog("Failed to load WeatherMap");
                // TODO Fatal
            }*/

            LogManager.BootLog("Loading holiday chart...");
            HolidayLoader hLoader = new HolidayLoader();
            hLoader.Load();

            // TODO DNS Cache

            // TODO Assign GSNs
            LogManager.BootLog("Assigning GSN's");
            Macros.ASSIGN_GSN("evasive style");
            // TODO Assign remainder

            LogManager.BootLog("Loading planes...");
            DoLuaScript(Program.GetAppSetting("PlanesFile"));

            AreaListLoader aLoader = new AreaListLoader();
            _loaders.Add(aLoader);
            aLoader.Load();

            // TODO init_supermob();

            LogManager.BootLog("Fixing exits");
            db.FixExits();

            LogManager.BootLog("Initializing economy");
            db.initialize_economy();

            if (fCopyOver)
            {
                LogManager.BootLog("Loading world state...");
                hotboot.load_world();
            }

            LogManager.BootLog("Resetting areas");
            db.area_update();

            LogManager.BootLog("Loading buildlist");
            db.load_buildlist();

            LogManager.BootLog("Loading boards");
            boards.load_boards();

            LogManager.BootLog("Loading clans");
            ClanLoader clLoader = new ClanLoader();
            _loaders.Add(clLoader);
            clLoader.Load();

            LogManager.BootLog("Loading Councils");
            CouncilLoader coLoader = new CouncilLoader();
            _loaders.Add(coLoader);
            coLoader.Load();

            LogManager.BootLog("Loading deities");
            DeityListLoader dLoader = new DeityListLoader();
            _loaders.Add(dLoader);
            dLoader.Load();

            LogManager.BootLog("Loading watches");
            WatchListLoader wLoader = new WatchListLoader();
            _loaders.Add(wLoader);
            wLoader.Load();

            LogManager.BootLog("Loading bans");
            ban.load_banlist();

            LogManager.BootLog("Loading reserved names");
            db.load_reserved();

            LogManager.BootLog("Loading corpses");
            save.load_corpses();

            LogManager.BootLog("Loading Immortal Hosts");
            // TODO load_imm_host();

            LogManager.BootLog("Loading projects");
            db.load_projects();

            LogManager.BootLog("Loading morphs");
            DoLuaScript(Program.GetAppSetting("MorphsFile"));

            // TODO MOBTrigger = true;

            // TODO init_chess();

        }

        #region Liquids
        public List<LiquidData> LIQUIDS;
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
        public List<SkillData> HERBS;
        public SkillData GetHerb(string name)
        {
            return HERBS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }
        public bool IsValidHerb(int sn)
        {
            return sn >= 0 && sn < HERBS.Count && HERBS[sn] != null && !HERBS[sn].Name.IsNullOrEmpty();
        }
        #endregion

        #region Skills

        public readonly List<SkillData> SKILLS;

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
                    LogManager.Bug("Skill entry {0} not found", name);
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
            SKILLS.Add(new SkillData(Program.MAX_CLASS, Program.MAX_RACE) { Name = name, ID = newId });
            return newId;
        }

        #endregion

        #region SpecFuns
        public List<SpecialFunction> SPEC_FUNS;

        public SpecialFunction GetSpecFun(string name)
        {
            return SPEC_FUNS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }
        #endregion
    }
}