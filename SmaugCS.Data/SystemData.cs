using System.Collections.Generic;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    public class SystemData
    {
        public object dlHandle { get; set; }
        public string time_of_max { get; set; }
        public string MudTitle { get; set; }
        public string guild_overseer { get; set; }
        public string guild_advisor { get; set; }
        public int SaveFlags { get; set; }
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
        public int StunPvP { get; set; }
        public int StunPvE { get; set; }
        public int DodgeMod { get; set; }
        public int ParryMod { get; set; }
        public int TumbleMod { get; set; }
        public int DamagePvP { get; set; }
        public int DamagePvE { get; set; }
        public int DamageEvP { get; set; }
        public int DamageEvE { get; set; }
        public int level_forcepc { get; set; }
        public int BestowDifference { get; set; }
        public int max_sn { get; set; }
        public int SaveFrequency { get; set; }
        public int CheckImmortalHost { get; set; }
        public int MorphOpt { get; set; }
        public bool SavePets { get; set; }
        public int ban_site_level { get; set; }
        public int ban_class_level { get; set; }
        public int ban_race_level { get; set; }
        public int PlayerKillLoot { get; set; }
        public bool NO_NAME_RESOLVING { get; set; }
        public bool DENY_NEW_PLAYERS { get; set; }
        public bool WAIT_FOR_AUTH { get; set; }
        public bool WizardLock { get; set; }
        public int MaxHolidays { get; set; }
        public int SecondsPerTick { get; set; }
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
        public int HourOfMidnight { get; set; }

        public Dictionary<PlayerPermissionTypes, int> PlayerPermissions = new Dictionary<PlayerPermissionTypes, int>()
            /*{
                {PlayerPermissionTypes.ReadAllMail, Program.LEVEL_DEMI},
                {PlayerPermissionTypes.ReadMailFree, Program.LEVEL_IMMORTAL},
                {PlayerPermissionTypes.WriteMailFree, Program.LEVEL_IMMORTAL},
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
            return PlayerPermissions.ContainsKey(type) ? PlayerPermissions[type] : 0;
        }

        public SystemData()
        {
            StunPvP = 65;
            StunPvE = 15;
            DodgeMod = 2;
            ParryMod = 2;
            TumbleMod = 2;
            DamagePvP = 100;
            DamagePvE = 100;
            DamageEvP = 100;
            DamageEvE = 100;
            SaveFrequency = 20;
            BestowDifference = 5;
            CheckImmortalHost = 1;
            MorphOpt = 1;
            PlayerKillLoot = 1;
            WizardLock = false;
            SecondsPerTick = 70;
            PulsesPerSecond = 4;
            HoursPerDay = 24;
            DaysPerWeek = 7;
            DaysPerMonth = 31;
            MonthsPerYear = 17;
            SaveFlags = (int)(AutoSaveFlags.Death | AutoSaveFlags.PasswordChange |
                         AutoSaveFlags.Auto | AutoSaveFlags.Put | AutoSaveFlags.Drop |
                         AutoSaveFlags.Give | AutoSaveFlags.Auction | AutoSaveFlags.ZapDrop |
                         AutoSaveFlags.Idle);
        }
    }
}
