using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class GuildTalk
    {
        public static void do_guildtalk(CharacterInstance ch, string argument)
        {
            // TODO fix
            //if (CheckFunctions.CheckIfNotAuthorized(ch, ch, "Huh?")) return;
            //if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ((PlayerInstance)ch).PlayerData.Clan == null
            //                                   || ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.Guild, "Huh?")) return;

            //ChatManager.talk_channel(ch, argument, ChannelTypes.Guild, "guildtalk");
        }
    }
}
