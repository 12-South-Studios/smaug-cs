using Realm.Library.Common.Attributes;

namespace SmaugCS.Constants.Enums
{
    public enum AffectedByTypes
    {
        None = 0,
        Blind,

        [Descriptor("(Invis) ")]
        Invisible,

        [Name("detect_evil")]
        [Descriptor("(Red Aura) ")]
        DetectEvil,

        [Name("detect_invis")]
        DetectInvisibility,

        [Name("detect_magic")]
        DetectMagic,

        [Name("detect_hidden")]
        DetectHidden,
        Hold,
        Sanctuary,

        [Name("faerie_fire")]
        [Descriptor("(Pink Aura) ")]
        FaerieFire,
        Infrared,
        Curse,
        Flaming,
        Poison,
        Protect,
        Paralysis,
        Sneak,

        [Descriptor("(Hide) ")]
        Hide,
        Sleep,

        [VisibleAffect(ATType = ATTypes.AT_MAGIC, Description = "%s wanders in a dazed, zombie-like state.")]
        Charm,
        Flying,

        [Name("pass_door")]
        [Descriptor("(Translucent) ")]
        PassDoor,
        Floating,

        [Name("true_sight")]
        TrueSight,

        [Name("detect_traps")]
        DetectTraps,
        Scrying,

        [VisibleAffect(ATType = ATTypes.AT_FIRE, Description = "{0} is engulfed within a blaze of mystical flame.")]
        FireShield,

        [VisibleAffect(ATType = ATTypes.AT_BLUE, Description = "%s is surrounded by cascading torrents of energy.")]
        ShockShield,
        Haus1,

        [VisibleAffect(ATType = ATTypes.AT_LBLUE, Description = "%s is ensphered by shards of glistening ice.")]
        IceShield,

        [VisibleAffect(ATType = ATTypes.AT_MAGIC, Description = "%s appears to be in a deep trance...")]
        Possess,

        [Descriptor("(Wild-eyed) ")]
        Berserk,

        [Name("aqua_breath")]
        AquaBreath,

        [Name("recurring_spell")]
        RecurringSpell,
        Contagious,

        [VisibleAffect(ATType = ATTypes.AT_GREEN, Description = "%s is visible through a cloud of churning mist.")]
        AcidMist,

        [VisibleAffect(ATType = ATTypes.AT_GREEN, Description = "%s is enshrouded in a choking cloud of gas.")]
        VenomShield
    }
}
