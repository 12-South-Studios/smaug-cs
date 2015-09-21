using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Objects;
using SmaugCS.Repository;

namespace SmaugCS.Managers
{
    public sealed class CalendarManager : ICalendarManager
    {
        private SystemData _systemData;
        private static bool _winterFreeze;

        private readonly ILogManager _logManager;
        private readonly IGameManager _gameManager;
        private readonly ISmaugDbContext _dbContext;

        public TimeInfoData GameTime { get; private set; }

        public CalendarManager(ILogManager logManager, IGameManager gameManager, ISmaugDbContext dbContext)
        {
            _logManager = logManager;
            _gameManager = gameManager;
            _dbContext = dbContext;
        }

        public static ICalendarManager Instance => Program.Kernel.Get<ICalendarManager>();

        public void Initialize()
        {
            _systemData = _gameManager.SystemData;
            _logManager.Boot("Setting time and weather.");

            try
            {
                TimeInfoData timeInfo;

                var gameState = _dbContext.GameStates.FirstOrDefault();
                if (gameState == null)
                    timeInfo = new TimeInfoData {Day = 28, Hour = 0, Month = 6, Year = 628};
                else
                {
                    timeInfo = new TimeInfoData
                    {
                        Year = gameState.GameYear,
                        Month = gameState.GameMonth,
                        Day = gameState.GameDay,
                        Hour = gameState.GameHour
                    };
                }

                UpdateGameTime(timeInfo);
                GameTime = timeInfo;
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
            }
        }

        private void UpdateGameTime(TimeInfoData timeInfo)
        {
            _gameManager.SetGameTime(timeInfo);

            _logManager.Boot("Resetting mud time based on current system time.");
            var lhour = (DateTime.Now.ToFileTimeUtc() - 650336715) /
                         (GameConstants.GetSystemValue<int>("PulseTick") / GameConstants.GetSystemValue<int>("PulsesPerSecond"));
            _gameManager.GameTime.Hour = (int)(lhour % GameConstants.GetSystemValue<int>("HoursPerDay"));

            var lday = lhour / GameConstants.GetSystemValue<int>("HoursPerDay");
            _gameManager.GameTime.Day = (int)(lday % GameConstants.GetSystemValue<int>("DaysPerMonth"));

            var lmonth = lday / GameConstants.GetSystemValue<int>("DaysPerMonth");
            _gameManager.GameTime.Month = (int)(lmonth % GameConstants.GetSystemValue<int>("MonthsPerYear"));

            _gameManager.GameTime.Year = (int)(lmonth % GameConstants.GetSystemValue<int>("MonthsPerYear"));
        }

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
            var count = 0;
            foreach (var tz in TimezoneTable)
            {
                if (tz.Name.EqualsIgnoreCase(arg) || tz.Zone.EqualsIgnoreCase(arg))
                    return count;
                count++;
            }
            return -1;
        }

        private static void StartWinterSeason()
        {
            act_wiz.echo_to_all(ATTypes.AT_CYAN, "The air takes on a chilling cold as winter sets in.", (int)EchoTypes.All);
            act_wiz.echo_to_all(ATTypes.AT_CYAN, "Freshwater bodies everywhere have frozen over.\r\n", (int)EchoTypes.All);

            _winterFreeze = true;
            foreach (var room in RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values
                .Where(x => x.SectorType == SectorTypes.DeepWater
                    || x.SectorType == SectorTypes.ShallowWater))
            {
                room.WinterSector = room.SectorType;
                room.SectorType = SectorTypes.Ice;
            }
        }

        private static void StartSpringSeason()
        {
            act_wiz.echo_to_all(ATTypes.AT_DGREEN, "The chill recedes from the air as spring begins to take hold.",
                                (int) EchoTypes.All);
            act_wiz.echo_to_all(ATTypes.AT_BLUE, "Freshwater bodies everywhere have thawed out.\r\n",
                                (int) EchoTypes.All);

            _winterFreeze = true;
            foreach (var room in RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values
                .Where(x => x.SectorType == SectorTypes.Ice
                    && x.SectorType != SectorTypes.Unknown))
            {
                room.SectorType = room.WinterSector;
                room.WinterSector = SectorTypes.Unknown;
            }
        }

        private static void StartSummerSeason()
        {
            act_wiz.echo_to_all(ATTypes.AT_YELLOW, "The days grow longer and hotter as summer grips the world.\r\n",
                                (int) EchoTypes.All);
        }

        private static void StartFallSeason()
        {
            act_wiz.echo_to_all(ATTypes.AT_ORANGE, "The leaves begin changing colors signaling the start of fall.\r\n",
                                (int) EchoTypes.All);
        }

        private static void UpdateSeason(TimeInfoData gameTime)
        {
            var day = db.GetHoliday(gameTime.Month, gameTime.Day);
            if (gameTime.Day + 1 == day?.Day && gameTime.Hour == 0)
                act_wiz.echo_to_all(ATTypes.AT_IMMORT, day.Announce, (int)EchoTypes.All);

            if (gameTime.Season == SeasonTypes.Winter && !_winterFreeze)
            {
                _winterFreeze = true;
                foreach (var room in RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values
                    .Where(x => x.SectorType == SectorTypes.DeepWater
                        || x.SectorType == SectorTypes.ShallowWater))
                {
                    room.WinterSector = room.SectorType;
                    room.SectorType = SectorTypes.Ice;
                }
            }
        }

        public void CalculateSeason(TimeInfoData gameTime)
        {
            var day = gameTime.Month * GameConstants.GetSystemValue<int>("DaysPerMonth") + gameTime.Day;
            var daysPerYear = GameConstants.GetSystemValue<int>("DaysPerYear");

            if (day < daysPerYear / 4)
            {
                gameTime.Season = SeasonTypes.Spring;
                if (gameTime.Hour == 0 && day == 0)
                    StartSpringSeason();
            }
            else if (day < (daysPerYear / 4) * 2)
            {
                gameTime.Season = SeasonTypes.Summer;
                if (gameTime.Hour == 0 && day == (daysPerYear / 4))
                    StartSummerSeason();
            }
            else if (day < (daysPerYear / 4) * 3)
            {
                gameTime.Season = SeasonTypes.Fall;
                if (gameTime.Hour == 0 && day == (daysPerYear / 4) * 2)
                    StartFallSeason();
            }
            else if (day < daysPerYear)
            {
                gameTime.Season = SeasonTypes.Winter;
                if (gameTime.Hour == 0 && day == (daysPerYear / 4) * 3)
                    StartWinterSeason();
            }
            else
                gameTime.Season = SeasonTypes.Spring;

            UpdateSeason(gameTime);
        }
    }
}
