using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ResetFlags
    {
        None = 0,
        Door = 1,
        Object = 2,
        Mobile = 4,
        Room = 8,
        TypeMask = 16,
        DoorThreshold = 32,
        DoorMask = 64,
        Set = 128,
        Toggle = 256,
        FreeBits = 512
    }
}
