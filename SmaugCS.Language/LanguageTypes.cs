using System;
using Realm.Library.Common;

namespace SmaugCS.Language
{
    [Flags]
    public enum LanguageTypes
    {
        Unknown = 0,
        Common = 1 << 0,

        [Name("elvish")]
        Elven = 1 << 1,
        Dwarven = 1 << 2,
        Pixie = 1 << 3,
        Ogre = 1 << 4,
        Orcish = 1 << 5,

        [Name("trollese")]
        Trollish = 1 << 6,
        Rodent = 1 << 7,
        Insectoid = 1 << 8,
        Mammal = 1 << 9,
        Reptile = 1 << 10,
        Dragon = 1 << 11,
        Spiritual = 1 << 12,
        Magical = 1 << 13,
        Goblin = 1 << 14,
        God = 1 << 15,
        Ancient = 1 << 16,
        Halfling = 1 << 17,
        Clan = 1 << 18,
        Gith = 1 << 19,
        Gnome = 1 << 20,
        ValidLanguages = (Common | Elven | Dwarven | Pixie | Ogre
            | Orcish | Trollish | Goblin | Halfling | Gith | Gnome)
    }
}
