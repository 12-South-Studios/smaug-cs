using System;
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum RaceTypes
    {
        [Player]
        [Name("human")]
        Human = 0,

        [Player]
        [Name("g-elf")]
        [DragValue(ModValue = -3)]
        [ShoveValue(ModValue = -3)]
        Caorlei = 1,

        [Player]
        [Name("dwarf")]
        [DragValue(ModValue = 3)]
        [ShoveValue(ModValue = 3)]
        Taemier = 2,

        [Name("unsd1")] Unused1 = 3,

        [Name("unsd2")] Unused2 = 4,

        [Player]
        [Name("vamp")]
        Vampire = 5,

        [Name("unsd3")] Unused3 = 6,

        [Player]
        [Name("ork")]
        [DragValue(ModValue = 7)]
        [ShoveValue(ModValue = 7)]
        Ork = 7,

        [Name("unsd4")] Unused4 = 8,

        [Player]
        [Name("h-elf")]
        [DragValue(ModValue = -2)]
        [ShoveValue(ModValue = -2)]
        Denorlei = 9,

        [Player]
        [Name("goblin")]
        Goblin = 10,

        [Player]
        [Name("d-elf")]
        [ShoveValue(ModValue = 1)]
        Uralei = 11,

        [Player]
        [Name("s-elf")]
        [ShoveValue(ModValue = -1)]
        Teralei = 12,

        [Name("unsd5")] Unused5 = 13,

        [Player]
        [Name("gnome")]
        [ShoveValue(ModValue = -2)]
        Azurnim = 14,

        [Player]
        [Name("vlatur")]
        [ShoveValue(ModValue = 4)]
        Valatur = 15,

        halfling,
        pixie,
        gith,
        r5,
        r6,
        r7,
        r8,
        troll,
        ant,
        ape,
        baboon,
        bat,
        bear,
        bee,
        beetle,
        boar,
        bugbear,
        cat,
        dog,
        dragon,
        ferret,
        fly,
        gargoyle,
        gelatin,
        ghoul,
        gnoll,
        golem,
        gorgon,
        harpy,
        hobgoblin,
        kobold,
        lizardman,
        locust,
        lycanthrope,
        minotaur,
        mold,
        mule,
        neanderthal,
        ooze,
        rat,
        rustmonster,
        shadow,
        shapeshifter,
        shrew,
        shrieker,
        skeleton,
        slime,
        snake,
        spider,
        stirge,
        thoul,
        troglodyte,
        undead,
        wight,
        wolf,
        worm,
        zombie,
        bovine,
        canine,
        feline,
        porcine,
        mammal,
        rodent,
        avis,
        reptile,
        amphibian,
        fish,
        crustacean,
        insect,
        spirit,
        magical,
        horse,
        animal,
        humanoid,
        monster,
        god
    }
}
