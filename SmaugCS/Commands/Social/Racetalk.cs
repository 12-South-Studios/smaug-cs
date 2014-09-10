using SmaugCS.Communication;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class Racetalk
    {
        public static void do_racetalk(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.RaceTalk, "racetalk");
        }
    }
}
