using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Movement
{
    public static class Climb
    {
        public static void do_climb(CharacterInstance ch, string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                foreach (var ext in ch.CurrentRoom.Exits.Where(ext => ext.Flags.IsSet(ExitFlags.xClimb)))
                {
                    Move.move_char(ch, ext, 0);
                    return;
                }

                ch.SendTo("You cannot climb here.");
                return;
            }

            var exit = ch.FindExit(argument, true);
            if (exit != null && exit.Flags.IsSet(ExitFlags.xClimb))
            {
                Move.move_char(ch, exit, 0);
                return;
            }

            ch.SendTo("You cannot climb there.");
        }
    }
}
