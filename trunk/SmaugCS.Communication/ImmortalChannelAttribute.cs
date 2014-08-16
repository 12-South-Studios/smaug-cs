using SmaugCS.Data;
using System;

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
