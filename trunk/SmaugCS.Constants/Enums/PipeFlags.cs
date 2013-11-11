using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum PipeFlags
    {
        Tamped = 1 << 1,
        Lit = 1 << 2,
        Hot = 1 << 3,
        Dirty = 1 << 4,
        Filthy = 1 << 5,
        GoingOut = 1 << 6,
        Burnt = 1 << 7,
        FullOfAsh = 1 << 8
    }
}
