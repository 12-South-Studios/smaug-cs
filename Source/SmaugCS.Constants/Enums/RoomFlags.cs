using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum RoomFlags : long
{
    None = 0,
    Dark = 1,
    Death = 2,
    NoMob = 4,
    Indoors = 8,
    Lawful = 16,
    Neutral = 32,
    Chaotic = 64,
    NoMagic = 128,
    Tunnel = 256,
    Private = 512,
    Safe = 1024,
    Solitary = 2048,
    PetShop = 4096,
    NoRecall = 8192,
    Donation = 16384,
    NoDropAll = 32768,
    Silence = 65536,
    LogSpeech = 131072,
    NoDrop = 262144,
    ClanStoreroom = 524288,
    NoSummon = 1048576,
    NoAstral = 2097152,
    Teleport = 4194304,
    TeleportShowDesc = 8388608,
    NoFloor = 16777216,
    NoSupplicate = 33554432,
    Arena = 67108864,
    NoMissile = 134217728,
    R4 = 268435456,  // Unused 
    R5 = 536870912,  // Unused
    Prototype = 1073741824,
    DoNotDisturb = 214783648,
    BfsMark = 4294967296
}