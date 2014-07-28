using System;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands
{
    public static class RIP
    {
        public static void do_rip(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (Helpers.CheckFunctions.CheckIfEmptyString(ch, firstArg, "Rip ON or OFF?")) return;

            if (firstArg.EqualsIgnoreCase("on"))
                EnableRip(ch);
            else if (firstArg.EqualsIgnoreCase("off"))
                DisableRip(ch);
            else 
                color.send_to_char("Huh?!?", ch);
        }

        private static void EnableRip(CharacterInstance ch)
        {
            act_comm.send_rip_screen(ch);
            ch.Act.IsSet(PlayerFlags.Rip);
            ch.Act.IsSet(PlayerFlags.Ansi); 
        }

        private static void DisableRip(CharacterInstance ch)
        {
            ch.Act.RemoveBit(PlayerFlags.Rip);
            color.send_to_char("!|*\r\nRIP now off...", ch);
        }
    }
}
