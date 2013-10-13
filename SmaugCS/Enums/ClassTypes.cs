using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum ClassTypes
    {
        None        = 1 << 0,
        Mage        = 1 << 1,
        Cleric      = 1 << 2,
        Thief       = 1 << 3,
        Warrior     = 1 << 4,
        Vampire     = 1 << 5,
        Druid       = 1 << 6,
        Ranger      = 1 << 7,
        Augurer     = 1 << 8,
        Paladin     = 1 << 9,
        Nephandi    = 1 << 10,
        Savage      = 1 << 11
    }
}
