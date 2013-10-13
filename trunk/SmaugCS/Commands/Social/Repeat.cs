using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Repeat
    {
        public static void do_repeat(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc() || !ch.IsImmortal()
                || ch.PlayerData.TellHistory == null
                || ch.PlayerData.TellHistory.Count == 0)
            {
                color.ch_printf(ch, "Huh?\r\n");
                return;
            }

            int tindex = 0;
            if (string.IsNullOrWhiteSpace(argument))
                tindex = ch.PlayerData.TellHistory.Count - 1;
            else if (Char.IsLetter(argument.ToCharArray()[0]) && argument.Length == 1)
                tindex = Char.ToLower(argument.ToCharArray()[0]) - 'a';
            else
            {
                color.ch_printf(ch, "You may only index your tell history using a single letter.\r\n");
                return;
            }

            if (!string.IsNullOrWhiteSpace(ch.PlayerData.TellHistory[tindex]))
            {
                color.set_char_color(ATTypes.AT_TELL, ch);
                color.send_to_char(ch.PlayerData.TellHistory[tindex], ch);
            }
            else
                color.send_to_char("No one like that has sent you a tell.\r\n", ch);
        }
    }
}
