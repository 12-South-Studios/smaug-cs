using System.Collections.Generic;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class GuildTalk
    {
        public static void do_guildtalk(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNotAuthorized(ch, ch, "Huh?")) return;
            if (CheckFunctions.CheckIf(ch, args =>
            {
                var actor = (CharacterInstance) args[0];
                return (actor.IsNpc() || actor.PlayerData.Clan == null
                        || actor.PlayerData.Clan.ClanType != ClanTypes.Guild);
            }, "Huh?", new List<object> {ch})) return;

            ChatManager.talk_channel(ch, argument, ChannelTypes.Guild, "guildtalk");
        }
    }
}
