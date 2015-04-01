using System;
using Realm.Library.Common;

namespace SmaugCS.Common.Enumerations
{
    [Flags]
    public enum SkillTypes
    {
        None = 0,

        [Enum("spell")]
        Spell = 1,

        [Name("skill")]
        Skill = 2,

        [Name("weapon")]
        Weapon = 4,

        [Name("tongue")]
        Tongue = 8,

        [Name("herb")]
        Herb = 16,

        [Name("racial")]
        Racial = 32,

        [Name("disease")]
        Disease = 64
    }
}
