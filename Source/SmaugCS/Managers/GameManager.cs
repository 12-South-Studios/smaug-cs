using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using Realm.Library.NCalcExt;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.Managers
{
    public sealed class GameManager : IGameManager
    {
        private static IRepositoryManager _dbManager;
        private static ILogManager _logger;
        private static ITimer _timer;
        private static ISmaugDbContext _dbContext;

        public GameManager(IRepositoryManager databaseManager, ILogManager logManager, ITimer timer)
        {
            _dbManager = databaseManager;
            _logger = logManager;
            _timer = timer;
            _timer.Elapsed += TimerOnElapsed;

            ExpParser = new ExpressionParser(ExpressionTableInitializer.GetExpressionTable());
            SystemData = new SystemData();
        }

        public static IGameManager Instance
        {
            get { return Program.Kernel.Get<IGameManager>(); }
        }

        public SystemData SystemData { get; private set; }

        public TimeInfoData GameTime { get; private set; }
        public void SetGameTime(TimeInfoData gameTime)
        {
            if (GameTime == null)
                GameTime = gameTime;
        }

        public ExpressionParser ExpParser { get; private set; }

        public static CharacterInstance CurrentCharacter { get; set; }

        private enum PulseTypes
        {
            [Name("PulseArea")]
            Area,
            [Name("PulseMobile")]
            Mobile,
            [Name("PulseViolence")]
            Violence,
            [Name("PulseTick")]
            Point,
            [Name("PulsesPerSecond")]
            Second,
            Time,
            [Name("PulseAuction")]
            Auction
        };

        private static readonly Dictionary<PulseTypes, int> PulseTrackerTable = new Dictionary<PulseTypes, int>();

        private int GetPulseTimer(PulseTypes pulseType)
        {
            if (!PulseTrackerTable.ContainsKey(pulseType))
                PulseTrackerTable[pulseType] = GameConstants.GetSystemValue<int>(pulseType.GetName());
            return PulseTrackerTable[pulseType];
        }

        private void SetPulseTimer(PulseTypes pulseType, int value)
        {
            PulseTrackerTable[pulseType] = value;
        }

        public void DoLoop()
        {
            InitializeResets();

            if (_timer.Enabled) return;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DateTime start = DateTime.Now;

            UpdateAreas();
            UpdateMobiles();
            UpdateViolence();

            // todo pulse calendar

            UpdateTick();
            UpdateSecond();

            // todo mpsleep_update, tele_update, aggr_update, obj_act_update
            // todo room_act_update, clean_obj_queue, clean_char_queue

            DateTime end = DateTime.Now;
            _logger.Info("Timing Complete: {0} seconds.", end.Subtract(start).TotalSeconds);
        }

        private void UpdateSecond()
        {
            var pulseTimer = GetPulseTimer(PulseTypes.Second);
            if ((pulseTimer - 1) <= 0)
            {
                var pulseSecond = GameConstants.GetSystemValue<int>(PulseTypes.Second.GetName());
                SetPulseTimer(PulseTypes.Second, pulseSecond);
                // todo char_check, check_dns, reboot_check

                _logger.Info("Update Seconds");
            }
        }

        private void UpdateTick()
        {
            var pulseTimer = GetPulseTimer(PulseTypes.Point);
            if ((pulseTimer - 1) <= 0)
            {
                var pulseTick = GameConstants.GetSystemValue<int>(PulseTypes.Point.GetName());
                SetPulseTimer(PulseTypes.Point, SmaugRandom.Between((int) (pulseTick*0.75f), (int) (pulseTick*1.2f)));

                // todo auth_update, time_update, updateweather, hint_update, char_update, obj_update, clear_vrooms

                _logger.Info("Update Tick");
            }
        }

        private void UpdateViolence()
        {
            var pulseTimer = GetPulseTimer(PulseTypes.Violence);
            if ((pulseTimer - 1) <= 0)
            {
                var pulseViolence = GameConstants.GetSystemValue<int>(PulseTypes.Violence.GetName());
                SetPulseTimer(PulseTypes.Violence, pulseViolence);
                // todo violence_update

                _logger.Info("Update Violence");
            }
        }

        private void UpdateMobiles()
        {
            var pulseTimer = GetPulseTimer(PulseTypes.Mobile);
            if ((pulseTimer - 1) <= 0)
            {
                var pulseMobile = GameConstants.GetSystemValue<int>(PulseTypes.Mobile.GetName());
                SetPulseTimer(PulseTypes.Mobile, pulseMobile);
                // todo mobile_update

                _logger.Info("Update Mobiles");
            }
        }

        private void UpdateAreas()
        {
            var pulseTimer = GetPulseTimer(PulseTypes.Area);
            if ((pulseTimer - 1) <= 0)
            {
                var pulseArea = GameConstants.GetSystemValue<int>(PulseTypes.Area.GetName());
                SetPulseTimer(PulseTypes.Area, SmaugRandom.Between(pulseArea/2, 3*pulseArea/2));
                // todo area_update

                _logger.Info("Update Areas");
            }
        }

        private void InitializeResets()
        {
            foreach (var area in _dbManager.AREAS.Values)
                area.OnStartup(this, new EventArgs());
        }
    }
}
