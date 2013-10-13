
using System;

namespace SmaugCS.Enums
{
    [Flags]
    public enum ConditionTypes
    {
        Drunk           = 1 << 0, 
        Full            = 1 << 1, 
        Thirsty         = 1 << 2,
        Bloodthirsty    = 1 << 3
    }
}
