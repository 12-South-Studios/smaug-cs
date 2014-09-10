using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Commands.Movement
{
    public static class Climb
    {
        public static void do_climb(CharacterInstance ch, string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                foreach (ExitData ext in ch.CurrentRoom.Exits.Where(ext => ext.Flags.IsSet(ExitFlags.xClimb)))
                {
                    Move.move_char(ch, ext, 0);
                    return;
                }

                color.send_to_char("You cannot climb here.", ch);
                return;
            }

            ExitData exit = ch.FindExit(argument, true);
            if (exit != null && exit.Flags.IsSet(ExitFlags.xClimb))
            {
                Move.move_char(ch, exit, 0);
                return;
            }

            color.send_to_char("You cannot climb there.", ch);
        }
    }
}
