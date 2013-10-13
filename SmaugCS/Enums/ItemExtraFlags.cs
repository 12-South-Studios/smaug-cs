using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Enums
{
    [Flags]
    public enum ItemExtraFlags
    {
        Glow = 1 << 0,
        Hum = 1 << 1,
        Dark = 1 << 2,
        Loyal = 1 << 3,
        Evil = 1 << 4,
        Invisible = 1 << 5,
        Magical = 1 << 6,
        NoDrop = 1 << 7,
        Blessed = 1 << 8,
        AntiGood = 1 << 9,
        AntiEvil = 1 << 10,
        AntiNeutral = 1 << 11,
        NoRemove = 1 << 12,
        Inventory = 1 << 13,
        AntiMage = 1 << 14,
        AntiThief = 1 << 15,
        AntiWarrior = 1 << 16,
        AntiCleric = 1 << 17,
        Organic = 1 << 18,
        Metallic = 1 << 19,
        Donation = 1 << 20,
        ClanObject = 1 << 21,
        ClanCorpse = 1 << 22,
        AntiVampire = 1 << 23,
        AntiDruid = 1 << 24,
        Hidden = 1 << 25,
        Poisoned = 1 << 26,
        Covering = 1 << 27,
        DeathRot = 1 << 28,
        Buried = 1 << 29,
        Prototype = 1 << 30,
        NoLocate = 1 << 31,
        GroundRot = 1 << 32,
        Lootable = 1 << 33,
        Personal = 1 << 34,
        MultiInvoke = 1 << 35,
        Enchanted = 1 << 36
    }
}
