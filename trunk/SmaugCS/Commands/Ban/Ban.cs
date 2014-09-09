﻿using System;
using Realm.Library.Common;
using SmaugCS.Ban;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Admin
{
    public static class Ban
    {
        public static void do_ban(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc(), "Monsters are too dumb to do that!")) return;

            PlayerInstance pch = (PlayerInstance) ch;

            color.set_char_color(ATTypes.AT_IMMORT, ch);
            string[] args = new string[4];
            args[0] = argument.ParseWord(1, " ");
            args[1] = argument.ParseWord(2, " ");
            args[2] = argument.ParseWord(3, " ");
            args[3] = argument.ParseWord(4, " ");

            int tempTime = args[3].IsNullOrWhitespace() && !args[3].IsNumber() ? -1 : Convert.ToInt32(args[3]);
            if (CheckFunctions.CheckIfTrue(ch, tempTime != -1 && (tempTime < 1 || tempTime > 1000),
                "Time value is -1 (forever) or from 1 to 1000.")) return;

            // Convert the value from DAYS to SECONDS
            int duration = tempTime > 0 ? (tempTime * 86400) : tempTime;

            if (CheckFunctions.CheckIfTrue(pch, pch.SubState == CharacterSubStates.Restricted,
                "You cannot use this command from within another command.")) return;

            if (pch.SubState == CharacterSubStates.None)
                pch.tempnum = (int) CharacterSubStates.None;
            else if (pch.SubState == CharacterSubStates.BanDescription)
            {
                // TODO: add_ban(ch, "", "", 0, 0);
                return;
            }
            else
            {
                LogManager.Instance.Bug("Illegal Characer Substate (Name={0}, SubState={1}", pch.Name, pch.SubState);
                return;
            }

            if (args[0].IsNullOrWhitespace())
            {
                SendSyntaxMessage(ch);
                return;
            }

            // site, race, class, show (default is null)
            if (args[0].EqualsIgnoreCase("site"))
                DoSiteBan(ch, args, duration);
            else if (args[0].EqualsIgnoreCase("race"))
                DoRaceBan(ch, args, duration);
            else if (args[0].EqualsIgnoreCase("class"))
                DoClassBan(ch, args, duration);
            else if (args[0].EqualsIgnoreCase("show") || args[0].IsNullOrWhitespace())
                DoShowBans(ch, args);
            else 
                SendSyntaxMessage(ch);
        }

        private static void SendSyntaxMessage(CharacterInstance ch)
        {
            color.send_to_char("Syntax: ban site  <address> <type> <duration>\r\n", ch);
            color.send_to_char("Syntax: ban race  <race>    <type> <duration>\r\n", ch);
            color.send_to_char("Syntax: ban class <class>   <type> <duration>\r\n", ch);
            color.send_to_char("Syntax: ban show  <field>   <number>\r\n", ch);
            color.send_to_char("Ban site lists current bans.\r\n", ch);
            color.send_to_char("Duration is the length of the ban in days.\r\n", ch);
            color.send_to_char("Type can be:  newbie, mortal, all, warn or level.\r\n", ch);
            color.send_to_char("In ban show, the <field> is site, race or class,", ch);
            color.send_to_char("  and the <number> is the ban number.\r\n", ch);
        }

        private static void DoSiteBan(CharacterInstance ch, string[] args, int duration)
        {
            if (args[1].IsNullOrWhitespace())
            {
                ShowBans(ch, BanTypes.Site);
                return;
            }

            if (ch.Trust < GameManager.Instance.SystemData.ban_site_level)
            {
                color.ch_printf(ch, "You must be {0} level to add bans.", GameManager.Instance.SystemData.ban_site_level);
                return;
            }

            if (args[2].IsNullOrWhitespace())
            {
                SendSyntaxMessage(ch);
                return;
            }
            
            // TODO: add_ban(ch, args[1], arg[2], duration, BanTypes.Site);
        }

        private static void DoRaceBan(CharacterInstance ch, string[] args, int duration)
        {

        }

        private static void DoClassBan(CharacterInstance ch, string[] args, int duration)
        {

        }

        private static void DoShowBans(CharacterInstance ch, string[] args)
        {

        }

        private static void ShowBans(CharacterInstance ch, BanTypes type)
        {
            
        }

        public static int AddBan(PlayerInstance ch, string arg1, string arg2, int duration, BanTypes type)
        {
            if (ch.SubState == CharacterSubStates.Restricted)
            {
                color.send_to_char("You cannot use this command from within another command.\r\n", ch);
                return 0;
            }

            if (ch.SubState == CharacterSubStates.BanDescription)
                return AddBanDescription(ch, arg1, arg2, duration, type);


            return 0;
        }

        private static int AddBanDescription(PlayerInstance ch, string arg1, string arg2, int duration, BanTypes type)
        {
            BanData ban = ch.DestinationBuffer.CastAs<BanData>();
            if (ban == null)
            {
                LogManager.Instance.Bug("Null DestinationBuffer for character {0}", ch.Name);
                ch.SubState = CharacterSubStates.None;
                return 0;
            }

            if (!ban.Note.IsNullOrWhitespace())
                ban.Note = string.Empty;

            //ban.Note = build.copy_buffer(ch);
            //build.stop_editing(ch);

            ch.SubState = EnumerationExtensions.GetEnum<CharacterSubStates>(ch.tempnum);
            BanManager.Instance.AddBan(ban);

            if (ban.Duration > 0)
                color.ch_printf(ch, "{0} is banned for {1} days.\r\n", ban.Name, ban.Duration / 86400);
            else 
                color.ch_printf(ch, "{0} is banned forever.\r\n", ban.Name);

            return 1;
        }

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
        //                    level = LevelConstants.AvatarLevel;
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
    }
}
