﻿
using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Common;
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
            if (ch.IsNpc() || ch.Trust < Program.LEVEL_GOD)
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
            if (ch.Trust >= Program.LEVEL_GOD)
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
            if (mob.Vnum >= ch.PlayerData.BuilderArea.LowMobNumber
                && mob.Vnum <= ch.PlayerData.BuilderArea.HighMobNumber)
                return true;

            color.send_to_char("That mobile is not in your allocated range.\r\n", ch);
            return false;
        }

        private static int GetIndexOf(string value, List<string> sourceList)
        {
            return sourceList.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public static int get_otype(string value)
        {
            return GetIndexOf(value, BuilderConstants.o_types);
        }

        public static int get_aflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.a_flags);
        }

        public static int get_trapflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.trap_flags);
        }

        public static int get_atype(string value)
        {
            return GetIndexOf(value, BuilderConstants.a_types);
        }

        public static int get_npc_class(string value)
        {
            return GetIndexOf(value, GameConstants.npc_class);
        }

        public static int get_npc_race(string value)
        {
            return GetIndexOf(value, GameConstants.npc_race);
        }

        public static int get_pc_class(string value)
        {
            return db.CLASSES.FindIndex(x => x.Name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public static int get_pc_race(string value)
        {
            return db.RACES.FindIndex(x => x.Name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public static int get_wearloc(string value)
        {
            return GetIndexOf(value, BuilderConstants.wear_locs);
        }

        public static int get_secflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.sec_flags);
        }

        public static int get_exflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.ex_flags);
        }

        public static int get_pulltype(string value)
        {
            if (value.Equals("none", StringComparison.OrdinalIgnoreCase)
                || value.Equals("clear", StringComparison.OrdinalIgnoreCase))
                return 0;

            int index = GetIndexOf(value, BuilderConstants.ex_pmisc);
            if (index > -1)
                return index + (int)PlaneTypes.Water;

            index = GetIndexOf(value, BuilderConstants.ex_pair);
            if (index > -1)
                return index + (int)PlaneTypes.Air;

            index = GetIndexOf(value, BuilderConstants.ex_pearth);
            if (index > -1)
                return index + (int)PlaneTypes.Earth;

            index = GetIndexOf(value, BuilderConstants.ex_pfire);
            if (index > -1)
                return index + (int)PlaneTypes.Fire;

            return -1;
        }

        public static int get_attackflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.attack_flags);
        }
        public static int get_rflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.r_flags);
        }
        public static int get_mpflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.mprog_flags);
        }
        public static int get_oflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.o_flags);
        }
        public static int get_areaflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.area_flags);
        }

        public static int get_wflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.w_flags);
        }

        public static int get_actflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.act_flags);
        }

        public static int get_pcflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.pc_flags);
        }

        public static int get_plrflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.plr_flags);
        }

        public static int get_risflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.ris_flags);
        }

        public static int get_cmdflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.cmd_flags);
        }

        public static int get_trigflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.trig_flags);
        }

        public static int get_partflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.part_flags);
        }

        public static int get_defenseflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.defense_flags);
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
            foreach (LanguageData lang in db.LANGUAGES)
            {
                if (lang.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return count;
                count++;
            }
            return -1;
        }

        public static int get_npc_position(string value)
        {
            return GetIndexOf(value, BuilderConstants.npc_position);
        }

        public static int get_npc_sex(string value)
        {
            return GetIndexOf(value, BuilderConstants.npc_sex);
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
                LogManager.Bug("No descriptor");
                return;
            }

            if (ch.SubState == CharacterSubStates.Restricted)
            {
                LogManager.Bug("Restricted sub-state for Character {0}", ch.Name);
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
                LogManager.Bug("Missing descriptor {0}", ch.Name);
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

            int loc = get_atype(arg2);
            if (loc < 1)
            {
                color.ch_printf(ch, "Unknown field: %s\r\n", arg2);
                return;
            }

            int value = 0;
            bool found = false;
            int bitv = 0;

            if (loc == (int) ApplyTypes.Affect)
            {
                Tuple<string, string> tuple2 = arg.FirstArgument();

                value = get_aflag(tuple2.Item2);
                if (value < 0 || value >= EnumerationFunctions.Max<AffectedByTypes>())
                    color.ch_printf(ch, "Unknown affect: %s\r\n", tuple2.Item2);
                else
                    found = true;
            }
            else if (loc == (int) ApplyTypes.Resistance
                     || loc == (int) ApplyTypes.Immunity
                     || loc == (int) ApplyTypes.Susceptibility)
            {
                List<string> words = arg.ToWords();
                foreach (string word in words)
                {
                    value = get_risflag(word);
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
            else if (loc == (int) ApplyTypes.WeaponSpell
                     || loc == (int) ApplyTypes.WearSpell
                     || loc == (int) ApplyTypes.RemoveSpell
                     || loc == (int) ApplyTypes.StripSN
                     || loc == (int) ApplyTypes.RecurringSpell)
            {
                value = db.LookupSkill(arg);
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
                foreach(CharacterInstance vch in ch.CurrentRoom.Persons)
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
                            foreach(CharacterInstance vch in ch.CurrentRoom.Persons)
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
                LogManager.Bug("{0}: d.ConnectionStatus != Editing", ch.Name);
                return;
            }

            if ((int) ch.SubState <= (int) CharacterSubStates.Pause)
            {
                color.send_to_char("You can't do that!\r\n", ch);
                LogManager.Bug("{0}: Illegal Character SubState {1}", ch.Name, ch.SubState);
                d.ConnectionStatus = ConnectionTypes.Playing;
                return;
            }

            if (ch.CurrentEditor == null)
            {
                color.send_to_char("You can't do that!\r\n", ch);
                LogManager.Bug("{0}: Null Editor", ch.Name);
                d.ConnectionStatus = ConnectionTypes.Playing;
                return;
            }

            EditorData edit = ch.CurrentEditor;
            bool save = false;

            if (argument.StartsWith("/") || argument.StartsWith("\\"))
            {
                Tuple<string, string> tuple = argument.FirstArgument();
                string arg1 = tuple.Item1;
                string arg2 = tuple.Item2;

                if (arg1.StartsWith("/?"))
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
                    if (ch.Trust > Program.LEVEL_IMMORTAL)
                        color.send_to_char("/! <command>    execute command (do not use another editing command)\r\n", ch);
                    color.send_to_char("/s              save buffer\r\n\r\n> ", ch);
                    return;
                }

                if (arg1.StartsWith("/c"))
                {
                    edit.NumberOfLines = 0;
                    edit.OnLine = 0;
                    color.send_to_char("Buffer cleared.\r\n", ch);
                    return;
                }


            }
        }

        public static void assign_area(CharacterInstance ch)
        {
            // TODO
        }

        public static ExtraDescriptionData SetRExtra(RoomTemplate room, string keywords)
        {
            // TODO
            return null;
        }

        public static bool DelRExtra(RoomTemplate room, string keywords)
        {
            // TODO
            return false;
        }

        public static ExtraDescriptionData SetOExtra(ObjectInstance obj, string keywords)
        {
            // TODO
            return null;
        }

        public static bool DelOExtra(ObjectInstance obj, string keywords)
        {
            // TODO
            return false;
        }

        public static ExtraDescriptionData SetOExtraProto(ObjectTemplate obj, string keywords)
        {
            // TODO
            return null;
        }

        public static bool DelOExtraProto(ObjectTemplate obj, string keywords)
        {
            // TODO
            return false;
        }

        public static void fwrite_fuss_exdesc(FileStream fs, ExtraDescriptionData ed)
        {
            // TODO
        }

        public static void fwrite_fuss_extra(FileStream fs, ExitData pexit)
        {
            // TODO
        }

        public static void fwrite_fuss_affect(FileStream fs, AffectData paf)
        {
            // TODO
        }

        public static bool mprog_write_prog(FileStream fs, MudProgData mprog)
        {
            // TODO
            return false;
        }

        public static void save_reset_level(FileStream fs, ResetData start_reset, int level)
        {
            // TODO
        }

        public static void fwrite_fuss_room(FileStream fs, RoomTemplate room, bool install)
        {
            // TODO
        }

        public static void fwrite_fuss_object(FileStream fs, ObjectTemplate pObjIndex, bool install)
        {
            // TODO
        }

        public static void fwrite_fuss_mobile(FileStream fs, MobTemplate pMobIndex, bool install)
        {
            // TODO
        }

        public static void fwrite_area_header(FileStream fs, AreaData tarea, bool install)
        {
            // TODO
        }

        public static void fold_area(AreaData tarea, string fname, bool install)
        {
            // TODO
        }

        public static void old_fold_area(AreaData tarea, string filename, bool install)
        {
            // TODO
        }

        public static void write_area_list()
        {
            // TODO
        }

        public static bool check_for_area_conflicts(AreaData carea, int lo, int hi)
        {
            // TODO
            return false;
        }

        public static void RelCreate(RelationTypes tp, object actor, object subject)
        {
            // TODO
        }

        public static void RelDestroy(RelationTypes tp, object actor, object subject)
        {
            // TODO
        }
    }
}
