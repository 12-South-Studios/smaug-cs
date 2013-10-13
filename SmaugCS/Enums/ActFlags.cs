using System;

namespace SmaugCS.Enums
{
    [Flags]
    public enum ActFlags
    {
        IsNpc = 0,
        Sentinel = 1,
        Scavenger = 2,
        // 3
        // 4
        Aggressive = 5,
        StayArea = 6,
        Wimpy = 7,
        Pet = 8,
        Train = 9,
        Practice = 10,
        Immortal = 11,
        Deadly = 12,
        PolySelf = 13,
        MetaAggr = 14,
        Guardian = 15,
        Running = 16,
        NoWander = 17,
        Mountable = 18,
        Mounted = 19,
        Scholar = 20,
        Secretive = 21,
        Hardhat = 22,
        MobInvisibility = 23,
        NoAssist = 24,
        Autonomous = 25,
        Pacifist = 26,
        NoAttack = 27,
        Annoying = 28,
        StatShield = 29,
        Prototype = 30,
    }
}
