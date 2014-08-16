using SmaugCS.Data;
using System;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HeroChannelAttribute : ChannelAttribute
    {
        public override bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0)
        {
            return ch.IsHero();
        }
    }
}
