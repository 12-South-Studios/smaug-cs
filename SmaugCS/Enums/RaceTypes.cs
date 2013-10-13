using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Enums
{
    [Flags]
    public enum RaceTypes
    {
        Human           = 1 << 0,
        Elf             = 1 << 1,
        Dwarf           = 1 << 2,
        Halfling        = 1 << 3,
        Pixie           = 1 << 4,
        Vampire         = 1 << 5,
        HalfOgre        = 1 << 6,
        HalfOrc         = 1 << 7,
        HalfTroll       = 1 << 8,
        HalfElf         = 1 << 9,
        Gith            = 1 << 10,
        Drow           = 1 << 11,
        SeaElf          = 1 << 12,
        Lizardman       = 1 << 13,
        Gnome           = 1 << 14,
        Dragon          = 1 << 15
    }
}
