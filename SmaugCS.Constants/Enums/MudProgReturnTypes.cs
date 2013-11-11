using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    public enum MudProgReturnTypes
    {
        CommandOK = 1,
        IfTrue = 2,
        IfFalse = 3,
        OrTrue = 4,
        OrFalse = 5,
        FoundElse = 6,
        FoundEndIf = 7,
        IfIgnored = 8,
        OrIgnored = 9
    }
}
