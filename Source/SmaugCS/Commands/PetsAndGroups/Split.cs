using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Split
    {
        public static void do_split(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Split how much?")) return;

            var amount = Convert.ToInt32(firstArg);
            if (CheckFunctions.CheckIfTrue(ch, amount < 0, "Your group wouldn't like that.")) return;
            if (CheckFunctions.CheckIfTrue(ch, amount == 0, "You hand out zero coins, but no one notices.")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentCoin < amount, "You don't have that much coin.")) return;

            var members = ch.CurrentRoom.Persons.Count(x => x.IsSameGroup(ch));
            if (ch.Act.IsSet(PlayerFlags.AutoGold) && members < 2)
                return;

            if (CheckFunctions.CheckIfTrue(ch, members < 2, "Just keep it all.")) return;

            var share = amount / members;
            if (CheckFunctions.CheckIfTrue(ch, share == 0, "Don't even bother, cheapskate.")) return;

            var extra = amount % members;
            ch.CurrentCoin -= amount;
            ch.CurrentCoin += share + extra;

           ch.SetColor(ATTypes.AT_GOLD);
            ch.Printf("You split %d gold coins.  Your share is %d gold coins.\r\n", amount, share + extra);

            var buffer = $"$n splits {amount} gold coins. Your share is {share} gold coins.";
            foreach (var gch in ch.CurrentRoom.Persons.Where(x => x.IsSameGroup(ch) && x != ch))
            {
                comm.act(ATTypes.AT_GOLD, buffer, ch, null, gch, ToTypes.Victim);
                gch.CurrentCoin += share;
            }
        }
    }
}
