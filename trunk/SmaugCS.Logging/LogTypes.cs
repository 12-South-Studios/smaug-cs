using System;

namespace SmaugCS.Logging
{
    [Flags]
    public enum LogTypes
    {
        Info,
        Error,
        Bug,
        Debug,
        Fatal
    }
}
