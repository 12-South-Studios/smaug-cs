using System;
using System.IO;
using Realm.Library.Common;
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

            if (string.IsNullOrWhiteSpace(argument))
            {
                color.send_to_char("\r\nUsage:  'typo <message>'  (your location is automatically recorded)\r\n", ch);
                if (ch.Trust >= LevelConstants.GetLevel("ascendant"))
                    color.send_to_char("Usage:  'typo list' or 'typo clear now'\r\n", ch);
                return;
            }

            if (argument.Equals("clear now", StringComparison.OrdinalIgnoreCase)
                && ch.Trust >= LevelConstants.GetLevel("ascendant"))
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
