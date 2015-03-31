using System;

namespace SmaugCS.Data.Organizations
{
    [Flags]
    public enum ClanTypes
    {
        None = 0,
        Vampire = 1,
        Warrior = 2,
        Druid = 3,
        Mage = 4,
        Celtic = 5,
        Thief = 6,
        Cleric = 7,
        Undead = 8,
        Chaotic = 9,
        Neutral = 10,
        Lawful = 11,
        NoKill = 12,
        Order = 13,
        Guild = 14
    }
}
