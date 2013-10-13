
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Organizations;

namespace SmaugCS.Commands.Social
{
    public static class OrderTalk
    {
        public static void do_ot(CharacterInstance ch, string argument)
        {
            do_ordertalk(ch, argument);
        }

        public static void do_ordertalk(CharacterInstance ch, string argument)
        {
            if (Macros.NOT_AUTHORIZED(ch))
            {
                color.send_to_char("Huh?\r\n", ch);
                return;
            }

            if (ch.IsNpc() || ch.PlayerData.Clan == null
                || ch.PlayerData.Clan.ClanType != ClanTypes.Order)
            {
                color.send_to_char("Huh?\n\r", ch);
                return;
            }

            ChatManager.talk_channel(ch, argument, ChannelTypes.Order, "ordertalk");
        }

    }
}
