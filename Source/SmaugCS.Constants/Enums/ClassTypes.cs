using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ClassTypes
    {
        None        = 0,

        [Player]
        [DragValue(ModValue = 15)]
        [ShoveValue(ModValue = 15)]
        Mage        = 1,

        [Player]
        [DragValue(ModValue = 35)]
        [ShoveValue(ModValue = 35)]
        Cleric      = 2,

        [Player]
        [DragValue(ModValue = 30)]
        [ShoveValue(ModValue = 30)]
        Thief       = 3,

        [Player]
        [DragValue(ModValue = 70)]
        [ShoveValue(ModValue = 70)]
        Warrior     = 4,

        [Player]
        [DragValue(ModValue = 65)]
        [ShoveValue(ModValue = 65)]
        Vampire     = 5,

        [Player]
        [DragValue(ModValue = 45)]
        [ShoveValue(ModValue = 45)]
        Druid       = 6,

        [Player]
        [DragValue(ModValue = 60)]
        [ShoveValue(ModValue = 60)]
        Ranger      = 7,

        [Player]
        [ShoveValue(ModValue = 20)]
        Augurer     = 8,

        [Player]
        [Descriptor("(Red Aura) ", "(Grey Aura) ", "(White Aura) ")]
        [ShoveValue(ModValue = 55)]
        Paladin     = 9,

        [ShoveValue(ModValue = 20)]
        Nephandi    = 10,

        [ShoveValue(ModValue = 70)]
        Savage      = 11,

        Pirate,
        Baker,
        Butcher,
        Blacksmith,
        Mayor,
        King,
        Queen
    }
}
