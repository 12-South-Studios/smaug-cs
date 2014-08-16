using System;
using SmaugCS.Data.Organizations;

namespace SmaugCS.Communication
{
    [Flags]
    public enum ChannelTypes
    {
        [PublicChannel(MinTrust = 2)]
        [ChannelPrint(On = "&G+AUCTION", Off = "&g-auction")]
        Auction     = 1 << 0,

        [PublicChannel]
        [ChannelPrint(On = "&G+CHAT", Off = "&g-chat")]
        Chat        = 1 << 1,

        [PublicChannel]
        [ChannelPrint(On = "&G+QUEST", Off = "&g-quest")]
        Quest       = 1 << 2,

        [ImmortalChannel]
        [ChannelPrint(On = "&G+IMMTALK", Off = "&g-immtalk")]
        ImmTalk     = 1 << 3,

        [PublicChannel]
        [ChannelPrint(On = "&G+MUSIC", Off = "&g-music")]
        Music       = 1 << 4,

        [PublicChannel]
        [ChannelPrint(On = "&G+ASK", Off = "&g-ask")]
        Ask         = 1 << 5,

        [PublicChannel]
        [ChannelPrint(On = "&G+SHOUT", Off = "&g-shout")]
        Shout       = 1 << 6,

        [PublicChannel]
        [ChannelPrint(On = "&G+YELL", Off = "&g-yell")]
        Yell        = 1 << 7,

        [ImmortalChannel]
        [ChannelPrint(On = "&G+MONITOR", Off = "&g-monitor")]
        Monitor     = 1 << 8,

        [PublicChannel(TrustType = "Log")]
        [ChannelPrint(On = "&G+LOG", Off = "&g-log")]
        Log         = 1 << 9,

        [ImmortalChannel(TrustType = "Muse")]
        [ChannelPrint(On = "&G+HIGHGOD", Off = "&g-highgod")]
        Highgod     = 1 << 10,

        [ClanChannel(NoNpc = true)]
        Clan        = 1 << 11,

        [PublicChannel(TrustType = "Log")]
        [ChannelPrint(On = "&G+BUILD", Off = "&g-build")]
        Build       = 1 << 12,

        [PublicChannel(TrustType = "Think")]
        [ChannelPrint(On = "&G+HIGH", Off = "&g-high")]
        High        = 1 << 13,

        [HeroChannel]
        AvTalk      = 1 << 14,

        [PublicChannel]
        [ChannelPrint(On = "&G+PRAY", Off = "&g-pray")]
        Pray        = 1 << 15,

        [CouncilChannel(NoNpc = true)]
        Council     = 1 << 16,

        [ClanChannel(ClanType = ClanTypes.Guild, NoNpc = true)]
        Guild       = 1 << 17,

        [PublicChannel(TrustType = "Log")]
        [ChannelPrint(On = "&G+COMM", Off = "&g-comm")]
        Comm        = 1 << 18,

        [PublicChannel]
        [ChannelPrint(On = "&G+TELLS", Off = "&g-tells")]
        Tells       = 1 << 19,

        [ClanChannel(ClanType = ClanTypes.Order, NoNpc = true)]
        Order       = 1 << 20,
        Newbie      = 1 << 21,

        [PublicChannel]
        [ChannelPrint(On = "&G+WARTALK", Off = "&g-wartalk")]
        WarTalk     = 1 << 22,

        [PublicChannel]
        [ChannelPrint(On = "&G+RACETALK", Off = "&g-racetalk")]
        RaceTalk    = 1 << 23,

        [PublicChannel(TrustType = "Log")]
        [ChannelPrint(On = "&G+WARN", Off = "&g-warn")]
        Warn        = 1 << 24,

        [PublicChannel]
        [ChannelPrint(On = "&G+WHISPER", Off = "&g-whisper")]
        Whisper     = 1 << 25,

        [ImmortalChannel]
        [ChannelPrint(On = "&G+AUTH", Off = "&g-auth")]
        Auth        = 1 << 26,

        [PublicChannel]
        [ChannelPrint(On = "&G+TRAFFIC", Off = "&g-traffic")]
        Traffic     = 1 << 27
    }
}
