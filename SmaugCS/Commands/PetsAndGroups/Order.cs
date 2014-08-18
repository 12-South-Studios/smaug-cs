﻿using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Logging;

namespace SmaugCS.Commands.PetsAndGroups
{
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
                victim = CharacterInstanceExtensions.GetCharacterInRoom(ch, secondArg);
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

            LogManager.Instance.Info("{0}: order {1}.", ch.Name, string.Format(argument, (int)LogTypes.Info, ch.Level));
            color.send_to_char("Ok.\r\n", ch);
            Macros.WAIT_STATE(ch, 12);
        }
    }
}