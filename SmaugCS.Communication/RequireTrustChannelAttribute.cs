using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class RequireTrustChannelAttribute : ChannelAttribute
    {
        public string TrustType { get; set; }

        public RequireTrustChannelAttribute(string TrustType = "")
        {
            this.TrustType = TrustType;
        }

        public override bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0)
        {
            return ch.Trust >= minTrust;
        }
    }
}
