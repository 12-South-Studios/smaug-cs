using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common.Extensions;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Split
    {
        public static void do_split(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();

            if (string.IsNullOrEmpty(firstArg))
            {
                color.send_to_char("Split how much?\r\n", ch);
                return;
            }

            int amount = Convert.ToInt32(firstArg);

            if (amount < 0)
            {
                color.send_to_char("Your group wouldn't like that.\r\n", ch);
                return;
            }

            if (amount == 0)
            {
                color.send_to_char("You hand out zero coins, but no one notices.\r\n", ch);
                return;
            }

            if (ch.CurrentCoin < amount)
            {
                color.send_to_char("You don't have that much gold.\r\n", ch);
                return;
            }

            int members = ch.CurrentRoom.Persons.Count(x => x.IsSameGroup(ch));

            if (ch.Act.IsSet((int)PlayerFlags.AutoGold) && members < 2)
                return;

            if (members < 2)
            {
                color.send_to_char("Just keep it all.\r\n", ch);
                return;
            }

            int share = amount / members;
            int extra = amount % members;

            if (share == 0)
            {
                color.send_to_char("Don't even bother, cheapskate.\r\n", ch);
                return;
            }

            ch.CurrentCoin -= amount;
            ch.CurrentCoin += share + extra;

            color.set_char_color(ATTypes.AT_GOLD, ch);
            color.ch_printf(ch, "You split %d gold coins.  Your share is %d gold coins.\r\n", amount, share + extra);

            string buffer = string.Format("$n splits {0} gold coins. Your share is {1} gold coins.", amount, share);

            foreach (CharacterInstance gch in ch.CurrentRoom.Persons.Where(x => x.IsSameGroup(ch) && x != ch))
            {
                comm.act(ATTypes.AT_GOLD, buffer, ch, null, gch, ToTypes.Victim);
                gch.CurrentCoin += share;
            }
        }
    }
}
