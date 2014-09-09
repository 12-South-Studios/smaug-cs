using SmaugCS.Data;
using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PublicChannelAttribute : RequireTrustChannelAttribute
    {
        public int MinTrust { get; set; }

        public PublicChannelAttribute(int MinTrust = 0, string TrustType = "") 
            : base(TrustType)
        {
            this.MinTrust = MinTrust;
        }

        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        {
            if (ch.Trust < MinTrust)
                return false;
            return base.Verify(channelType, ch, minTrust);
        }
    }
}
