using System;
using Realm.Library.Common;

namespace SmaugCS.Language
{
    [Flags]
    public enum LanguageTypes
    {
        None = 0,
        Common = 1,

        [Name("elvish")]
        Elven = 2,
        Dwarven = 4,
        Pixie = 8,
        Ogre = 16,
        Orcish = 32,

        [Name("trollese")]
        Trollish = 64,
        Rodent = 128,
        Insectoid = 256,
        Mammal = 512,
        Reptile = 1024,
        Dragon = 2048,
        Spiritual = 4096,
        Magical = 8192,
        Goblin = 16384,
        God = 32768,
        Ancient = 65536,
        Halfling = 131072,
        Clan = 262144,
        Gith = 524288,
        Gnome = 1048576,
        ValidLanguages = (Common | Elven | Dwarven | Pixie | Ogre
            | Orcish | Trollish | Goblin | Halfling | Gith | Gnome)
    }
}
