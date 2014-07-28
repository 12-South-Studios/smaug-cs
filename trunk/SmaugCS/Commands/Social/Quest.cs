using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class Quest
    {
        public static void do_quest(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.Quest, "quest");
        }
    }
}
