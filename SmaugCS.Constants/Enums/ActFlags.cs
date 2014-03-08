using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ActFlags
    {
        IsNpc = 1 << 0,
        Sentinel = 1 << 1,
        Scavenger = 1 << 2,
        // 1 << 3
        // 1 << 4
        Aggressive = 1 << 5,
        StayArea = 1 << 6,
        Wimpy = 1 << 7,
        Pet = 1 << 8,
        Train = 1 << 9,
        Practice = 1 << 10,
        Immortal = 1 << 11,
        Deadly = 1 << 12,
        PolySelf = 1 << 13,
        MetaAggr = 1 << 14,
        Guardian = 1 << 15,
        Running = 1 << 16,
        NoWander = 1 << 17,
        Mountable = 1 << 18,
        Mounted = 1 << 19,
        Scholar = 1 << 20,
        Secretive = 1 << 21,
        Hardhat = 1 << 22,
        MobInvisibility = 1 << 23,
        NoAssist = 1 << 24,
        Autonomous = 1 << 25,
        Pacifist = 1 << 26,
        NoAttack = 1 << 27,
        Annoying = 1 << 28,
        StatShield = 1 << 29,
        Prototype = 1 << 30,
    }
}
