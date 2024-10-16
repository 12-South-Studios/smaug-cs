﻿using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.PetsAndGroups;

public static class Order
{
  public static void do_order(CharacterInstance ch, string argument)
  {
    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Order whom to do what?")) return;

    if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Charm),
          "You feel like taking, not giving, orders.")) return;

    if (CheckFunctions.CheckIfTrue(ch, firstArg.EqualsIgnoreCase("mp"), "No... I don't think so.")) return;

    bool all = false;
    CharacterInstance victim = null;

    string secondArg = argument.SecondWord();
    if (secondArg.EqualsIgnoreCase("all"))
      all = true;
    else
    {
      victim = ch.GetCharacterInRoom(secondArg);
      if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
      if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "Aye aye, right away!")) return;
      if (CheckFunctions.CheckIfTrue(ch, !victim.IsAffected(AffectedByTypes.Charm) || victim.Master != ch,
            "Do it yourself!")) return;
    }

    bool found = false;
    foreach (CharacterInstance och in ch.CurrentRoom.Persons
               .Where(och => och.IsAffected(AffectedByTypes.Charm)
                             && och.Master == ch && (all || och == victim)))
    {
      found = true;
      comm.act(ATTypes.AT_ACTION, "$n orders you to '$t'", ch, firstArg, och, ToTypes.Victim);
      interp.interpret(och, firstArg);
    }

    if (CheckFunctions.CheckIfTrue(ch, !found, "You have no followers here.")) return;

    Program.LogManager.Info("{0}: order {1}.", ch.Name, string.Format(argument, (int)LogTypes.Info, ch.Level));
    ch.SendTo("Ok.");
    Macros.WAIT_STATE(ch, 12);
  }
}