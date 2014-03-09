using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands.Player
{
    public static class Gold
    {
        public static void do_gold(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_GOLD, ch);
            color.ch_printf(ch, "You have {0} gold pieces.", act_info.num_punct(ch.CurrentCoin));
        }
    }
}
