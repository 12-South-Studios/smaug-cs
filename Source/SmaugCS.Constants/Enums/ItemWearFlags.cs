using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum ItemWearFlags
{
    None = 0,
    Take = 1,
    Finger = 2,
    Neck = 4,
    Body = 8,
    Head = 16,
    Legs = 32,
    Feet = 64,
    Hands = 128,
    Arms = 256,
    Shield = 512,
    About = 1024,
    Waist = 2048,
    Wrist = 4096,
    Wield = 8192,
    Hold = 16384,
    DualWield = 32768,
    Ears = 65536,
    Eyes = 131072,
    MissileWield = 262144,
    Back = 524288,
    Face = 1048576,
    Ankle = 2097152
}