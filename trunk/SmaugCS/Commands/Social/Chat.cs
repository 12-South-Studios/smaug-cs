
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class Chat
    {
        public static void do_chat(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.Chat, "chat");
        }
    }
}
