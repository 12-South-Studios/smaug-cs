using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HeroChannelAttribute : ChannelAttribute
    {
        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        {
            return ch.IsHero();
        }
    }
}
