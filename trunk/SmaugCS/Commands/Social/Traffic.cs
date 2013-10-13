using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Traffic
    {
        public static void do_traffic(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.Traffic, "openly traffic");
        }
    }
}
