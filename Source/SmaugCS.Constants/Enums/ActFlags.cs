using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ActFlags
    {
        IsNpc = 1,
        Sentinel = 2,
        Scavenger = 4,
        Aggressive = 8,
        StayArea = 16,
        Wimpy = 32,
        Pet = 64,
        Train = 128,
        Practice = 256,
        Immortal = 512,
        Deadly = 1024,
        PolySelf = 2048,
        MetaAggr = 4096,
        Guardian = 8192,
        Running = 16384,
        NoWander = 32768,
        Mountable = 65536,
        Mounted = 131072,
        Scholar = 262144,
        Secretive = 524288,
        Hardhat = 1048576,
        MobInvisibility = 2097152,
        NoAssist = 4194304,
        Autonomous = 8388608,
        Pacifist = 16777216,
        NoAttack = 33554432,
        Annoying = 67108864,
        StatShield = 134217728,

        [Descriptor("(PROTO) ")]
        Prototype = 268435456
    }
}
