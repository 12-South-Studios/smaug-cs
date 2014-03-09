using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands.Player
{
    public static class Score
    {
        public static void do_score(CharacterInstance ch, string argument)
        {
            color.set_pager_color(ATTypes.AT_SCORE, ch);
            color.pager_printf(ch, "\r\nScore for {0}{1}.\r\n", ch.Name, ch.PlayerData.Title);
            if (ch.Trust != ch.Level)
                color.pager_printf(ch, "You are trusted at level {0}.\r\n", ch.Trust);


            // TODO
        }
    }
}
