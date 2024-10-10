using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Movement;

public static class Enter
{
  public static void do_enter(CharacterInstance ch, string argument)
  {
    if (string.IsNullOrEmpty(argument))
    {
      EnterExit(ch);
      return;
    }

    ExitData exit = ch.FindExit(argument, true);
    if (exit != null && exit.Flags.IsSet(ExitFlags.xEnter))
    {
      Move.move_char(ch, exit, 0);
      return;
    }

    ch.SendTo("You cannot enter that.");
  }

  private static void EnterExit(CharacterInstance ch)
  {
    ExitData exit = ch.CurrentRoom.Exits.FirstOrDefault(x => x.Flags.IsSet(ExitFlags.xEnter));
    if (exit != null)
    {
      Move.move_char(ch, exit, 0);
      return;
    }

    if (ch.CurrentRoom.SectorType != SectorTypes.Inside && ch.IsOutside())
    {
      foreach (ExitData xit in ch.CurrentRoom.Exits)
      {
        RoomTemplate room = xit.GetDestination(Program.RepositoryManager);
        if (room == null ||
            (room.SectorType != SectorTypes.Inside && !room.Flags.IsSet(RoomFlags.Indoors))) continue;

        Move.move_char(ch, xit, 0);
        return;
      }
    }

    ch.SendTo("You cannot find an entrance here.");
  }
}