using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class AvTalk
    {
        public static void do_avtalk(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument.Drunkify(ch), ChannelTypes.AvTalk, "avtalk");
        }
    }
}
