
namespace SmaugCS.Enums
{
    public enum ReturnTypes
    {
        None,
        CharacterDied,
        VictimDied,
        BothDied,
        CharacterQuit,
        VictimQuit,
        BothQuit,
        SpellFailed,
        ObjectScrapped,
        ObjectEaten,
        ObjectExpired,
        ObjectTimer,
        ObjectSacrificed,
        ObjectQuaffed,
        ObjectUsed,
        ObjectExtracted,
        ObjectDrunk,
        CharacterImmune,
        VictimImmune,
        CharacterAndObjectExtracted = 128,
        Error = 255
    }
}
