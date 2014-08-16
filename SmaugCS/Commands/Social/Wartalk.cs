using SmaugCS.Communication;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

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
