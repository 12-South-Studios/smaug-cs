using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum SpellDamageTypes
    {
        None        = 1 << 0,
        Fire        = 1 << 1,
        Cold        = 1 << 2,
        Electricty  = 1 << 3, 
        Energy      = 1 << 4,
        Acid        = 1 << 5,
        Poison      = 1 << 6,
        Drain       = 1 << 7
    }
}
