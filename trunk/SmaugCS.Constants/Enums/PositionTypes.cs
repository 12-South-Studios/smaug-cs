using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum PositionTypes
    {
        [Descriptor(new [] {" is DEAD!!"})]
        Dead            = 1 << 0, 

        [Descriptor(new [] {" is mortally wounded."})]
        Mortal          = 1 << 1,

        [Descriptor(new [] {" is incapacitated."})]
        Incapacitated   = 1 << 2,

        [Descriptor(new [] {" is laying here stunned."})]
        Stunned         = 1 << 3,

        [Descriptor(new[] { " is sleeping nearby.", " is deep in slumber here."})]
        Sleeping,

        [Descriptor(new[]
        {
            " is here, fighting thin air???", 
            " is here, fighting YOU!",
            " is here, fighting.",
            " is here, fighting someone who left?"
        })]
        Berserk         = 1 << 5,
        Resting         = 1 << 6,

        [Descriptor(new[]
        {
            " is here, fighting thin air???", 
            " is here, fighting YOU!",
            " is here, fighting.",
            " is here, fighting someone who left?"
        })]
        Aggressive      = 1 << 7,

        [Descriptor(new[]
        {
            " sits here with you.",
            " sits nearby as you lay around.",
            " sits upright here."
        })]
        Sitting         = 1 << 8,

        [Descriptor(new[]
        {
            " is here, fighting thin air???", 
            " is here, fighting YOU!",
            " is here, fighting.",
            " is here, fighting someone who left?"
        })]
        Fighting        = 1 << 9,

        [Descriptor(new[]
        {
            " is here, fighting thin air???", 
            " is here, fighting YOU!",
            " is here, fighting.",
            " is here, fighting someone who left?"
        })]
        Defensive       = 1 << 10,

        [Descriptor(new[]
        {
            " is here, fighting thin air???", 
            " is here, fighting YOU!",
            " is here, fighting {0}.",
            " is here, fighting someone who left?"
        })]
        Evasive         = 1 << 11,

        [Descriptor(new[]
        {
            " is here before you.", 
            " is drowning here.",
            " is here in the water.",
            " is hovering here.",
            " is standing here."
        })]
        Standing = 1 << 12,

        [Descriptor(new[]
        {
            " is here, upon thin air???", 
            " is here, upon your back.",
            " is here, upon {0}.",
            " is here, upon someone who left?"
        })]
        Mounted         = 1 << 13,

        [Descriptor(new[] { " is being shoved around."})]
        Shove           = 1 << 14,

        [Descriptor(new[] { " is being dragged around." })]
        Drag            = 1 << 15
    }
}
