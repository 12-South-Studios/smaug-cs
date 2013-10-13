
using SmaugCS.Enums;
using SmaugCS.Common;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Shout
    {
        public static void do_shout(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument.Drunkify(ch), ChannelTypes.Shout, "shout");
            Macros.WAIT_STATE(ch, 12);
        }
    }
}
