using System;

namespace SmaugCS.Logging
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
