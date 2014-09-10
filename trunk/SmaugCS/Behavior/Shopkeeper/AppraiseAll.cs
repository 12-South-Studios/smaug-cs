using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Behavior.Shopkeeper
{
    public static class AppraiseAll
    {
        public static void appraise_all(CharacterInstance ch, MobileInstance keeper, string fixstr)
        {
            int total = 0, cost = 0;
            string buffer;

            foreach (ObjectInstance obj in ch.Carrying.Where(obj => obj.WearLocation == WearLocations.None
                                                                    && ch.CanSee(obj)
                                                                    && (obj.ItemType.GetAttribute<ValuedAttribute>() != null)))
            {
                if (!ch.CanDrop(obj))
                    color.ch_printf(ch, "You can't let go of %s.", obj.Name);
                else if ((cost = Appraise.GetAdjustedRepairCost(keeper, obj)) > 0)
                {
                    comm.act(ATTypes.AT_TELL,
                             cost != -2
                                 ? "$n tells you, 'Sorry, I can't do anything with $p.'"
                                 : "$n tells you, '$p looks fine to me!'", keeper, obj, ch, ToTypes.Victim);
                }
                else
                {
                    buffer = string.Format("$N tells you, 'It will cost {0} piece{1} of gold to {2} {3}.'",
                                           cost, cost == 1 ? "" : "s", fixstr, obj.Name);
                    comm.act(ATTypes.AT_TELL, buffer, ch, null, keeper, ToTypes.Character);
                    total += cost;
                }
            }

            if (total > 0)
            {
                color.send_to_char("\r\n", ch);

                buffer = string.Format("$N tells you, 'It will cost {0} piece{1} of gold in total.'", total,
                                              cost == 1 ? "" : "s");
                comm.act(ATTypes.AT_TELL, buffer, ch, null, keeper, ToTypes.Character);
                comm.act(ATTypes.AT_TELL, "$N tells you, 'Remember there is a 10% surcharge for repairing all your items.", ch, null, keeper, ToTypes.Character);
            }
        }
    }
}
