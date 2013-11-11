using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum AreaFlags
    {
        NoPKill = 1 << 0,
        FreeKill = 1 << 1,
        NoTeleport = 1 << 2,
        SpellLimit = 1 << 3,
        Prototype = 1 << 4,
        Loaded = 1 << 5
    }
}
