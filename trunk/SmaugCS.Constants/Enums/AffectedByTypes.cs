using System;
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum AffectedByTypes
    { //1, 2, 4, 8, 16, 32, 64, 128, 256, 512
        None = -1,
        Blind = 1 << 0,     // 1
        Invisible = 1 << 1, // 2

        [Name("detect_evil")]
        DetectEvil = 1 << 2,    // 4

        [Name("detect_invis")]
        DetectInvisibility = 1 << 3,    // 8

        [Name("detect_magic")] 
        DetectMagic = 1 << 4,   // 16

        [Name("detect_hidden")]
        DetectHidden = 1 << 5,  // 32
        Hold = 1 << 6,      // 64
        Sanctuary = 1 << 7, // 128

        [Name("faerie_fire")]
        FaerieFire = 1 << 8,    // 256
        Infrared = 1 << 9,  // 512
        Curse = 1 << 10, //1024
        Flaming = 1 << 11, //2048
        Poison = 1 << 12,//4096
        Protect = 1 << 13,//8192
        Paralysis = 1 << 14,//16384
        Sneak = 1 << 15,//32768
        Hide = 1 << 16,
        Sleep = 1 << 17,

        [VisibleAffect(ATTypes.AT_MAGIC, "%s wanders in a dazed, zombie-like state.\r\n")]
        Charm = 1 << 18,
        Flying = 1 << 19,

        [Name("pass_door")]
        PassDoor = 1 << 20,
        Floating = 1 << 21,

        [Name("true_sight")]
        TrueSight = 1 << 22,

        [Name("detect_traps")]
        DetectTraps = 1 << 23,
        Scrying = 1 << 24,

        [VisibleAffect(ATTypes.AT_FIRE, "{0} is engulfed within a blaze of mystical flame.\r\n")]
        FireShield = 1 << 25,

        [VisibleAffect(ATTypes.AT_BLUE, "%s is surrounded by cascading torrents of energy.")]
        ShockShield = 1 << 26,
        Haus1 = 1 << 27,

        [VisibleAffect(ATTypes.AT_LBLUE, "%s is ensphered by shards of glistening ice.\r\n")]
        IceShield = 1 << 28,

        [VisibleAffect(ATTypes.AT_MAGIC, "%s appears to be in a deep trance...\r\n")]
        Possess = 1 << 29,
        Berserk = 1 << 30,

        [Name("aqua_breath")]
        AquaBreath = 1 << 31,

        [Name("recurring_spell")]
        RecurringSpell = 1 << 32,
        Contagious = 1 << 33,

        [VisibleAffect(ATTypes.AT_GREEN, "%s is visible through a cloud of churning mist.\r\n")]
        AcidMist = 1 << 34,

        [VisibleAffect(ATTypes.AT_GREEN, "%s is enshrouded in a choking cloud of gas.\r\n")]
        VenomShield = 1 << 35
    }
}
