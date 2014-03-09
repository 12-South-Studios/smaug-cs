using System;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands
{
    public static class Bug
    {
        public static void do_bug(CharacterInstance ch, string argument)
        {
            DateTime tm = DateTime.Now;

            color.set_char_color(ATTypes.AT_PLAIN, ch);
            if (string.IsNullOrWhiteSpace(argument))
            {
                color.send_to_char("\r\nUsage:  'bug <message>'  (your location is automatically recorded)\r\n", ch);
                return;
            }

            db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.PBug),
                           string.Format("({0}):  {1}", tm.ToLongDateString(), argument));
            color.send_to_char("Thanks, your bug notice has been recorded.\r\n", ch);
        }
    }
}
