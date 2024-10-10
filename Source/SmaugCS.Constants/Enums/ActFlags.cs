using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum ActFlags
{
    IsNpc,
    Sentinel,
    Scavenger,
    Aggressive,
    StayArea,
    Wimpy,
    Pet,
    Train,
    Practice,
    Immortal,
    Deadly,
    PolySelf,
    MetaAggr,
    Guardian,
    Running,
    NoWander,
    Mountable,
    Mounted,
    Scholar,
    Secretive,
    Hardhat,
    MobInvisibility,
    NoAssist,
    Autonomous,
    Pacifist,
    NoAttack,
    Annoying,
    StatShield,

    [Descriptor("(PROTO) ")]
    Prototype
}