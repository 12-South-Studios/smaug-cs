using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Traffic
    {
        public static void do_traffic(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.Traffic, "openly traffic");
        }
    }
}
