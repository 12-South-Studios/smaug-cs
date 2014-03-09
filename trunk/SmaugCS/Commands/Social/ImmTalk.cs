
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

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
