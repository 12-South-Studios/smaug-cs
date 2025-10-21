using SmaugCS.Constants.Enums;
using System.Collections.Generic;

namespace SmaugCS.Data;

public class SystemData
{
  public object dlHandle { get; set; }
  public string time_of_max { get; set; }
  public string MudTitle { get; set; }
  public string guild_overseer { get; set; }
  public string guild_advisor { get; set; }
  public int SaveFlags { get; set; } = (int)(AutoSaveFlags.Death | AutoSaveFlags.PasswordChange |
                                             AutoSaveFlags.Auto | AutoSaveFlags.Put | AutoSaveFlags.Drop |
                                             AutoSaveFlags.Give | AutoSaveFlags.Auction | AutoSaveFlags.ZapDrop |
                                             AutoSaveFlags.Idle);

  public int maxplayers { get; set; }
  public int alltimemax { get; set; }
  public int global_looted { get; set; }
  public int upill_val { get; set; }
  public int upotion_val { get; set; }
  public int brewed_used { get; set; }
  public int scribed_used { get; set; }
  public int bash_plr_vs_plr { get; set; }
  public int bash_nontank { get; set; }
  public int gouge_plr_vs_plr { get; set; }
  public int gouge_nontank { get; set; }
  public int StunPvP { get; set; } = 65;
  public int StunPvE { get; set; } = 15;
  public int DodgeMod { get; set; } = 2;
  public int ParryMod { get; set; } = 2;
  public int TumbleMod { get; set; } = 2;
  public int DamagePvP { get; set; } = 100;
  public int DamagePvE { get; set; } = 100;
  public int DamageEvP { get; set; } = 100;
  public int DamageEvE { get; set; } = 100;
  public int level_forcepc { get; set; }
  public int BestowDifference { get; set; } = 5;
  public int max_sn { get; set; }
  public int SaveFrequency { get; set; } = 20;
  public int CheckImmortalHost { get; set; } = 1;
  public int MorphOpt { get; set; } = 1;
  public bool SavePets { get; set; }
  public int ban_site_level { get; set; }
  public int ban_class_level { get; set; }
  public int ban_race_level { get; set; }
  public int PlayerKillLoot { get; set; } = 1;
  public bool NO_NAME_RESOLVING { get; set; }
  public bool DENY_NEW_PLAYERS { get; set; }
  public bool WAIT_FOR_AUTH { get; set; }
  public bool WizardLock { get; set; } = false;

  public int MaxHolidays { get; set; }
  /*public int SecondsPerTick { get; set; }
  public int PulsesPerSecond { get; set; }
  public int PulseTick { get; set; }
  public int PulseViolence { get; set; }
  public int PulseMobile { get; set; }
  public int PulseCalendar { get; set; }
  public int HoursPerDay { get; set; }
  public int DaysPerWeek { get; set; }
  public int DaysPerMonth { get; set; }
  public int MonthsPerYear { get; set; }
  public int DaysPerYear { get; set; }

  public int HourOfSunrise { get; set; }
  public int HourOfDayBegin { get; set; }
  public int HourOfNoon { get; set; }
  public int HourOfSunset { get; set; }
  public int HourOfNightBegin { get; set; }
  public int HourOfMidnight { get; set; }*/

  public Dictionary<PlayerPermissionTypes, int> PlayerPermissions = []
    /*{
        {PlayerPermissionTypes.ReadAllMail, Program.LEVEL_DEMI},
        {PlayerPermissionTypes.ReadMailFree, Program.GetLevel("immortal")},
        {PlayerPermissionTypes.WriteMailFree, Program.GetLevel("immortal")},
        {PlayerPermissionTypes.TakeOthersMail, Program.LEVEL_DEMI},
        {PlayerPermissionTypes.MuseLevel, Program.LEVEL_DEMI},
        {PlayerPermissionTypes.ThinkLevel, Program.LEVEL_HIGOD},
        {PlayerPermissionTypes.BuildLevel, Program.LEVEL_DEMI},
        {PlayerPermissionTypes.LogLevel, Program.LEVEL_LOG},
        {PlayerPermissionTypes.LevelModifyPrototype, Program.LEVEL_LESSER},
        {PlayerPermissionTypes.LevelOverridePrivate, Program.LEVEL_GREATER},
        {PlayerPermissionTypes.LevelMSetPlayer, Program.LEVEL_LESSER},
        {PlayerPermissionTypes.LevelGetObjectNoTake, Program.LEVEL_GREATER}
    }*/;

  public int GetMinimumLevel(PlayerPermissionTypes type)
  {
    return PlayerPermissions.GetValueOrDefault(type, 0);
  }
}