using System;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class ANSI
    {
        public static void do_ansi(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrWhiteSpace(firstArg))
            {
                color.send_to_char("ANSI ON or OFF?\r\n", ch);
                return;
            }

            if (firstArg.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                ch.Act.IsSet((int)PlayerFlags.Ansi);
                color.set_char_color(ATTypes.AT_WHITE | ATTypes.AT_BLINK, ch);
                color.send_to_char("ANSI ON!!!\r\n", ch);
                return;
            }

            if (firstArg.Equals("off", StringComparison.OrdinalIgnoreCase))
            {
                ch.Act.RemoveBit((int)PlayerFlags.Ansi);
                color.send_to_char("Okay... ANSI support is now off.\r\n", ch);
            }
        }
    }
}
