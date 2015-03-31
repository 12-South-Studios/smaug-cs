using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum DirectionTypes
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        Up = 16,
        Down = 32,
        Northeast = 64,
        Northwest = 128,
        Southeast = 256,
        Southwest = 512,
        Somewhere = 1024,
        Portal = 2048
    }
}
