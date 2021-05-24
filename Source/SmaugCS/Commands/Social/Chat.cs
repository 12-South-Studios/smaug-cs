using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Chat
    {
        public static void do_chat(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.Chat, "chat");
        }
    }
}
