using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PublicChannelAttribute : RequireTrustChannelAttribute
    {
        public int MinTrust { get; set; }

        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        {
            if (ch.Trust < MinTrust)
                return false;
            return base.Verify(channelType, ch, minTrust);
        }
    }
}
