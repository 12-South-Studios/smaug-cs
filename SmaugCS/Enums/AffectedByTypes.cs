using System;

namespace SmaugCS.Enums
{
    [Flags]
    public enum AffectedByTypes
    {
        None = -1,
        Blind = 1 << 0,
        Invisible = 1 << 1,
        DetectEvil = 1 << 2,
        DetectInvisibility = 1 << 3,
        DetectMagic = 1 << 4,
        DetectHidden = 1 << 5,
        Hold = 1 << 6,
        Sanctuary = 1 << 7,
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
        PassDoor = 1 << 20,
        Floating = 1 << 21,
        TrueSight = 1 << 22,
        DetectTraps = 1 << 23,
        Scrying = 1 << 24,
        FireShield = 1 << 25,
        ShockShield = 1 << 26,
        Haus1 = 1 << 27,
        IceShield = 1 << 28,
        Possess = 1 << 29,
        Berserk = 1 << 30,
        AquaBreath = 1 << 31,
        RecurringSpell = 1 << 32,
        Contagious = 1 << 33,
        AcidMist = 1 << 34,
        VenomShield = 1 << 35
    }
}
