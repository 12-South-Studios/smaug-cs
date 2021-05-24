using SmaugCS.Communication;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class NewbieChat
    {
        public static void do_newbiechat(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch,
                ch.IsNpc() ||
                (!ch.IsNotAuthorized() && !ch.IsImmortal() &&
                 !(((PlayerInstance)ch).PlayerData.Council != null && ((PlayerInstance)ch).PlayerData.Council.Name.Equals("Newbie Council"))), "Huh?"))
                return;

            ChatManager.talk_channel(ch, argument, ChannelTypes.Newbie, "newbiechat");
        }
    }
}
