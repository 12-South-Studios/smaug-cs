using System;
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum RaceTypes
    {
        [Name("human")]
        Human           = 0,

        [Name("g-elf")]
        Caorlei         = 1,

        [Name("dwarf")]
        Taemier         = 2,

        [Name("unsd1")]
        Unused1         = 3,

        [Name("unsd2")]
        Unused2         = 4,

        [Name("vamp")]
        Vampire         = 5,

        [Name("unsd3")]
        Unused3         = 6,

        [Name("ork")]
        Ork             = 7,

        [Name("unsd4")]
        Unused4         = 8,

        [Name("h-elf")]
        Denorlei        = 9,

        [Name("goblin")]
        Goblin          = 10,

        [Name("d-elf")]
        Uralei          = 11,

        [Name("s-elf")]
        Teralei         = 12,

        [Name("unsd5")]
        Unused5         = 13,

        [Name("gnome")]
        Azurnim         = 14,

        [Name("vlatur")]
        Valatur         = 15
    }
}
