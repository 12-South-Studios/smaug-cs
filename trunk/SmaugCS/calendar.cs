
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Common;

namespace SmaugCS
{
    public static class calendar
    {
        public static bool WinterFreeze { get; set; }

        private static readonly List<TimezoneData> TimezoneTable = new List<TimezoneData>
            {
                new TimezoneData("GMT-12", "Eniwetok", -12, 0),
                new TimezoneData("GMT-11", "Samoa", -11, 0),
                new TimezoneData("GMT-10", "Hawaii", -10, 0),
                new TimezoneData("GMT-9", "Alaska", -9, 0),
                new TimezoneData("GMT-8", "Pacific US", -8, -7),
                new TimezoneData("GMT-7", "Mountain US", -7, -6),
                new TimezoneData("GMT-6", "Central US", -6, -5),
                new TimezoneData("GMT-5", "Eastern US", -5, -4),
                new TimezoneData("GMT-4", "Atlantic, Canada", -4, 0),
                new TimezoneData("GMT-3", "Brazilia, Buenos Aries", -3, 0),
                new TimezoneData("GMT-2", "Mid-Atlantic", -2, 0),
                new TimezoneData("GMT-1", "Cape Verdes", -1, 0),
                new TimezoneData("GMT", "Greenwich Mean Time, Greenwich", 0, 0),
                new TimezoneData("GMT+1", "Berlin, Rome", 1, 0),
                new TimezoneData("GMT+2", "Israel, Cairo", 2, 0),
                new TimezoneData("GMT+3", "Moscow, Kuwait", 3, 0),
                new TimezoneData("GMT+4", "Abu Dhabi, Muscat", 4, 0),
                new TimezoneData("GMT+5", "Islamabad, Karachi", 5, 0),
                new TimezoneData("GMT+6", "Almaty, Dhaka", 6, 0),
                new TimezoneData("GMT+7", "Bangkok, Jakarta", 7, 0),
                new TimezoneData("GMT+8", "Hong Kong, Beijing", 8, 0),
                new TimezoneData("GMT+9", "Tokyo, Osaka", 9, 0),
                new TimezoneData("GMT+10", "Sydney, Melbourne, Guam", 10, 0),
                new TimezoneData("GMT+11", "Magadan, Soloman Is", 11, 0),
                new TimezoneData("GMT+12", "Fiji, Wellington, Auckland", 12, 0)
            };

        public static int tzone_lookup(string arg)
        {
            int count = 0;
            foreach (TimezoneData tz in TimezoneTable)
            {
                if (tz.Name.EqualsIgnoreCase(arg) || tz.Zone.EqualsIgnoreCase(arg))
                    return count;
                count++;
            }
            return -1;
        }

        public static string c_time(DateTime curtime, int tz)
        {
            // TODO
            return string.Empty;
        }

        public static string mini_c_time(DateTime curtime, int tz)
        {
            // TODO
            return string.Empty;
        }

        //public static int DUR_ADDS()

        public static string sec_to_hms(DateTime loctime, string tstr)
        {
            // TODO
            return string.Empty;
        }

        public static HolidayData get_holiday(int month, int day)
        {
            return db.HOLIDAYS.FirstOrDefault(holiday => month + 1 == holiday.Month 
                && day + 1 == holiday.Day);
        }

        public static TimeInfoData load_timedata()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "time.dat";
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                TimeInfoData timeInfo = new TimeInfoData();

                List<string> lines = proxy.ReadIntoList();
                foreach (string line in lines.Where(x => !x.StartsWith("*")))
                {
                    if (line.StartsWith("#TIME"))
                    {
                        timeInfo.Load(lines);
                        break;
                    }
                    if (line.Equals("END"))
                        break;
                }

