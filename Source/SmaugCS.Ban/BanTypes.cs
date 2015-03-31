using System;

namespace SmaugCS.Ban
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
