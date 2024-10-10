using System;

namespace SmaugCS.Constants.Enums;

[Flags]
public enum AutoSaveFlags
{
    Death = 1 << 0,
    Kill = 1 << 1,
    PasswordChange = 1 << 2,
    Drop = 1 << 3,
    Put = 1 << 4,
    Give = 1 << 5,
    Auto = 1 << 6,
    ZapDrop = 1 << 7,
    Auction = 1 << 8,
    Get = 1 << 9,
    Receive = 1 << 10,
    Idle = 1 << 11,
    Backup = 1 << 12,
    QuitBackup = 1 << 13,
    Fill = 1 << 14,
    Empty = 1 << 15
}