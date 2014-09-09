using SmaugCS.Communication;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class Think
    {
        public static void do_think(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.High, "think");
        }
    }
}
