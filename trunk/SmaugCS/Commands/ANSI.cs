using System;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class ANSI
    {
        public static void do_ansi(CharacterInstance ch, string argument)
        {
            string arg = argument.FirstWord();

            if (string.IsNullOrWhiteSpace(arg))
                AnsiError(ch);
            else if (arg.EqualsIgnoreCase("on"))
                AnsiOn(ch);
            else if (arg.EqualsIgnoreCase("off"))
                AnsiOff(ch);
        }

        private static void AnsiError(CharacterInstance actor)
        {
            color.send_to_char("ANSI ON or OFF?\r\n", actor);
        }

        private static void AnsiOn(CharacterInstance actor)
        {
            actor.Act.IsSet(PlayerFlags.Ansi);
            color.set_char_color(ATTypes.AT_WHITE | ATTypes.AT_BLINK, actor);
            color.send_to_char("ANSI ON!!!\r\n", actor);
        }

        private static void AnsiOff(CharacterInstance actor)
        {
            actor.Act.RemoveBit(PlayerFlags.Ansi);
            color.send_to_char("Okay... ANSI support is now off.\r\n", actor);
        }
    }
}
