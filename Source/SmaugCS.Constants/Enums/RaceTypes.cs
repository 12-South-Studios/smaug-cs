using Realm.Library.Common.Attributes;
using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum RaceTypes
    {
        None = 0,

        [Player]
        [Name("human")]
        Human = 1,

        [Player]
        [Name("g-elf")]
        [DragValue(ModValue = -3)]
        [ShoveValue(ModValue = -3)]
        Caorlei = 2,

        [Player]
        [Name("dwarf")]
        [DragValue(ModValue = 3)]
        [ShoveValue(ModValue = 3)]
        Taemier = 4,

        [Player]
        [Name("vamp")]
        Vampire = 8,

        [Player]
        [Name("ork")]
        [DragValue(ModValue = 7)]
        [ShoveValue(ModValue = 7)]
        Ork = 16,

        [Player]
        [Name("h-elf")]
        [DragValue(ModValue = -2)]
        [ShoveValue(ModValue = -2)]
        Denorlei = 32,

        [Player]
        [Name("goblin")]
        Goblin = 64,

        [Player]
        [Name("d-elf")]
        [ShoveValue(ModValue = 1)]
        Uralei = 128,

        [Player]
        [Name("s-elf")]
        [ShoveValue(ModValue = -1)]
        Teralei = 256,

        [Player]
        [Name("gnome")]
        [ShoveValue(ModValue = -2)]
        Azurnim = 512,

        [Player]
        [Name("vlatur")]
        [ShoveValue(ModValue = 4)]
        Valatur = 1024,

        Monster = 2048
    }
}
