using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum PositionTypes
{
    [Descriptor(" is DEAD!!")]
    Dead = 1 << 0,

    [Descriptor(" is mortally wounded.")]
    Mortal = 1 << 1,

    [Descriptor(" is incapacitated.")]
    Incapacitated = 1 << 2,

    [Descriptor(" is laying here stunned.")]
    Stunned = 1 << 3,

    [MentalState(ModValue = 5, Condition = ConditionTypes.Thirsty)]
    [MentalState(ModValue = 4, Condition = ConditionTypes.Full)]
    [Descriptor(" is sleeping nearby.", " is deep in slumber here.")]
    Sleeping,

    [Descriptor(" is here, fighting thin air???", " is here, fighting YOU!", " is here, fighting.", " is here, fighting someone who left?")]
    Berserk = 1 << 5,

    [MentalState(ModValue = 3, Condition = ConditionTypes.Full | ConditionTypes.Thirsty)]
    Resting = 1 << 6,

    [Descriptor(" is here, fighting thin air???", " is here, fighting YOU!", " is here, fighting.", " is here, fighting someone who left?")]
    Aggressive = 1 << 7,

    [MentalState(ModValue = 2, Condition = ConditionTypes.Full | ConditionTypes.Thirsty)]
    [Descriptor(" sits here with you.", " sits nearby as you lay around.", " sits upright here.")]
    Sitting = 1 << 8,

    [Descriptor(" is here, fighting thin air???", " is here, fighting YOU!", " is here, fighting.", " is here, fighting someone who left?")]
    Fighting = 1 << 9,

    [Descriptor(" is here, fighting thin air???", " is here, fighting YOU!", " is here, fighting.", " is here, fighting someone who left?")]
    Defensive = 1 << 10,

    [Descriptor(" is here, fighting thin air???", " is here, fighting YOU!", " is here, fighting {0}.", " is here, fighting someone who left?")]
    Evasive = 1 << 11,

    [Descriptor(" is here before you.", " is drowning here.", " is here in the water.", " is hovering here.", " is standing here.")]
    Standing = 1 << 12,

    [MentalState(ModValue = 2, Condition = ConditionTypes.Full | ConditionTypes.Thirsty)]
    [Descriptor(" is here, upon thin air???", " is here, upon your back.", " is here, upon {0}.", " is here, upon someone who left?")]
    Mounted = 1 << 13,

    [Descriptor(" is being shoved around.")]
    Shove = 1 << 14,

    [Descriptor(" is being dragged around.")]
    Drag = 1 << 15
}