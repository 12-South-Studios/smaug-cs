using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PublicChannelAttribute : RequireTrustChannelAttribute
    {
        public int MinTrust { get; set; }

        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
            => ch.Trust >= MinTrust && base.Verify(channelType, ch, minTrust);
    }
}
