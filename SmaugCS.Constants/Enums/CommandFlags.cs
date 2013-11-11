using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum CommandFlags
    {
        Possess = 1 << 0,
        Polymorphed = 1 << 1,
        Watch = 1 << 2
    }
}
