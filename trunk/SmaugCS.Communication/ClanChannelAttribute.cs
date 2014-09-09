using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using System;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ClanChannelAttribute : ChannelAttribute
    {
        public bool NoNpc { get; set; }
        public ClanTypes ClanType { get; set; }

        public ClanChannelAttribute(bool NoNpc = false, ClanTypes ClanType = ClanTypes.Plain)
        {
            this.NoNpc = NoNpc;
            this.ClanType = ClanType;
        }

        public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        {
            if (ch.PlayerData.Clan == null)
                return false;

            if (NoNpc && ch.IsNpc())
                return false;

            if (ClanType != ch.PlayerData.Clan.ClanType)
                return false;

            return true;
        }
    }
}
