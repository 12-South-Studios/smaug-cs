
using Realm.Library.Common.Attributes;
using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ApplyTypes
    {
        None = -1,
        Strength,
        Dexterity,
        Intelligence,
        Wisdom,
        Constitution,
        Charisma,
        Luck,

        [Name("sex")]
        Gender,
        Class,
        Level,
        Age,
        Height,
        Weight,
        Mana,

        [Name("hp")]
        Hit,

        [Name("moves")]
        Movement,
        Gold,
        Experience,

        [Name("armor class")]
        ArmorClass,

        [Name("hit roll")]
        HitRoll,

        [Name("damage roll")]
        DamageRoll,

        [Name("save vs poison")]
        SaveVsPoison,

        [Name("save vs rod")]
        SaveVsRod,

        [Name("save vs paralysis")]
        SaveVsParalysis,

        [Name("save vs breath")]
        SaveVsBreath,

        [Name("save vs spell")]
        SaveVsSpell,

        [Name("affected_by")]
        Affect,

        [Name("resistant")]
        Resistance,

        [Name("immune")]
        Immunity,

        [Name("susceptible")]
        Susceptibility,

        [Name("weapon spell")]
        WeaponSpell,
        Backstab,
        Pick,
        Track,
        Steal,
        Sneak,
        Hide,
        Palm,
        Detrap,
        Dodge,
        Peek,
        Scan,
        Gouge,
        Search,
        Mount,
        Disarm,
        Kick,
        Parry,
        Bash,
        Stun,
        Punch,
        Climb,
        Grip,
        Scribe,
        Brew,

        [Name("wear spell")]
        WearSpell,

        [Name("remove spell")]
        RemoveSpell,

        [Name("emotional state")]
        Emotion,

        [Name("mental state")]
        MentalState,

        [Name("dispel")]
        StripSN,
        Remove,
        Dig,

        [Name("hunger")]
        Full,
        Thirst,
        Drunk,
        Blood,
        Cook,

        [Name("recurring spell")]
        RecurringSpell,
        Contagious,
        ExtendedAffect,
        Odor,
        RoomFlag,
        SectorType,
        RoomLight,

        [Name("teleport vnum")]
        TeleportVnum,

        [Name("teleport delay")]
        TeleportDelay,

        IsNotRemovable = WearSpell | RemoveSpell | StripSN
    }
}
