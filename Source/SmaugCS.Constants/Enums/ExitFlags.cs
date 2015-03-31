using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ExitFlags
    {
        IsDoor = 1 << 0,
        Closed = 1 << 1,
        Locked = 1 << 2,
        Secret = 1 << 3,
        Swim = 1 << 4,
        PickProof = 1 << 5,
        Fly = 1 << 6,
        Climb = 1 << 7,
        Dig = 1 << 8,
        EatKey = 1 << 9,
        NoPassDoor = 1 << 10,
        Hidden = 1 << 11,
        Passage = 1 << 12,
        Portal = 1 << 13,
        Res1 = 1 << 14,
        Res2 = 1 << 15,
        xClimb = 1 << 16,
        xEnter = 1 << 17,
        xLeave = 1 << 18,
        xAuto = 1 << 19,
        NoFlee = 1 << 20,
        xSearchable = 1 << 21,
        Bashed = 1 << 22,
        BashProof = 1 << 23,
        NoMob = 1 << 24,
        Window = 1 << 25,
        xLook = 1 << 26,
        IsBolt = 1 << 27,
        Bolted = 1 << 28
    }
}
