
using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class ImmTalk
    {
        public static void do_immtalk(CharacterInstance ch, string argument)
        {
            ChatManager.SendToChat(ch, argument, ChannelTypes.ImmTalk, "immtalk");
        }
    }
}
