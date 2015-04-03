using System;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Repository;

namespace SmaugCS.Commands.Movement
{
    public static class Leave
    {
        public static void do_leave(CharacterInstance ch, string argument)
        {
            if (String.IsNullOrEmpty(argument))
            {
                LeaveRoom(ch);
                return;
            }

            var exit = ch.FindExit(argument, true);
            if (exit != null && exit.Flags.IsSet(ExitFlags.xLeave))
            {
                Move.move_char(ch, exit, 0);
                return;
            }

            ch.SendTo("You cannot leave that way.");
        }

        private static void LeaveRoom(CharacterInstance ch)
        {
            var exit = ch.CurrentRoom.Exits.FirstOrDefault(x => x.Flags.IsSet(ExitFlags.xLeave));
            if (exit != null)
            {
                Move.move_char(ch, exit, 0);
                return;
            }

            if (ch.CurrentRoom.SectorType != SectorTypes.Inside && !ch.IsOutside())
            {
                foreach (var xit in ch.CurrentRoom.Exits)
                {
                    var room = xit.GetDestination(RepositoryManager.Instance);
                    if (room == null ||
                        (room.SectorType != SectorTypes.Inside && !room.Flags.IsSet(RoomFlags.Indoors))) continue;

                    Move.move_char(ch, xit, 0);
                    return;
                }
            }

            ch.SendTo("You cannot find an exit here.");
        }
    }
}
