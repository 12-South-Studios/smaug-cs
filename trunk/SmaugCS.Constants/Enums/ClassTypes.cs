using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ClassTypes
    {
        None        = 0,
        Mage        = 1,
        Cleric      = 2,
        Thief       = 3,
        Warrior     = 4,
        Vampire     = 5,
        Druid       = 6,
        Ranger      = 7,
        Augurer     = 8,

        [Descriptor(new[] { "(Red Aura) ", "(Grey Aura) ", "(White Aura) " })]
        Paladin     = 9,
        Nephandi    = 10,
        Savage      = 11
    }
}
