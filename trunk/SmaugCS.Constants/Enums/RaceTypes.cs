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
        [DragValue(ModValue = -3)]
        [ShoveValue(ModValue = -3)]
        Caorlei         = 1,

        [Name("dwarf")]
        [DragValue(ModValue = 3)]
        [ShoveValue(ModValue = 3)]
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
        [DragValue(ModValue = 7)]
        [ShoveValue(ModValue = 7)]
        Ork             = 7,

        [Name("unsd4")]
        Unused4         = 8,

        [Name("h-elf")]
        [DragValue(ModValue = -2)]
        [ShoveValue(ModValue = -2)]
        Denorlei        = 9,

        [Name("goblin")]
        Goblin          = 10,

        [Name("d-elf")]
        [ShoveValue(ModValue = 1)]
        Uralei          = 11,

        [Name("s-elf")]
        [ShoveValue(ModValue = -1)]
        Teralei         = 12,

        [Name("unsd5")]
        Unused5         = 13,

        [Name("gnome")]
        [ShoveValue(ModValue = -2)]
        Azurnim         = 14,

        [Name("vlatur")]
        [ShoveValue(ModValue = 4)]
        Valatur         = 15
    }
}
