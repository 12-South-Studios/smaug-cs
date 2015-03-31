using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum TrapTriggerTypes
    {
        Room = 1 << 0,
        Object = 1 << 1,
        EnterRoom = 1 << 2,
        LeaveRoom = 1 << 3,
        Open = 1 << 4,
        Close = 1 << 5,
        Get = 1 << 6,
        Put = 1 << 7,
        Pick = 1 << 8,
        Unlock = 1 << 9,
        North = 1 << 10,
        South = 1 << 11,
        East = 1 << 12,
        West = 1 << 13,
        Up = 1 << 14,
        Down = 1 << 15,
        Examine = 1 << 16,
        Northeast = 1 << 17,
        Northwest = 1 << 18,
        Southeast = 1 << 19,
        Southwest = 1 << 20
    }
}
