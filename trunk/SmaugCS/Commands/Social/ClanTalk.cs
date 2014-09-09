using SmaugCS.Communication;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class ClanTalk
    {
        public static void do_clantalk(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNotAuthorized(ch, ch, "Huh?")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ((PlayerInstance)ch).PlayerData.Clan == null
                                               || ((PlayerInstance)ch).PlayerData.Clan.ClanType == ClanTypes.Order
                                               || ((PlayerInstance)ch).PlayerData.Clan.ClanType == ClanTypes.Guild, "Huh?")) return;

            ChatManager.talk_channel(ch, argument, ChannelTypes.Clan, "clantalk");
        }
    }
}
