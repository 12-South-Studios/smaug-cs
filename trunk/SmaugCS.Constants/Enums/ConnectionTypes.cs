using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Enums
{
    public enum ConnectionTypes
    {
        GetName                     = -99,
        Playing                     = 0,
        GetOldPassword              = 1,
        ConfirmNewName              = 2,
        GetNewPassword              = 3,
        ConfirmNewPassword          = 4,
        GetNewSex                   = 5,
        GetNewClass                 = 6,
        ReadMotd                    = 7,
        GetNewRace                  = 8,
        GetEmulation                = 9,
        GetWandRipAnsi              = 10,
        Title                       = 11,
        PressEnter                  = 12,
        Wait1                       = 13,
        Wait2                       = 14,
        Wait3                       = 15,
        Accepted                    = 16,
        GetPKill                    = 17,
        ReadIMotd                   = 18,
        CopyoverRecover             = 19,
        Editing                     = 20
    }
}
