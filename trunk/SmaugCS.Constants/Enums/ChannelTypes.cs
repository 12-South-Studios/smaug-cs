using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ChannelTypes
    {
        Auction     = 1 << 0,
        Chat        = 1 << 1,
        Quest       = 1 << 2,
        ImmTalk     = 1 << 3,
        Music       = 1 << 4,
        Ask         = 1 << 5,
        Shout       = 1 << 6,
        Yell        = 1 << 7,
        Monitor     = 1 << 8,
        Log         = 1 << 9,
        Highgod     = 1 << 10,
        Clan        = 1 << 11,
        Build       = 1 << 12,
        High        = 1 << 13,
        AvTalk      = 1 << 14,
        Pray        = 1 << 15,
        Council     = 1 << 16,
        Guild       = 1 << 17,
        Comm        = 1 << 18,
        Tells       = 1 << 19,
        Order       = 1 << 20,
        Newbie      = 1 << 21,
        WarTalk     = 1 << 22,
        RaceTalk    = 1 << 23,
        Warn        = 1 << 24,
        Whisper     = 1 << 25,
        Auth        = 1 << 26,
        Traffic     = 1 << 27
    }
}
