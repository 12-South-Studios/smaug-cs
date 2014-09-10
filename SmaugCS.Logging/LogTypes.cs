using System;

namespace SmaugCS.Logging
{
    [Flags]
    public enum LogTypes
    {
        Info = 0,
        Error,
        Bug,
        Debug,
        Fatal
    }
}
