﻿using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Split
    {
        public static void do_split(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Split how much?")) return;

            int amount = Convert.ToInt32(firstArg);
            if (CheckFunctions.CheckIfTrue(ch, amount < 0, "Your group wouldn't like that.")) return;
            if (CheckFunctions.CheckIfTrue(ch, amount == 0, "You hand out zero coins, but no one notices.")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentCoin < amount, "You don't have that much coin.")) return;

            int members = ch.CurrentRoom.Persons.Count(x => x.IsSameGroup(ch));
            if (ch.Act.IsSet(PlayerFlags.AutoGold) && members < 2)
                return;

            if (CheckFunctions.CheckIfTrue(ch, members < 2, "Just keep it all.")) return;

            int share = amount / members;
            if (CheckFunctions.CheckIfTrue(ch, share == 0, "Don't even bother, cheapskate.")) return;

            int extra = amount % members;
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