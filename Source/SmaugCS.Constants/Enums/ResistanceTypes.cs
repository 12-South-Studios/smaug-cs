using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum ResistanceTypes
{
    Unknown = 0,
    Fire = 1,
    Cold = 2,
    Electricity = 4,
    Energy = 8,
    Blunt = 16,
    Pierce = 32,
    Slash = 64,
    Acid = 128,
    Poison = 256,
    Drain = 512,
    Sleep = 1024,
    Charm = 2048,
    Hold = 4096,
    NonMagic = 8192,
    Plus1 = 16384,
    Plus2 = 32768,
    Plus3 = 65536,
    Plus4 = 131072,
    Plus5 = 262144,
    Plus6 = 524288,
    Magic = 1048576,
    Paralysis = 2097152
}