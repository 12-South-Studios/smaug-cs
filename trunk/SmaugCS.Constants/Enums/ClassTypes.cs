using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ClassTypes
    {
        None        = 0,

        [DragValue(ModValue = 15)]
        [ShoveValue(ModValue = 15)]
        Mage        = 1,

        [DragValue(ModValue = 35)]
        [ShoveValue(ModValue = 35)]
        Cleric      = 2,

        [DragValue(ModValue = 30)]
        [ShoveValue(ModValue = 30)]
        Thief       = 3,

        [DragValue(ModValue = 70)]
        [ShoveValue(ModValue = 70)]
        Warrior     = 4,

        [DragValue(ModValue = 65)]
        [ShoveValue(ModValue = 65)]
        Vampire     = 5,

        [DragValue(ModValue = 45)]
        [ShoveValue(ModValue = 45)]
        Druid       = 6,

        [DragValue(ModValue = 60)]
        [ShoveValue(ModValue = 60)]
        Ranger      = 7,

        [ShoveValue(ModValue = 20)]
        Augurer     = 8,

        [Descriptor(new[] { "(Red Aura) ", "(Grey Aura) ", "(White Aura) " })]
        [ShoveValue(ModValue = 55)]
        Paladin     = 9,

        [ShoveValue(ModValue = 20)]
        Nephandi    = 10,

        [ShoveValue(ModValue = 70)]
        Savage      = 11
    }
}
