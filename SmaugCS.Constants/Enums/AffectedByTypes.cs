using System;
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum AffectedByTypes
    { //1, 2, 4, 8, 16, 32, 64, 128, 256, 512
        None = -1,
        Blind = 1 << 0,
        Invisible = 1 << 1,

        [Name("detect_evil")]
        DetectEvil = 1 << 2,

        [Name("detect_invis")]
        DetectInvisibility = 1 << 3,

        [Name("detect_magic")]
        DetectMagic = 1 << 4,

        [Name("detect_hidden")]
        DetectHidden = 1 << 5,
        Hold = 1 << 6,
        Sanctuary = 1 << 7,

        [Name("faerie_fire")]
        FaerieFire = 1 << 8,
        Infrared = 1 << 9,
        Curse = 1 << 10,
        Flaming = 1 << 11,
        Poison = 1 << 12,
        Protect = 1 << 13,
        Paralysis = 1 << 14,
        Sneak = 1 << 15,
        Hide = 1 << 16,
        Sleep = 1 << 17,
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
        FireShield = 1 << 25,
        ShockShield = 1 << 26,
        Haus1 = 1 << 27,
        IceShield = 1 << 28,
        Possess = 1 << 29,
        Berserk = 1 << 30,

        [Name("aqua_breath")]
        AquaBreath = 1 << 31,

        [Name("recurring_spell")]
        RecurringSpell = 1 << 32,
        Contagious = 1 << 33,
        AcidMist = 1 << 34,
        VenomShield = 1 << 35
    }
}
