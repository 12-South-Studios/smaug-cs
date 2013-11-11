using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ItemExtraFlags
    {
        Glow = 1,
        Hum = 2,
        Dark = 3,
        Loyal = 4,
        Evil = 5,
        Invisible = 6,
        Magical = 7,
        NoDrop = 8,
        Blessed = 9,
        AntiGood = 10,
        AntiEvil = 11,
        AntiNeutral = 12,
        NoRemove = 13,
        Inventory = 14,
        AntiMage = 15,
        AntiThief = 16,
        AntiWarrior = 17,
        AntiCleric = 18,
        Organic = 19,
        Metallic = 20,
        Donation = 21,
        ClanObject = 22,
        ClanCorpse = 23,
        AntiVampire = 24,
        AntiDruid = 25,
        Hidden = 26,
        Poisoned = 27,
        Covering = 28,
        DeathRot = 29,
        Buried = 30,
        Prototype = 31,
        NoLocate = 32,
        GroundRot = 33,
        Lootable = 34,
        Personal = 35,
        MultiInvoke = 36,
        Enchanted = 37
    }
}
