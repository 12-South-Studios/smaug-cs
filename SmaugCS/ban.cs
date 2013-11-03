
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class ban
    {
        public static void load_banlist()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Bans);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<TextSection> sections = proxy.ReadSections(new[] { "CLASS", "RACE", "SITE" }, null, null, "END");
                foreach (TextSection section in sections)
                {
                    BanTypes banType = EnumerationExtensions.GetEnum<BanTypes>(section.Header);
                    if (ReadBanTable.ContainsKey(banType))
                        ReadBanTable[banType].Invoke(section);
                    else
                        LogManager.Bug("Invalid ban type {0} for Section {1}", banType, section.ToString());
                }
            }
        }

        private static readonly Dictionary<BanTypes, Func<TextSection, BanData>> ReadBanTable = new Dictionary<BanTypes, Func<TextSection, BanData>>()
            {
                {BanTypes.Class, ReadClassBan},
                {BanTypes.Race, ReadRaceBan},
                {BanTypes.Site, ReadSiteBan},
                {BanTypes.Warn, ReadWarnBan}
            };
        private static BanData ReadSiteBan(TextSection section)
        {
            BanData newBan = new BanData(BanTypes.Site);

            for (int i = 0; i < section.Lines.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        newBan.Name = section.Lines[i].TrimEnd('~');
                        break;
                    case 1:
                        string[] words = section.Lines[i].Split(' ');
                        newBan.Level = words[0].ToInt32();
                        newBan.Duration = words[1].ToInt32();

                        DateTime unbanDate;
                        DateTime.TryParse(words[2], out unbanDate);
                        newBan.UnbanDate = unbanDate;

                        newBan.Prefix = Convert.ToBoolean(words[3]);
                        newBan.Suffix = Convert.ToBoolean(words[4]);
                        newBan.Warn = Convert.ToBoolean(words[5]);
                        break;
                    case 2:
                        newBan.BannedBy = section.Lines[i].TrimEnd('~');
                        break;
                    case 3:
                        DateTime banTime;
                        DateTime.TryParse(section.Lines[i].TrimEnd('~'), out banTime);
                        newBan.BannedOn = banTime;
                        break;
                    case 4:
                        newBan.Note = section.Lines[i].TrimEnd('~');
                        break;
                    default:
                        LogManager.Bug("Unknown line '{0}' in Ban section {1}", section.Lines[i], section.Header);
                        break;
                }
            }

            db.BANS.Add(newBan);
            return newBan;
        }
        private static BanData ReadRaceBan(TextSection section)
        {
            BanData newBan = new BanData(BanTypes.Race);

            for (int i = 0; i < section.Lines.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        newBan.Name = section.Lines[i].TrimEnd('~');
                        break;
                    case 1:
                        string[] words = section.Lines[i].Split(' ');
                        newBan.Level = words[0].ToInt32();
                        newBan.Duration = words[1].ToInt32();

                        DateTime unbanDate;
                        DateTime.TryParse(words[2], out unbanDate);
                        newBan.UnbanDate = unbanDate;

                        newBan.Warn = Convert.ToBoolean(words[3]);
                        break;
                    case 2:
                        newBan.BannedBy = section.Lines[i].TrimEnd('~');
                        break;
                    case 3:
                        DateTime banTime;
                        DateTime.TryParse(section.Lines[i].TrimEnd('~'), out banTime);
                        newBan.BannedOn = banTime;
                        break;
                    case 4:
                        newBan.Note = section.Lines[i].TrimEnd('~');
                        break;
                    default:
                        LogManager.Bug("Unknown line '{0}' in Ban section {1}", section.Lines[i], section.Header);
                        break;
                }
            }

            db.BANS.Add(newBan);
            return newBan;
        }
        private static BanData ReadClassBan(TextSection section)
        {
            BanData newBan = new BanData(BanTypes.Class);

            for (int i = 0; i < section.Lines.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        newBan.Name = section.Lines[i].TrimEnd('~');
                        break;
                    case 1:
                        string[] words = section.Lines[i].Split(' ');
                        newBan.Level = words[0].ToInt32();
                        newBan.Duration = words[1].ToInt32();

                        DateTime unbanDate;
                        DateTime.TryParse(words[2], out unbanDate);
                        newBan.UnbanDate = unbanDate;

                        newBan.Warn = Convert.ToBoolean(words[3]);
                        break;
                    case 2:
                        newBan.BannedBy = section.Lines[i].TrimEnd('~');
                        break;
                    case 3:
                        DateTime banTime;
                        DateTime.TryParse(section.Lines[i].TrimEnd('~'), out banTime);
                        newBan.BannedOn = banTime;
                        break;
                    case 4:
                        newBan.Note = section.Lines[i].TrimEnd('~');
                        break;
                    default:
                        LogManager.Bug("Unknown line '{0}' in Ban section {1}", section.Lines[i], section.Header);
                        break;
                }
            }

            db.BANS.Add(newBan);
            return newBan;
        }
        private static BanData ReadWarnBan(TextSection section)
        {
            throw new NotImplementedException("ReadWarnBan is not implemented!");
        }

        public static void save_banlist()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Bans);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                db.BANS.Where(x => x.Type == BanTypes.Site).ToList().ForEach(x => SaveSiteBan(proxy, x));
                db.BANS.Where(x => x.Type == BanTypes.Race).ToList().ForEach(x => SaveRaceBan(proxy, x));
                db.BANS.Where(x => x.Type == BanTypes.Class).ToList().ForEach(x => SaveClassBan(proxy, x));
                proxy.Write("END\n");
            }
        }
        private static void SaveSiteBan(TextWriterProxy proxy, BanData ban)
        {
            proxy.Write("SITE\n");
            proxy.Write("{0}~\n", ban.Name);
            proxy.Write(string.Format("{0} {1} {2} {3} {4} {5}\n", ban.Level, ban.Duration,
                ban.UnbanDate.ToFileTimeUtc(), ban.Prefix, ban.Suffix, ban.Warn));
            proxy.Write("{0}~\n{1}~\n{2}~\n", ban.BannedBy, ban.BannedOn.ToFileTimeUtc(), ban.Note);
        }
        private static void SaveClassBan(TextWriterProxy proxy, BanData ban)
        {
            proxy.Write("CLASS\n");
            proxy.Write("{0}~\n", ban.Name);
            proxy.Write(string.Format("{0} {1} {2} {3}\n", ban.Level, ban.Duration,
                ban.UnbanDate.ToFileTimeUtc(), ban.Warn));
            proxy.Write("{0}~\n{1}~\n{2}~\n", ban.BannedBy, ban.BannedOn.ToFileTimeUtc(), ban.Note);
        }
        private static void SaveRaceBan(TextWriterProxy proxy, BanData ban)
        {
            proxy.Write("RACE\n");
            proxy.Write("{0}~\n", ban.Name);
            proxy.Write(string.Format("{0} {1} {2} {3}\n", ban.Level, ban.Duration,
                ban.UnbanDate.ToFileTimeUtc(), ban.Warn));
            proxy.Write("{0}~\n{1}~\n{2}~\n", ban.BannedBy, ban.BannedOn.ToFileTimeUtc(), ban.Note);
        }

        public static int add_ban(CharacterInstance ch, string arg1, string arg2, int btime, int type)
        {
            switch (ch.SubState)
            {
                case CharacterSubStates.Restricted:
                    color.send_to_char("You cannot use this command from within another command.\r\n", ch);
                    return 0;
                case CharacterSubStates.None:
                    Tuple<string, string> tuple = arg1.FirstArgument();

                    string arg = tuple.Item1.Replace('~', '\0');
                    string arg1Remainder = tuple.Item2;

                    if (string.IsNullOrWhiteSpace(arg) || string.IsNullOrWhiteSpace(arg2))
                        return 0;

                    int level = GetBanLevelFromArgument(ch, arg2);
                    if (level == 0)
                        return 0;

                    BanTypes banType = EnumerationExtensions.GetEnum<BanTypes>(type);
                    BanData newBan = new BanData(banType);

                    int value;
                    switch (banType)
                    {
                        case BanTypes.Class:
                            if (string.IsNullOrWhiteSpace(arg))
                                return 0;

                            if (arg.IsNumber())
                                value = arg.ToInt32();
                            else
                            {
                                int count = 0;
                                foreach (ClassData cls in db.CLASSES)
                                {
                                    if (cls.Name.EqualsIgnoreCase(arg))
                                        break;
                                    count++;
                                }
                                value = count;
                            }

                            if (value < 0 || value >= db.CLASSES.Count)
                            {
                                color.send_to_char("Unknown class.\r\n", ch);
                                return 0;
                            }

                            foreach (BanData cBan in db.BANS
                                .Where(x => x.Type == BanTypes.Class)
                                .Where(cBan => cBan.Flag == value))
                            {
                                if (cBan.Level == level)
                                {
                                    color.send_to_char("That entry already exists.\r\n", ch);
                                    return 0;
                                }

                                cBan.Level = level;
                                cBan.Warn = (cBan.Level == (int)BanTypes.Warn);
                                cBan.BannedOn = DateTime.Now;
                                cBan.Duration = btime > 0 ? btime : -1;
                                cBan.UnbanDate = btime > 0
                                                     ? DateTime.Now.Add(new TimeSpan(btime, 0, 0))
                                                     : DateTime.MaxValue;
                                cBan.BannedBy = ch.Name;
                                color.send_to_char("Updated entry.\r\n", ch);
                                return 1;
                            }

                            newBan.Name = db.CLASSES[value].Name;
                            newBan.Flag = value;
                            newBan.Level = level;
                            newBan.BannedBy = ch.Name;
                            db.BANS.Add(newBan);
                            break;
                        case BanTypes.Race:
                            if (arg.IsNumber())
                                value = arg.ToInt32();
                            else
                            {
                                int count = 0;
                                foreach (RaceData race in db.RACES)
                                {
                                    if (race.Name.EqualsIgnoreCase(arg))
                                        break;
                                    count++;
                                }
                                value = count;
                            }

                            if (value < 0 || value >= db.RACES.Count)
                            {
                                color.send_to_char("Unknown race.\r\n", ch);
                                return 0;
                            }

                            foreach (BanData rBan in db.BANS
                                .Where(x => x.Type == BanTypes.Race)
                                .Where(rBan => rBan.Flag == value))
                            {
                                if (rBan.Level == level)
                                {
                                    color.send_to_char("That entry already exists.\r\n", ch);
                                    return 0;
                                }

                                rBan.Level = level;
                                rBan.Warn = (rBan.Level == (int)BanTypes.Warn);
                                rBan.BannedOn = DateTime.Now;
                                rBan.Duration = btime > 0 ? btime : -1;
                                rBan.UnbanDate = btime > 0
                                                     ? DateTime.Now.Add(new TimeSpan(btime, 0, 0))
                                                     : DateTime.MaxValue;
                                rBan.BannedBy = ch.Name;
                                color.send_to_char("Updated entry.\r\n", ch);
                                return 1;
                            }
                            newBan.Name = db.RACES[value].Name;
                            newBan.Flag = value;
                            newBan.Level = level;
                            newBan.BannedBy = ch.Name;
                            db.BANS.Add(newBan);
                            break;
                        case BanTypes.Site:
                            bool userName = false;
                            string tempHost = string.Empty;
                            string tempUser = string.Empty;

                            char[] letters = arg.ToCharArray();

                            for (int i = 0; i < letters.Length; i++)
                            {
                                if (letters[i] == '@')
                                {
                                    userName = true;
                                    tempHost += letters[i];
                                    tempUser = arg;
                                }
                            }

                            string name = !userName ? arg : tempHost;
                            if (!string.IsNullOrEmpty(name))
                            {
                                color.send_to_char("Name was null.\r\n", ch);
                                return 0;
                            }

                            bool prefix = name.ToCharArray()[0] == '*';

                            // TODO Finish site bans
                            break;
                        default:
                            LogManager.Bug("Bad type {0}", type);
                            return 0;
                    }

                    newBan.BannedOn = DateTime.Now;
                    newBan.Duration = btime > 0 ? btime : -1;
                    newBan.UnbanDate = btime > 0 ? DateTime.Now.Add(new TimeSpan(btime, 0, 0)) : DateTime.MaxValue;
                    newBan.Warn = (newBan.Level == (int)BanTypes.Warn);

                    ch.SubState = CharacterSubStates.BanDescription;
                    ch.DestinationBuffer = newBan;
                    build.start_editing(ch, newBan.Note);
                    return 1;

                case CharacterSubStates.BanDescription:
                    BanData ban = ch.DestinationBuffer.CastAs<BanData>();
                    if (ban == null)
                    {
                        LogManager.Bug("Null Dest_buff in Character {0}", ch.Name);
                        ch.SubState = CharacterSubStates.None;
                        return 0;
                    }

                    if (!string.IsNullOrEmpty(ban.Note))
                        ban.Note = string.Empty;

                    ban.Note = build.copy_buffer(ch);
                    build.stop_editing(ch);
                    ch.SubState = EnumerationExtensions.GetEnum<CharacterSubStates>(ch.tempnum);
                    save_banlist();
                    if (ban.Duration > 0)
                        color.ch_printf(ch, "%s banned for %d days.\r\n", ban.Name, ban.Duration);
                    else
                        color.ch_printf(ch, "%s banned forever.\r\n", ban.Name);
                    return 1;
                default:
                    LogManager.Bug("Illegal substate {0}", ch.SubState);
                    return 0;
            }
        }

        private static int GetBanLevelFromArgument(CharacterInstance ch, string arg2)
        {
            int level;
            if (arg2.IsNumber())
            {
                level = arg2.ToInt32();
                if (level < 0 || level > Program.LEVEL_SUPREME)
                {
                    color.ch_printf(ch, "Level range is from 0 to %d.\r\n", Program.LEVEL_SUPREME);
                    level = 0;
                }
                return level;
            }

            switch (arg2.ToLower())
            {
                case "all":
                    level = Program.LEVEL_SUPREME;
                    break;
                case "newbie":
                    level = 1;
                    break;
                case "mortal":
                    level = Program.LEVEL_AVATAR;
                    break;
                case "warn":
                    level = (int)BanTypes.Warn;
                    break;
                default:
                    LogManager.Bug("Bad string for flag {0}", arg2);
                    level = 0;
                    break;
            }
            return level;
        }

        public static void show_bans(CharacterInstance ch, int type)
        {
            color.set_pager_color(ATTypes.AT_IMMORT, ch);
            int count = 1;

            BanTypes banType = EnumerationExtensions.GetEnum<BanTypes>(type);
            switch (banType)
            {
                case BanTypes.Site:
                    color.send_to_pager("Banned sites:\r\n", ch);
                    color.send_to_pager("[ #] Warn (Lv) Time                     By              For   Site\r\n", ch);
                    color.send_to_pager("---- ---- ---- ------------------------ --------------- ----  ---------------\r\n", ch);

                    foreach (BanData ban in db.BANS.Where(x => x.Type == BanTypes.Site))
                    {
                        color.pager_printf(ch, "[%2d] %-4s (%2d) %-24s %-15s %4d  %c%s%c\r\n",
                                           count, ban.Warn ? "YES" : "no", ban.Level, ban.BannedOn,
                                           ban.BannedBy, ban.Duration, ban.Prefix ? "*" : " ", ban.Name,
                                           ban.Suffix ? "*" : " ");
                        count++;
                    }
                    return;

                case BanTypes.Race:
                    color.send_to_pager("Banned races:\r\n", ch);
                    color.send_to_pager("[ #] Warn (Lv) Time                     By              For   Race\r\n", ch);
                    break;
                case BanTypes.Class:
                    color.send_to_pager("Banned classes:\r\n", ch);
                    color.send_to_pager("[ #] Warn (Lv) Time                     By              For   Class\r\n", ch);
                    break;
                default:
                    LogManager.Bug("Invalid type {0}", type);
                    return;
            }

            color.send_to_pager("---- ---- ---- ------------------------ --------------- ----  ---------------\r\n", ch);
            color.set_pager_color(ATTypes.AT_PLAIN, ch);

            foreach (BanData ban in db.BANS.Where(x => x.Type != BanTypes.Site && x.Type != BanTypes.Warn))
            {
                color.pager_printf(ch, "[%2d] %-4s (%2d) %-24s %-15s %4d  %s\r\n",
                                   count, ban.Warn ? "YES" : "no", ban.Level, ban.BannedOn,
                                   ban.BannedBy, ban.Duration, ban.Name);
                count++;
            }
        }

        private static bool CheckBanExpiration(BanData ban)
        {
            if (check_expire(ban))
            {
                //dispose_ban(ban, BAN_SITE);
                save_banlist();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check for totally banned sites
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool check_total_bans(DescriptorData d)
        {
            string host = d.host.ToLower();

            foreach (BanData ban in db.BANS.Where(ban => ban.Level == Program.LEVEL_SUPREME))
            {
                if (ban.Prefix && ban.Suffix && host.Contains(ban.Name))
                    return CheckBanExpiration(ban);

                if (ban.Suffix && host.StartsWith(ban.Name))
                    return CheckBanExpiration(ban);

                if (ban.Prefix && host.EndsWith(ban.Name))
                    return CheckBanExpiration(ban);

                if (host.EqualsIgnoreCase(ban.Name))
                    return CheckBanExpiration(ban);
            }

            return false;
        }

        public static bool check_bans(CharacterInstance ch, int type)
        {
            switch (type)
            {
                case (int)BanTypes.Race:
                    foreach (BanData ban in db.BANS.Where(x => x.Type == BanTypes.Race)
                        .Where(ban => ban.Flag == (int)ch.CurrentRace))
                    {
                        return CheckBanExpireAndLevel(ban, ch);
                    }
                    break;
                case (int)BanTypes.Class:
                    foreach (BanData ban in db.BANS.Where(x => x.Type == BanTypes.Class)
                        .Where(ban => ban.Flag == (int)ch.CurrentClass))
                    {
                        return CheckBanExpireAndLevel(ban, ch);
                    }
                    break;
                case (int)BanTypes.Site:
                    string host = ch.Descriptor.host.ToLower();
                    bool match = false;

                    foreach (BanData ban in db.BANS.Where(x => x.Type == BanTypes.Site))
                    {
                        if (ban.Prefix && ban.Suffix && host.Contains(ban.Name))
                            match = true;
                        else if (ban.Suffix && host.StartsWith(ban.Name))
                            match = true;
                        else if (ban.Prefix && host.EndsWith(ban.Name))
                            match = true;
                        else if (host.EqualsIgnoreCase(ban.Name))
                            match = true;

                        if (match)
                            return CheckBanExpireAndLevel(ban, ch);
                    }
                    break;
                default:
                    LogManager.Bug("Invalid ban type {0}", type);
                    return false;
            }

            return false;
        }

        private static bool CheckBanExpireAndLevel(BanData ban, CharacterInstance ch)
        {
            if (CheckBanExpiration(ban))
                return false;
            if (ch.Level == ban.Level)
            {
                if (ban.Warn)
                {
                    // TODO log_printf_plus
                }
                return false;
            }
            return true;
        }

        public static bool check_expire(BanData ban)
        {
            if (ban.UnbanDate == DateTime.MaxValue)
                return false;

            if (ban.UnbanDate <= DateTime.Now)
            {
                LogManager.Log(LogTypes.Warn, db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LogLevel),
                               "{0} ban has expired.", ban.Name);
                return true;
            }

            return false;
        }

        public static void dispose_ban(BanData ban, int type)
        {
            if (ban == null || (type != (int)BanTypes.Class
                                && type != (int)BanTypes.Race
                                && type != (int)BanTypes.Site))
            {
                LogManager.Bug("Unknown Ban Type {0}", type);
                return;
            }

            db.BANS.Remove(ban);
        }
    }
}
