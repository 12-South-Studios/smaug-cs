using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    public enum PlayerPermissionTypes
    {
        None,
        ReadAllMail,
        ReadMailFree,
        WriteMailFree,
        TakeOthersMail,
        MuseLevel,
        ThinkLevel,
        BuildLevel,
        LogLevel,
        LevelModifyPrototype,
        LevelOverridePrivate,
        LevelMSetPlayer,
        LevelGetObjectNoTake
    }
}
