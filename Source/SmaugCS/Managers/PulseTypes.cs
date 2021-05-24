using Realm.Library.Common.Attributes;

namespace SmaugCS
{
    public enum PulseTypes
    {
        [Name("PulseArea")]
        Area,
        [Name("PulseMobile")]
        Mobile,
        [Name("PulseViolence")]
        Violence,
        [Name("PulseTick")]
        Point,
        [Name("PulsesPerSecond")]
        Second,
        Time,
        [Name("PulseAuction")]
        Auction
    };
}
