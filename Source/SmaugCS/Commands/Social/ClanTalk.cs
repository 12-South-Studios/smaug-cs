using SmaugCS.Communication;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;

namespace SmaugCS.Commands
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
