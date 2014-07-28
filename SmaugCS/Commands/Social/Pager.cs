using System;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Social
{
    public static class Pager
    {
        public static void do_pager(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch)) return;

            color.set_char_color(ATTypes.AT_NOTE, ch);
            string firstArg = argument.FirstWord();

            if (string.IsNullOrEmpty(firstArg))
            {
                TogglePager(ch);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, !firstArg.IsNumber(), "Set page pausing to how many lines?")) return;

            ch.PlayerData.PagerLineCount = Convert.ToInt32(firstArg);
            if (ch.PlayerData.PagerLineCount < 5)
                ch.PlayerData.PagerLineCount = 5;

            color.ch_printf(ch, "Page pausing set to {0} lines.", ch.PlayerData.PagerLineCount);
        }

        private static void TogglePager(CharacterInstance ch)
        {
            if (ch.PlayerData.Flags.IsSet((int)PCFlags.PagerOn))
            {
                color.send_to_char("Pager disabled.", ch);
                Config.do_config(ch, "-pager");
            }
            else
            {
                color.ch_printf(ch, "Pager is now enabled at {0} lines.", ch.PlayerData.PagerLineCount);
                Config.do_config(ch, "+pager");
            }
        }
    }
}
