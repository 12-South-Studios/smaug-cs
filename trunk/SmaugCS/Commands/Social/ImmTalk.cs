
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class ImmTalk
    {
        public static void do_immtalk(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.ImmTalk, "immtalk");
        }
    }
}
