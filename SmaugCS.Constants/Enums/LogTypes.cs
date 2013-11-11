using System;

namespace SmaugCS.Constants.Enums
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
