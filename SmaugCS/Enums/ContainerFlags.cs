﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum ContainerFlags
    {
        Closeable = 1 << 0,
        PickProof = 1 << 1,
        Closed = 1 << 2,
        Locked = 1 << 3,
        EatKey = 1 << 4
    }
}
