﻿using Library.Common;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS;

public static class interp
{
  private static readonly List<KeyValuePair<PositionTypes, string>> PositionMap
    =
    [
      new KeyValuePair<PositionTypes, string>(PositionTypes.Dead,
        "A little difficult to do when you are DEAD...\r\n"),

      new KeyValuePair<PositionTypes, string>(PositionTypes.Mortal, "You are hurt far too badly for that.\r\n"),
      new KeyValuePair<PositionTypes, string>(PositionTypes.Incapacitated,
        "You are hurt far too badly for that.\r\n"),

      new KeyValuePair<PositionTypes, string>(PositionTypes.Stunned, "You are too stunned to do that.\r\n"),
      new KeyValuePair<PositionTypes, string>(PositionTypes.Sleeping, "In your dreams, or what?\r\n"),
      new KeyValuePair<PositionTypes, string>(PositionTypes.Resting, "Nah... You feel too relaxed...\r\n"),
      new KeyValuePair<PositionTypes, string>(PositionTypes.Sitting, "You can't do that sitting down.\r\n"),
      new KeyValuePair<PositionTypes, string>(PositionTypes.Fighting,
        "This fighting style is too demanding for that!\r\n"),

      new KeyValuePair<PositionTypes, string>(PositionTypes.Defensive,
        "This fighting style is too demanding for that!\r\n"),

      new KeyValuePair<PositionTypes, string>(PositionTypes.Aggressive,
        "This fighting style is too demanding for that!\r\n"),

      new KeyValuePair<PositionTypes, string>(PositionTypes.Berserk,
        "This fighting style is too demanding for that!\r\n"),

      new KeyValuePair<PositionTypes, string>(PositionTypes.Evasive, "No way!  You are still fighting!\r\n")
    ];

  private const string FightingMessage = "No way!  You are still fighting!\r\n";

  public static bool check_pos(CharacterInstance ch, int position)
  {
    if (ch.IsNpc() && (int)ch.CurrentPosition > 3)
      return true;

    if ((int)ch.CurrentPosition >= position) return true;
    KeyValuePair<PositionTypes, string> kvp = PositionMap.FirstOrDefault(x => x.Key == ch.CurrentPosition);

    if (ch.IsInCombatPosition() && position <= (int)PositionTypes.Evasive)
      ch.SendTo(FightingMessage);
    else
      ch.SendTo(kvp.Value);
    return false;

  }

  public static bool valid_watch(string logline)
  {
    switch (logline.Length)
    {
      case 1 when logline.StartsWith('n') || logline.StartsWith('s')
                                          || logline.StartsWith('w') || logline.StartsWith('u') ||
                                          logline.StartsWith('d'):
      case 2 when logline.StartsWith("ne") || logline.StartsWith("nw"):
        return false;
      default:
        return logline.Length != 3 || (!logline.StartsWith("se") && !logline.StartsWith("sw"));
    }
  }

