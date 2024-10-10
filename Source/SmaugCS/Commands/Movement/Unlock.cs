using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement;

public static class Unlock
{
  public static void do_unlock(CharacterInstance ch, string argument)
  {
    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Unlock what?")) return;

    ExitData exit = ch.FindExit(firstArg, true);
    if (exit != null)
    {
      UnlockDoor(ch, exit, firstArg);
      return;
    }

    ObjectInstance obj = ch.GetObjectOnMeOrInRoom(firstArg);
    if (obj != null)
    {
      UnlockObject(ch, obj);
      return;
    }

    ch.Printf("You see no %s here.", firstArg);
  }

  private static void UnlockObject(CharacterInstance ch, ObjectInstance obj)
  {
    if (CheckFunctions.CheckIfTrue(ch, obj.ItemType != ItemTypes.Container, "That's not a container.")) return;
    if (CheckFunctions.CheckIfNotSet(ch, obj.Value.ToList()[1], ContainerFlags.Closed, "It's not closed.")) return;
    if (CheckFunctions.CheckIfTrue(ch, obj.Value.ToList()[2] < 0, "It can't be unlocked.")) return;

    ObjectInstance key = ch.HasKey(obj.Value.ToList()[2]);
    if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
    if (CheckFunctions.CheckIfNotSet(ch, obj.Value.ToList()[1], ExitFlags.Locked, "It's already unlocked.")) return;

    obj.Value.ToList()[1].RemoveBit(ContainerFlags.Locked);
    ch.SendTo("*Click*");
    int count = key.Count;
    key.Count = 1;
    comm.act(ATTypes.AT_ACTION, "$n unlocks $p with $P.", ch, obj, key, ToTypes.Room);
    key.Count = count;

    if (!obj.Value.ToList()[1].IsSet(ContainerFlags.EatKey)) return;
    key.Split();
    key.Extract();
  }

  private static void UnlockDoor(CharacterInstance ch, ExitData exit, string firstArg)
  {
    if (exit.Flags.IsSet(ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(firstArg))
    {
      ch.Printf("You see no %s here.", firstArg);
      return;
    }

    if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsDoor, "You can't do that.")) return;
    if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Closed, "It's not closed.")) return;
    if (CheckFunctions.CheckIfTrue(ch, exit.Key < 0, "It can't be unlocked.")) return;

    ObjectInstance key = ch.HasKey(exit.Key);
    if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
    if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Locked, "It's already unlocked.")) return;

    if (exit.Flags.IsSet(ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(firstArg)) return;
    ch.SendTo("*Click*");
    int count = key.Count;
    key.Count = 1;
    comm.act(ATTypes.AT_ACTION, "$n unlocks the $d with $p.", ch, key, exit.Keywords, ToTypes.Room);
    key.Count = count;

    if (exit.Flags.IsSet(ExitFlags.EatKey))
    {
      key.Split();
      key.Extract();
    }

    exit.RemoveFlagFromSelfAndReverseExit(ExitFlags.Locked);
  }
}