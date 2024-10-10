using System;

namespace SmaugCS.Common.Enumerations;

[Flags]
public enum LogTypes
{
    None = 0,
    Info = 1,
    Error = 2,
    Bug = 4,
    Debug = 8,
    Fatal = 16
}