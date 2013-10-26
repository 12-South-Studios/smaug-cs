using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Exceptions;
using SmaugCS.Language;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Organizations;

namespace SmaugCS
{
    public static class db
    {
        public static List<string> ReservedNames = new List<string>();

        public static RoomTemplate[] ROOM_INDEX_HASH = new RoomTemplate[Program.MAX_KEY_HASH];
        public static MobTemplate[] MOB_INDEX_HASH = new MobTemplate[Program.MAX_KEY_HASH];
        public static ObjectTemplate[] OBJECT_INDEX_HASH = new ObjectTemplate[Program.MAX_KEY_HASH];
        public static RoomTemplate[] VROOM_HASH = new RoomTemplate[64];
        public static CommandData[] COMMAND_HASH = new CommandData[126];

        public static List<CommandData> COMMANDS = new List<CommandData>();
        public static CommandData GetCommand(string command)
        {
            return COMMANDS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(command));
        }

        public static List<SocialData> SOCIALS = new List<SocialData>();
        public static SocialData GetSocial(string command)
        {
            return SOCIALS.FirstOrDefault(x => x.Name.EqualsIgnoreCase(command));
        }

        #region Objects
        public static int NumberOfObjectsLoaded { get; set; }
        public static int PhysicalObjects { get; set; }
        public static int CurrentObject { get; set; }
        public static bool CurrentObjectExtracted { get; set; }
        public static ReturnTypes GlobalObjectCode { get; set; }
        public static ObjectInstance SupermobObject { get; set; }
        #endregion

        #region Mobiles/Characters
        public static int NumberOfMobsLoaded { get; set; }
        public static CharacterInstance Supermob { get; set; }
        public static CharacterInstance CurrentChar { get; set; }
        public static bool CurrentCharDied { get; set; }
        #endregion

        #region Languages
        public static List<LanguageData> LANGUAGES = new List<LanguageData>();
        public static LanguageData GetLanguage(string name)
        {
            return LANGUAGES.FirstOrDefault(x => x.Name.Equals(name));
        }
        public static int GetLanguageCount(int languages)
        {
            return GameConstants.LanguageTable.Keys.ToList()
                     .Where(x => x != (int)LanguageTypes.Clan
                         && x != (int)LanguageTypes.Unknown)
                     .Select(x => (languages & x) > 0).Count();
        }
        #endregion

        #region Areas
        public static List<AreaData> AREAS = new List<AreaData>();
        public static List<AreaData> BUILD_AREAS = new List<AreaData>();
        public static int TopArea { get; set; }
        #endregion

        #region Helps
        public static List<HelpData> HELPS = new List<HelpData>();
        public static string HelpGreeting { get; set; }
        #endregion

