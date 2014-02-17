using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ResetFlags
    {
        Door = 0,
        Object = 1,
        Mobile = 2,
        Room = 3,
        TypeMask = 0xFF,
        DoorThreshold = 8,
        DoorMask = 0xFF00,
        Set = 1 << 30,
        Toggle = 1 << 31,
        FreeBits = 0x3FFF0000
    }
}
