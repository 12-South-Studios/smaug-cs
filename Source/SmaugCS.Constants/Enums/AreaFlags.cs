using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum AreaFlags
{
    NoPlayerVsPlayer,
    FreeKill,
    NoTeleport,
    SpellLimit,
    Prototype,
    Loaded
}