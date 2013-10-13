
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

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
