﻿using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class NewbieChat
    {
        public static void do_newbiechat(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch,
                ch.IsNpc() ||
                (!ch.IsNotAuthorized() && !ch.IsImmortal() &&
                 !(ch.PlayerData.Council != null && ch.PlayerData.Council.Name.Equals("Newbie Council"))), "Huh?"))
                return;

            ChatManager.talk_channel(ch, argument, ChannelTypes.Newbie, "newbiechat");
        }
    }
}
