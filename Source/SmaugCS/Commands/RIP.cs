using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;

namespace SmaugCS.Commands
{
    public static class RIP
    {
        public static void do_rip(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Rip ON or OFF?")) return;

            if (firstArg.EqualsIgnoreCase("on"))
                EnableRip((PlayerInstance)ch);
            else if (firstArg.EqualsIgnoreCase("off"))
                DisableRip((PlayerInstance)ch);
            else
                ch.SendTo("Huh?!?");
        }

        private static void EnableRip(PlayerInstance ch)
        {
            ch.SendRIPScreen();
            ch.Act.IsSet((int)PlayerFlags.Rip);
            ch.Act.IsSet((int)PlayerFlags.Ansi);
        }

        private static void DisableRip(PlayerInstance ch)
        {
            ch.Act.RemoveBit((int)PlayerFlags.Rip);
            ch.SendTo("!|*\r\nRIP now off...");
        }
    }
}
