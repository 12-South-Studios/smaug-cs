using System;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands
{
    public static class Bug
    {
        public static void do_bug(CharacterInstance ch, string argument)
        {
            DateTime tm = DateTime.Now;

            color.set_char_color(ATTypes.AT_PLAIN, ch);
            if (CheckFunctions.CheckIfEmptyString(ch, argument,
                "Usage:  'bug <message>'  (your location is automatically recorded)")) return;

            db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.PBug),
                           string.Format("({0}):  {1}", tm.ToLongDateString(), argument));
            color.send_to_char("Thanks, your bug notice has been recorded.", ch);
        }
    }
}
