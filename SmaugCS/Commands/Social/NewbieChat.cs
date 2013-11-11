
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class NewbieChat
    {
        public static void do_newbiechat(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc() ||
                (!ch.IsNotAuthorized() && !ch.IsImmortal()
                 && !(ch.PlayerData.Council != null &&
                      ch.PlayerData.Council.Name.Equals("Newbie Council"))))
            {
                color.send_to_char("Huh?\r\n", ch);
                return;
            }

            ChatManager.talk_channel(ch, argument, ChannelTypes.Newbie, "newbiechat");
        }
    }
}
