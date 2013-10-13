﻿
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class NewbieChat
    {
        public static void do_newbiechat(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc() ||
                (!Macros.NOT_AUTHORIZED(ch) && !ch.IsImmortal()
                 && !(ch.PlayerData.Council != null &&
                      ch.PlayerData.Council.Name.Equals("Newbie Council"))))
            {
                color.send_to_char("Huh?\r\n", ch);
                return;
            }

            ChatManager.talk_channel(ch, argument, ChannelTypes.Newbie, "newbiechat");
        }
    }
}
