using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum RoomFlags
    {
        Dark = 1 << 1,
        Death = 1 << 2,
        NoMob = 1 << 3,
        Indoors = 1 << 4,
        Lawful = 1 << 5,
        Neutral = 1 << 6,
        Chaotic = 1 << 7,
        NoMagic = 1 << 8,
        Tunnel = 1 << 9,
        Private = 1 << 10,
        Safe = 1 << 11,
        Solitary = 1 << 12,
        PetShop = 1 << 13,
        NoRecall = 1 << 14,
        Donation = 1 << 15,
        NoDropAll = 1 << 16,
        Silence = 1 << 17,
        LogSpeech = 1 << 18,
        NoDrop = 1 << 19,
        ClanStoreroom = 1 << 20,
        NoSummon = 1 << 21,
        NoAstral = 1 << 22,
        Teleport = 1 << 23,
        TeleportShowDesc = 1 << 24,
        NoFloor = 1 << 25,
        NoSupplicate = 1 << 26,
        Arena = 1 << 27,
        NoMissile = 1 << 28,
        R4 = 1 << 29,  // Unused 
        R5 = 1 << 30,  // Unused
        Prototype = 1 << 31,
        DoNotDisturb = 1 << 32,
        BfsMark = 1 << 33
    }
}
