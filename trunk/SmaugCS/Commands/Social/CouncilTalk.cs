
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class CouncilTalk
    {
        public static void do_counciltalk(CharacterInstance ch, string argument)
        {
            if (ch.IsNotAuthorized())
            {
                color.send_to_char("Huh?\r\n", ch);
                return;
            }

            if (ch.IsNpc() || ch.PlayerData.Council == null)
            {
                color.send_to_char("Huh\r\n", ch);
                return;
            }

            ChatManager.talk_channel(ch, argument, ChannelTypes.Council, "counciltalk");
        }
    }
}
