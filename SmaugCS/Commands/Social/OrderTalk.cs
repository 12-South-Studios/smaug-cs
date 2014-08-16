using SmaugCS.Communication;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class OrderTalk
    {
        public static void do_ordertalk(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNotAuthorized(ch, ch, "Huh?")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ch.PlayerData.Clan == null
                       || ch.PlayerData.Clan.ClanType != ClanTypes.Order, "Huh?")) return;

            ChatManager.talk_channel(ch, argument, ChannelTypes.Order, "ordertalk");
        }
    }
}
