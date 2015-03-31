using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum TeleportTriggerFlags
    {
        ShowDescription = 1 << 0,
        TransportAll = 1 << 1,
        TransportAllPlus = 1 << 2
    }
}
