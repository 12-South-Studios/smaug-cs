using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ResistanceTypes
    {
        Fire = 1 << 0,
        Cold = 1 << 1,
        Electricity = 1 << 2,
        Energy = 1 << 3,
        Blunt = 1 << 4,
        Pierce = 1 << 5,
        Slash = 1 << 6,
        Acid = 1 << 7,
        Poison = 1 << 8,
        Drain = 1 << 9,
        Sleep = 1 << 10,
        Charm = 1 << 11,
        Hold = 1 << 12,
        NonMagic = 1 << 13,
        Plus1 = 1 << 14,
        Plus2 = 1 << 15,
        Plus3 = 1 << 16,
        Plus4 = 1 << 17,
        Plus5 = 1 << 18,
        Plus6 = 1 << 19,
        Magic = 1 << 20,
        Paralysis = 1 << 21,
        Unknown = 1 << 22
    }
}
