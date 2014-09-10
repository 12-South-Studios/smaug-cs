using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Commands
{
    public static class RIP
    {
        public static void do_rip(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (Helpers.CheckFunctions.CheckIfEmptyString(ch, firstArg, "Rip ON or OFF?")) return;

            if (firstArg.EqualsIgnoreCase("on"))
                EnableRip(((PlayerInstance)ch));
            else if (firstArg.EqualsIgnoreCase("off"))
                DisableRip(((PlayerInstance)ch));
            else 
                color.send_to_char("Huh?!?", ch);
        }

        private static void EnableRip(PlayerInstance ch)
        {
            ch.SendRIPScreen();
            ch.Act.IsSet(PlayerFlags.Rip);
            ch.Act.IsSet(PlayerFlags.Ansi); 
        }

        private static void DisableRip(PlayerInstance ch)
        {
            ch.Act.RemoveBit(PlayerFlags.Rip);
            color.send_to_char("!|*\r\nRIP now off...", ch);
        }
    }
}