                return timeInfo;
            }
        }

        public static void start_winter()
        {
            act_wiz.echo_to_all(ATTypes.AT_CYAN, "The air takes on a chilling cold as winter sets in.", (int)EchoTypes.All);
            act_wiz.echo_to_all(ATTypes.AT_CYAN, "Freshwater bodies everywhere have frozen over.\r\n", (int)EchoTypes.All);

            WinterFreeze = true;
            foreach (RoomTemplate room in db.ROOMS
                .Where(x => x.SectorType == SectorTypes.DeepWater 
                    || x.SectorType == SectorTypes.ShallowWater))
            {
                room.WinterSector = room.SectorType;
                room.SectorType = SectorTypes.Ice;
            }
        }

        public static void start_spring()
        {
            act_wiz.echo_to_all(ATTypes.AT_DGREEN, "The chill recedes from the air as spring begins to take hold.", (int)EchoTypes.All);
            act_wiz.echo_to_all(ATTypes.AT_BLUE, "Freshwater bodies everywhere have thawed out.\r\n", (int)EchoTypes.All);

            WinterFreeze = true;
            foreach (RoomTemplate room in db.ROOMS
                .Where(x => x.SectorType == SectorTypes.Ice
                    && x.SectorType != SectorTypes.Unknown))
            {
                room.SectorType = room.WinterSector;
                room.WinterSector = SectorTypes.Unknown;
            }
        }

        public static void start_summer()
        {
            act_wiz.echo_to_all(ATTypes.AT_YELLOW, "The days grow longer and hotter as summer grips the world.\r\n", (int)EchoTypes.All);
        }

        public static void start_fall()
        {
            act_wiz.echo_to_all(ATTypes.AT_ORANGE, "The leaves begin changing colors signaling the start of fall.\r\n", (int)EchoTypes.All);
        }

        public static void season_update()
        {
            HolidayData day = get_holiday(db.GameTime.Month, db.GameTime.Day);
            if (day != null)
            {
                if (db.GameTime.Day + 1 == day.Day && db.GameTime.Hour == 0)
                {
                    act_wiz.echo_to_all(ATTypes.AT_IMMORT, day.Announce, (int)EchoTypes.All);
                }
            }

            if (db.GameTime.Season == SeasonTypes.Winter && !WinterFreeze)
            {
                WinterFreeze = true;
                foreach (RoomTemplate room in db.ROOMS
                    .Where(x => x.SectorType == SectorTypes.DeepWater
                        || x.SectorType == SectorTypes.ShallowWater))
                {
                    room.WinterSector = room.SectorType;
                    room.SectorType = SectorTypes.Ice;
                }
            }
        }

        public static void calc_season()
        {
            int day = db.GameTime.Month*db.SystemData.DaysPerMonth + db.GameTime.Day;
            if (day < db.SystemData.DaysPerYear / 4)
            {
                db.GameTime.Season = SeasonTypes.Spring;
                if (db.GameTime.Hour == 0 && day == 0)
                    start_spring();
            }
            else if (day < (db.SystemData.DaysPerYear/4)*2)
            {
                db.GameTime.Season = SeasonTypes.Summer;
                if (db.GameTime.Hour == 0 && day == (db.SystemData.DaysPerYear / 4))
                    start_summer();
            }
            else if (day < (db.SystemData.DaysPerYear/4)*3)
            {
                db.GameTime.Season = SeasonTypes.Fall;
                if (db.GameTime.Hour == 0 && day == (db.SystemData.DaysPerYear/4)*2)
                    start_fall();
            }
            else if (day < db.SystemData.DaysPerYear)
            {
                db.GameTime.Season = SeasonTypes.Winter;
                if (db.GameTime.Hour == 0 && day == (db.SystemData.DaysPerYear/4)*3)
                    start_winter();
            }
            else
                db.GameTime.Season = SeasonTypes.Spring;
            
            season_update();
        }

        public static void load_holidays()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Holiday);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                int dayCount = 0;
                List<TextSection> sections = proxy.ReadSections(new[] {"#HOLIDAY"}, new[]{"*"}, null, "END");
                foreach (TextSection section in sections)
                {
                    if (dayCount >= db.SystemData.MaxHolidays)
                    {
                        LogManager.Bug("Exceeded maximum holidays {0}", dayCount);
                        return;
                    }

                    HolidayData newHoliday = new HolidayData();
                    newHoliday.Load(section);
                    dayCount++;
                    db.HOLIDAYS.Add(newHoliday);
                }
            }
        }

        public static void save_holidays()
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Holiday);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                foreach (HolidayData day in db.HOLIDAYS)
                {
                    proxy.Write("#HOLIDAY\n");
                    proxy.Write("Name     {0}~\n", day.Name);
                    proxy.Write("Announce {0}~\n", day.Announce);
                    proxy.Write("Month    {0}\n", day.Month);
                    proxy.Write("Day      {0}\n", day.Day);
                    proxy.Write("End\n\n");
                }
                proxy.Write("#END\n");
            }
        }
    }
}
