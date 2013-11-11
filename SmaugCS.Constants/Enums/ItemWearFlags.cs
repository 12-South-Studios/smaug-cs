using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ItemWearFlags
    {
        Take = 1 << 0,
        Finger = 1 << 1,
        Neck = 1 << 2,
        Body = 1 << 3,
        Head = 1 << 4,
        Legs = 1 << 5,
        Feet = 1 << 6,
        Hands = 1 << 7,
        Arms = 1 << 8,
        Shield = 1 << 9,
        About = 1 << 10,
        Waist = 1 << 11,
        Wrist = 1 << 12,
        Wield = 1 << 13,
        Hold = 1 << 14,
        DualWield = 1 << 15,
        Ears = 1 << 16,
        Eyes = 1 << 17,
        MissileWield = 1 << 18,
        Back = 1 << 19,
        Face = 1 << 20,
        Ankle = 1 << 21
    }
}
