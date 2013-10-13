using System;

namespace SmaugCS.Enums
{
    [Flags]
    public enum LogTypes
    {
        Normal,
        Always,
        Never,
        Build,
        High,
        Comm,
        Warn,
        All
    }
}
