using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands
{
    public static class Idea
    {
        public static void do_idea(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_PLAIN, ch);
            if (string.IsNullOrWhiteSpace(argument))
            {
                color.send_to_char("\r\nUsage:  'idea <message>'\r\n", ch);
                return;
            }

            db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.Idea), argument);
            color.send_to_char("Thanks, your idea has been recorded.\r\n", ch);
        }
    }
}
