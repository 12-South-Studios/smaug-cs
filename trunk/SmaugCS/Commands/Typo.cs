using System;
using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands
{
    public static class Typo
    {
        public static void do_typo(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_PLAIN, ch);

            if (Helpers.CheckFunctions.CheckIfEmptyString(ch, argument,
                "Usage:  'typo <message>'  (your location is automatically recorded)"))
            {
                if (ch.Trust >= LevelConstants.GetLevel(ImmortalTypes.Ascendant))
                    color.send_to_char("Usage:  'typo list' or 'typo clear now'", ch);
                return;
            }

            if (argument.EqualsIgnoreCase("clear now")
                && ch.Trust >= LevelConstants.GetLevel(ImmortalTypes.Ascendant))
            {
                string path = SystemConstants.GetSystemFile(SystemFileTypes.Typo);
                using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path, false)))
                {
                    proxy.Write(string.Empty);
                }
                color.send_to_char("Typo file cleared.\r\n", ch);
            }
        }
    }
}
