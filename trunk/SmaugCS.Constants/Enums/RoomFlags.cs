using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum RoomFlags
    {
        Dark                = 1, 
        Death               = 2, 
        NoMob               = 3,
        Indoors             = 4, 
        Lawful              = 5, 
        Neutral             = 6, 
        Chaotic             = 7,
        NoMagic             = 8,
        Tunnel              = 9,
        Private             = 10, 
        Safe                = 11,
        Solitary            = 12, 
        PetShop             = 13,
        NoRecall            = 14, 
        Donation            = 15, 
        NoDropAll           = 16,
        Silence             = 17,
        LogSpeech           = 18, 
        NoDrop              = 19,
        ClanStoreroom       = 20,
        NoSummon            = 21, 
        NoAstral            = 22,
        Teleport            = 23,
        TeleportShowDesc    = 24,
        NoFloor             = 25,
        NoSupplicate        = 26,
        Arena               = 27, 
        NoMissile           = 28, 
        R4                  = 29,  // Unused 
        R5                  = 30,  // Unused
        Prototype           = 31, 
        DoNotDisturb        = 32,
        BfsMark             = 33
    }
}
