using System;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Constants.Enums
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
