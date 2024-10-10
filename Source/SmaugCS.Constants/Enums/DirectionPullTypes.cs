namespace SmaugCS.Constants.Enums;

public enum DirectionPullTypes
{
    Undefined,

    [Pullcheck(ToChar = "You are sucked into a swirling vortex of colors!",
        ToRoom = "$n is sucked into a swirling vortex of colors!",
        DestRoom = "$n appears from a swirling vortex of colors!",
        ObjMsg = "$p is sucked into a swirling vortex of colors!",
        DestObj = "$p appears from a swirling vortex of colors!")]
    Vortex,

    [Pullcheck(ToChar = "You are sucked $T!",
        ToRoom = "$n is sucked $T!",
        DestRoom = "$n is sucked in from $T!",
        ObjMsg = "$p is sucked $T.",
        DestObj = "$p is sucked in from $T!")]
    Vacuum,

    [Pullcheck(ToChar = "You lose your footing!",
        ToRoom = "$n loses $s footing!",
        DestRoom = "$n slides in from $T!",
        ObjMsg = "$p slides $T.",
        DestObj = "$p slides in from $T.")]
    Slip,
    Ice,
    Mysterious,

    [Pullcheck(ToChar = "You drift $T.",
        ToRoom = "$n drifts $T.",
        DestRoom = "$n drifts in from $T.",
        ObjMsg = "$p drifts $T.",
        DestObj = "$p drifts in from $T.")]
    Current = 100,

    [Pullcheck(ToChar = "You are pushed $T!",
        ToRoom = "$n is pushed $T!",
        DestRoom = "$n is pushed in from $T!",
        DestObj = "$p floats in from $T.")]
    Wave,

    [Pullcheck(ToChar = "You are sucked $T!",
        ToRoom = "$n is sucked $T!",
        DestRoom = "$n is sucked in from $T!",
        ObjMsg = "$p is sucked $T.",
        DestObj = "$p is sucked in from $T!")]
    Whirlpool,

    [Pullcheck(ToChar = "You are pushed $T!",
        ToRoom = "$n is pushed $T!",
        DestRoom = "$n is pushed in from $T!",
        DestObj = "$p floats in from $T.")]
    Geyser,

    [Pullcheck(ToChar = "A strong wind pushes you $T!",
        ToRoom = "$n is blown $t by a strong wind!",
        DestRoom = "$n is blown in from $T by a strong wind!",
        ObjMsg = "$p is blown $T.",
        DestObj = "$p is blown in from $T.")]
    Wind = 200,

    [Pullcheck(ToChar = "The raging storm drives you $T!",
        ToRoom = "$n is driven $T by the rating storm!",
        DestRoom = "$n is driven in from $T by a raging storm!",
        ObjMsg = "$p is blown $T.",
        DestObj = "$p is blown in from $T.")]
    Storm,

    [Pullcheck(ToChar = "A bitter cold wind forces you $T!",
        ToRoom = "$n is forced $t by a bitter cold wind!",
        DestRoom = "$n is forced in from $T by a bitter cold wind!",
        ObjMsg = "$p is blown $T.",
        DestObj = "$p is blown in from $T.")]
    ColdWind,

    [Pullcheck(ToChar = "You drift $T.",
        ToRoom = "$n drifts $T.",
        DestRoom = "$n drifts in from $T.",
        ObjMsg = "$p drifts $T in the breeze.",
        DestObj = "$p drifts in from $T.")]
    Breeze,

    [Pullcheck(ToChar = "The ground starts to slide $T, taking you with it!",
        ToRoom = "The ground starts to slide $T, taking $n with it!",
        DestRoom = "$n slides in from $T!",
        ObjMsg = "$p slides $T.",
        DestObj = "$p slides in from $T.")]
    Landslide = 300,

    [Pullcheck(ToChar = "The ground suddenly gives way and you fall $T!",
        ToRoom = "The ground suddenly gives way beneath $n!",
        DestRoom = "$n falls from $T!",
        ObjMsg = "$p falls $T.",
        DestObj = "$p falls from $T.")]
    Sinkhole,

    [Pullcheck(ToChar = "You begin to sink $T into the quicksand!",
        ToRoom = "$n begins to sink $t into the quicksand!",
        DestRoom = "$n sinks in from $T!",
        ObjMsg = "$p begins to sink $t into the quicksand.",
        DestObj = "$p sinks in from $T.")]
    Quicksand,

    [Pullcheck(ToChar = "The earth opens up and you fall $T!",
        ToRoom = "The earth opens up and $n falls $T!",
        DestRoom = "$n falls from $T!",
        ObjMsg = "$p falls $T.",
        DestObj = "$p falls from $T.")]
    Earthquake,

    [Pullcheck(ToChar = "You drift $T.",
        ToRoom = "$n drifts $T.",
        DestRoom = "$n drifts in from $T.",
        ObjMsg = "$p drifts $T.",
        DestObj = "$p drifts in from $T.")]
    Lava = 400,

    [Pullcheck(ToChar = "A blast of hot air blows you $T!",
        ToRoom = "$n is blown $T by a blast of hot air!",
        DestRoom = "$n is blown in from $T by a blast of hot air!",
        ObjMsg = "$p is blown $T,",
        DestObj = "$p is blown in from $T.")]
    HotAir
}