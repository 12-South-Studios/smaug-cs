using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Language;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class build
    {
        public static bool can_rmodify(CharacterInstance ch, RoomTemplate room)
        {
            if (ch.IsNpc() || ch.Trust < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelModifyPrototype))
                return false;
            if (!room.Flags.IsSet((int)RoomFlags.Prototype))
            {
                color.send_to_char("You cannot modify this room.\r\n", ch);
                return false;
            }
            if (ch.PlayerData == null || ch.PlayerData.BuilderArea == null)
            {
                color.send_to_char("You must have an assigned area to modify this room.\r\n", ch);
                return false;
            }
            if (room.Vnum >= ch.PlayerData.BuilderArea.LowRoomNumber
                && room.Vnum <= ch.PlayerData.BuilderArea.HighRoomNumber)
            {
                color.send_to_char("That room is not in your allocated range.\r\n", ch);
                return false;
            }

            return true;
        }

        public static bool can_omodify(CharacterInstance ch, ObjectInstance obj)
        {
            if (ch.IsNpc() || ch.Trust < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelModifyPrototype))
                return false;
            if (!Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Prototype))
            {
                color.send_to_char("You cannot modify this object.\r\n", ch);
                return false;
            }
            if (ch.PlayerData == null || ch.PlayerData.BuilderArea == null)
            {
                color.send_to_char("You must have an assigned area to modify this room.\r\n", ch);
                return false;
            }
            if (obj.ObjectIndex.Vnum >= ch.PlayerData.BuilderArea.LowObjectNumber
                && obj.ObjectIndex.Vnum <= ch.PlayerData.BuilderArea.HighObjectNumber)
            {
                color.send_to_char("That object is not in your allocated range.\r\n", ch);
                return false;
            }

            return true;
        }

        public static bool can_oedit(CharacterInstance ch, ObjectTemplate obj)
        {
            if (ch.IsNpc() || ch.Trust < Program.GetLevel("god"))
                return false;
            if (!Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Prototype))
            {
                color.send_to_char("You cannot modify this object.\r\n", ch);
                return false;
            }
            if (ch.PlayerData == null || ch.PlayerData.BuilderArea == null)
            {
                color.send_to_char("You must have an assigned area to modify this room.\r\n", ch);
                return false;
            }
            if (obj.Vnum >= ch.PlayerData.BuilderArea.LowObjectNumber
                && obj.Vnum <= ch.PlayerData.BuilderArea.HighObjectNumber)
            {
                color.send_to_char("That object is not in your allocated range.\r\n", ch);
                return false;
            }

            return true;
        }

        public static bool can_mmodify(CharacterInstance ch, CharacterInstance mob)
        {
            if (mob == ch)
                return true;
            if (!mob.IsNpc())
            {
                if (ch.Trust >= db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelModifyPrototype)
                    && ch.Trust > mob.Trust)
                    return true;

                color.send_to_char("You can't do that.\r\n", ch);
                return false;
            }

            if (ch.IsNpc())
                return false;
            if (ch.Trust >= db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelModifyPrototype))
                return true;
            if (!mob.Act.IsSet((int)ActFlags.Prototype))
            {
                color.send_to_char("You cannot modify this mobile.\r\n", ch);
                return false;
            }
            if (ch.PlayerData == null || ch.PlayerData.BuilderArea == null)
            {
                color.send_to_char("You must have an assigned area to modify this mobile.\r\n", ch);
                return false;
            }
            if (mob.MobIndex.Vnum >= ch.PlayerData.BuilderArea.LowMobNumber
                && mob.MobIndex.Vnum <= ch.PlayerData.BuilderArea.HighMobNumber)
                return true;

            color.send_to_char("That mobile is not in your allocated range.\r\n", ch);
            return false;
        }

        public static bool can_medit(CharacterInstance ch, MobTemplate mob)
        {
            if (ch.IsNpc())
                return false;
            if (ch.Trust >= Program.GetLevel("god"))
                return true;
            if (!mob.GetActFlags().IsSet((int)ActFlags.Prototype))
            {
                color.send_to_char("You cannot modify this mobile.\r\n", ch);
                return false;
            }
            if (ch.PlayerData == null || ch.PlayerData.BuilderArea == null)
            {
                color.send_to_char("You must have an assigned area to modify this mobile.\r\n", ch);
                return false;
            }
            if (mob.Vnum >= ch.PlayerData.BuilderArea.LowMobNumber
                && mob.Vnum <= ch.PlayerData.BuilderArea.HighMobNumber)
                return true;

            color.send_to_char("That mobile is not in your allocated range.\r\n", ch);
            return false;
        }

        public static int get_npc_class(string value)
        {
            return FlagLookup.GetIndexOf(value, GameConstants.npc_class);
        }

        public static int get_npc_race(string value)
        {
            return FlagLookup.GetIndexOf(value, GameConstants.npc_race);
        }

        public static int get_pc_class(string value)
        {
            return
                DatabaseManager.Instance.CLASSES.ToList()
                               .FindIndex(x => x.Name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public static int get_pc_race(string value)
        {
            return DatabaseManager.Instance.RACES.ToList().FindIndex(x => x.Name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public static int get_langflag(string value)
        {
            try
            {
                return (int)EnumerationExtensions.GetEnum<LanguageTypes>(value);
            }
            catch (ArgumentException)
            {
                return (int)LanguageTypes.Unknown;
            }
        }

        public static int get_langnum(string value)
        {
            int count = 0;
            foreach (LanguageData lang in DatabaseManager.Instance.LANGUAGES)
            {
                if (lang.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return count;
                count++;
            }
            return -1;
        }

        public static string strip_cr(string str)
        {
            return str.Replace('\r', '\0').Replace('\n', '\0');
        }

        public static string smush_tilde(string str)
        {
            return str.Replace('~', '-');
        }

        public static void start_editing(CharacterInstance ch, string data)
        {
            if (ch.Descriptor == null)
            {
                LogManager.Instance.Bug("No descriptor");
                return;
            }

            if (ch.SubState == CharacterSubStates.Restricted)
            {
                LogManager.Instance.Bug("Restricted sub-state for Character {0}", ch.Name);
                return;
            }

            color.set_char_color(ATTypes.AT_GREEN, ch);
            color.send_to_char("Begin entering your text now (/? = help /s = save /c = clear /l = list)\r\n", ch);
            color.send_to_char("-----------------------------------------------------------------------\r\n> ", ch);

            if (ch.CurrentEditor != null)
                stop_editing(ch);

            EditorData edit = new EditorData();

            char[] charArray = data.ToCharArray();
            int lines = 0;
            int lpos = 0;

            int size;
            for (size = 0; size < charArray.Length; size++)
            {
                char c = charArray[size++];
                if (c == '\0')
                {
                    edit.Text[lines].SetChar(lpos, '\0');
                    break;
                }
                if (c == '\r') continue;
                if (c == '\n' || lpos > 78)
                {
                    edit.Text[lines].SetChar(lpos, '\0');
                    ++lines;
                    lpos = 0;
                }
                else
                    edit.Text[lines].SetChar(lpos++, c);

                if (lines >= 49 || size > 4096)
                {
                    edit.Text[lines].SetChar(lpos, '\0');
                    break;
                }
            }
            if (lpos > 0 && lpos < 78 && lines < 49)
            {
                edit.Text[lines].SetChar(lpos, '~');
                edit.Text[lines].SetChar(lpos + 1, '\0');
                ++lines;
            }

            edit.NumberOfLines = lines;
            edit.Size = size;
            edit.OnLine = lines;
            ch.CurrentEditor = edit;
            ch.Descriptor.ConnectionStatus = ConnectionTypes.Editing;
        }

        public static string copy_buffer_nohash(CharacterInstance ch)
        {
            if (ch == null || ch.CurrentEditor == null)
                return string.Empty;

            string buffer = string.Empty;

            for (int x = 0; x < ch.CurrentEditor.NumberOfLines; x++)
            {
                string tmp = ch.CurrentEditor.Text[x];
                if (!tmp.IsNullOrEmpty() && tmp.EndsWith("~"))
                    tmp.SetChar(tmp.Length - 1, '\0');
                else
                    tmp += "\n";
                tmp.SmashTilde();
                buffer += tmp;
            }

            return buffer;
        }

        public static string copy_buffer(CharacterInstance ch)
        {
            if (ch == null || ch.CurrentEditor == null)
                return string.Empty;

            string buffer = string.Empty;

            for (int x = 0; x < ch.CurrentEditor.NumberOfLines; x++)
            {
                string tmp = ch.CurrentEditor.Text[x];
                if (!tmp.IsNullOrEmpty() && tmp.EndsWith("~"))
                    tmp.SetChar(tmp.Length - 1, '\0');
                else
                    tmp += "\n";
                tmp.SmashTilde();
                buffer += tmp;
            }

            return buffer;
        }

        public static void stop_editing(CharacterInstance ch)
        {
            color.set_char_color(ATTypes.AT_PLAIN, ch);
            ch.CurrentEditor = null;
            color.send_to_char("Done.\r\n", ch);
            ch.DestinationBuffer = null;
            ch.spare_ptr = null;
            ch.SubState = CharacterSubStates.None;

            if (ch.Descriptor == null)
            {
                LogManager.Instance.Bug("Missing descriptor {0}", ch.Name);
                return;
            }

            ch.Descriptor.ConnectionStatus = ConnectionTypes.Playing;
        }

        public static DirectionTypes get_dir(string txt)
        {
            if (txt.EqualsIgnoreCase("northeast") || txt.Equals("6") || txt.EqualsIgnoreCase("ne"))
                return DirectionTypes.Northeast;
            if (txt.EqualsIgnoreCase("northwest") || txt.Equals("7") || txt.EqualsIgnoreCase("nw"))
                return DirectionTypes.Northwest;
            if (txt.EqualsIgnoreCase("southeast") || txt.Equals("8") || txt.EqualsIgnoreCase("se"))
                return DirectionTypes.Southeast;
            if (txt.EqualsIgnoreCase("southwest") || txt.Equals("9") || txt.EqualsIgnoreCase("sw"))
                return DirectionTypes.Southwest;
            if (txt.EqualsIgnoreCase("somewhere") || txt.Equals("?"))
                return DirectionTypes.Somewhere;
            if (txt.StartsWithIgnoreCase("n") || txt.Equals("0"))
                return DirectionTypes.North;
            if (txt.StartsWithIgnoreCase("e") || txt.Equals("1"))
                return DirectionTypes.East;
            if (txt.StartsWithIgnoreCase("s") || txt.Equals("2"))
                return DirectionTypes.South;
            if (txt.StartsWithIgnoreCase("w") || txt.Equals("3"))
                return DirectionTypes.West;
            if (txt.StartsWithIgnoreCase("u") || txt.Equals("4"))
                return DirectionTypes.Up;
            if (txt.StartsWithIgnoreCase("d") || txt.Equals("5"))
                return DirectionTypes.Down;

            return DirectionTypes.Somewhere;
        }

        public static void add_room_affect(RoomTemplate location, CharacterInstance ch, bool indexaffect, string argument)
        {
            Tuple<string, string> tuple = argument.FirstArgument();
            string arg = tuple.Item1;
            string arg2 = tuple.Item2;

            if (arg2.IsNullOrEmpty() || arg.IsNullOrEmpty())
            {
                color.send_to_char(
                    !indexaffect
                        ? "Usage: redit affect <field> <value>\r\n"
                        : "Usage: redit permaffect <field> <value>\r\n", ch);
                return;
            }

            int loc = FlagLookup.get_atype(arg2);
            if (loc < 1)
            {
                color.ch_printf(ch, "Unknown field: %s\r\n", arg2);
                return;
            }

            int value = 0;
            bool found = false;
            int bitv = 0;

            if (loc == (int)ApplyTypes.Affect)
            {
                Tuple<string, string> tuple2 = arg.FirstArgument();

                value = FlagLookup.get_aflag(tuple2.Item2);
                if (value < 0 || value >= EnumerationFunctions.MaximumEnumValue<AffectedByTypes>())
                    color.ch_printf(ch, "Unknown affect: %s\r\n", tuple2.Item2);
                else
                    found = true;
            }
            else if (loc == (int)ApplyTypes.Resistance
                     || loc == (int)ApplyTypes.Immunity
                     || loc == (int)ApplyTypes.Susceptibility)
            {
                List<string> words = arg.ToWords();
                foreach (string word in words)
                {
                    value = FlagLookup.get_risflag(word);
                    if (value < 0 || value > 31)
                        color.ch_printf(ch, "Unknown flag: %s\r\n", value);
                    else
                    {
                        bitv.SetBit(1 << value);
                        found = true;
                    }

                    if (bitv == 0)
                        return;
                    value = bitv;
                }
            }
            else if (loc == (int)ApplyTypes.WeaponSpell
                     || loc == (int)ApplyTypes.WearSpell
                     || loc == (int)ApplyTypes.RemoveSpell
                     || loc == (int)ApplyTypes.StripSN
                     || loc == (int)ApplyTypes.RecurringSpell)
            {
                value = DatabaseManager.Instance.LookupSkill(arg);
                if (!Macros.IS_VALID_SN(value))
                    color.ch_printf(ch, "Invalid spell: %s", arg);
                else
                    found = true;
            }
            else
            {
                value = arg.ToInt32();
                found = true;
            }

            if (!found)
                return;

            AffectData paf = new AffectData
                {
                    Type = AffectedByTypes.None,
                    Duration = -1,
                    Location = EnumerationExtensions.GetEnum<ApplyTypes>(loc),
                    Modifier = value
                };
            paf.BitVector.ClearBits();

            if (!indexaffect)
                location.Affects.Add(paf);
            else
                location.PermanentAffects.Add(paf);

            color.send_to_char("Room affect added.\r\n", ch);

            if (paf.Location != ApplyTypes.WearSpell
                && paf.Location != ApplyTypes.RemoveSpell
                && paf.Location != ApplyTypes.StripSN)
            {
                // apply the affect to anyone in the room
                foreach (CharacterInstance vch in ch.CurrentRoom.Persons)
                    vch.AddAffect(paf);
            }
        }

        public static void remove_room_affect(RoomTemplate location, CharacterInstance ch, bool indexaffect, string argument)
        {
            if (argument.IsNullOrEmpty())
            {
                color.send_to_char(!indexaffect
                    ? "Usage: redit rmaffect <affect#>\r\n"
                    : "Usage: redit rmpermaffect <affect#>\r\n", ch);
                return;
            }

            int count = 0;
            int loc = argument.ToInt32();
            if (loc < 1)
            {
                color.send_to_char("Invalid number.\r\n", ch);
                return;
            }

            if (!indexaffect)
            {
                foreach (AffectData paf in location.Affects)
                {
                    if (++count == loc)
                    {
                        if (paf.Location != ApplyTypes.WearSpell
                            && paf.Location != ApplyTypes.RemoveSpell
                            && paf.Location != ApplyTypes.StripSN)
                        {
                            // Remove the affect from people in the room
                            foreach (CharacterInstance vch in ch.CurrentRoom.Persons)
                                handler.affect_modify(vch, paf, false);
                        }
                        location.Affects.Remove(paf);
                        color.send_to_char("Room affect removed.\r\n", ch);
                        return;
                    }
                }
            }
            else
            {
                foreach (AffectData paf in location.PermanentAffects)
                {
                    if (++count == loc)
                    {
                        if (paf.Location != ApplyTypes.WearSpell
                            && paf.Location != ApplyTypes.RemoveSpell
                            && paf.Location != ApplyTypes.StripSN)
                        {
                            // Remove the affect from people in the room
                            foreach (CharacterInstance vch in ch.CurrentRoom.Persons)
                                handler.affect_modify(vch, paf, false);
                        }
                        location.PermanentAffects.Remove(paf);
                        color.send_to_char("Room index affect removed.\r\n", ch);
                        return;
                    }
                }
            }

            color.send_to_char("Room affect not found.\r\n", ch);
        }

        public static void edit_buffer(CharacterInstance ch, string argument)
        {
            DescriptorData d = ch.Descriptor;
            if (d == null)
            {
                color.send_to_char("You have no descriptor.\r\n", ch);
                return;
            }

            if (d.ConnectionStatus != ConnectionTypes.Editing)
            {
                color.send_to_char("You can't do that!\r\n", ch);
                LogManager.Instance.Bug("{0}: d.ConnectionStatus != Editing", ch.Name);
                return;
            }

            if ((int)ch.SubState <= (int)CharacterSubStates.Pause)
            {
                color.send_to_char("You can't do that!\r\n", ch);
                LogManager.Instance.Bug("{0}: Illegal Character SubState {1}", ch.Name, ch.SubState);
                d.ConnectionStatus = ConnectionTypes.Playing;
                return;
            }

            if (ch.CurrentEditor == null)
            {
                color.send_to_char("You can't do that!\r\n", ch);
                LogManager.Instance.Bug("{0}: Null Editor", ch.Name);
                d.ConnectionStatus = ConnectionTypes.Playing;
                return;
            }

            EditorData edit = ch.CurrentEditor;
            bool save = false;
            bool exit = false;

            if (argument.StartsWith("/") || argument.StartsWith("\\"))
            {
                Tuple<string, string> tuple = argument.FirstArgument();
                string arg1 = tuple.Item1;
                string arg2 = tuple.Item2;

                if (EditBufferTable.ContainsKey(arg1.ToLower()))
                    exit = EditBufferTable[arg1.ToLower()].Invoke(ch, arg2);

                if (exit)
                    return;

                if (ch.Trust > Program.GetLevel("immortal") && arg1.Equals("/!"))
                {
                    // do last command
                    return;
                }
            }

            if (ch.CurrentEditor.Text.Sum(x => x.Length) + argument.Length + 1 > Program.MAX_STRING_LENGTH - 1)
                color.send_to_char("Your buffer is full.\r\n", ch);
            else
            {
                string buffer;
                if (argument.Length > 79)
                {
                    buffer = argument.Substring(0, 79);
                    color.send_to_char("(Long line trimmed)\r\n", ch);
                }
                else
                    buffer = argument;

                edit.Text[edit.OnLine++] = buffer;
                if (edit.OnLine > edit.NumberOfLines)
                    edit.NumberOfLines++;
                if (edit.NumberOfLines > Program.GetIntegerConstant("MaximumBufferLines"))
                {
                    edit.NumberOfLines = Program.GetIntegerConstant("MaximumBufferLines");
                    color.send_to_char("Buffer full.\r\n", ch);
                    save = true;
                }
            }

            if (save)
            {
                EditBufferStop(ch, "");
                return;
            }

            color.send_to_char("> ", ch);
        }

        private static readonly Dictionary<string, Func<CharacterInstance, string, bool>> EditBufferTable = new Dictionary
            <string, Func<CharacterInstance, string, bool>>()
            {
                {"/?", EditBufferHelp},
                {"/c", EditBufferClear},
                {"/r", EditBufferReplace},
                {"/f", EditBufferFormat},
                {"/i", EditBufferInsert},
                {"/d", EditBufferDelete},
                {"/g", EditBufferGoto},
                {"/l", EditBufferList},
                {"/a", EditBufferAbort},
                {"/s", EditBufferStop}
            };

        private static bool EditBufferHelp(CharacterInstance ch, string arg)
        {
            color.send_to_char("Editing commands\r\n---------------------------------\r\n", ch);
            color.send_to_char("/l              list buffer\r\n", ch);
            color.send_to_char("/c              clear buffer\r\n", ch);
            color.send_to_char("/d [line]       delete line\r\n", ch);
            color.send_to_char("/g <line>       goto line\r\n", ch);
            color.send_to_char("/i <line>       insert line\r\n", ch);
            color.send_to_char("/f <format>     format text in buffer\r\n", ch);
            color.send_to_char("/r <old> <new>  global replace\r\n", ch);
            color.send_to_char("/a              abort editing\r\n", ch);
            if (ch.Trust > Program.GetLevel("immortal"))
                color.send_to_char("/! <command>    execute command (do not use another editing command)\r\n", ch);
            color.send_to_char("/s              save buffer\r\n\r\n> ", ch);
            return true;
        }
        private static bool EditBufferClear(CharacterInstance ch, string arg)
        {
            ch.CurrentEditor.NumberOfLines = 0;
            ch.CurrentEditor.OnLine = 0;
            color.send_to_char("Buffer cleared.\r\n", ch);
            return true;
        }
        private static bool EditBufferReplace(CharacterInstance ch, string arg)
        {
            string[] words = arg.Split(' ');
            if (words.Length == 0 || words[0].IsNullOrWhitespace() || words[1].IsNullOrWhitespace())
            {
                color.send_to_char("Need word to replace, and replacement.\r\n", ch);
                return true;
            }

            if (words[0].Equals(words[1]))
            {
                color.send_to_char("Done.\r\n", ch);
                return true;
            }

            color.ch_printf(ch, "Replacing all occurrences of %s with %s...\r\n", words[0], words[1]);
            int count = 0;

            for (int x = 0; x < ch.CurrentEditor.NumberOfLines; x++)
            {
                if (ch.CurrentEditor.Text[x].Contains(words[0]))
                {
                    count += Regex.Matches(ch.CurrentEditor.Text[x], words[0]).Count;
                    string line = ch.CurrentEditor.Text[x].Replace(words[0], words[1]);
                    ch.CurrentEditor.Text[x] = line;
                }
            }

            color.ch_printf(ch, "Found and replaced %d occurrence(s).\r\n", count);
            return true;
        }
        private static bool EditBufferFormat(CharacterInstance ch, string arg)
        {
            color.pager_printf(ch, "Reformatting...\r\n");

            string buffer = string.Empty;
            for (int x = 0; x < ch.CurrentEditor.NumberOfLines; x++)
            {
                buffer += ch.CurrentEditor.Text[x] + ' ';
            }

            int end_mark = buffer.Length;
            int old_p = 0;
            int p = 75;
            ch.CurrentEditor.OnLine = 0;
            ch.CurrentEditor.NumberOfLines = 0;

            while (old_p < end_mark)
            {
                while (buffer[p] != ' ' && p > old_p)
                    p--;

                if (p == old_p)
                    p += 75;

                if (p > end_mark)
                    p = end_mark;

                int ep = 0;
                for (int x = old_p; x < p; x++)
                {
                    ch.CurrentEditor.Text[x].SetChar(ep, buffer[x]);
                    ep++;
                }
                ch.CurrentEditor.Text[ch.CurrentEditor.OnLine].SetChar(ep, '\0');
                ch.CurrentEditor.OnLine++;
                ch.CurrentEditor.NumberOfLines++;

                old_p = p + 1;
                p += 75;
            }

            color.pager_printf(ch, "Reformatting done.\r\n");
            return true;
        }
        private static bool EditBufferInsert(CharacterInstance ch, string arg)
        {
            if (ch.CurrentEditor.NumberOfLines >= Program.GetIntegerConstant("MaximumBufferLines"))
            {
                color.send_to_char("Buffer is full.\r\n", ch);
                return true;
            }

            int line = arg[2] == ' ' ? arg.FirstWord().ToInt32() : ch.CurrentEditor.OnLine;
            if (line < 0)
                line = ch.CurrentEditor.OnLine;
            if (line < 0 || line > ch.CurrentEditor.NumberOfLines)
            {
                color.send_to_char("Out of range.\r\n", ch);
                return true;
            }

            ch.CurrentEditor.Text.Insert(line, "");
            color.send_to_char("Line inserted.\r\n", ch);
            return true;
        }
        private static bool EditBufferDelete(CharacterInstance ch, string arg)
        {
            if (ch.CurrentEditor.NumberOfLines == 0 || ch.CurrentEditor.Text.Count == 0)
            {
                color.send_to_char("Buffer is empty.\r\n", ch);
                return true;
            }

            int line = arg[2] == ' ' ? arg.FirstWord().ToInt32() : ch.CurrentEditor.OnLine;
            if (line < 0)
                line = ch.CurrentEditor.OnLine;
            if (line < 0 || line > ch.CurrentEditor.NumberOfLines)
            {
                color.send_to_char("Out of range.\r\n", ch);
                return true;
            }

            if (line == 0 && ch.CurrentEditor.NumberOfLines == 1)
            {
                ch.CurrentEditor = null;
                color.send_to_char("Line deleted.\r\n", ch);
                return true;
            }

            ch.CurrentEditor.Text.RemoveAt(line);
            if (ch.CurrentEditor.OnLine > ch.CurrentEditor.NumberOfLines)
                ch.CurrentEditor.OnLine = ch.CurrentEditor.NumberOfLines;

            color.send_to_char("Line deleted.\r\n", ch);
            return true;
        }
        private static bool EditBufferGoto(CharacterInstance ch, string arg)
        {
            if (ch.CurrentEditor.NumberOfLines == 0 || ch.CurrentEditor.Text.Count == 0)
            {
                color.send_to_char("Buffer is empty.\r\n", ch);
                return true;
            }

            int line = arg[2] == ' ' ? arg.FirstWord().ToInt32() : ch.CurrentEditor.OnLine;
            if (line < 0)
                line = ch.CurrentEditor.OnLine;
            if (line < 0 || line > ch.CurrentEditor.NumberOfLines)
            {
                color.send_to_char("Out of range.\r\n", ch);
                return true;
            }

            ch.CurrentEditor.OnLine = line;
            color.ch_printf(ch, "(On line %d)\r\n", line + 1);
            return true;
        }
        private static bool EditBufferList(CharacterInstance ch, string arg)
        {
            if (ch.CurrentEditor.NumberOfLines == 0 || ch.CurrentEditor.Text.Count == 0)
            {
                color.send_to_char("Buffer is empty.\r\n", ch);
                return true;
            }

            color.send_to_char("------------------\r\n", ch);
            for (int x = 0; x < ch.CurrentEditor.NumberOfLines; x++)
                color.ch_printf(ch, "%2d> %s\r\n", x + 1, ch.CurrentEditor.Text[x]);
            color.send_to_char("------------------\r\n> ", ch);
            return true;
        }
        private static bool EditBufferAbort(CharacterInstance ch, string arg)
        {
            color.send_to_char("\r\nAborting... ", ch);
            stop_editing(ch);
            return true;
        }
        private static bool EditBufferStop(CharacterInstance ch, string arg)
        {
            ch.Descriptor.ConnectionStatus = ConnectionTypes.Playing;
            if (ch.LastCommand == null)
                return true;
            ch.LastCommand.Value.Invoke(ch, "");
            return true;
        }

        public static void assign_area(CharacterInstance ch)
        {
            if (ch.IsNpc())
                return;
            if (ch.Trust <= Program.GetLevel("immortal") || ch.PlayerData.r_range_lo <= 0 || ch.PlayerData.r_range_hi <= 0)
                return;

            AreaData area = ch.PlayerData.BuilderArea;
            string areaName = string.Format("{0}.are", ch.Name.CapitalizeFirst());
            if (area == null)
            {
                foreach (AreaData tmpArea in db.BUILD_AREAS.Where(tmpArea => tmpArea.Filename.EqualsIgnoreCase(areaName)))
                {
                    area = tmpArea;
                    break;
                }
            }

            bool created = false;
            if (area == null)
            {
                LogManager.Instance.Log(LogTypes.Normal, ch.Level, "Creating area entry for {0}", ch.Name);
                area = new AreaData(db.AREAS.Count + db.BUILD_AREAS.Count + 1,
                                    string.Format("{{PROTO}} {0}'s area in progress", ch.Name))
                           {

                               Filename = areaName,
                               Author = ch.Name
                           };
                db.BUILD_AREAS.Add(area);
                created = true;
            }
            else
                LogManager.Instance.Log(LogTypes.Normal, ch.Level, "Updating area entry for {0}", ch.Name);

            area.LowRoomNumber = ch.PlayerData.r_range_lo;
            area.LowObjectNumber = ch.PlayerData.o_range_lo;
            area.LowMobNumber = ch.PlayerData.m_range_lo;
            area.HighRoomNumber = ch.PlayerData.r_range_hi;
            area.HighObjectNumber = ch.PlayerData.o_range_hi;
            area.HighMobNumber = ch.PlayerData.m_range_hi;
            ch.PlayerData.BuilderArea = area;
            if (created)
                db.sort_area(area, true);
        }

        public static void save_reset_level(TextWriterProxy proxy, IEnumerable<ResetData> resets, int level)
        {
            foreach (ResetData reset in resets)
            {
                string buffer = reset.Command.PadLeft(level * 2, '.');
                switch (reset.Command.ToUpper()[0])
                {
                    case '*':
                        break;
                    default:
                        proxy.Write(string.Format("{0}Reset {1} {2} {3} {4} {5}\n", buffer, reset.Command.ToUpper(), reset.Extra, reset.Args[0], reset.Args[1], reset.Args[2]));
                        break;
                    case 'G':
                    case 'R':
                        proxy.Write(string.Format("{0}Reset {1} {2} {3} {4}\n", buffer, reset.Extra, reset.Args[0], reset.Args[1], reset.Args[2]));
                        break;
                }

                // Save nested resets
                save_reset_level(proxy, reset.Resets, level + 1);
            }
        }

        public static void fold_area(AreaData area, string fname, bool install)
        {
            string filename = fname + ".bak";
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(filename)))
            {
                area.Version = Program.AREA_VERSION_WRITE;
                proxy.Write("#FUSSAREA\n");
                //area.SaveHeader(proxy, install);

                for (int i = area.LowMobNumber; i <= area.HighMobNumber; ++i)
                {
                    MobTemplate mob = DatabaseManager.Instance.MOBILE_INDEXES.CastAs<Repository<long,MobTemplate>>().Get(i);
                    //if (mob != null)
                    //    mob.SaveFUSS(proxy, install);
                }
                for (int i = area.LowObjectNumber; i <= area.HighObjectNumber; ++i)
                {
                    ObjectTemplate obj = DatabaseManager.Instance.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Get(i);
                    //if (obj != null)
                    //    obj.SaveFUSS(proxy, install);
                }
                for (int i = area.LowRoomNumber; i <= area.HighRoomNumber; ++i)
                {
                    RoomTemplate room = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(i);
                    //if (room != null)
                    //     room.SaveFUSS(proxy, install);
                }
                proxy.Write("#ENDAREA\n");
            }
        }

        public static void old_fold_area(AreaData tarea, string filename, bool install)
        {
            // TODO
        }

        public static void write_area_list()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Areas);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                proxy.Write("help.are\n");
                foreach (AreaData area in db.AREAS)
                    proxy.Write("{0}\n", area.Filename);
                proxy.Write("$\n");
            }
        }

        /// <summary>
        /// Check other areas for a conflict while ignoring the current area
        /// </summary>
        /// <param name="currentArea"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static bool check_for_area_conflicts(AreaData currentArea, int lo, int hi)
        {
            return db.AREAS.Any(area => area != currentArea && act_wiz.check_area_conflict(area, lo, hi))
                   || db.BUILD_AREAS.Any(area => area != currentArea && act_wiz.check_area_conflict(area, lo, hi));
        }

        public static void RelCreate(RelationTypes tp, object actor, object subject)
        {
            if (actor == null || subject == null)
            {
                LogManager.Instance.Bug("Null actor or subject.");
                return;
            }

            foreach (RelationData relation in db.RELATIONS)
            {
                if (relation.Types == tp && relation.Actor == actor && relation.Subject == subject)
                {
                    LogManager.Instance.Bug("Duplicated Relation");
                    return;
                }
            }

            RelationData newRelation = new RelationData
                {
                    Types = tp,
                    Actor = actor,
                    Subject = subject
                };
            db.RELATIONS.Add(newRelation);
        }

        public static void RelDestroy(RelationTypes tp, object actor, object subject)
        {
            if (actor == null || subject == null)
            {
                LogManager.Instance.Bug("NULL actor or subject.");
                return;
            }

            RelationData foundRelation =
                db.RELATIONS.FirstOrDefault(x => x.Types == tp && x.Actor == actor && x.Subject == subject);
            if (foundRelation != null)
                db.RELATIONS.Remove(foundRelation);
        }
    }
}
