using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum PCFlags
{
    R1 = 1 << 0,
    Deadly = 1 << 1,
    Unauthorized = 1 << 2,
    NoRecall = 1 << 3,
    NoIntro = 1 << 4,
    Gag = 1 << 5,
    Retired = 1 << 6,
    Guest = 1 << 7,
    NoSummon = 1 << 8,
    PagerOn = 1 << 9,
    NoTitle = 1 << 10,
    GroupWho = 1 << 11,
    Diagnose = 1 << 12,
    HighGag = 1 << 13,
    Watch = 1 << 14,
    HelpStart = 1 << 15,
    DoNotDisturb = 1 << 16,
    Idle = 1 << 17,
    Hints = 1 << 18
}