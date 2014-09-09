using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Movement
{
    public static class Leave
    {
        public static void do_leave(CharacterInstance ch, string argument)
        {
            if (String.IsNullOrEmpty(argument))
            {
                LeaveRoom(ch, argument);
                return;
            }

            ExitData exit = act_move.find_door(ch, argument, true);
            if (exit != null && exit.Flags.IsSet((int) ExitFlags.xLeave))
            {
                Move.move_char(ch, exit, 0);
                return;
            }

            color.send_to_char("You cannot leave that way.", ch);
        }

        private static void LeaveRoom(CharacterInstance ch, string argument)
        {
            ExitData exit = ch.CurrentRoom.Exits.FirstOrDefault(x => x.Flags.IsSet((int) ExitFlags.xLeave));
            if (exit != null)
            {
                Move.move_char(ch, exit, 0);
                return;
            }

            if (ch.CurrentRoom.SectorType != SectorTypes.Inside && !ch.IsOutside())
            {
                foreach (ExitData xit in ch.CurrentRoom.Exits)
                {
                    RoomTemplate room = xit.GetDestination(DatabaseManager.Instance);
                    if (room == null ||
                        (room.SectorType != SectorTypes.Inside && !room.Flags.IsSet((int)RoomFlags.Indoors))) continue;

                    Move.move_char(ch, xit, 0);
                    return;
                }
            }

            color.send_to_char("You cannot find an exit here.", ch);
        }
    }
}
