using System;

namespace SmaugCS.Data.Organizations
{
    [Flags]
    public enum ClanTypes
    {
        Plain = 1 << 0,
        Vampire = 1 << 1,
        Warrior = 1 << 2,
        Druid = 1 << 3,
        Mage = 1 << 4,
        Celtic = 1 << 5,
        Thief = 1 << 6,
        Cleric = 1 << 7,
        Undead = 1 << 8,
        Chaotic = 1 << 9,
        Neutral = 1 << 10,
        Lawful = 1 << 11,
        NoKill = 1 << 12,
        Order = 1 << 13,
        Guild = 1 << 14
    }
}
