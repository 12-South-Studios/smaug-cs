using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum CharacterSubStates
{
    None,
    Pause,
    PersonalDescription,
    BanDescription,
    ObjectShort,
    ObjectLong,
    ObjectExtra,
    MobileLong,
    MobileDescription,
    RoomDescription,
    RoomExtra,
    RoomExitDescription,
    WritingNote,
    MProgEditing,
    HelpEditing,
    WritingMap,
    PersonalBiography,
    RepeatCommand,
    Restricted,
    DeityDescription,
    MorphDescription,
    MorphHelp,
    ProjectDescription,
    NewsPosting,
    NewsEditing,
    TimerDoAbort = 128,
    TimerCannotAbort
}