using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ImmortalChannelAttribute : RequireTrustChannelAttribute
    {
        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        {
            if (!ch.IsImmortal())
                return false;
            return base.Verify(channelType, ch, minTrust);
        }
    }
}
