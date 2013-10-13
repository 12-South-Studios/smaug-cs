using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Enums
{
    [Flags]
    public enum RoomFlags
    {
        Dark                = 1 << 0, 
        Death               = 1 << 1, 
        NoMob               = 1 << 2,
        Indoors             = 1 << 3, 
        Lawful              = 1 << 4, 
        Neutral             = 1 << 5, 
        Chaotic             = 1 << 6,
        NoMagic             = 1 << 7,
        Tunnel              = 1 << 8,
        Private             = 1 << 9, 
        Safe                = 1 << 10,
        Solitary            = 1 << 11, 
        PetShop             = 1 << 12,
        NoRecall            = 1 << 13, 
        Donation            = 1 << 14, 
        NoDropAll           = 1 << 15,
        Silence             = 1 << 16,
        LogSpeech           = 1 << 17, 
        NoDrop              = 1 << 18,
        ClanStoreroom       = 1 << 19,
        NoSummon            = 1 << 20, 
        NoAstral            = 1 << 21,
        Teleport            = 1 << 22,
        TeleportShowDesc    = 1 << 23,
        NoFloor             = 1 << 24,
        NoSupplicate        = 1 << 25,
        Arena               = 1 << 26, 
        NoMissile           = 1 << 27, 
        R4                  = 1 << 28,  // Unused 
        R5                  = 1 << 29,  // Unused
        Prototype           = 1 << 30, 
        DoNotDisturb        = 1 << 31, 
        BfsMark             = 1 << 32
    }
}
