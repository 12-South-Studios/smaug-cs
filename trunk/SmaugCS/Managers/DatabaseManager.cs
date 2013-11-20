﻿using System;
using System.Collections.Generic;
using Realm.Library.Common.Objects;
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

        public void Initialize(bool fCopyOver)
        {
            LogManager.BootLog("---------------------[ Boot Log ]--------------------");

            db.SystemData = new SystemData();

            LogManager.Log("Loading commands...");
            tables.load_commands();

            LogManager.Log("Loading spec funs...");
            special.load_specfuns();

            LogManager.Log("Loading sysdata configuration...");
            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadAllMail, Program.GetLevel("demi"));
            db.SystemData.PlayerPermissions.Add(PlayerPermissionTypes.ReadMailFree, Program.GetLevel("immortal"));
            // TODO Do the rest of the system data

            if (db.load_systemdata(db.SystemData))
            {
                LogManager.Log("Not found. Creating new configuration.");
                db.SystemData.alltimemax = 0;
                db.SystemData.MudTitle = "(Name not set)";
                act_wiz.update_timers();
                act_wiz.update_calendar();
                db.save_sysdata(db.SystemData);
            }

            LogManager.Log("Loading socials");
            tables.load_socials();

            LogManager.Log("Loading skill table");
            tables.load_skill_table();
            tables.remap_slot_numbers();

            //db.NumberSortedSkills = db.SKILLS.Count;

            LogManager.Log("Loading classes");
            tables.load_classes();

            LogManager.Log("Loading races");
            tables.load_races();

            LogManager.Log("Loading news data");
            news.load_news();

            LogManager.Log("Loading liquids");
            LiquidLoader lLoader = new LiquidLoader();
            _loaders.Add(lLoader);
            lLoader.Load();

            LogManager.Log("Loading mixtures");
            MixtureLoader mLoader = new MixtureLoader();
            _loaders.Add(mLoader);
            mLoader.Load();

            LogManager.Log("Loading herbs");
            tables.load_herb_table();

            LogManager.Log("Loading tongues");
            tables.load_tongues();

            LogManager.Log("Making wizlist");
            db.make_wizlist();

            // TODO Had auction stuff, not sure why it was needed

            // TODO Save equipment is inside each object now

            LogManager.Log("Setting time and weather.");
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

            LogManager.Log("Loading holiday chart...");
            HolidayLoader hLoader = new HolidayLoader();
            hLoader.Load();

            // TODO DNS Cache

            // TODO Assign GSNs
            LogManager.Log("Assigning GSN's");
            Macros.ASSIGN_GSN("evasive style");
            // TODO Assign remainder

            bool usePlanes = Program.GetBooleanConstant("UsePlanes");
            if (usePlanes)
            {
                LogManager.Log("Reading in plane file...");
                planes.load_planes();
            }

            AreaListLoader aLoader = new AreaListLoader();
            _loaders.Add(aLoader);
            aLoader.Load();

            if (usePlanes)
            {
                LogManager.Log("Making sure rooms are planed...");
                //planes.check_planes();
            }

            // TODO init_supermob();

            LogManager.Log("Fixing exits");
            db.FixExits();

            LogManager.Log("Initializing economy");
            db.initialize_economy();

            if (fCopyOver)
            {
                LogManager.Log("Loading world state...");
                hotboot.load_world();
            }

            LogManager.Log("Resetting areas");
            db.area_update();

            LogManager.Log("Loading buildlist");
            db.load_buildlist();

            LogManager.Log("Loading boards");
            boards.load_boards();

            LogManager.Log("Loading clans");
            ClanLoader clLoader = new ClanLoader();
            _loaders.Add(clLoader);
            clLoader.Load();

            LogManager.Log("Loading Councils");
            CouncilLoader coLoader = new CouncilLoader();
            _loaders.Add(coLoader);
            coLoader.Load();

            LogManager.Log("Loading deities");
            DeityListLoader dLoader = new DeityListLoader();
            _loaders.Add(dLoader);
            dLoader.Load();

            LogManager.Log("Loading watches");
            WatchListLoader wLoader = new WatchListLoader();
            _loaders.Add(wLoader);
            wLoader.Load();

            LogManager.Log("Loading bans");
            ban.load_banlist();

            LogManager.Log("Loading reserved names");
            db.load_reserved();

            LogManager.Log("Loading corpses");
            save.load_corpses();

            LogManager.Log("Loading Immortal Hosts");
            // TODO load_imm_host();

            LogManager.Log("Loading projects");
            db.load_projects();

            LogManager.Log("Loading morphs");
            // TODO load_morphs();

            // TODO MOBTrigger = true;

            // TODO init_chess();

        }
    }
}