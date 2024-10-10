using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Autofac.Features.AttributeFilters;
using Library.Common;
using Library.Common.Extensions;
using Library.NCalc;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Mobile;
using SmaugCS.Extensions.Player;
using SmaugCS.Logging;
using SmaugCS.MudProgs;
using SmaugCS.Repository;

namespace SmaugCS.Managers;

public sealed class GameManager : IGameManager
{
  private static IRepositoryManager _dbManager;
  private static ILogManager _logger;
  private static ITimer _timer;

  public GameManager(IRepositoryManager databaseManager, ILogManager logManager,
    [KeyFilter("GameLoopTimer")] ITimer timer)
  {
    _dbManager = databaseManager;
    _logger = logManager;
    _timer = timer;
    _timer.Elapsed += TimerOnElapsed;

    ExpParser = new ExpressionParser(ExpressionTableInitializer.GetExpressionTable());
    SystemData = new SystemData();
  }

  public SystemData SystemData { get; private set; }

  public TimeInfoData GameTime { get; private set; }

  public void SetGameTime(TimeInfoData gameTime)
  {
    GameTime ??= gameTime;
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
      foreach (RoomTemplate room in area.Rooms)
      {
        foreach (ResetData reset in room.Resets)
        {
          reset.Process();
        }
      }
    }
  }

  private static readonly Dictionary<PulseTypes, Action<int>> PulseTypeTable = new()
  {
    { PulseTypes.Area, UpdateArea },
    { PulseTypes.Mobile, UpdateMobile },
    { PulseTypes.Violence, UpdateViolence },
    { PulseTypes.Point, UpdateTick },
    { PulseTypes.Second, UpdateSecond }
  };

  private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
  {
    DateTime start = DateTime.Now;


    foreach (PulseTypes pulseType in PulseTypeTable.Keys)
    {
      int pulseTimer = GetPulseTimer(pulseType);
      if (pulseTimer - 1 > 0) continue;
      int pulseValue = GameConstants.GetSystemValue<int>(pulseType.GetName());
      PulseTypeTable[pulseType].Invoke(pulseValue);
    }

    mud_prog.mpsleep_update();
    update.tele_update();
    update.aggr_update();
    mud_prog.obj_act_update();
    mud_prog.room_act_update();

    DateTime end = DateTime.Now;
    _logger.Info("Timing Complete: {0} seconds.", end.Subtract(start).TotalSeconds);
  }

  private static readonly Dictionary<PulseTypes, int> PulseTrackerTable = new();

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
    SetPulseTimer(PulseTypes.Point, SmaugRandom.Between((int)(pulseValue * 0.75f), (int)(pulseValue * 1.2f)));

    update.auth_update();
    update.time_update();
    Program.WeatherManager.Weather.Update(Program.GameManager.GameTime);
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

    int players = 0;
    int mobiles = 0;

    foreach (CharacterInstance ch in Program.RepositoryManager.CHARACTERS.Values)
    {
      switch (ch)
      {
        case PlayerInstance instance:
          instance.ProcessUpdate();
          players++;
          break;
        case MobileInstance instance:
          instance.ProcessUpdate(Program.RepositoryManager);
          mobiles++;
          break;
      }
    }

    _logger.Info("Updated {0} Players and {1} Mobiles.", players, mobiles);
  }

  private static void UpdateArea(int pulseValue)
  {
    SetPulseTimer(PulseTypes.Area, SmaugRandom.Between(pulseValue / 2, 3 * pulseValue / 2));

    int updated = 0;
    int notified = 0;
    foreach (AreaData area in _dbManager.AREAS.Values)
    {
      int resetAge = area.ResetFrequency > 0 ? area.ResetFrequency : 15;
      if ((resetAge == -1 && area.Age == -1) || area.Age + 1 < resetAge - 1) continue;

      if (area.NumberOfPlayers > 0 && area.Age == resetAge - 1)
      {
        string buffer = !string.IsNullOrEmpty(area.ResetMessage)
          ? area.ResetMessage
          : "You hear some squeaking sounds...";

        foreach (CharacterInstance pch in _dbManager.CHARACTERS.Values
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

      if (area.NumberOfPlayers != 0 && area.Age < resetAge) continue;
      reset.reset_area(area);
      _logger.Info("Updated Area {0}/{1}", area.Id, area.Name);

      updated++;
      area.Age = resetAge == -1 ? -1 : SmaugRandom.Between(0, resetAge / 5);
    }

    _logger.Info("Updated {0} areas and notified {1} players.", updated, notified);
  }

  #endregion
}