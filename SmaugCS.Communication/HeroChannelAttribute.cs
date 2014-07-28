using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
