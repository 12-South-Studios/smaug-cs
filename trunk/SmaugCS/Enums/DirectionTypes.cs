using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Enums
{
    [Flags]
    public enum DirectionTypes
    {
        North, 
        East,
        South, 
        West,
        Up, 
        Down,
        Northeast, 
        Northwest,
        Southeast,
        Southwest, 
        Somewhere
    }
}
