using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CouncilChannelAttribute : ChannelAttribute
    {
        public bool NoNpc { get; set; }

        public CouncilChannelAttribute(bool NoNpc = false)
        {
            this.NoNpc = NoNpc;
        }

        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        {
            if (ch.PlayerData.Council == null)
                return false;

            if (NoNpc && ch.IsNpc())
                return false;

            return true;
        }
    }
}
