
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class CouncilTalk
    {
        public static void do_counciltalk(CharacterInstance ch, string argument)
        {
            if (Macros.NOT_AUTHORIZED(ch))
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
