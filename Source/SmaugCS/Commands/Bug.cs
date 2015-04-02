using System;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands
{
    public static class Bug
    {
        public static void do_bug(CharacterInstance ch, string argument)
        {
            var tm = DateTime.Now;

            ch.SetColor(ATTypes.AT_PLAIN);
            if (CheckFunctions.CheckIfEmptyString(ch, argument,
                "Usage:  'bug <message>'  (your location is automatically recorded)")) return;

            db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.PBug),
                           string.Format("({0}):  {1}", tm.ToLongDateString(), argument));
            ch.SendTo("Thanks, your bug notice has been recorded.");
        }
    }
}
