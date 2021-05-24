using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Yell
    {
        public static void do_yell(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument.Drunkify(ch), ChannelTypes.Yell, "yell");
        }
    }
}
