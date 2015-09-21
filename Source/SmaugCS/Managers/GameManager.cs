using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using Realm.Library.NCalcExt;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Mobile;
using SmaugCS.Extensions.Player;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Repository;
using SmaugCS.Weather;

namespace SmaugCS.Managers
{
    public sealed class GameManager : IGameManager
    {
        private static IRepositoryManager _dbManager;
        private static ILogManager _logger;
        private static ITimer _timer;

        public GameManager(IRepositoryManager databaseManager, ILogManager logManager, ITimer timer)
        {
            _dbManager = databaseManager;
            _logger = logManager;
            _timer = timer;
            _timer.Elapsed += TimerOnElapsed;

            ExpParser = new ExpressionParser(ExpressionTableInitializer.GetExpressionTable());
            SystemData = new SystemData();
        }

        public static IGameManager Instance => Program.Kernel.Get<IGameManager>();

        public SystemData SystemData { get; private set; }

        public TimeInfoData GameTime { get; private set; }
        public void SetGameTime(TimeInfoData gameTime)
        {
            if (GameTime == null)
                GameTime = gameTime;
        }

        public ExpressionParser ExpParser { get; private set; }

        public static CharacterInstance CurrentCharacter { get; set; }

        #region Game Loop
        public void StartMainGameLoop()
        {
            InitializeRoomResets();

            if (_timer.Enabled) return;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void InitializeRoomResets()
        {
            foreach (AreaData area in _dbManager.AREAS.Values)
            {
                foreach (var room in area.Rooms)
                {
                    foreach (var reset in room.Resets)
                    {
                        reset.Process();
                    }
                }
            }
        }

        private static readonly Dictionary<PulseTypes, Action<int>> PulseTypeTable = new Dictionary
            <PulseTypes, Action<int>>
        {
            {PulseTypes.Area, UpdateArea},
            {PulseTypes.Mobile, UpdateMobile},
            {PulseTypes.Violence, UpdateViolence},
            {PulseTypes.Point, UpdateTick},
            {PulseTypes.Second, UpdateSecond}
        };

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DateTime start = DateTime.Now;


            foreach (var pulseType in PulseTypeTable.Keys)
            {
                var pulseTimer = GetPulseTimer(pulseType);
                if ((pulseTimer - 1) <= 0)
                {
                    var pulseValue = GameConstants.GetSystemValue<int>(pulseType.GetName());
                    PulseTypeTable[pulseType].Invoke(pulseValue);
                }
            }

            mud_prog.mpsleep_update();
            update.tele_update();
            update.aggr_update();
            mud_prog.obj_act_update();
            mud_prog.room_act_update();

            DateTime end = DateTime.Now;
            _logger.Info("Timing Complete: {0} seconds.", end.Subtract(start).TotalSeconds);
        }

        private static readonly Dictionary<PulseTypes, int> PulseTrackerTable = new Dictionary<PulseTypes, int>();

        private static int GetPulseTimer(PulseTypes pulseType)
        {
            if (!PulseTrackerTable.ContainsKey(pulseType))
                PulseTrackerTable[pulseType] = GameConstants.GetSystemValue<int>(pulseType.GetName());
            return PulseTrackerTable[pulseType];
        }

        private static void SetPulseTimer(PulseTypes pulseType, int value)
        {
            PulseTrackerTable[pulseType] = value;
        }

        private static void UpdateSecond(int pulseValue)
        {
            SetPulseTimer(PulseTypes.Second, pulseValue);

            update.char_check();
            update.reboot_check();

            _logger.Info("Update Seconds");
        }

        private static void UpdateTick(int pulseValue)
        {
            SetPulseTimer(PulseTypes.Point, SmaugRandom.Between((int) (pulseValue*0.75f), (int) (pulseValue*1.2f)));

            update.auth_update();
            update.time_update();
            WeatherManager.Instance.Weather.Update(Instance.GameTime);
            update.hint_update();
            update.char_update();
            update.obj_update();
           
            _logger.Info("Update Tick");
        }

        private static void UpdateViolence(int pulseValue)
        {
            SetPulseTimer(PulseTypes.Violence, pulseValue);
            
            fight.violence_update();

            _logger.Info("Update Violence");
        }

        private static void UpdateMobile(int pulseValue)
        {
            SetPulseTimer(PulseTypes.Mobile, pulseValue);

            var players = 0;
            var mobiles = 0;

            foreach (var ch in RepositoryManager.Instance.CHARACTERS.Values)
            {
                if (ch is PlayerInstance)
                {
                    ((PlayerInstance)ch).ProcessUpdate();
                    players++;
                }
                else if (ch is MobileInstance)
                {
                    ((MobileInstance)ch).ProcessUpdate(RepositoryManager.Instance);
                    mobiles++;
                }
            }

            _logger.Info("Updated {0} Players and {1} Mobiles.", players, mobiles);
        }

        private static void UpdateArea(int pulseValue)
        {
            SetPulseTimer(PulseTypes.Area, SmaugRandom.Between(pulseValue/2, 3*pulseValue/2));

            var updated = 0;
            var notified = 0;
            foreach (var area in _dbManager.AREAS.Values)
            {
                var resetAge = area.ResetFrequency > 0 ? area.ResetFrequency : 15;
                if ((resetAge == -1 && area.Age == -1) || (area.Age + 1 < (resetAge - 1))) continue;

                if (area.NumberOfPlayers > 0 && area.Age == (resetAge - 1))
                {
                    var buffer = !string.IsNullOrEmpty(area.ResetMessage)
                        ? area.ResetMessage
                        : "You hear some squeaking sounds...";

                    foreach(var pch in _dbManager.CHARACTERS.Values
                        .Where(x => !x.IsNpc())
                        .Where(x => x.IsAwake())
                        .Where(x => x.CurrentRoom != null)
                        .Where(x => x.CurrentRoom.Area.Equals(area)))
                    {
                        pch.SetColor(ATTypes.AT_RESET);
                        pch.SendTo(buffer);
                        notified++;
                    }
                }

                if (area.NumberOfPlayers == 0 || area.Age >= resetAge)
                {
                    reset.reset_area(area);
                    _logger.Info("Updated Area {0}/{1}", area.ID, area.Name);

                    updated++;
                    area.Age = (resetAge == -1) ? -1 : SmaugRandom.Between(0, resetAge/5);
                }
            }

            _logger.Info("Updated {0} areas and notified {1} players.", updated, notified);
        }

        #endregion
    }
}
