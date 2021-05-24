using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.Linq;

namespace SmaugCS.Behavior
{
    public static class AppraiseAll
    {
        public static void appraise_all(CharacterInstance ch, MobileInstance keeper, string fixstr)
        {
            int total = 0, cost = 0;
            string buffer;

            foreach (var obj in ch.Carrying.Where(obj => obj.WearLocation == WearLocations.None
                                                                    && ch.CanSee(obj)
                                                                    && (obj.ItemType.GetAttribute<ValuedAttribute>() != null)))
            {
                if (!ch.CanDrop(obj))
                {
                    ch.Printf("You can't let go of %s.", obj.Name);
                    continue;
                }

                if ((cost = Appraise.GetAdjustedRepairCost(keeper, obj)) > 0)
                {
                    comm.act(ATTypes.AT_TELL,
                        cost != -2
                            ? "$n tells you, 'Sorry, I can't do anything with $p.'"
                            : "$n tells you, '$p looks fine to me!'", keeper, obj, ch, ToTypes.Victim);
                    continue;
                }

                buffer =
                    $"$N tells you, 'It will cost {cost} piece{(cost == 1 ? "" : "s")} of gold to {fixstr} {obj.Name}.'";
                comm.act(ATTypes.AT_TELL, buffer, ch, null, keeper, ToTypes.Character);
                total += cost;
            }

            if (total <= 0) return;

            ch.SendTo("\r\n");

            buffer = $"$N tells you, 'It will cost {total} piece{(cost == 1 ? "" : "s")} of gold in total.'";
            comm.act(ATTypes.AT_TELL, buffer, ch, null, keeper, ToTypes.Character);
            comm.act(ATTypes.AT_TELL, "$N tells you, 'Remember there is a 10% surcharge for repairing all your items.", ch, null, keeper, ToTypes.Character);
        }
    }
}
