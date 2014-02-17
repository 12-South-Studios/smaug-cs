using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum PositionTypes
    {
        Dead            = 1 << 0, 
        Mortal          = 1 << 1,
        Incapacitated   = 1 << 2,
        Stunned         = 1 << 3,
        Sleeping        = 1 << 4, 
        Berserk         = 1 << 5,
        Resting         = 1 << 6, 
        Aggressive      = 1 << 7,
        Sitting         = 1 << 8,
        Fighting        = 1 << 9, 
        Defensive       = 1 << 10,
        Evasive         = 1 << 11,
        Standing        = 1 << 12, 
        Mounted         = 1 << 13, 
        Shove           = 1 << 14, 
        Drag            = 1 << 15
    }
}