  public static void interpret(CharacterInstance ch, string argument)
  {
    Validation.IsNotNull(ch, "ch");
    if (ch.CurrentRoom == null)
      throw new NullReferenceException("Null room reference");

    string logLine = string.Empty;
    CommandData foundCmd = null;

    if (((PlayerInstance)ch).SubState == CharacterSubStates.RepeatCommand)
    {
      DoFunction fun = ch.LastCommand;
      if (fun == null)
      {
        ((PlayerInstance)ch).SubState = CharacterSubStates.None;
        throw new InvalidDataException("CharacterSubStates.RepeatCommand with null LastCommand");
      }

      foreach (CommandData cmd in Program.RepositoryManager.COMMANDS.Values)
      {
        if (cmd.DoFunction != fun) continue;
        foundCmd = cmd;
        break;
      }

      if (foundCmd == null)
        throw new InvalidDataException("CharacterSubStates.RepeatCommand: LastCommand was invalid");

      logLine = $"({foundCmd.Name}) {argument}";
    }

    if (foundCmd != null) return;
    // TODO      }

    string lastPlayerCmd = $"{ch.Name} used {logLine}";
    if (foundCmd != null && foundCmd.Log == LogAction.Never)
      logLine = "XXXXXXXX XXXXXXXX XXXXXXXX";

    if (!ch.IsNpc() && ((PlayerInstance)ch).Descriptor != null && valid_watch(logLine))
    {
      if (foundCmd != null && foundCmd.Flags.IsSet(CommandFlags.Watch))
      {
        // TODO Write the watch
      }
      else if (((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Watch))
      {
        // TODO Write the watch
      }
    }

    // TODO Some more logging/snooping stuff

    TimerData timer = ch.GetTimer(TimerTypes.DoFunction);
    if (timer != null)
    {
      CharacterSubStates substate = ((PlayerInstance)ch).SubState;
      ((PlayerInstance)ch).SubState = CharacterSubStates.TimerDoAbort;
      timer.Action.Value.Invoke(ch, string.Empty);
      if (ch.CharDied())
        return;
      if (((PlayerInstance)ch).SubState != CharacterSubStates.TimerDoAbort)
      {
        ((PlayerInstance)ch).SubState = substate;
        // TODO Extract timer
      }
      else
      {
        ((PlayerInstance)ch).SubState = substate;
        return;
      }
    }

    // TODO Look for command in skill/social table

    if (!check_pos(ch, foundCmd.Position))
      return;

    string buf = check_cmd_flags(ch, foundCmd);
    if (!buf.IsNullOrEmpty())
    {
      ch.SendTo(buf);
      return;
    }

    // TODO Nuisance

    ch.PreviousCommand = ch.LastCommand;
    ch.LastCommand = foundCmd.DoFunction;

    // TODO Timer

    // tail_chain();
  }

  public static bool check_social(CharacterInstance ch, string command, string argument)
  {
    SocialData social = Program.RepositoryManager.GetEntity<SocialData>(command);
    if (social == null)
      return false;

    if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.NoEmote),
          "You are anti-social!"))
      return true;

    switch (ch.CurrentPosition)
    {
      case PositionTypes.Dead:
        ch.SendTo("Lie still; you are DEAD.");
        return true;
      case PositionTypes.Incapacitated:
      case PositionTypes.Mortal:
        ch.SendTo("You are hurt far too badly for that.");
        return true;
      case PositionTypes.Stunned:
        ch.SendTo("You are too stunned to do that.");
        return true;
      case PositionTypes.Sleeping:
        if (social.Name.EqualsIgnoreCase("snore"))
          break;
        ch.SendTo("In your dreams, or what?");
        return true;
      default:
        throw new ArgumentOutOfRangeException();
    }

    int i = 0;
    // search the room for characters ignoring the social-sender and
    // temporarily remove them from the room until the social has
    // been completed
    RoomTemplate room = ch.CurrentRoom;
    List<CharacterInstance> ignoringList = [];
    foreach (CharacterInstance victim in ch.CurrentRoom.Persons)
    {
      if (i == 127)
        break;
      if (!victim.IsIgnoring(ch)) continue;
      if (!ch.IsImmortal() || victim.Trust > ch.Trust)
      {
        ignoringList.Add(victim);
        i++;
        room.Persons.Remove(victim);
      }
      else
      {
        victim.SetColor(ATTypes.AT_IGNORE);
        victim.Printf("You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
      }
    }

    // TODO
    return false;
  }

  public static string check_cmd_flags(CharacterInstance ch, CommandData cmd)
  {
    string buf = string.Empty;
    if (ch.IsAffected(AffectedByTypes.Possess) && cmd.Flags.IsSet(CommandFlags.Possess))
      buf = $"You can't {cmd.Name} while you are possessing someone!";
    else if (ch.CurrentMorph != null && cmd.Flags.IsSet(CommandFlags.Polymorphed))
      buf = $"You can't {cmd.Name} while you are polymorphed!";
    return buf;
  }
}