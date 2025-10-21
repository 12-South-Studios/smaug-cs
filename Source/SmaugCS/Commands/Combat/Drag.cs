using System;
using System.Collections.Generic;
using Library.Common.Extensions;
using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using EnumerationExtensions = Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS.Commands.Combat;

public static class Drag
{
  public static void do_drag(CharacterInstance ch, string argument)
  {
    if (CheckFunctions.CheckIfNpc(ch, ch, "Only characters can drag.")) return;
    if (CheckFunctions.CheckIfTrue(ch, ch.HasTimer(TimerTypes.PKilled), "You can't drag a player right now."))
      return;

    PlayerInstance pch = (PlayerInstance)ch;

    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(pch, firstArg, "Drag whom?")) return;

    CharacterInstance victim = pch.GetCharacterInRoom(firstArg);
    if (CheckFunctions.CheckIfNullObject(pch, victim, "They aren't here.")) return;
    if (CheckFunctions.CheckIfEquivalent(pch, pch, victim,
          "You take yourself by the scruff of your neck, but go nowhere.")) return;
    if (CheckFunctions.CheckIfNpc(ch, victim, "You can only drag characters.")) return;
    if (CheckFunctions.CheckIfTrue(ch,
          !victim.Act.IsSet((int)PlayerFlags.ShoveDrag) || (!victim.IsNpc() && !victim.IsDeadly()),
          "That character doesn't seem to appreciate your attentions.")) return;
    if (CheckFunctions.CheckIfTrue(ch, victim.HasTimer(TimerTypes.PKilled),
          "You can't drag that player right now.")) return;
    if (CheckFunctions.CheckIf(ch, HelperFunctions.IsFighting,
          "You try, but can't get close enough.", [ch])) return;
    if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && !victim.IsDeadly() && ch.IsDeadly(),
          "You can't drag a deadly character.")) return;
    if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && !ch.IsDeadly() && (int)ch.CurrentPosition > 3,
          "They don't seem to need your assistance.")) return;

    string secondArg = argument.SecondWord();
    if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Drag them in which direction?")) return;
    if (CheckFunctions.CheckIfTrue(ch, Math.Abs(ch.Level - victim.Level) > 5,
          "There is too great an experience difference for you to even bother.")) return;
    if (CheckFunctions.CheckIfTrue(ch,
          victim.CurrentRoom.Flags.IsSet(RoomFlags.Safe) && victim.GetTimer(TimerTypes.ShoveDrag) == null,
          "That character cannot be dragged right now."))
      return;

    DirectionTypes exitDir = EnumerationExtensions.GetEnumByName<DirectionTypes>(secondArg);
    ExitData exit = ch.CurrentRoom.GetExit(exitDir);
    if (CheckFunctions.CheckIfNullObject(ch, exit, "There's no exit in that direction.")) return;
    if (CheckFunctions.CheckIfTrue(ch, !IsPassable(exit, victim), "There's no exit in that direction.")) return;

    RoomTemplate toRoom = exit.GetDestination();
    if (CheckFunctions.CheckIfSet(ch, toRoom.Flags, RoomFlags.Death,
          "You cannot drag someone into a death trap.")) return;

    if (CheckFunctions.CheckIfTrue(ch, ch.CurrentRoom.Area != toRoom.Area && !toRoom.Area.IsInHardRange(victim),
          "That character cannot enter that area."))
    {
      victim.CurrentPosition = PositionTypes.Standing;
      return;
    }

    int chance = CalculateChanceToDrag(ch, victim);
    if (CheckFunctions.CheckIfTrue(ch, chance < SmaugRandom.D100(), "You failed."))
    {
      victim.CurrentPosition = PositionTypes.Standing;
      return;
    }

    if ((int)victim.CurrentPosition < (int)PositionTypes.Standing)
    {
      DragIntoNextRoom(ch, victim, exit);
      return;
    }

    ch.SendTo("You cannot do that to someone who is standing.");
  }

  private static bool IsPassable(ExitData exit, CharacterInstance victim)
  {
    return exit.Flags.IsSet(ExitFlags.Closed)
           &&
           (!victim.IsAffected(AffectedByTypes.PassDoor) ||
            exit.Flags.IsSet(ExitFlags.NoPassDoor));
  }

  private static int CalculateChanceToDrag(CharacterInstance ch, CharacterInstance victim)
  {
    int chance = GetChanceByCharacterClass(ch);
    chance += (ch.GetCurrentStrength() - 15) * 3;
    chance += ch.Level - victim.Level;
    chance += GetBonusByCharacterRace(ch);
    return chance;
  }

  private static void DragIntoNextRoom(CharacterInstance ch, CharacterInstance victim, ExitData exit)
  {
    PositionTypes temp = victim.CurrentPosition;
    victim.CurrentPosition = PositionTypes.Drag;

    comm.act(ATTypes.AT_ACTION, "You drag $M into the next room.", ch, victim, null, ToTypes.Character);
    comm.act(ATTypes.AT_ACTION, "$n grabs your hair and drags you.", ch, victim, null, ToTypes.Victim);
    Move.move_char(victim, exit, 0);

    if (!victim.CharDied())
      victim.CurrentPosition = temp;

    Move.move_char(ch, exit, 0);
    Macros.WAIT_STATE(ch, 12);
  }

  private static int GetChanceByCharacterClass(CharacterInstance ch)
  {
    DragValueAttribute attrib = ch.CurrentClass.GetAttribute<DragValueAttribute>();
    return attrib?.ModValue ?? 0;
  }

  private static int GetBonusByCharacterRace(CharacterInstance ch)
  {
    DragValueAttribute attrib = ch.CurrentRace.GetAttribute<DragValueAttribute>();
    return attrib?.ModValue ?? 0;
  }
}