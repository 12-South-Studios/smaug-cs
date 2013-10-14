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
        public static List<ObjectTemplate> OBJECT_INDEX = new List<ObjectTemplate>();
        public static ObjectTemplate get_obj_index(int vnum)
        {
            return OBJECT_INDEX.FirstOrDefault(x => x.Vnum == vnum);
        }
        public static List<ObjectInstance> OBJECTS = new List<ObjectInstance>();
        public static int NumberOfObjectsLoaded { get; set; }
        public static int PhysicalObjects { get; set; }
        public static int CurrentObject { get; set; }
        public static bool CurrentObjectExtracted { get; set; }
        public static ReturnTypes GlobalObjectCode { get; set; }
        public static ObjectInstance SupermobObject { get; set; }
        #endregion

        #region Mobiles/Characters
        public static List<MobTemplate> MOBILE_INDEX = new List<MobTemplate>();
        public static MobTemplate get_mob_index(int vnum)
        {
            return MOBILE_INDEX.FirstOrDefault(x => x.Vnum == vnum);
        }
        public static List<CharacterInstance> CHARACTERS = new List<CharacterInstance>();
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
        public static List<lmsg_data> LMSG = new List<lmsg_data>();
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

        public static List<RoomTemplate> ROOMS = new List<RoomTemplate>();
        public static RoomTemplate get_room_index(int vnum)
        {
            return ROOMS.FirstOrDefault(x => x.Vnum == vnum);
        }


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
            foreach (RoomTemplate room in ROOMS)
            {
                bool fexit = false;
                foreach (ExitData exit in room.Exits)
                {
                    exit.Room_vnum = room.Vnum;
                    exit.Destination = get_room_index(exit.vnum);
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

            foreach (RoomTemplate room in ROOMS)
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

        public static void add_char(CharacterInstance ch)
        {
            CHARACTERS.Add(ch);
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
                    MobTemplate mob = get_mob_index(x);
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

                    foreach (CharacterInstance pch in CHARACTERS
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
                    RoomTemplate room = get_room_index(Program.ROOM_VNUM_SCHOOL);
                    if (room != null && room.Area == area && area.ResetFrequency == 0)
                        area.Age = 15 - 3;
                }
            }
        }

        public static CharacterInstance create_mobile(MobTemplate index)
        {
            CharacterInstance mob = new CharacterInstance
                {
                    Parent = index,
                    Name = index.Name,
                    ShortDescription = index.ShortDescription,
                    LongDescription = index.LongDescription,
                    Description = index.Description,
                    SpecialFunction = index.SpecialFunction,
                    Level = SmaugRandom.Fuzzy(index.Level),
                    Act = index.Act,
                    HomeVNum = -1,
                    ResetVnum = -1,
                    ResetNum = -1,
                    AffectedBy = index.AffectedBy,
                    CurrentAlignment = index.Alignment,
                    Gender = EnumerationExtensions.GetEnum<GenderTypes>(index.Gender)
                };

            if (!string.IsNullOrEmpty(index.spec_funname))
                mob.SpecialFunctionName = index.spec_funname;

            if (mob.Act.IsSet((int)ActFlags.MobInvisibility))
                mob.MobInvisible = mob.Level;

            mob.ArmorClass = index.ArmorClass > 0 ? index.ArmorClass : interpolate(mob.Level, 100, -100);

            if (index.HitDice == null || index.HitDice.NumberOf == 0)
                mob.MaximumHealth = mob.Level * 8 + SmaugRandom.Between(mob.Level * mob.Level / 4, mob.Level * mob.Level);
            else
                mob.MaximumHealth = index.HitDice.NumberOf * SmaugRandom.Between(1, index.HitDice.SizeOf) + index.HitDice.Bonus;

            mob.CurrentCoin = index.Gold;
            mob.Experience = index.Experience;
            mob.Position = index.Position;
            mob.DefPosition = index.DefPosition;
            mob.BareDice = new DiceData() { NumberOf = index.DamageDice.NumberOf, SizeOf = index.DamageDice.SizeOf };
            mob.ToHitArmorClass0 = index.ToHitArmorClass0;
            mob.HitRoll = new DiceData() { Bonus = index.HitDice.Bonus };
            mob.DamageRoll = new DiceData() { Bonus = index.DamageDice.Bonus };
            mob.PermanentStrength = index.PermanentStrength;
            mob.PermanentDexterity = index.PermanentDexterity;
            mob.PermanentWisdom = index.PermanentWisdom;
            mob.PermanentIntelligence = index.PermanentIntelligence;
            mob.PermanentConstitution = index.PermanentConstitution;
            mob.PermanentCharisma = index.PermanentCharisma;
            mob.PermanentLuck = index.PermanentLuck;
            mob.CurrentRace = EnumerationExtensions.GetEnum<RaceTypes>(index.Race);
            mob.CurrentClass = EnumerationExtensions.GetEnum<ClassTypes>(index.Class);
            mob.ExtraFlags = index.ExtraFlags;
            mob.SavingThrows = new SavingThrowData(index.SavingThrows);
            mob.Height = index.Height;
            mob.Weight = index.Weight;
            mob.Resistance = index.Resistance;
            mob.Immunity = index.Immunity;
            mob.Susceptibility = index.Susceptibility;
            mob.Attacks = index.Attacks;
            mob.Defenses = index.Defenses;
            mob.NumberOfAttacks = index.NumberOfAttacks;
            mob.Speaks = index.Speaks;
            mob.Speaking = index.Speaking;

            add_char(mob);

            return mob;
        }

        private static int CurrentObjectSerial { get; set; }

        public static ObjectInstance create_object(ObjectTemplate index, int level)
        {
            ObjectInstance obj = new ObjectInstance
                {
                    Parent = index,
                    Level = level,
                    WearLocation = WearLocations.None,
                    Count = 1
                };

            CurrentObjectSerial = Check.Maximum((CurrentObjectSerial + 1) & (Program.BV30 - 1), 1);
            obj.serial = obj.ObjectIndex.serial = CurrentObjectSerial;

            obj.Name = index.Name;
            obj.ShortDescription = index.ShortDescription;
            obj.Description = index.Description;
            obj.ActionDescription = index.ActionDescription;
            obj.ItemType = index.Type;
            obj.ExtraFlags = index.ExtraFlags;
            obj.WearFlags = index.WearFlags;

            Array.Copy(index.Value, obj.Value, 5);

            obj.Weight = index.Weight;
            obj.Cost = index.Cost;

            if (obj.ItemType == ItemTypes.Food || obj.ItemType == ItemTypes.Cook)
                obj.Timer = (obj.Value[4] > 0) ? obj.Value[4] : obj.Value[1];
            if (obj.ItemType == ItemTypes.Salve)
                obj.Value[3] = SmaugRandom.Fuzzy(obj.Value[3]);
            if (obj.ItemType == ItemTypes.Scroll)
                obj.Value[0] = SmaugRandom.Fuzzy(obj.Value[0]);
            if (obj.ItemType == ItemTypes.Wand || obj.ItemType == ItemTypes.Staff)
            {
                obj.Value[0] = SmaugRandom.Fuzzy(obj.Value[0]);
                obj.Value[1] = SmaugRandom.Fuzzy(obj.Value[1]);
                obj.Value[2] = obj.Value[1];
            }
            if (obj.ItemType == ItemTypes.Weapon || obj.ItemType == ItemTypes.MissileWeapon ||
                obj.ItemType == ItemTypes.Projectile)
            {
                if (obj.Value[1] > 0 && obj.Value[2] > 0)
                    obj.Value[2] = obj.Value[1];
                else
                {
                    obj.Value[1] = SmaugRandom.Fuzzy(1 * level / 4 + 2);
                    obj.Value[2] = SmaugRandom.Fuzzy(3 * level / 4 + 6);
                }
                if (obj.Value[0] == 0)
                    obj.Value[0] = Program.INIT_WEAPON_CONDITION;
            }
            if (obj.ItemType == ItemTypes.Armor)
            {
                if (obj.Value[0] == 0)
                    obj.Value[0] = SmaugRandom.Fuzzy(level / 4 + 2);
                if (obj.Value[1] == 0)
                    obj.Value[1] = obj.Value[0];
            }
            if (obj.ItemType == ItemTypes.Potion || obj.ItemType == ItemTypes.Pill)
                obj.Value[0] = SmaugRandom.Fuzzy(obj.Value[0]);
            if (obj.ItemType == ItemTypes.Money)
                obj.Value[0] = obj.Cost > 0 ? obj.Cost : 1;

            OBJECTS.Add(obj);

            return obj;
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
            CHARACTERS.Remove(ch);
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

        public static int interpolate(int level, int value_00, int value_32)
        {
            return value_00 + level * (value_32 - value_00) / 32;
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
            RoomTemplate limbo = get_room_index(Program.ROOM_VNUM_LIMBO);

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

            foreach (CharacterInstance och in CHARACTERS)
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
            ROOMS.Remove(room);

            // TODO: Room hash stuff here, but can be removed?
        }

        public static void delete_obj(ObjectTemplate obj)
        {
            OBJECTS.Where(x => x.ObjectIndex == obj).ToList().ForEach(handler.extract_obj);
            obj.ExtraDescriptions.Clear();
            obj.Affects.Clear();
            obj.MudProgs.Clear();
            OBJECT_INDEX.Remove(obj);

            // TODO Object hash stuff here, but can be removed?
        }

        public static void delete_mob(MobTemplate mob)
        {
            foreach (CharacterInstance ch in CHARACTERS)
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
            MOBILE_INDEX.Remove(mob);

            // TODO Mob hash stuff here, but can be removed?
        }

        public static RoomTemplate make_room(int vnum, AreaData area)
        {
            if (area == null || vnum < 1)
                throw new Exception("Invalid data");
            if (ROOMS.Any(x => x.Vnum == vnum))
                throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);

            RoomTemplate newRoom = new RoomTemplate
                {
                    Name = "Floating in a void",
                    SectorType = SectorTypes.Inside,
                    Area = area,
                    Vnum = vnum
                };
            newRoom.Flags.SetBit((int)RoomFlags.Prototype);

            ROOMS.Add(newRoom);
            area.Rooms.Add(newRoom);
            return newRoom;
        }

        public static ObjectTemplate make_object(int vnum, int cvnum, string name)
        {
            if (vnum < 1 || string.IsNullOrWhiteSpace(name))
                throw new Exception("Invalid data");
            if (OBJECT_INDEX.Any(x => x.Vnum == vnum))
                throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);

            ObjectTemplate cloneObject = null;
            if (cvnum > 0)
            {
                cloneObject = OBJECT_INDEX.FirstOrDefault(x => x.Vnum == cvnum);
            }

            ObjectTemplate newObject = new ObjectTemplate
                {
                    Name = string.Format("A newly created {0}", name),
                    Description = string.Format("Somebody dropped a newly created {0} here.", name),
                    Type = ItemTypes.Trash,
                    Weight = 1,
                    Vnum = vnum
                };
            newObject.ExtraFlags.SetBit((int)ItemExtraFlags.Prototype);

            if (cloneObject != null)
            {
                newObject.ShortDescription = cloneObject.ShortDescription;
                newObject.Description = cloneObject.Description;
                newObject.ActionDescription = cloneObject.ActionDescription;
                newObject.Type = cloneObject.Type;
                newObject.ExtraFlags = new ExtendedBitvector(cloneObject.ExtraFlags);
                newObject.WearFlags = cloneObject.WearFlags;
                Array.Copy(cloneObject.Value, newObject.Value, cloneObject.Value.Length);
                newObject.Weight = cloneObject.Weight;
                newObject.Cost = cloneObject.Cost;
                newObject.ExtraDescriptions = new List<ExtraDescriptionData>(cloneObject.ExtraDescriptions);
                newObject.Affects = new List<AffectData>(cloneObject.Affects);
            }

            OBJECT_INDEX.Add(newObject);
            return newObject;
        }

        public static MobTemplate make_mobile(int vnum, int cvnum, string name)
        {
            if (vnum < 1 || string.IsNullOrWhiteSpace(name))
                throw new Exception("Invalid data");
            if (MOBILE_INDEX.Any(x => x.Vnum == vnum))
                throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);

            MobTemplate cloneMob = null;
            if (cvnum > 1)
            {
                cloneMob = MOBILE_INDEX.FirstOrDefault(x => x.Vnum == cvnum);
            }

            MobTemplate newMob = new MobTemplate()
                {
                    Name = string.Format("A newly created {0}", name),
                    Vnum = vnum,
                    LongDescription = string.Format("Somebody abandoned a newly created {0} here.", name),
                    Level = 1,
                    Position = PositionTypes.Standing,
                    DefPosition = PositionTypes.Standing,
                    PermanentStrength = 13,
                    PermanentDexterity = 13,
                    PermanentIntelligence = 13,
                    PermanentWisdom = 13,
                    PermanentCharisma = 13,
                    PermanentConstitution = 13,
                    PermanentLuck = 13,
                    Class = 3
                };
            newMob.Act.SetBit((int)ActFlags.IsNpc);
            newMob.Act.SetBit((int)ActFlags.Prototype);

            if (cloneMob != null)
            {
                newMob.ShortDescription = cloneMob.ShortDescription;
                newMob.LongDescription = cloneMob.LongDescription;
                newMob.Description = cloneMob.Description;
                newMob.Act = cloneMob.Act;
                newMob.AffectedBy = cloneMob.AffectedBy;
                newMob.SpecialFunction = cloneMob.SpecialFunction;
                newMob.Alignment = cloneMob.Alignment;
                newMob.Level = cloneMob.Level;
                newMob.ToHitArmorClass0 = cloneMob.ToHitArmorClass0;
                newMob.ArmorClass = cloneMob.ArmorClass;
                newMob.HitDice = new DiceData(cloneMob.HitDice);
                newMob.DamageDice = new DiceData(cloneMob.DamageDice);
                newMob.Gold = cloneMob.Gold;
                newMob.Experience = cloneMob.Experience;
                newMob.Position = cloneMob.Position;
                newMob.DefPosition = cloneMob.DefPosition;
                newMob.Gender = cloneMob.Gender;
                newMob.PermanentStrength = cloneMob.PermanentStrength;
                newMob.PermanentDexterity = cloneMob.PermanentDexterity;
                newMob.PermanentIntelligence = cloneMob.PermanentIntelligence;
                newMob.PermanentWisdom = cloneMob.PermanentWisdom;
                newMob.PermanentCharisma = cloneMob.PermanentCharisma;
                newMob.PermanentConstitution = cloneMob.PermanentConstitution;
                newMob.PermanentLuck = cloneMob.PermanentLuck;
                newMob.Race = cloneMob.Race;
                newMob.Class = cloneMob.Class;
                newMob.ExtraFlags = cloneMob.ExtraFlags;
                newMob.Resistance = cloneMob.Resistance;
                newMob.Susceptibility = cloneMob.Susceptibility;
                newMob.Immunity = cloneMob.Immunity;
                newMob.NumberOfAttacks = cloneMob.NumberOfAttacks;
                newMob.Attacks = new ExtendedBitvector(cloneMob.Attacks);
                newMob.Defenses = new ExtendedBitvector(cloneMob.Defenses);
            }

            MOBILE_INDEX.Add(newMob);
            return newMob;
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
                RoomTemplate room = get_room_index(rnum);
                if (room == null)
                    continue;

                bool fexit = false;
                foreach (ExitData exit in room.Exits)
                {
                    fexit = true;
                    exit.Room_vnum = room.Vnum;
                    exit.Destination = exit.vnum <= 0 ? null : get_room_index(exit.vnum);
                }
                if (!fexit)
                    room.Flags.SetBit((int)RoomFlags.NoMob);
            }

            for (int rnum = area.LowRoomNumber; rnum <= area.HighRoomNumber; rnum++)
            {
                RoomTemplate room = get_room_index(rnum);
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

        public static void load_watchlist()
        {
            // TODO
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
            // TODO
        }

        public static void save_loginmsg()
        {
            // TODO
        }

        public static void add_loginmsg(string name, short type, string argument)
        {
            // TODO
        }

        public static void check_loginmsg(CharacterInstance ch)
        {
            // TODO
        }

    }
}
