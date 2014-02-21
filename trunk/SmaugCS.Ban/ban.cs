//using System;
//using System.Collections.Generic;
//using System.IO;
//using Realm.Library.Common;
//using SmaugCS.Common;
//using SmaugCS.Constants.Constants;
//using SmaugCS.Constants.Enums;
//using Realm.Library.Common.Extensions;

//namespace SmaugCS.Ban
//{
//    public static class ban
//    {

//        public static int add_ban(CharacterInstance ch, string arg1, string arg2, int btime, int type)
//        {
//            switch (ch.SubState)
//            {
//                case CharacterSubStates.Restricted:
//                    color.send_to_char("You cannot use this command from within another command.\r\n", ch);
//                    return 0;
//                case CharacterSubStates.None:
//                    Tuple<string, string> tuple = arg1.FirstArgument();

//                    string arg = tuple.Item1.Replace('~', '\0');
//                    string arg1Remainder = tuple.Item2;

//                    if (string.IsNullOrWhiteSpace(arg) || string.IsNullOrWhiteSpace(arg2))
//                        return 0;

//                    int level = GetBanLevelFromArgument(ch, arg2);
//                    if (level == 0)
//                        return 0;

//                    BanTypes banType = EnumerationExtensions.GetEnum<BanTypes>(type);
//                    BanData newBan = new BanData(banType);

//                    int value;
//                    switch (banType)
//                    {
//                        case BanTypes.Class:
//                            if (string.IsNullOrWhiteSpace(arg))
//                                return 0;

//                            if (arg.IsNumber())
//                                value = arg.ToInt32();
//                            else
//                            {
//                                int count = 0;
//                                foreach (ClassData cls in DatabaseManager.Instance.CLASSES)
//                                {
//                                    if (cls.Name.EqualsIgnoreCase(arg))
//                                        break;
//                                    count++;
//                                }
//                                value = count;
//                            }

//                            if (value < 0 || value >= DatabaseManager.Instance.CLASSES.Count())
//                            {
//                                color.send_to_char("Unknown class.\r\n", ch);
//                                return 0;
//                            }

//                            foreach (BanData cBan in db.BANS
//                                .Where(x => x.Type == BanTypes.Class)
//                                .Where(cBan => cBan.Flag == value))
//                            {
//                                if (cBan.Level == level)
//                                {
//                                    color.send_to_char("That entry already exists.\r\n", ch);
//                                    return 0;
//                                }

//                                cBan.Level = level;
//                                cBan.Warn = (cBan.Level == (int)BanTypes.Warn);
//                                cBan.BannedOn = DateTime.Now;
//                                cBan.Duration = btime > 0 ? btime : -1;
//                                cBan.UnbanDate = btime > 0
//                                                     ? DateTime.Now.Add(new TimeSpan(btime, 0, 0))
//                                                     : DateTime.MaxValue;
//                                cBan.BannedBy = ch.Name;
//                                color.send_to_char("Updated entry.\r\n", ch);
//                                return 1;
//                            }

//                            newBan.Name = DatabaseManager.Instance.CLASSES.ToList()[value].Name;
//                            newBan.Flag = value;
//                            newBan.Level = level;
//                            newBan.BannedBy = ch.Name;
//                            db.BANS.Add(newBan);
//                            break;
//                        case BanTypes.Race:
//                            if (arg.IsNumber())
//                                value = arg.ToInt32();
//                            else
//                            {
//                                int count = 0;
//                                foreach (RaceData race in DatabaseManager.Instance.RACES)
//                                {
//                                    if (race.Name.EqualsIgnoreCase(arg))
//                                        break;
//                                    count++;
//                                }
//                                value = count;
//                            }

//                            if (value < 0 || value >= DatabaseManager.Instance.RACES.Count())
//                            {
//                                color.send_to_char("Unknown race.\r\n", ch);
//                                return 0;
//                            }

//                            foreach (BanData rBan in db.BANS
//                                .Where(x => x.Type == BanTypes.Race)
//                                .Where(rBan => rBan.Flag == value))
//                            {
//                                if (rBan.Level == level)
//                                {
//                                    color.send_to_char("That entry already exists.\r\n", ch);
//                                    return 0;
//                                }

//                                rBan.Level = level;
//                                rBan.Warn = (rBan.Level == (int)BanTypes.Warn);
//                                rBan.BannedOn = DateTime.Now;
//                                rBan.Duration = btime > 0 ? btime : -1;
//                                rBan.UnbanDate = btime > 0
//                                                     ? DateTime.Now.Add(new TimeSpan(btime, 0, 0))
//                                                     : DateTime.MaxValue;
//                                rBan.BannedBy = ch.Name;
//                                color.send_to_char("Updated entry.\r\n", ch);
//                                return 1;
//                            }
//                            newBan.Name = DatabaseManager.Instance.RACES.ToList()[value].Name;
//                            newBan.Flag = value;
//                            newBan.Level = level;
//                            newBan.BannedBy = ch.Name;
//                            db.BANS.Add(newBan);
//                            break;
//                        case BanTypes.Site:
//                            bool userName = false;
//                            string tempHost = string.Empty;
//                            string tempUser = string.Empty;

//                            char[] letters = arg.ToCharArray();

//                            for (int i = 0; i < letters.Length; i++)
//                            {
//                                if (letters[i] == '@')
//                                {
//                                    userName = true;
//                                    tempHost += letters[i];
//                                    tempUser = arg;
//                                }
//                            }

