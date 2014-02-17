using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum FurnitureFlags
    {
        SitOn = 1,
        SitIn = 2,
        SitAt = 4,
        SitUnder = 8,
        SitInside = 16,
        SitBehind = 32,
        StandOn = 64,
        StandIn = 128,
        StandAt = 256,
        StandUnder = 512,
        StandInside = 1024,
        StandBehind = 2048,
        SleepOn = 4096,
        SleepIn = 8192,
        SleepAt = 16384,
        SleepUnder = 32768,
        SleepInside = 65536,
        SleepBehind = 131072,
        RestOn = 262144,
        RestIn = 524288,
        RestAt = 1048576,
        RestUnder = 2097152,
        RestInside = 4194304,
        RestBehind = 8388608,
        HideUnder = 16777216,
        HideInside = 33554432,
        HideBehind = 67108864
    }
}
