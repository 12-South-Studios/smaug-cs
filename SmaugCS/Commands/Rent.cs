using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands
{
    public static class Rent
    {
        public static void do_rent(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.send_to_char("There is no rent here. Just save and quit.\r\n", ch);
        }
    }
}
