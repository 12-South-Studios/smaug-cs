using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Shout
    {
        public static void do_shout(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument.Drunkify(ch), ChannelTypes.Shout, "shout");
            Macros.WAIT_STATE(ch, 12);
        }
    }
}
