using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum BanTypes
    {
        Site = 1,
        Class = 2,
        Race = 3,
        Warn = 4
    }
}
