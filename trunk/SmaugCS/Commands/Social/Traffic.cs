using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class Traffic
    {
        public static void do_traffic(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.Traffic, "openly traffic");
        }
    }
}
