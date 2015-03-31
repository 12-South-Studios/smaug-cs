using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum SectorTypes
    {
        [MovementLoss(ModValue = 1)]
        Inside              = 0, 

        [MovementLoss(ModValue = 1)]
        City                = 1, 

        [MovementLoss(ModValue = 2)]
        Field               = 2, 

        [MovementLoss(ModValue = 3)]
        Forest              = 4, 

        [MovementLoss(ModValue = 3)]
        Hills               = 8,

        [MovementLoss(ModValue = 8)]
        Mountain            = 16,

        [MovementLoss(ModValue = 3)]
        ShallowWater        = 32, 

        [MovementLoss(ModValue = 6)]
        DeepWater           = 64, 

        [MovementLoss(ModValue = 6)]
        Underwater          = 128,

        [MovementLoss(ModValue = 3)]
        [Thirst(ModValue = -2)]
        Air                 = 256, 
        
        [MovementLoss(ModValue = 3)]
        [Thirst(ModValue = -3)]
        Desert              = 512,

        Unknown             = 1024, 
        
        [MovementLoss(ModValue = 6)]
        OceanFloor          = 2048,

        [MovementLoss(ModValue = 4)]
        Underground         = 4096,

        [MovementLoss(ModValue = 8)]
        [Thirst(ModValue = -5)]
        Lava                = 8192, 

        [MovementLoss(ModValue = 4)]
        Swamp               = 16384, 

        [MovementLoss(ModValue = 6)]
        [Thirst(ModValue = -2)]
        Ice                 = 32768
    }
}
