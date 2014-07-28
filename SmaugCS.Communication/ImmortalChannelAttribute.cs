using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ImmortalChannelAttribute : RequireTrustChannelAttribute
    {
        public ImmortalChannelAttribute(string TrustType = "") : base(TrustType)
        {
        }

        public override bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0)
        {
            if (!ch.IsImmortal())
                return false;
            return base.Verify(channelType, ch, minTrust);
        }
    }
}
