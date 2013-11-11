using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Managers;
using SmaugCS.Objects;

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

        public static int GetTimezone(string arg)
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

        public static void start_winter()
        {
            act_wiz.echo_to_all(ATTypes.AT_CYAN, "The air takes on a chilling cold as winter sets in.", (int)EchoTypes.All);
            act_wiz.echo_to_all(ATTypes.AT_CYAN, "Freshwater bodies everywhere have frozen over.\r\n", (int)EchoTypes.All);

            WinterFreeze = true;
            foreach (RoomTemplate room in DatabaseManager.Instance.ROOMS.Values
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
            foreach (RoomTemplate room in DatabaseManager.Instance.ROOMS.Values
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
            HolidayData day = db.GetHoliday(db.GameTime.Month, db.GameTime.Day);
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
                foreach (RoomTemplate room in DatabaseManager.Instance.ROOMS.Values
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
            int day = db.GameTime.Month * db.SystemData.DaysPerMonth + db.GameTime.Day;
            if (day < db.SystemData.DaysPerYear / 4)
            {
                db.GameTime.Season = SeasonTypes.Spring;
                if (db.GameTime.Hour == 0 && day == 0)
                    start_spring();
            }
            else if (day < (db.SystemData.DaysPerYear / 4) * 2)
            {
                db.GameTime.Season = SeasonTypes.Summer;
                if (db.GameTime.Hour == 0 && day == (db.SystemData.DaysPerYear / 4))
                    start_summer();
            }
            else if (day < (db.SystemData.DaysPerYear / 4) * 3)
            {
                db.GameTime.Season = SeasonTypes.Fall;
                if (db.GameTime.Hour == 0 && day == (db.SystemData.DaysPerYear / 4) * 2)
                    start_fall();
            }
            else if (day < db.SystemData.DaysPerYear)
            {
                db.GameTime.Season = SeasonTypes.Winter;
                if (db.GameTime.Hour == 0 && day == (db.SystemData.DaysPerYear / 4) * 3)
                    start_winter();
            }
            else
                db.GameTime.Season = SeasonTypes.Spring;

            season_update();
        }
    }
}
