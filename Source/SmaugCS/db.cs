using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Shops;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Repository;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS
{
    public static class db
    {
        public static List<MorphData> MORPHS = new List<MorphData>();
        public static MorphData GetMorph(string arg)
        {
            return string.IsNullOrWhiteSpace(arg) ? null : MORPHS.FirstOrDefault(morph => morph.name.EqualsIgnoreCase(arg));
        }
        public static MorphData GetMorph(int vnum)
        {
            return vnum < 1 ? null : MORPHS.FirstOrDefault(morph => morph.vnum == vnum);
        }
        public static MorphData FindMorph(CharacterInstance ch, string target, bool isCast)
        {
            return string.IsNullOrWhiteSpace(target) ? null : MORPHS.Where(morph => !morph.name.EqualsIgnoreCase(target)).FirstOrDefault(morph => ch.CanMorph(morph, isCast));
        }

        public static List<string> ReservedNames = new List<string>();
        public static bool IsReservedName(string name)
        {
            return ReservedNames.Any(reservedName => reservedName.EqualsIgnoreCase(name)
                || (reservedName.StartsWithIgnoreCase("*")
                && reservedName.ContainsIgnoreCase(name)));
        }

        #region Objects
        public static ObjectInstance SupermobObject { get; set; }
        #endregion

        #region Mobiles/Characters
        public static CharacterInstance Supermob { get; set; }
        #endregion

        #region Helps
        public static readonly List<HelpData> HELPS = new List<HelpData>();
        public static string HelpGreeting { get; set; }
        #endregion


        public static List<ExtraDescriptionData> ExtraDescriptions = new List<ExtraDescriptionData>();

        public static List<WizardData> WIZARDS = new List<WizardData>();
        public static List<LoginMessageData> LOGIN_MESSAGES = new List<LoginMessageData>();
        public static List<RepairShopData> REPAIR = new List<RepairShopData>();
        public static readonly List<TeleportData> TELEPORT = new List<TeleportData>();

        public static List<map_index_data> MAP = new List<map_index_data>();



        public static List<RelationData> RELATIONS = new List<RelationData>();

        public static List<HolidayData> HOLIDAYS = new List<HolidayData>();
        public static HolidayData GetHoliday(int month, int day)
        {
            return HOLIDAYS.FirstOrDefault(holiday => month + 1 == holiday.Month
                && day + 1 == holiday.Day);
        }


        public static List<HintData> HINTS = new List<HintData>();
        public static string GetHint(int level)
        {
            if (level < 0)
                return $"HintLevel error, level was {level}";

            var count = HINTS.Count(hint => level >= hint.Low && level <= hint.High);
            if (count > 1)
            {
                var which = SmaugRandom.Between(1, count);
                count = 0;
                foreach (var hint in HINTS)
                {
                    if (level >= hint.Low && level <= hint.High)
                        ++count;
                    if (count == which)
                        return hint.Text;
                }
            }
            else if (count == 1)
            {
                foreach (var hint in HINTS)
                {
                    if (level >= hint.Low && level <= hint.High)
                        return hint.Text;
                }
            }

            return " ";
        }

        public static readonly List<DescriptorData> DESCRIPTORS = new List<DescriptorData>();
        
        public static List<WatchData> WATCHES = new List<WatchData>();

        public static Dictionary<string, Dictionary<string, string>> TITLES = new Dictionary<string, Dictionary<string, string>>();

        public static List<SkillData> DISEASES = new List<SkillData>();

        public static SkillData GetDisease(string name)
        {
            return DISEASES.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        }

        public static int LastPKRoom { get; set; }


        public static void shutdown_mud(string reason)
        {
            var path = SystemConstants.GetSystemFile(SystemFileTypes.Shutdown);
            using (var proxy = new TextWriterProxy(new StreamWriter(path, true)))
            {
                proxy.Write("{0}\n", reason);
            }
        }

        public static void FixExits()
        {
            foreach (var room in RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values)
            {
                var fexit = false;
                foreach (var exit in room.Exits)
                {
                    exit.Room_vnum = room.ID;
                    exit.Destination = RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(exit.vnum).ID;
                    if (exit.vnum <= 0 || exit.Destination <= 0)
                    {
                        /*if (DatabaseManager.BootDb)
                        {
                            // TODO boot_log error
                        }*/

                        LogManager.Instance.Bug("Deleting %s exit in room %d",
                                                LookupManager.Instance.GetLookup("DirectionNames", (int) exit.Direction),
                                                room.ID);
                        exit.Extract();
                    }
                    else
                        fexit = true;
                }

                if (!fexit)
                    room.Flags.SetBit((int)RoomFlags.NoMob);
            }

            foreach (var room in RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values)
            {
                foreach (var exit in room.Exits)
                {
                    if (exit.Destination <= 0 && exit.GetReverse() == null)
                    {
                        var reverseExit = exit.GetDestination().GetExitTo(LookupConstants.rev_dir[(int)exit.Direction], room.ID);
                        if (reverseExit != null)
                        {
                            exit.Reverse = reverseExit.ID;
                            reverseExit.Reverse = exit.ID;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a help page to the list if it is not a duplicate of an existing page. 
        /// Page is insert-sorted by keyword. (The reason for sorting is to keep do_hlist looking nice)
        /// </summary>
        public static void add_help(HelpData newHelp)
        {
            if (HELPS.Any(x => x.Level == newHelp.Level && x.Keyword.Equals(newHelp.Keyword)))
            {
                LogManager.Instance.Bug("Duplicate Help {0}", newHelp.Keyword);
                return;
            }

            // TODO At some point do some sorting based on keyword

            HELPS.Add(newHelp);
        }

        public static void load_helps(TextReaderProxy proxy)
        {
            do
            {
                var help = new HelpData
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
            foreach (var area in RepositoryManager.Instance.AREAS.Values)
            {
                if (area.HighEconomy > 0 || area.LowMobNumber > 10000)
                    continue;

                var range = area.HighSoftRange - area.LowSoftRange;
                range = range > 0 ? range / 2 : 25;
                var gold = range * range * 50000;

                area.BoostEconomy(gold);

                for (var x = area.LowMobNumber; x < area.HighMobNumber; x++)
                {
                    var mob = RepositoryManager.Instance.MOBILETEMPLATES.CastAs<Repository<long, MobileTemplate>>().Get(x);
                    if (mob != null)
                        area.BoostEconomy(mob.GetStatistic<int>(StatisticTypes.Coin) * 10);
                }
            }
        }

        public static ExitData get_exit_number(RoomTemplate room, int xit)
        {
            var count = 0;
            return room.Exits.FirstOrDefault(exit => ++count == xit);
        }

        public static void sort_exits(RoomTemplate room)
        {
            // TODO
        }

        public static void randomize_exits(RoomTemplate room, int maxdir)
        {
            // TODO
        }

        public static void area_update()
        {
            foreach (var area in RepositoryManager.Instance.AREAS.Values)
            {
                var resetAge = area.ResetFrequency > 0 ? area.ResetFrequency : 15;
                if ((resetAge == -1 && area.Age == -1)
                    || (++area.Age < resetAge - 1))
                    continue;

                //// Check players
                if (area.NumberOfPlayers > 0 && area.Age == resetAge - 1)
                {
                    var buffer = !string.IsNullOrEmpty(area.ResetMessage)
                                        ? area.ResetMessage + "\r\n"
                                        : "You hear some squeaking sounds...\r\n";

                    foreach (var pch in RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values
                        .Where(pch => !pch.IsNpc()
                            && pch.IsAwake()
                            && pch.CurrentRoom != null
                            && pch.CurrentRoom.Area == area))
                    {
                       pch.SetColor(ATTypes.AT_RESET);
                       pch.SendTo(buffer);
                    }
                }

                //// Check age and reset
                if (area.NumberOfPlayers == 0 || area.Age >= resetAge)
                {
                    reset.reset_area(area);
                    area.Age = resetAge == -1 ? -1 : SmaugRandom.Between(0, resetAge / 5);

                    //// Mud Academy resets every 3 minutes
                    var room = RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_SCHOOL);
                    if (room != null && room.Area == area && area.ResetFrequency == 0)
                        area.Age = 15 - 3;
                }
            }
        }

        /*public static void clear_char(CharacterInstance ch)
        {
            ch.CurrentEditor = null;
            ch.CurrentHunting = null;
            ch.CurrentFearing = null;
            ch.CurrentHating = null;
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
            ch.ArmorClass = 100;
            ch.CurrentPosition = PositionTypes.Standing;
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
            ch.BareDice = new DiceData();
            ch.BareDice.NumberOf = 1;
            ch.BareDice.SizeOf = 4;
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
        }*/

        /*public static void free_char(CharacterInstance ch)
        {
            ch.CurrentMorph = null;

            ObjectInstance obj;
            while ((obj = ch.Carrying.First()) != null)
                obj.Extract();

            AffectData af;
            while ((af = ch.Affects.First()) != null)
                ch.RemoveAffect(af);

            ch.Timers.Clear();

            //if (ch.CurrentEditor != null)
            //    build.stop_editing(ch);

            ch.StopHunting();
            ch.StopHating();
            ch.StopFearing();
            ch.StopFighting(false);

            //ch.NoteList.Clear();

            foreach (VariableData vd in ch.Variables)
                variables.delete_variable(vd);

            if (ch is PlayerInstance && ((PlayerInstance)ch).PlayerData != null)
            {
                if (((PlayerInstance)ch).PlayerData.Pet != null)
                {
                    ((PlayerInstance)ch).PlayerData.Pet.Extract(true);
                    ((PlayerInstance)ch).PlayerData.Pet = null;
                }

                ((PlayerInstance)ch).PlayerData.Ignored.Clear();
                ((PlayerInstance)ch).PlayerData.TellHistory.Clear();

                // TODO IMC

                ((PlayerInstance)ch).PlayerData = null;
            }

            //ch.Comments.Clear();
            RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Delete(ch.ID);
        }*/

        public static string get_extra_descr(string name, IEnumerable<ExtraDescriptionData> extraDescriptions)
        {
            foreach (var ed in extraDescriptions.Where(ed => ed.Keyword.IsEqual(name)))
                return ed.Description;
            return string.Empty;
        }

        public static bool is_valid_filename(CharacterInstance ch, string direct, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || filename.Length < 3)
            {
                if (string.IsNullOrWhiteSpace(filename))
                    ch.SendTo("Empty filename is not valid");
                else
                    ch.Printf("Filename {0} is too short\r\n", filename);
                return false;
            }

            if (filename.StartsWith("..") || filename.StartsWith("/") || filename.StartsWith("\\"))
            {
                ch.SendTo("A filename may not start with a '..', '/', or '\\'.");
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

            using (var proxy = new TextWriterProxy(new StreamWriter(file, true)))
            {
                proxy.Write("[{0}] {1}: {2}\n", ch.CurrentRoom?.ID ?? 0, ch.Name, str);
            }
        }

        public static void append_to_file(string file, string str)
        {
            using (var proxy = new TextWriterProxy(new StreamWriter(file, true)))
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
            using (var proxy = new TextReaderProxy(new StreamReader(filename)))
            {
                IEnumerable<string> lines = proxy.ReadIntoList();
                foreach (var line in lines)
                {
                    ch.SendToPagerColor(line.Trim('\r', '\n'));
                }
            }
        }

        public static void towizfile(string line)
        {
            var outline = string.Empty;

            if (!string.IsNullOrEmpty(line))
            {
                var filler = 78 - line.Length;
                if (filler < 1)
                    filler = 1;
                filler /= 2;
                for (var x = 0; x < filler; x++)
                    outline += " ";
                outline += line;
            }

            outline += "\r\n";
            using (
                var proxy =
                    new TextWriterProxy(new StreamWriter(SystemConstants.GetSystemFile(SystemFileTypes.Wizards), true)))
            {
                proxy.Write(outline);
            }
        }

        public static void make_wizlist()
        {
            var wizList = new List<WizardData>();
            var directory = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.God);
            var files = new DirectoryProxy().GetFiles(directory);
            foreach (var file in files.Where(x => !x.StartsWithIgnoreCase(".")))
            {
                using (var proxy = new TextReaderProxy(new StreamReader(file)))
                {
                    var wizard = new WizardData { Name = file };
                    wizard.Level = wizard.Load(proxy.ReadIntoList());
                    wizList.Add(wizard);
                }
            }

            var buffer = $" Masters of the {GameManager.Instance.SystemData.MudTitle}!";

            var iLevel = 65535;
            foreach (var wiz in wizList)
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
                    if (iLevel == LevelConstants.MaxLevel)
                        towizfile(" Supreme Entity");
                    else if (iLevel == LevelConstants.MaxLevel - 1)
                        towizfile(" Infinite");
                    else if (iLevel == LevelConstants.MaxLevel - 2)
                        towizfile(" Eternal");
                    else if (iLevel == LevelConstants.MaxLevel - 3)
                        towizfile(" Ancient");
                    else if (iLevel == LevelConstants.MaxLevel - 4)
                        towizfile(" Exalted Gods");
                    else if (iLevel == LevelConstants.MaxLevel - 5)
                        towizfile(" Ascendant Gods");
                    else if (iLevel == LevelConstants.MaxLevel - 6)
                        towizfile(" Greater Gods");
                    else if (iLevel == LevelConstants.MaxLevel - 7)
                        towizfile(" Gods");
                    else if (iLevel == LevelConstants.MaxLevel - 8)
                        towizfile(" Lesser Gods");
                    else if (iLevel == LevelConstants.MaxLevel - 9)
                        towizfile(" Immortals");
                    else if (iLevel == LevelConstants.MaxLevel - 10)
                        towizfile(" Demi Gods");
                    else if (iLevel == LevelConstants.MaxLevel - 11)
                        towizfile(" Saviors");
                    else if (iLevel == LevelConstants.MaxLevel - 12)
                        towizfile(" Creators");
                    else if (iLevel == LevelConstants.MaxLevel - 13)
                        towizfile(" Acolytes");
                    else if (iLevel == LevelConstants.MaxLevel - 14)
                        towizfile(" Neophytes");
                    else if (iLevel == LevelConstants.MaxLevel - 15)
                        towizfile(" Retired");
                    else if (iLevel == LevelConstants.MaxLevel - 16)
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
            foreach (var type in Enum.GetValues(typeof(MudProgTypes)).Cast<MudProgTypes>().Where(type => type.GetName().EqualsIgnoreCase(name)))
                return (int)type;
            return (int)MudProgTypes.Error;
        }

        public static void mudprog_file_read(Template index, string filename)
        {
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Prog) + filename;

            using (var proxy = new TextReaderProxy(new StreamReader(path)))
            {
                do
                {
                    var line = proxy.ReadLine();
                    if (line.StartsWith("|"))
                        break;
                    if (line.StartsWith(">"))
                        continue;

                    var type = (MudProgTypes)EnumerationFunctions.GetEnumByName<MudProgTypes>(proxy.ReadNextWord());
                    if (type == MudProgTypes.Error
                        || type == MudProgTypes.InFile)
                    {
                        LogManager.Instance.Bug("Invalid mud prog type {0} in file {1}", type, path);
                        continue;
                    }

                    var prog = new MudProgData
                    {
                        Type = type,
                        ArgList = proxy.ReadString(),
                        Script = proxy.ReadString(),
                        IsFileProg = true
                    };

                    index.AddMudProg(prog);
                    break;

                } while (!proxy.EndOfStream);
            }
        }

        public static void mudprog_read_programs(TextReaderProxy proxy, Template index)
        {
            for (; ; )
            {
                var letter = proxy.ReadNextLetter();
                if (letter == '|')
                    return;
                if (letter != '>')
                {
                    LogManager.Instance.Bug("Vnum {0} MudProg Char", index.ID);
                    throw new Exception();
                }

                var prog = new MudProgData();
                index.AddMudProg(prog);

                var type = (MudProgTypes)EnumerationFunctions.GetEnumByName<MudProgTypes>(proxy.ReadNextWord());
                if (type == MudProgTypes.Error)
                {
                    LogManager.Instance.Bug("Invalid mud prog type {0} for Index {1}", type, index.ID);
                    throw new Exception();
                }
                prog.Type = type;

                if (type == MudProgTypes.InFile)
                {
                    prog.IsFileProg = false;
                    prog.ArgList = proxy.ReadString();
                    mudprog_file_read(index, prog.ArgList);
                }
                else
                {
                    // index.ProgTypes.SetBit((int)prog.Type);
                    prog.IsFileProg = false;
                    prog.ArgList = proxy.ReadString();
                    prog.Script = proxy.ReadString();
                }
            }
        }

        /*public static void delete_room(RoomTemplate room)
        {
            RoomTemplate limbo = RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_LIMBO);

            CharacterInstance ch;
            while ((ch = room.Persons.FirstOrDefault()) != null)
            {
                if (!ch.IsNpc())
                {
                    room.RemoveFrom(ch);
                    limbo.AddTo(ch);
                }
                else
                    CharacterInstanceExtensions.Extract(ch, true);
            }

            foreach (CharacterInstance och in RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values)
            {
                if (och.PreviousRoom == room)
                    och.PreviousRoom = och.CurrentRoom;
                if (och.SubState == CharacterSubStates.RoomDescription
                    && och.DestinationBuffer == room)
                {
                    ch.SetColor("The room is no more.\r\n", och);
                    build.stop_editing(och);
                    och.SubState = CharacterSubStates.None;
                    och.DestinationBuffer = null;
                }
                else if (och.SubState == CharacterSubStates.RoomExtra
                         && och.DestinationBuffer != null)
                {
                    if (room.ExtraDescriptions.Any(e => e == och.DestinationBuffer))
                    {
                        ch.SetColor("The room is no more.\r\n", och);
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
            RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Delete(room.Vnum);

            // TODO: Room hash stuff here, but can be removed?
        }

        public static void delete_obj(ObjectTemplate obj)
        {
            RepositoryManager.Instance.OBJECTS.CastAs<Repository<long, ObjectInstance>>().Values.Where(x => x.ObjectIndex == obj).ToList().ForEach(handler.extract_obj);
            obj.ExtraDescriptions.Clear();
            obj.Affects.Clear();
            obj.MudProgs.Clear();
            RepositoryManager.Instance.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Delete(obj.Vnum);

            // TODO Object hash stuff here, but can be removed?
        }

        public static void delete_mob(MobTemplate mob)
        {
            foreach (CharacterInstance ch in RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values)
            {
                if (ch.MobIndex == mob)
                    CharacterInstanceExtensions.Extract(ch, true);
                else if (ch.SubState == CharacterSubStates.MProgEditing
                         && ch.DestinationBuffer != null)
                {
                    if (mob.MudProgs.Any(mp => mp == ch.DestinationBuffer))
                    {
                        ch.SetColor("Your victim has departed.\r\n", ch);
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
            RepositoryManager.Instance.MOBILE_INDEXES.CastAs<Repository<long, MobTemplate>>().Delete(mob.Vnum);

            // TODO Mob hash stuff here, but can be removed?
        }*/

        public static ExitData make_exit(RoomTemplate room, RoomTemplate to_room, int door)
        {
            var newExit = new ExitData(door, "An exit")
            {
                Direction = EnumerationExtensions.GetEnum<DirectionTypes>(door),
                Room_vnum = room.ID,
                Destination = to_room.ID,
                Distance = 1,
                Key = -1
            };

            var reverseExit = to_room.GetExitTo(LookupConstants.rev_dir[door], room.ID);
            if (reverseExit != null)
            {
                reverseExit.Reverse = newExit.ID;
                newExit.Reverse = reverseExit.ID;
            }

            var broke = room.Exits.Any(exit => door < (int)exit.Direction);
            if (room.Exits == null)
                room.Exits.Add(newExit);
            else
            {
                if (broke && reverseExit != null)
                {
                    room.Exits.ToList().Insert(room.Exits.First() == reverseExit ? 0 : 1, newExit);
                    return newExit;
                }
            }

            return newExit;
        }

       /* public static void fix_area_exits(AreaData area)
        {
            for (int rnum = area.LowRoomNumber; rnum <= area.HighRoomNumber; rnum++)
            {
                RoomTemplate room = RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(rnum);
                if (room == null)
                    continue;

                bool fexit = false;
                foreach (ExitData exit in room.Exits)
                {
                    fexit = true;
                    exit.Room_vnum = room.Vnum;
                    exit.Destination = exit.vnum <= 0 ? 0 : RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(exit.vnum).ID;
                }
                if (!fexit)
                    room.Flags.SetBit((int)RoomFlags.NoMob);
            }

            for (int rnum = area.LowRoomNumber; rnum <= area.HighRoomNumber; rnum++)
            {
                RoomTemplate room = RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(rnum);
                if (room == null)
                    continue;

                foreach (ExitData exit in room.Exits)
                {
                    if (exit.Reverse < -0)
                    {
                        ExitData reverseExit = exit.GetDestination().GetExitTo(GameConstants.rev_dir[(int)exit.Direction], room.Vnum);
                        if (reverseExit != null)
                        {
                            exit.Reverse = reverseExit.ID;
                            reverseExit.Reverse = exit.ID;
                        }
                    }
                }
            }
        }*/

        public static void process_sorting(AreaData area)
        {
            /*if (DatabaseManager.BootDb)
            {
                sort_area_by_name(area);
                sort_area(area, false);
            }*/

            var buffer =
                $"{area.Filename}:\n\tRooms: {area.LowRoomNumber} - {area.HighRoomNumber}\n\tObjects: {area.LowObjectNumber} - {area.HighObjectNumber}\n\tMobs: {area.LowMobNumber} - {area.HighMobNumber}\n";
            LogManager.Instance.Boot(buffer);

            area.status.SetBit((int)AreaFlags.Loaded);
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


        public static void tail_chain()
        {
            // do nothing
        }



 


        public static void check_loginmsg(CharacterInstance ch)
        {
            if (ch == null || ch.IsNpc())
                return;

            var loginMessages = LOGIN_MESSAGES.Where(x => x.Name.EqualsIgnoreCase(ch.Name)).ToList();

            foreach (var loginMessage in LOGIN_MESSAGES)
            {
                if (loginMessage.Type > Program.MAX_MSG)
                {
                    LogManager.Instance.Bug("Unknown login message type {0} for {1}", loginMessage.Type, ch.Name);
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
                        ch.SendTo(LookupManager.Instance.GetLookup("LoginMessage", loginMessage.Type));
                        break;
                }

                //save_loginmsg();
            }

            while (loginMessages.Count > 0)
                LOGIN_MESSAGES.Remove(loginMessages.First());
        }

        private static void ImmortalMessage(CharacterInstance ch, LoginMessageData lmsg)
        {
            if (!lmsg.Text.IsNullOrEmpty())
            {
                LogManager.Instance.Bug("NULL loginMessage text for type 0");
                return;
            }

            ch.Printf("\r\n&YThe game administrators have left you the following message:\r\n%s\r\n", lmsg.Text);
        }
        private static void DeathMessage(CharacterInstance ch, LoginMessageData lmsg)
        {
            if (!lmsg.Text.IsNullOrEmpty())
            {
                LogManager.Instance.Bug("NULL loginMessage text for type 17");
                return;
            }

            ch.Printf("\r\n&RYou were killed by %s while your character was link-dead.\r\n", lmsg.Text);
            ch.SendTo("You should look for your corpse immediately.");
        }
        private static void WorldChangeMessage(CharacterInstance ch, LoginMessageData lmsg)
        {
            if (!lmsg.Text.IsNullOrEmpty())
            {
                LogManager.Instance.Bug("NULL loginMessage text for type 18");
                return;
            }

            ch.Printf("\r\n&GA change in the Realms has affected you personally:\r\n%s\r\n", lmsg.Text);
        }
    }
}