//                            string name = !userName ? arg : tempHost;
//                            if (!string.IsNullOrEmpty(name))
//                            {
//                                color.send_to_char("Name was null.\r\n", ch);
//                                return 0;
//                            }

//                            bool prefix = name.ToCharArray()[0] == '*';

//                            // TODO Finish site bans
//                            break;
//                        default:
//                            LogManager.Instance.Bug("Bad type {0}", type);
//                            return 0;
//                    }

//                    newBan.BannedOn = DateTime.Now;
//                    newBan.Duration = btime > 0 ? btime : -1;
//                    newBan.UnbanDate = btime > 0 ? DateTime.Now.Add(new TimeSpan(btime, 0, 0)) : DateTime.MaxValue;
//                    newBan.Warn = (newBan.Level == (int)BanTypes.Warn);

//                    ch.SubState = CharacterSubStates.BanDescription;
//                    ch.DestinationBuffer = newBan;
//                    build.start_editing(ch, newBan.Note);
//                    return 1;

//                case CharacterSubStates.BanDescription:
//                    BanData ban = ch.DestinationBuffer.CastAs<BanData>();
//                    if (ban == null)
//                    {
//                        LogManager.Instance.Bug("Null Dest_buff in Character {0}", ch.Name);
//                        ch.SubState = CharacterSubStates.None;
//                        return 0;
//                    }

//                    if (!string.IsNullOrEmpty(ban.Note))
//                        ban.Note = string.Empty;

//                    ban.Note = build.copy_buffer(ch);
//                    build.stop_editing(ch);
//                    ch.SubState = EnumerationExtensions.GetEnum<CharacterSubStates>(ch.tempnum);
//                    save_banlist();
//                    if (ban.Duration > 0)
//                        color.ch_printf(ch, "%s banned for %d days.\r\n", ban.Name, ban.Duration);
//                    else
//                        color.ch_printf(ch, "%s banned forever.\r\n", ban.Name);
//                    return 1;
//                default:
//                    LogManager.Instance.Bug("Illegal substate {0}", ch.SubState);
//                    return 0;
//            }
//        }

//        private static int GetBanLevelFromArgument(CharacterInstance ch, string arg2)
//        {
//            int level;
//            if (arg2.IsNumber())
//            {
//                level = arg2.ToInt32();
//                if (level < 0 || level > LevelConstants.GetLevel("supreme"))
//                {
//                    color.ch_printf(ch, "Level range is from 0 to %d.\r\n", LevelConstants.GetLevel("supreme"));
//                    level = 0;
//                }
//                return level;
//            }

//            switch (arg2.ToLower())
//            {
//                case "all":
//                    level = LevelConstants.GetLevel("supreme");
//                    break;
//                case "newbie":
//                    level = 1;
//                    break;
//                case "mortal":
//                    level = LevelConstants.GetLevel("avatar");
//                    break;
//                case "warn":
//                    level = (int)BanTypes.Warn;
//                    break;
//                default:
//                    LogManager.Instance.Bug("Bad string for flag {0}", arg2);
//                    level = 0;
//                    break;
//            }
//            return level;
//        }

//        public static void show_bans(CharacterInstance ch, int type)
//        {
//            color.set_pager_color(ATTypes.AT_IMMORT, ch);
//            int count = 1;

//            BanTypes banType = EnumerationExtensions.GetEnum<BanTypes>(type);
//            switch (banType)
//            {
//                case BanTypes.Site:
//                    color.send_to_pager("Banned sites:\r\n", ch);
//                    color.send_to_pager("[ #] Warn (Lv) Time                     By              For   Site\r\n", ch);
//                    color.send_to_pager("---- ---- ---- ------------------------ --------------- ----  ---------------\r\n", ch);

//                    foreach (BanData ban in db.BANS.Where(x => x.Type == BanTypes.Site))
//                    {
//                        color.pager_printf(ch, "[%2d] %-4s (%2d) %-24s %-15s %4d  %c%s%c\r\n",
//                                           count, ban.Warn ? "YES" : "no", ban.Level, ban.BannedOn,
//                                           ban.BannedBy, ban.Duration, ban.Prefix ? "*" : " ", ban.Name,
//                                           ban.Suffix ? "*" : " ");
//                        count++;
//                    }
//                    return;

//                case BanTypes.Race:
//                    color.send_to_pager("Banned races:\r\n", ch);
//                    color.send_to_pager("[ #] Warn (Lv) Time                     By              For   Race\r\n", ch);
//                    break;
//                case BanTypes.Class:
//                    color.send_to_pager("Banned classes:\r\n", ch);
//                    color.send_to_pager("[ #] Warn (Lv) Time                     By              For   Class\r\n", ch);
//                    break;
//                default:
//                    LogManager.Instance.Bug("Invalid type {0}", type);
//                    return;
//            }

//            color.send_to_pager("---- ---- ---- ------------------------ --------------- ----  ---------------\r\n", ch);
//            color.set_pager_color(ATTypes.AT_PLAIN, ch);

//            foreach (BanData ban in db.BANS.Where(x => x.Type != BanTypes.Site && x.Type != BanTypes.Warn))
//            {
//                color.pager_printf(ch, "[%2d] %-4s (%2d) %-24s %-15s %4d  %s\r\n",
//                                   count, ban.Warn ? "YES" : "no", ban.Level, ban.BannedOn,
//                                   ban.BannedBy, ban.Duration, ban.Name);
//                count++;
//            }
//        }
//    }
//}
