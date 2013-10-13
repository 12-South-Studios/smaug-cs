﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum ItemMagicFlags
    {
        Returning = 1 << 0,
        Backstabber = 1 << 1,
        Bane = 1 << 2,
        Loyal = 1 << 3,
        Haste = 1 << 4,
        Drain = 1 << 5,
        LightningBlade = 1 << 6,
        PKDisarmed = 1 << 7
    }
}
