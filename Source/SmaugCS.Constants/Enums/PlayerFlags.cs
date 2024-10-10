using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum PlayerFlags : long
{
    None = 0,
    IsNpc = 1,
    BoughtPet = 2,
    ShoveDrag = 4,
    AutoExit = 8,
    AutoLoot = 16,
    AutoSacrifice = 32,
    Blank = 64,
    Outcast = 128,
    Brief = 256,
    Combine = 512,
    Prompt = 1024,
    TelnetGA = 2048,
    HolyLight = 4096,
    WizardInvisibility = 8192,
    RoomVNum = 16384,
    Silence = 32768,
    NoEmote = 65536,

    [Descriptor("(ATTACKER) ")]
    Attacker = 131072,
    NoTell = 262144,
    Log = 524288,
    Deny = 1048576,
    Freeze = 2097152,

    [Descriptor("(THIEF) ")]
    Thief = 4194304,

    [Descriptor("(KILLER) ")]
    Killer = 8388608,

    [Descriptor("(LITTERBUG) ")]
    Litterbug = 16777216,
    Ansi = 33554432,
    Rip = 67108864,
    Nice = 134217728,
    Flee = 268435456,
    AutoGold = 536870912,
    AutoMap = 1073741824,

    [Descriptor("[AFK]")]
    AwayFromKeyboard = 214783648,
    InvisibilePrompt = 4294967296,
    Compass = 8589934592
}