using SmaugCS.Communication;
using SmaugCS.Data.Instances;
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
