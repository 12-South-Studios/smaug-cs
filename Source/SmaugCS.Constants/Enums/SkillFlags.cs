using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum SkillFlags
{
    Water = 1,
    Earth = 2,
    Air = 4,
    Astral = 8,
    Area = 16,
    Distant = 32,
    Reverse = 64,
    NoSelf = 128,
    Accumulative = 256,
    ReCastable = 512,
    NoScribe = 1024,
    NoBrew = 2048,
    GroupSpell = 4096,
    Object = 8192,
    Character = 16384,
    SecretSkill = 32768,
    PKSensitive = 65536,
    StopOnFail = 131072,
    NoFight = 262144,
    NoDispel = 524288,
    RandomTarget = 1048576
}