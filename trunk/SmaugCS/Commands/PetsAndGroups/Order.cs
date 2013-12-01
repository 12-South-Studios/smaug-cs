using System;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Order
    {
        public static void do_order(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrWhiteSpace(firstArg))
            {
                color.send_to_char("Order whom to do what?\r\n", ch);
                return;
            }

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                color.send_to_char("You feel like taking, not giving, orders.\r\n", ch);
                return;
            }

            if (firstArg.Equals("mp", StringComparison.OrdinalIgnoreCase))
            {
                color.send_to_char("No... I don't think so.\r\n", ch);
                return;
            }

            bool all = false;
            CharacterInstance victim = null;

            string secondArg = argument.SecondWord();
            if (secondArg.Equals("all", StringComparison.OrdinalIgnoreCase))
                all = true;
            else
            {
                victim = handler.get_char_room(ch, secondArg);
                if (victim == null)
                {
                    color.send_to_char("They aren't here.\r\n", ch);
                    return;
                }

                if (victim == ch)
                {
                    color.send_to_char("Aye aye, right away!\r\n", ch);
                    return;
                }

                if (!victim.IsAffected(AffectedByTypes.Charm)
                    || victim.Master != ch)
                {
                    color.send_to_char("Do it yourself!\r\n", ch);
                    return;
                }
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

            if (found)
            {
                LogManager.Instance.Log("{0}: order {1}.", ch.Name, string.Format(argument, (int)LogTypes.Normal, ch.Level));
                color.send_to_char("Ok.\r\n", ch);
                Macros.WAIT_STATE(ch, 12);
            }
            else
                color.send_to_char("You have no followers here.\r\n", ch);
        }
    }
}
