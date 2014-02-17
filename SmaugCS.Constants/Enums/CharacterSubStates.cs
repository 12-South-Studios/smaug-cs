using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
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
}
