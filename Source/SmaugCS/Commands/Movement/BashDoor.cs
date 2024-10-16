﻿using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement;

public static class BashDoor
{
  public static void do_bashdoor(CharacterInstance ch, string argument)
  {
    SkillData skill = Program.RepositoryManager.GetEntity<SkillData>("bashdoor");
    if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ch.Level < skill.SkillLevels.ToList()[(int)ch.CurrentClass],
          "You're not enough of a warrior to bash doors!")) return;

    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Bash what?")) return;
    if (CheckFunctions.CheckIfNotNullObject(ch, ch.CurrentFighting, "You can't break off your fight.")) return;

    ExitData exit = ch.FindExit(firstArg);
    if (exit == null)
      Bash(ch, skill, "wall");
    else
      BashSomething(ch, exit, skill, firstArg);
  }

  private static void Bash(CharacterInstance actor, SkillData skill, string arg)
  {
    comm.act(ATTypes.AT_SKILL, "WHAAAAM!!! You bash against the $d, but it doesn't budge.", actor, null, arg,
      ToTypes.Character);
    comm.act(ATTypes.AT_SKILL, "WHAAAAM!!! $n bashes against the $d, but it holds strong.", actor, null, arg,
      ToTypes.Room);

    int damage = actor.MaximumHealth / 20 + 10;
    actor.CauseDamageTo(actor, damage, (int)skill.Id);
    skill.LearnFromFailure(actor);
  }

  private static void BashSomething(CharacterInstance actor, ExitData exit, SkillData skill, string arg)
  {
    if (CheckFunctions.CheckIfSet(actor, exit.Flags, ExitFlags.Closed, "Calm down. It is already open."))
      return;

    Macros.WAIT_STATE(actor, skill.Rounds);

    string keyword = exit.Flags.IsSet(ExitFlags.Secret) ? "wall" : exit.Keywords;

    long chance = !actor.IsNpc()
      ? Macros.LEARNED(actor, (int)skill.Id) / 2
      : 90;

    if (exit.Flags.IsSet(ExitFlags.Locked))
      chance /= 3;

    if (exit.Flags.IsSet(ExitFlags.BashProof)
        || actor.CurrentMovement < 15
        || SmaugRandom.D100() >= chance + 4 * (actor.GetCurrentStrength() - 19))
    {
      Bash(actor, skill, arg);
      return;
    }

    BashExit(exit);

    comm.act(ATTypes.AT_SKILL, "Crash! You bashed open the $d!", actor, null, keyword, ToTypes.Character);
    comm.act(ATTypes.AT_SKILL, "$n bashes open the $d!", actor, null, keyword, ToTypes.Room);
    skill.LearnFromSuccess(actor);

    ExitData reverseExit = exit.GetReverse();
    BashExit(reverseExit);

    RoomTemplate destination = exit.GetDestination(Program.RepositoryManager);
    foreach (CharacterInstance ch in destination.Persons)
      comm.act(ATTypes.AT_SKILL, "The $d crashes open!", ch, null, reverseExit.Keywords, ToTypes.Character);

    actor.CauseDamageTo(actor, actor.CurrentHealth / 20, (int)skill.Id);
  }

  private static void BashExit(ExitData exit)
  {
    exit.Flags.RemoveBit(ExitFlags.Closed);
    if (exit.Flags.IsSet(ExitFlags.Locked))
      exit.Flags.RemoveBit(ExitFlags.Locked);
    exit.Flags.SetBit(ExitFlags.Bashed);
  }
}