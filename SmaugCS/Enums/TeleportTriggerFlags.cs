using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum TeleportTriggerFlags
    {
        ShowDescription = 1 << 0,
        TransportAll = 1 << 1,
        TransportAllPlus = 1 << 2
    }
}
