using System;
using Realm.Library.Common.Attributes;

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

        [Name("invis")]
        Invisible = 6,

        [Name("magic")]
        Magical = 7,
        NoDrop = 8,

        [Name("bless")]
        Blessed = 9,

        [Name("anti-good")]
        AntiGood = 10,

        [Name("anti-evil")]
        AntiEvil = 11,

        [Name("anti-neutral")]
        AntiNeutral = 12,
        NoRemove = 13,
        Inventory = 14,

        [Name("anti-mage")]
        AntiMage = 15,

        [Name("anti-thief")]
        AntiThief = 16,

        [Name("anti-warrior")]
        AntiWarrior = 17,

        [Name("anti-cleric")]
        AntiCleric = 18,
        Organic = 19,

        [Name("metal")]
        Metallic = 20,
        Donation = 21,

        [Name("clan")]
        ClanObject = 22,

        [Name("clanbody")]
        ClanCorpse = 23,

        [Name("anti-vampire")]
        AntiVampire = 24,

        [Name("anti-druid")]
        AntiDruid = 25,
        Hidden = 26,

        [Name("poison")]
        Poisoned = 27,

        [Name("covered")]
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