        #region Organizations
        public static List<ClanData> CLANS = new List<ClanData>();
        public static ClanData GetClan(string name)
        {
            return CLANS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static List<CouncilData> COUNCILS = new List<CouncilData>();
        public static CouncilData GetCouncil(string name)
        {
            return COUNCILS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Skills

        public static readonly List<SkillData> SKILLS = new List<SkillData>();
        public static SkillData GetSkill(string name)
        {
            return SKILLS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static SkillData GetSkill(int skillNumber)
        {
            return SKILLS.FirstOrDefault(x => x.ID == skillNumber);
        }

        public static IEnumerable<SkillData> GetSkills(SkillTypes type)
        {
            return SKILLS.Where(x => x.Type == type);
        }

        /// <summary>
        /// Lookup a skill by name (or partial)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int LookupSkill(string name)
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

        public static int AddSkill(string name)
        {
            if (LookupSkill(name) > -1)
                return -1;

            int newId = SKILLS.Max(x => x.ID) + 1;
            SKILLS.Add(new SkillData() { Name = name, ID = newId });
            return newId;
        }

        #endregion

        #region Races
        public static List<RaceData> RACES = new List<RaceData>();
        public static RaceData GetRace(RaceTypes type)
        {
            return RACES.FirstOrDefault(x => x.Type == type);
        }
        public static RaceData GetRace(string name)
        {
            return RACES.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static RaceData GetRace(int id)
        {
            return RACES.FirstOrDefault(x => (int) x.Type == id);
        }
        #endregion

        #region Classes

        public static List<ClassData> CLASSES = new List<ClassData>();
        public static ClassData GetClass(ClassTypes type)
        {
            return CLASSES.FirstOrDefault(x => x.Type == type);
        }
        public static ClassData GetClass(string name)
        {
            return CLASSES.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static ClassData GetClass(int id)
        {
            return CLASSES.FirstOrDefault(x => (int) x.Type == id);
        }
        #endregion

        #region Bans
        public static List<BanData> BANS = new List<BanData>();
        #endregion

        #region Boards
        public static List<BoardData> BOARDS = new List<BoardData>();
        public static BoardData GetBoard(ObjectInstance obj)
        {
            return BOARDS.FirstOrDefault(x => x.BoardObjectVnum == obj.ObjectIndex.Vnum);
        }
        public static BoardData FindBoard(CharacterInstance ch)
        {
            return ch.CurrentRoom.Contents.Select(GetBoard).FirstOrDefault(board => board != null);
        }
        #endregion

        public static List<ExtraDescriptionData> ExtraDescriptions = new List<ExtraDescriptionData>();

        public static List<WizardData> WIZARDS = new List<WizardData>();
        public static List<LoginMessageData> LOGIN_MESSAGES = new List<LoginMessageData>();
        public static List<ShopData> SHOP = new List<ShopData>();
        public static List<RepairShopData> REPAIR = new List<RepairShopData>();
        public static List<TeleportData> TELEPORT = new List<TeleportData>();
        public static List<ProjectData> PROJECTS = new List<ProjectData>();
        public static List<ObjectInstance> ExtractedObjectQueue = new List<ObjectInstance>();
        public static List<ExtracedCharacterData> ExtractedCharQueue = new List<ExtracedCharacterData>();

        public static List<AuctionData> AUCTIONS = new List<AuctionData>();

        public static List<map_index_data> MAP = new List<map_index_data>();



        public static List<RelationData> RELATIONS = new List<RelationData>();
        public static List<HolidayData> HOLIDAYS = new List<HolidayData>();

        public static List<DeityData> DEITIES = new List<DeityData>();
        public static DeityData GetDeity(string name)
        {
            return DEITIES.Single(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }


        public static List<hint_data> HINT = new List<hint_data>();
        public static List<MixtureData> MIXTURES = new List<MixtureData>();
        public static List<LiquidData> LIQUIDS = new List<LiquidData>();
        public static List<DescriptorData> DESCRIPTORS = new List<DescriptorData>();
        public static List<SpecialFunction> SPEC_LIST = new List<SpecialFunction>();

        //public static List<RoomTemplate> ROOMS = new List<RoomTemplate>();
        //public static RoomTemplate get_room_index(int vnum)
        //{
        //    return ROOMS.FirstOrDefault(x => x.Vnum == vnum);
        //}


        public static List<WatchData> WATCHES = new List<WatchData>();

        public static Dictionary<string, Dictionary<string, string>> TITLES = new Dictionary<string, Dictionary<string, string>>();

        public static List<SkillData> HERBS = new List<SkillData>();
        public static SkillData GetHerb(string name)
        {
            return HERBS.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static List<SkillData> DISEASES = new List<SkillData>();
        public static SkillData GetDisease(string name)
        {
            return DISEASES.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static SystemData SystemData;
        public static TimeInfoData GameTime; // current game time

        public static int LastPKRoom { get; set; }


        public static void shutdown_mud(string reason)
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Shutdown);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path, true)))
            {
                proxy.Write("{0}\n", reason);
            }
        }

        public static void FixExits()
        {
            foreach (RoomTemplate room in DatabaseManager.Instance.ROOMS.Values)
            {
                bool fexit = false;
                foreach (ExitData exit in room.Exits)
                {
                    exit.Room_vnum = room.Vnum;
                    exit.Destination = DatabaseManager.Instance.ROOMS.Get(exit.vnum);
                    if (exit.vnum <= 0 || exit.Destination == null)
                    {
                        if (DatabaseManager.BootDb)
                        {
                            // TODO boot_log error
                        }

                        LogManager.Bug("Deleting %s exit in room %d", GameConstants.dir_name[exit.vdir], room.Vnum);
                        handler.extract_exit(room, exit);
                    }
                    else
                        fexit = true;
                }

                if (!fexit)
                    room.Flags.SetBit((int)RoomFlags.NoMob);
            }

            foreach (RoomTemplate room in DatabaseManager.Instance.ROOMS.Values)
            {
                foreach (ExitData exit in room.Exits)
                {
                    if (exit.Destination != null && exit.ReverseExit == null)
                    {
                        ExitData reverseExit = exit.Destination.GetExitTo(GameConstants.rev_dir[exit.vdir], room.Vnum);
                        if (reverseExit != null)
                        {
                            exit.ReverseExit = reverseExit;
                            reverseExit.ReverseExit = exit;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a help page to the list if it is not a duplicate of an existing page. 
        /// Page is insert-sorted by keyword. (The reason for sorting is to keep do_hlist looking nice)
        /// </summary>
        /// <param name="pHelp"></param>
        public static void add_help(HelpData newHelp)
        {
            if (HELPS.Any(x => x.Level == newHelp.Level && x.Keyword.Equals(newHelp.Keyword)))
            {
                LogManager.Bug("Duplicate Help {0}", newHelp.Keyword);
                return;
            }

            // TODO At some point do some sorting based on keyword

            HELPS.Add(newHelp);
        }

        public static void load_helps(TextReaderProxy proxy)
        {
            do
            {
                HelpData help = new HelpData
                    {
                        Level = proxy.ReadNumber(),
                        Keyword = proxy.ReadToEndOfLine()
                    };

                if (help.Keyword.StartsWith("#$"))
                    break;

                help.Text = proxy.ReadString(new[] { '~' });

                if (help.Keyword.EqualsIgnoreCase("greeting"))
                    HelpGreeting = help.Text;

                add_help(help);

            } while (!proxy.EndOfStream);
        }

        public static void load_mobiles(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_objects(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_resets(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_smaugwiz_reset(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_room_reset(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_rooms(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_shops(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_repairs(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_specials(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_ranges(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_climate(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void load_neighbor(AreaData tarea, FileStream fs)
        {
            // TODO
        }

        public static void initialize_economy()
        {
            foreach (AreaData area in AREAS)
            {
                if (area.HighEconomy > 0 || area.LowMobNumber > 10000)
                    continue;

                int range = area.HighSoftRange - area.LowSoftRange;
                range = (range > 0) ? range / 2 : 25;
                int gold = range * range * 50000;

                area.BoostEconomy(gold);

                for (int x = area.LowMobNumber; x < area.HighMobNumber; x++)
                {
                    MobTemplate mob = DatabaseManager.Instance.MOBILE_INDEXES.Get(x);
                    if (mob != null)
                        area.BoostEconomy(mob.Gold * 10);
                }
            }
        }

        public static ExitData get_exit_number(RoomTemplate room, int xit)
        {
            int count = 0;
            return room.Exits.FirstOrDefault(exit => ++count == xit);
        }

        public static void sort_exits(RoomTemplate room)
        {
            // TODO
        }

        public static void randomize_exits(RoomTemplate room, short maxdir)
        {
            // TODO
        }

        public static void area_update()
        {
            foreach (AreaData area in AREAS)
            {
                int resetAge = area.ResetFrequency > 0 ? area.ResetFrequency : 15;
                if ((resetAge == -1 && area.Age == -1)
                    || (++area.Age < (resetAge - 1)))
                    continue;

                //// Check players
                if (area.NumberOfPlayers > 0 && area.Age == (resetAge - 1))
                {
                    string buffer = !string.IsNullOrEmpty(area.ResetMessage)
                                        ? area.ResetMessage + "\r\n"
                                        : "You hear some squeaking sounds...\r\n";

                    foreach (CharacterInstance pch in DatabaseManager.Instance.CHARACTERS.Values
                        .Where(pch => !pch.IsNpc()
                            && pch.IsAwake()
                            && pch.CurrentRoom != null
                            && pch.CurrentRoom.Area == area))
                    {
                        color.set_char_color(ATTypes.AT_RESET, pch);
                        color.send_to_char(buffer, pch);
                    }
                }

                //// Check age and reset
                if (area.NumberOfPlayers == 0 || area.Age >= resetAge)
                {
                    reset.reset_area(area);
                    area.Age = (resetAge == -1) ? -1 : SmaugRandom.Between(0, resetAge / 5);

                    //// Mud Academy resets every 3 minutes
                    RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(Program.ROOM_VNUM_SCHOOL);
                    if (room != null && room.Area == area && area.ResetFrequency == 0)
                        area.Age = 15 - 3;
                }
            }
        }

        public static void clear_char(CharacterInstance ch)
        {
            ch.CurrentEditor = null;
            ch.CurrentHunting = null;
            ch.CurrentFearing = null;
            ch.CurrentHating = null;
            ch.Name = string.Empty;
            ch.ShortDescription = string.Empty;
            ch.LongDescription = string.Empty;
            ch.Description = string.Empty;
            ch.ReplyTo = null;
            ch.RetellTo = null;
            ch.Variables.Clear();
            ch.Carrying.Clear();
            ch.CurrentFighting = null;
            ch.Switched = null;
            ch.Affects.Clear();
            ch.DestinationBuffer = null;
            ch.CurrentMount = null;
            ch.CurrentMorph = null;
            ch.AffectedBy.ClearBits();
            ch.logon = DateTime.Now;
            ch.ArmorClass = 100;
            ch.Position = PositionTypes.Standing;
            ch.Practice = 0;
            ch.CurrentHealth = ch.MaximumHealth = 20;
            ch.CurrentMana = ch.MaximumMana = 100;
            ch.CurrentMovement = ch.MaximumMovement = 100;
            ch.Height = 72;
            ch.Weight = 180;
            ch.ExtraFlags = 0;
            ch.CurrentRace = RaceTypes.Human;
            ch.CurrentClass = ClassTypes.None;
            ch.Speaking = (int)LanguageTypes.Common;
            ch.Speaks = (int)LanguageTypes.Common;
            ch.BareDice = new DiceData() { NumberOf = 1, SizeOf = 4 };
            ch.SubState = (int)CharacterSubStates.None;
            ch.tempnum = 0;
            ch.PermanentStrength = 13;
            ch.PermanentDexterity = 13;
            ch.PermanentIntelligence = 13;
            ch.PermanentWisdom = 13;
            ch.PermanentConstitution = 13;
            ch.PermanentCharisma = 13;
            ch.PermanentLuck = 13;
            ch.ModStrength = 0;
            ch.ModDexterity = 0;
            ch.ModIntelligence = 0;
            ch.ModWisdom = 0;
            ch.ModConstitution = 0;
            ch.ModCharisma = 0;
            ch.ModLuck = 0;
        }

        public static void free_char(CharacterInstance ch)
        {
            ch.CurrentMorph = null;

            ObjectInstance obj;
            while ((obj = ch.Carrying.First()) != null)
                handler.extract_obj(obj);

            AffectData af;
            while ((af = ch.Affects.First()) != null)
                ch.RemoveAffect(af);

            TimerData timer;
            while ((timer = ch.Timers.First()) != null)
                handler.extract_timer(ch, timer);

            if (ch.CurrentEditor != null)
                build.stop_editing(ch);

            ch.StopHunting();
            ch.StopHating();
            ch.StopFearing();
            fight.free_fight(ch);

            NoteData note;
            while ((note = ch.NoteList.First()) != null)
                boards.free_note(note);

            foreach (VariableData vd in ch.Variables)
                variables.delete_variable(vd);

            if (ch.PlayerData != null)
            {
                if (ch.PlayerData.Pet != null)
                {
                    handler.extract_char(ch.PlayerData.Pet, true);
                    ch.PlayerData.Pet = null;
                }

                ch.PlayerData.Ignored.Clear();
                ch.PlayerData.TellHistory.Clear();

                // TODO IMC

                ch.PlayerData = null;
            }

            ch.Comments.Clear();
            DatabaseManager.Instance.CHARACTERS.Delete(ch.ID);
        }

        public static string get_extra_descr(string name, IEnumerable<ExtraDescriptionData> extraDescriptions)
        {
            foreach (ExtraDescriptionData ed in extraDescriptions.Where(ed => ed.Keyword.IsEqual(name)))
                return ed.Description;
            return string.Empty;
        }

        public static bool is_valid_filename(CharacterInstance ch, string direct, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || filename.Length < 3)
            {
                if (string.IsNullOrWhiteSpace(filename))
                    color.send_to_char("Empty filename is not valid\r\n", ch);
                else
                    color.ch_printf(ch, "Filename {0} is too short\r\n", filename);
                return false;
            }

            if (filename.StartsWith("..") || filename.StartsWith("/") || filename.StartsWith("\\"))
            {
                color.send_to_char("A filename may not start with a '..', '/', or '\\'.\r\n", ch);
                return false;
            }

            // TODO filename in use?


            return true;
        }

        public static int number_door()
        {
            int door;

            while ((door = SmaugRandom.RandomMM() % (16 - 1)) > 9)
            {
                // do nothing
            }

            return door;
        }

        public static void append_file(CharacterInstance ch, string file, string str)
        {
            if (ch.IsNpc() || string.IsNullOrEmpty(str))
                return;

            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(file, true)))
            {
                proxy.Write("[{0}] {1}: {2}\n", ch.CurrentRoom != null ? ch.CurrentRoom.Vnum : 0, ch.Name, str);
            }
        }

        public static void append_to_file(string file, string str)
        {
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(file, true)))
            {
                proxy.Write("{0}\n", str);
            }
        }

        /// <summary>
        /// Dumps the contents of a file to a player, one line at a time
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="filename"></param>
        public static void show_file(CharacterInstance ch, string filename)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(filename)))
            {
                List<string> lines = proxy.ReadIntoList();
                foreach (string line in lines)
                {
                    color.send_to_pager_color(line.Trim(new[] { '\r', '\n' }), ch);
                }
            }
        }

        public static void towizfile(string line)
        {
            string outline = string.Empty;

            if (!string.IsNullOrEmpty(line))
            {
                int filler = (78 - line.Length);
                if (filler < 1)
                    filler = 1;
                filler /= 2;
                for (int x = 0; x < filler; x++)
                    outline += " ";
                outline += line;
            }

            outline += "\r\n";
            using (
                TextWriterProxy proxy =
                    new TextWriterProxy(new StreamWriter(SystemConstants.GetSystemFile(SystemFileTypes.Wizards), true)))
            {
                proxy.Write(outline);
            }
        }

        public static void make_wizlist()
        {
            List<WizardData> wizList = new List<WizardData>();
            string directory = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.God);
            IEnumerable<string> files = new DirectoryProxy().GetFiles(directory);
            foreach (string file in files.Where(x => !x.StartsWithIgnoreCase(".")))
            {
                using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(file)))
                {
                    WizardData wizard = new WizardData { Name = file };
                    wizard.Level = wizard.Load(proxy.ReadIntoList());
                    wizList.Add(wizard);
                }
            }

            string buffer = string.Format(" Masters of the {0}!", SystemData.MudTitle);

            int iLevel = 65535;
            foreach (WizardData wiz in wizList)
            {
                if (wiz.Level < iLevel)
                {
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        towizfile(buffer);
                        buffer = string.Empty;
                    }
                    towizfile(string.Empty);
                    iLevel = wiz.Level;
                    if (iLevel == Program.MAX_LEVEL)
                        towizfile(" Supreme Entity");
                    else if (iLevel == Program.MAX_LEVEL - 1)
                        towizfile(" Infinite");
                    else if (iLevel == Program.MAX_LEVEL - 2)
                        towizfile(" Eternal");
                    else if (iLevel == Program.MAX_LEVEL - 3)
                        towizfile(" Ancient");
                    else if (iLevel == Program.MAX_LEVEL - 4)
                        towizfile(" Exalted Gods");
                    else if (iLevel == Program.MAX_LEVEL - 5)
                        towizfile(" Ascendant Gods");
                    else if (iLevel == Program.MAX_LEVEL - 6)
                        towizfile(" Greater Gods");
                    else if (iLevel == Program.MAX_LEVEL - 7)
                        towizfile(" Gods");
                    else if (iLevel == Program.MAX_LEVEL - 8)
                        towizfile(" Lesser Gods");
                    else if (iLevel == Program.MAX_LEVEL - 9)
                        towizfile(" Immortals");
                    else if (iLevel == Program.MAX_LEVEL - 10)
                        towizfile(" Demi Gods");
                    else if (iLevel == Program.MAX_LEVEL - 11)
                        towizfile(" Saviors");
                    else if (iLevel == Program.MAX_LEVEL - 12)
                        towizfile(" Creators");
                    else if (iLevel == Program.MAX_LEVEL - 13)
                        towizfile(" Acolytes");
                    else if (iLevel == Program.MAX_LEVEL - 14)
                        towizfile(" Neophytes");
                    else if (iLevel == Program.MAX_LEVEL - 15)
                        towizfile(" Retired");
                    else if (iLevel == Program.MAX_LEVEL - 16)
                        towizfile(" Guests");
                    else
                        towizfile(" Servants");
                }

                if (buffer.Length + wiz.Name.Length > 76)
                {
                    towizfile(buffer);
                    buffer = string.Empty;
                }
                buffer += " " + wiz.Name;
                if (buffer.Length > 70)
                {
                    towizfile(buffer);
                    buffer = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(buffer))
                towizfile(buffer);
        }

        public static int mprog_name_to_type(string name)
        {
            foreach (MudProgTypes type in Enum.GetValues(typeof(MudProgTypes)).Cast<MudProgTypes>().Where(type => type.GetName().EqualsIgnoreCase(name)))
                return (int)type;
            return (int)MudProgTypes.Error;
        }

        public static void mudprog_file_read(Template index, string filename)
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Prog) + filename;

            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                do
                {
                    string line = proxy.ReadLine();
                    if (line.StartsWith("|"))
                        break;
                    if (line.StartsWith(">"))
                        continue;

                    MudProgTypes type = (MudProgTypes)EnumerationFunctions.GetEnumByName<MudProgTypes>(proxy.ReadNextWord());
                    if (type == MudProgTypes.Error
                        || type == MudProgTypes.InFile)
                    {
                        LogManager.Bug("Invalid mud prog type {0} in file {1}", type, path);
                        continue;
                    }

                    MudProgData prog = new MudProgData
                                           {
                                               Type = type,
                                               arglist = proxy.ReadString(),
                                               comlist = proxy.ReadString(),
                                               IsFileProg = true
                                           };

                    index.ProgTypes.SetBit((int)prog.Type);
                    index.MudProgs.Add(prog);
                    break;

                } while (!proxy.EndOfStream);
            }
        }

        public static void mudprog_read_programs(TextReaderProxy proxy, Template index)
        {
            for (; ; )
            {
                char letter = proxy.ReadNextLetter();
                if (letter == '|')
                    return;
                if (letter != '>')
                {
                    LogManager.Bug("Vnum {0} MudProg Char", index.Vnum);
                    throw new Exception();
                }

                MudProgData prog = new MudProgData();
                index.MudProgs.Add(prog);

                MudProgTypes type = (MudProgTypes)EnumerationFunctions.GetEnumByName<MudProgTypes>(proxy.ReadNextWord());
                if (type == MudProgTypes.Error)
                {
                    LogManager.Bug("Invalid mud prog type {0} for Index {1}", type, index.Vnum);
                    throw new Exception();
                }
                prog.Type = type;

                if (type == MudProgTypes.InFile)
                {
                    prog.IsFileProg = false;
                    prog.arglist = proxy.ReadString();
                    mudprog_file_read(index, prog.arglist);
                }
                else
                {
                    index.ProgTypes.SetBit((int)prog.Type);
                    prog.IsFileProg = false;
                    prog.arglist = proxy.ReadString();
                    prog.comlist = proxy.ReadString();
                }
            }
        }

        public static void delete_room(RoomTemplate room)
        {
            RoomTemplate limbo = DatabaseManager.Instance.ROOMS.Get(Program.ROOM_VNUM_LIMBO);

            CharacterInstance ch;
            while ((ch = room.Persons.FirstOrDefault()) != null)
            {
                if (!ch.IsNpc())
                {
                    room.FromRoom(ch);
                    limbo.ToRoom(ch);
                }
                else
                    handler.extract_char(ch, true);
            }

            foreach (CharacterInstance och in DatabaseManager.Instance.CHARACTERS.Values)
            {
                if (och.PreviousRoom == room)
                    och.PreviousRoom = och.CurrentRoom;
                if (och.SubState == CharacterSubStates.RoomDescription
                    && och.DestinationBuffer == room)
                {
                    color.send_to_char("The room is no more.\r\n", och);
                    build.stop_editing(och);
                    och.SubState = CharacterSubStates.None;
                    och.DestinationBuffer = null;
                }
                else if (och.SubState == CharacterSubStates.RoomExtra
                         && och.DestinationBuffer != null)
                {
                    if (room.ExtraDescriptions.Any(e => e == och.DestinationBuffer))
                    {
                        color.send_to_char("The room is no more.\r\n", och);
                        build.stop_editing(och);
                        och.SubState = CharacterSubStates.None;
                        och.DestinationBuffer = null;
                    }
                }
            }

            room.Contents.ForEach(handler.extract_obj);
            reset.wipe_resets(room);
            room.ExtraDescriptions.Clear();
            room.Affects.Clear();
            room.PermanentAffects.Clear();
            room.Exits.ForEach(x => handler.extract_exit(room, x));
            room.MudProgActs.Clear();
            room.MudProgs.Clear();
            DatabaseManager.Instance.ROOMS.Delete(room.Vnum);

            // TODO: Room hash stuff here, but can be removed?
        }

        public static void delete_obj(ObjectTemplate obj)
        {
            DatabaseManager.Instance.OBJECTS.Values.Where(x => x.ObjectIndex == obj).ToList().ForEach(handler.extract_obj);
            obj.ExtraDescriptions.Clear();
            obj.Affects.Clear();
            obj.MudProgs.Clear();
            DatabaseManager.Instance.OBJECT_INDEXES.Delete(obj.Vnum);

            // TODO Object hash stuff here, but can be removed?
        }

        public static void delete_mob(MobTemplate mob)
        {
            foreach (CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.Values)
            {
                if (ch.MobIndex == mob)
                    handler.extract_char(ch, true);
                else if (ch.SubState == CharacterSubStates.MProgEditing
                         && ch.DestinationBuffer != null)
                {
                    if (mob.MudProgs.Any(mp => mp == ch.DestinationBuffer))
                    {
                        color.send_to_char("Your victim has departed.\r\n", ch);
                        build.stop_editing(ch);
                        ch.DestinationBuffer = null;
                        ch.SubState = CharacterSubStates.MProgEditing;
                    }
                }
            }

            mob.MudProgs.Clear();
            if (mob.Shop != null)
                SHOP.Remove(mob.Shop);
            if (mob.RepairShop != null)
                REPAIR.Remove(mob.RepairShop);
            DatabaseManager.Instance.MOBILE_INDEXES.Delete(mob.Vnum);

            // TODO Mob hash stuff here, but can be removed?
        }

        public static ExitData make_exit(RoomTemplate room, RoomTemplate to_room, int door)
        {
            ExitData newExit = new ExitData
                {
                    vdir = door,
                    Room_vnum = room.Vnum,
                    Destination = to_room,
                    Distance = 1,
                    Key = -1
                };

            ExitData reverseExit = null;
            if (to_room != null)
            {
                newExit.vnum = to_room.Vnum;
                reverseExit = to_room.GetExitTo(GameConstants.rev_dir[door], room.Vnum);
                if (reverseExit != null)
                {
                    reverseExit.ReverseExit = newExit;
                    newExit.ReverseExit = reverseExit;
                }
            }

            bool broke = room.Exits.Any(exit => door < exit.vdir);
            if (room.Exits == null)
                room.Exits.Add(newExit);
            else
            {
                if (broke && reverseExit != null)
                {
                    room.Exits.Insert(room.Exits.First() == reverseExit ? 0 : 1, newExit);
                    return newExit;
                }
            }

            return newExit;
        }

        public static void fix_area_exits(AreaData area)
        {
            for (int rnum = area.LowRoomNumber; rnum <= area.HighRoomNumber; rnum++)
            {
                RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(rnum);
                if (room == null)
                    continue;

                bool fexit = false;
                foreach (ExitData exit in room.Exits)
                {
                    fexit = true;
                    exit.Room_vnum = room.Vnum;
                    exit.Destination = exit.vnum <= 0 ? null : DatabaseManager.Instance.ROOMS.Get(exit.vnum);
                }
                if (!fexit)
                    room.Flags.SetBit((int)RoomFlags.NoMob);
            }

            for (int rnum = area.LowRoomNumber; rnum <= area.HighRoomNumber; rnum++)
            {
                RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(rnum);
                if (room == null)
                    continue;

                foreach (ExitData exit in room.Exits)
                {
                    if (exit.Destination != null && exit.ReverseExit == null)
                    {
                        ExitData reverseExit = exit.Destination.GetExitTo(GameConstants.rev_dir[exit.vdir], room.Vnum);
                        if (reverseExit != null)
                        {
                            exit.ReverseExit = reverseExit;
                            reverseExit.ReverseExit = exit;
                        }
                    }
                }
            }
        }

        public static void process_sorting(AreaData area)
        {
            if (DatabaseManager.BootDb)
            {
                sort_area_by_name(area);
                sort_area(area, false);
            }

            string buffer = string.Format("{0}:\n\tRooms: {1} - {2}\n\tObjects: {3} - {4}\n\tMobs: {5} - {6}\n",
                                          area.Filename, area.LowRoomNumber, area.HighRoomNumber, area.LowObjectNumber,
                                          area.HighObjectNumber, area.LowMobNumber, area.HighMobNumber);
            LogManager.BootLog(buffer);

            area.status.SetBit((int)AreaFlags.Loaded);
        }

        public static ExtraDescriptionData fread_fuss_exdesc(FileStream fs)
        {
            // TODO
            return null;
        }

        public static AffectData fread_fuss_affect(FileStream fs, string word)
        {
            // TODO
            return null;
        }

        public static void rprog_file_read(RoomTemplate prog_target, string f)
        {
            // TODO
        }

        public static void oprog_file_read(ObjectTemplate prog_target, string f)
        {
            // TODO
        }

        public static void fread_fuss_objprog(FileStream fs, MudProgData mprg, ObjectTemplate prog_target)
        {
            // TODO
        }

        public static void fread_fuss_object(FileStream fs, AreaData tarea)
        {
            // TODO
        }

        public static void mprog_file_read(MobTemplate prog_target, string f)
        {
            // TODO
        }

        public static void fread_fuss_mobprog(FileStream fs, MudProgData mprg, MobTemplate prog_target)
        {
            // TODO
        }

        public static void fread_fuss_mobile(FileStream fs, AreaData tarea)
        {
            // TODO
        }

        public static void load_reserved()
        {
            // TODO
        }

        public static void load_buildlist()
        {
            // TODO
        }

        public static void sort_reserved(List<string> reservedList)
        {
            // TODO   
        }

        public static void sort_area_by_name(AreaData pArea)
        {
            // TODO
        }

        public static void sort_area(AreaData pArea, bool proto)
        {
            // TODO
        }

        public static void show_vnums(CharacterInstance ch, int low, int high, bool proto, bool shown1, string loadst,
                                      string notloadst)
        {
            // TODO
        }

        public static void do_vnums(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_zones(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_newzones(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void save_sysdata(SystemData sys)
        {
            // TODO
        }

        public static void fread_sysdata(SystemData sys, FileStream fs)
        {
            // TODO
        }

        public static bool load_systemdata(SystemData sys)
        {
            // TODO
            return false;
        }

        public static void tail_chain()
        {
            // do nothing
        }

        public static void load_projects()
        {
            // TODO
        }

        public static ProjectData read_project(FileStream fs)
        {
            // TODO
            return null;
        }

        public static NoteData read_log(FileStream fs)
        {
            // TODO
            return null;
        }

        public static void write_projects()
        {
            // TODO
        }

        public static void fread_loginmsg(FileStream fs)
        {
            // TODO
        }

        public static void load_loginmsg()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.LoginMsg);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                // TODO
            }
        }

        public static void save_loginmsg()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.LoginMsg);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                foreach(LoginMessageData lmsg in LOGIN_MESSAGES)
                    lmsg.Save(proxy);

                proxy.Write("#END\n");
            }
        }

        public static void add_loginmsg(string name, int type, string argument)
        {
            if (type < 0 || name.IsNullOrEmpty())
            {
                LogManager.Bug("Bad name {0} or type {1} for Login Message", name, type);
                return;
            }

            LoginMessageData lmsg = new LoginMessageData
                {
                    Type = type,
                    Name = name
                };
            if (!argument.IsNullOrEmpty())
                lmsg.Text = argument;

            LOGIN_MESSAGES.Add(lmsg);
            save_loginmsg();
        }

        public static void check_loginmsg(CharacterInstance ch)
        {
            if (ch == null || ch.IsNpc())
                return;

            List<LoginMessageData> loginMessages = LOGIN_MESSAGES.Where(x => x.Name.EqualsIgnoreCase(ch.Name)).ToList();

            foreach (LoginMessageData loginMessage in LOGIN_MESSAGES)
            {
                if (loginMessage.Type > Program.MAX_MSG)
                {
                    LogManager.Bug("Unknown login message type {0} for {1}", loginMessage.Type, ch.Name);
                    continue;
                }

                switch (loginMessage.Type)
                {
                    case 0:
                        ImmortalMessage(ch, loginMessage);
                        break;
                    case 17:
                        DeathMessage(ch, loginMessage);
                        break;
                    case 18:
                        WorldChangeMessage(ch, loginMessage);
                        break;
                    default:
                        color.send_to_char_color(GameConstants.LoginMessages[loginMessage.Type], ch);
                        break;
                }

                save_loginmsg();
            }

            while (loginMessages.Count > 0)
                LOGIN_MESSAGES.Remove(loginMessages.First());
        }

        private static void ImmortalMessage(CharacterInstance ch, LoginMessageData lmsg)
        {
            if (!lmsg.Text.IsNullOrEmpty())
            {
                LogManager.Bug("NULL loginMessage text for type 0");
                return;
            }

            color.ch_printf(ch, "\r\n&YThe game administrators have left you the following message:\r\n%s\r\n", lmsg.Text);
        }
        private static void DeathMessage(CharacterInstance ch, LoginMessageData lmsg)
        {
            if (!lmsg.Text.IsNullOrEmpty())
            {
                LogManager.Bug("NULL loginMessage text for type 17");
                return;
            }

            color.ch_printf(ch, "\r\n&RYou were killed by %s while your character was link-dead.\r\n", lmsg.Text);
            color.send_to_char("You should look for your corpse immediately.\r\n", ch);
        }
        private static void WorldChangeMessage(CharacterInstance ch, LoginMessageData lmsg)
        {
            if (!lmsg.Text.IsNullOrEmpty())
            {
                LogManager.Bug("NULL loginMessage text for type 18");
                return;
            }

            color.ch_printf(ch, "\r\n&GA change in the Realms has affected you personally:\r\n%s\r\n", lmsg.Text);
        }
    }
}
