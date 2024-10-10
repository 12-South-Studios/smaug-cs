using System.Linq;
using Library.Common.Objects;
using Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Repository;

namespace SmaugCS.Extensions;

public static class ExitDataExtensions
{
  public static void Extract(this ExitData exit)
  {
    RoomTemplate room = Program.RepositoryManager.ROOMS.Get(exit.Room_vnum);
    room.Exits.Remove(exit);
    ExitData reverseExit = exit.GetReverse();
    reverseExit.Reverse = 0;
  }

  public static ExitData GetReverse(this ExitData exit)
  {
    if (exit.Reverse == 0) return null;
    return exit.GetDestination() == null ? null : exit.GetDestination().GetExit((int)exit.Reverse);
  }

  public static RoomTemplate GetDestination(this ExitData exit, IRepositoryManager dbManager = null)
  {
    return (dbManager ?? Program.RepositoryManager).ROOMS.CastAs<Repository<long, RoomTemplate>>()
      .Values.ToList()
      .Find(x => x.Id == exit.Destination);
  }

  /// <summary>
  /// Was formerly set_bexit_flag
  /// </summary>
  /// <param name="exit"></param>
  /// <param name="flag"></param>
  public static void SetFlagOnSelfAndReverseExit(this ExitData exit, ExitFlags flag)
  {
    exit.Flags = exit.Flags.SetBit(flag);

    ExitData reverseExit = exit.GetReverse();
    if (reverseExit != null && reverseExit != exit)
      reverseExit.Flags = reverseExit.Flags.SetBit(flag);
  }

  /// <summary>
  /// Was formerly remove_bexit_flag
  /// </summary>
  /// <param name="exit"></param>
  /// <param name="flag"></param>
  public static void RemoveFlagFromSelfAndReverseExit(this ExitData exit, ExitFlags flag)
  {
    exit.Flags = exit.Flags.RemoveBit(flag);

    ExitData reverseExit = exit.GetReverse();
    if (reverseExit != null && reverseExit != exit)
      reverseExit.Flags = reverseExit.Flags.RemoveBit(flag);
  }

  /// <summary>
  /// Was formerly toggle_bexit_flag
  /// </summary>
  /// <param name="exit"></param>
  /// <param name="flag"></param>
  public static void ToggleFlagOnSelfAndReverseExit(this ExitData exit, ExitFlags flag)
  {
    exit.Flags = exit.Flags.ToggleBit(flag);

    ExitData reverseExit = exit.GetReverse();
    if (reverseExit != null && reverseExit != exit)
      reverseExit.Flags = reverseExit.Flags.ToggleBit(flag);
  }
}