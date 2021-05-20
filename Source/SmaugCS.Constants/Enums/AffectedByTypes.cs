using System;
using Realm.Library.Common.Attributes;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum AffectedByTypes : long
    {
        None = 0,
        Blind = 1, 

        [Descriptor("(Invis) ")]
        Invisible = 2,

        [Name("detect_evil")]
        [Descriptor("(Red Aura) ")]
        DetectEvil = 4, 

        [Name("detect_invis")]
        DetectInvisibility = 8,

        [Name("detect_magic")] 
        DetectMagic = 16,

        [Name("detect_hidden")]
        DetectHidden = 32,
        Hold = 64,     
        Sanctuary = 128,

        [Name("faerie_fire")]
        [Descriptor("(Pink Aura) ")]
        FaerieFire = 256,    
        Infrared = 512,  
        Curse = 1024, 
        Flaming = 2048, 
        Poison = 4096,
        Protect = 8192,
        Paralysis = 16384,
        Sneak = 32768,

        [Descriptor("(Hide) ")]
        Hide = 65536,
        Sleep = 131072,

        [VisibleAffect(ATType = ATTypes.AT_MAGIC, Description = "%s wanders in a dazed, zombie-like state.")]
        Charm = 262144,
        Flying = 524288,

        [Name("pass_door")]
        [Descriptor("(Translucent) ")]
        PassDoor = 1048576,
        Floating = 2097152,

        [Name("true_sight")]
        TrueSight = 4194304,

        [Name("detect_traps")]
        DetectTraps = 8388608,
        Scrying = 16777216,

        [VisibleAffect(ATType = ATTypes.AT_FIRE, Description = "{0} is engulfed within a blaze of mystical flame.")]
        FireShield = 33554432,

        [VisibleAffect(ATType = ATTypes.AT_BLUE, Description = "%s is surrounded by cascading torrents of energy.")]
        ShockShield = 67108864,
        Haus1 = 134217728,

        [VisibleAffect(ATType = ATTypes.AT_LBLUE, Description = "%s is ensphered by shards of glistening ice.")]
        IceShield = 268435456,

        [VisibleAffect(ATType = ATTypes.AT_MAGIC, Description = "%s appears to be in a deep trance...")]
        Possess = 536870912,

        [Descriptor("(Wild-eyed) ")]
        Berserk = 1073741824,

        [Name("aqua_breath")]
        AquaBreath = 214783648,

        [Name("recurring_spell")]
        RecurringSpell = 4294967296,
        Contagious = 8589934592,

        [VisibleAffect(ATType = ATTypes.AT_GREEN, Description = "%s is visible through a cloud of churning mist.")]
        AcidMist = 17179869184,

        [VisibleAffect(ATType = ATTypes.AT_GREEN, Description = "%s is enshrouded in a choking cloud of gas.")]
        VenomShield = 34359738368
    }
}
