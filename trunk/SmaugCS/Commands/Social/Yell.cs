
using SmaugCS.Enums;
using SmaugCS.Common;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Yell
    {
        public static void do_yell(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument.Drunkify(ch), ChannelTypes.Yell, "yell");
        }
    }
}
