using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum SectorTypes
    {
        Inside              = 1 << 0, 
        City                = 1 << 1, 
        Field               = 1 << 2, 
        Forest              = 1 << 3, 
        Hills               = 1 << 4,
        Mountain            = 1 << 5,
        ShallowWater        = 1 << 6, 
        DeepWater           = 1 << 7, 
        Underwater          = 1 << 8,

        [Thirst(ModValue = -2)]
        Air                 = 1 << 9, 
        
        [Thirst(ModValue = -3)]
        Desert              = 1 << 10,
        Unknown             = 1 << 11, 
        OceanFloor          = 1 << 12,
        Underground         = 1 << 13,

        [Thirst(ModValue = -5)]
        Lava                = 1 << 14, 
        Swamp               = 1 << 15, 

        [Thirst(ModValue = -2)]
        Ice                 = 1 << 16
    }
}
