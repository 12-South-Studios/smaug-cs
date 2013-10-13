using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum SkillFlags
    {
        Water = 1 << 0,
        Earth = 1 << 1,
        Air = 1 << 2,
        Astral = 1 << 3,
        Area = 1 << 4,
        Distant = 1 << 5,
        Reverse = 1 << 6,
        NoSelf = 1 << 7,
        Accumulative = 1 << 9,
        ReCastable = 1 << 10,
        NoScribe = 1 << 11,
        NoBrew = 1 << 12,
        GroupSpell = 1 << 13,
        Object = 1 << 14,
        Character = 1 << 15,
        SecretSkill = 1 << 16,
        PKSensitive = 1 << 17,
        StopOnFail = 1 << 18,
        NoFight = 1 << 19,
        NoDispel = 1 << 20,
        RandomTarget = 1 << 21
    }
}
