using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum TriggerFlags
    {
        Up = 1 << 0,
        Unlock = 1 << 1,
        Lock = 1 << 2,
        D_North = 1 << 3,
        D_South = 1 << 4,
        D_East = 1 << 5,
        D_West = 1 << 6,
        D_Up = 1 << 7,
        D_Down = 1 << 8,
        Door = 1 << 9,
        Container = 1 << 10,
        Open = 1 << 11,
        Close = 1 << 12,
        Passage = 1 << 13,
        ObjectLoad = 1 << 14,
        MobileLoad = 1 << 15,
        Teleport = 1 << 16,
        TeleportAll = 1 << 17,
        TeleportPlus = 1 << 18,
        Death = 1 << 19,
        Cast = 1 << 20,
        FakeBlade = 1 << 21,
        Rand4 = 1 << 22,
        Rand6 = 1 << 23,
        TrapDoor = 1 << 24,
        AnotherRoom = 1 << 25,
        UseDial = 1 << 26,
        AbsoluteVNUM = 1 << 27,
        ShowRoomDescription = 1 << 28,
        AutoReturn = 1 << 29
    }
}
