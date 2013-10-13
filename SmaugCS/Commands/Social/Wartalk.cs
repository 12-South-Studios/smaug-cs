using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Wartalk
    {
        public static void do_wartalk(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.WarTalk, "war");
        }
    }
}
