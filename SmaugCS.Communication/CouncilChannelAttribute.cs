using SmaugCS.Data;
using System;

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

        public override bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0)
        {
            if (ch.PlayerData.Council == null)
                return false;

            if (NoNpc && ch.IsNpc())
                return false;

            return true;
        }
    }
}
