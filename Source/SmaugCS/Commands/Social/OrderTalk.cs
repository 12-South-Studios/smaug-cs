﻿using SmaugCS.Communication;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;

namespace SmaugCS.Commands
{
    public static class OrderTalk
    {
        public static void do_ordertalk(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNotAuthorized(ch, ch, "Huh?")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ((PlayerInstance)ch).PlayerData.Clan == null
                       || ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.Order, "Huh?")) return;

            ChatManager.talk_channel(ch, argument, ChannelTypes.Order, "ordertalk");
        }
    }
}
