using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Shops;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class shops
    {
        public static CharacterInstance find_keeper(CharacterInstance ch)
        {
            CharacterInstance keeper =
                ch.CurrentRoom.Persons.FirstOrDefault(mob => mob.IsNpc() && mob.MobIndex.Shop != null);

            if (keeper == null)
            {
                color.send_to_char("You can't do that here.\r\n", ch);
                return null;
            }

            if (!ch.IsNpc())
            {
                if (ch.Act.IsSet((int)PlayerFlags.Killer))
                    Say.do_say(keeper, "Murderers are not welcome here!");
                else if (ch.Act.IsSet((int)PlayerFlags.Thief))
                    Say.do_say(keeper, "Thieves are not welcome here!");

                if (ch.Act.IsSet((int)PlayerFlags.Killer))
                    Shout.do_shout(keeper, string.Format("{0} the KILLER is over here!", ch.Name));
                else if (ch.Act.IsSet((int)PlayerFlags.Thief))
                    Shout.do_shout(keeper, string.Format("{0} the THIEF is over here!", ch.Name));
                return null;
            }

            CharacterInstance whof = fight.who_fighting(keeper);
            if (whof != null)
            {
                if (whof == ch)
                    color.send_to_char("I don't think that's a good idea...\r\n", ch);
                else
                    Say.do_say(keeper, "I'm too busy for that!");

                return null;
            }

            if (!ch.IsNpc() && fight.who_fighting(ch) != null)
            {
                color.ch_printf(ch, "%s doesn't seem to wnat to get involved.\r\n", Macros.PERS(keeper, ch));
                return null;
            }

            ShopData shop = keeper.MobIndex.Shop;
            if (shop.OpenHour > shop.CloseHour)
            {
                if (GameManager.Instance.GameTime.Hour < shop.OpenHour
                    && GameManager.Instance.GameTime.Hour > shop.CloseHour)
                {
                    Say.do_say(keeper, "Sorry, come back later.");
                    return null;
                }
            }
            else
            {
                if (GameManager.Instance.GameTime.Hour < shop.OpenHour)
                {
                    Say.do_say(keeper, "Sorry, come back later.");
                    return null;
                }
                if (GameManager.Instance.GameTime.Hour > shop.CloseHour)
                {
                    Say.do_say(keeper, "Sorry, come back tomorrow.");
                    return null;
                }
            }

            if (keeper.CurrentPosition == PositionTypes.Sleeping)
            {
                color.send_to_char("While they're asleep?\r\n", ch);
                return null;
            }

            if ((int)keeper.CurrentPosition < (int)PositionTypes.Sleeping)
            {
                color.send_to_char("I don't think they can hear you...\r\n", ch);
                return null;
            }

            if (!keeper.CanSee(ch))
            {
                Say.do_say(keeper, "I don't trade with folks I can't see.");
                return null;
            }

            int speakswell = keeper.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, keeper));
            if ((Common.SmaugRandom.Percent() % 65) > speakswell)
            {
                string buffer;
                if (speakswell > 60)
                    buffer = string.Format("{0} Could you repeat that?  I didn't quite catch it.", ch.Name);
                else if (speakswell > 50)
                    buffer = string.Format("{0} Could you say that a little more clearly please?", ch.Name);
                else if (speakswell > 40)
                    buffer = string.Format("{0} Sorry... What was that you wanted?", ch.Name);
                else
                    buffer = string.Format("{0} I can't understand you.", ch.Name);

                Tell.do_tell(keeper, buffer);
                return null;
            }

            return keeper;
        }

        public static CharacterInstance find_fixer(CharacterInstance ch)
        {
            return find_keeper(ch);
        }

        public static int get_cost(CharacterInstance ch, CharacterInstance keeper, ObjectInstance obj, bool fBuy)
        {
            if (obj == null || keeper.MobIndex.Shop == null)
                return 0;

            ItemShopData shop = keeper.MobIndex.Shop.CastAs<ItemShopData>();
            int cost = 0;
            int profitMod;
            bool richCustomer = ch.CurrentCoin > (ch.Level * ch.Level * 100000);
            if (fBuy)
            {
                profitMod = 13 - ch.GetCurrentCharisma() + (richCustomer ? 15 : 0)
                            + ((ch.Level.GetNumberThatIsBetween(5, LevelConstants.GetLevel("avatar")) - 20) / 2);
                cost = (obj.Cost * (shop.ProfitSell + 1).GetHighestOfTwoNumbers(shop.ProfitBuy + profitMod) / 100);
            }
            else
            {
                profitMod = 13 - ch.GetCurrentCharisma() + (richCustomer ? 15 : 0);
                for (int i = 0; i < Program.MAX_TRADE; i++)
                {
                    if (shop.ItemTypes.ToList().Contains(obj.ItemType))
                    {
                        cost = (obj.Cost * (shop.ProfitBuy - 1).GetLowestOfTwoNumbers(shop.ProfitSell + profitMod)) / 100;
                        break;
                    }
                }

                if (keeper.Carrying.Any(carriedObj => obj.ObjectIndex == carriedObj.ObjectIndex))
                    cost = 0;
            }

            if (obj.ItemType == ItemTypes.Staff || obj.ItemType == ItemTypes.Wand)
                cost = cost * obj.Value[2] / obj.Value[1];

            return cost;
        }

        public static int get_repaircost(CharacterInstance keeper, ObjectInstance obj)
        {
            if (obj == null || keeper.MobIndex.Shop == null)
                return 0;

            RepairShopData shop = keeper.MobIndex.RepairShop;
            int cost = 0;
            bool found = false;

            for (int i = 0; i < Program.MAX_FIX; i++)
            {
                if (shop.ItemTypes.ToList().Any(x => x == obj.ItemType))
                {
                    cost = (obj.Cost * shop.ProfitFix / 1000);
                    found = true;
                    break;
                }
            }

            if (!found)
                cost = -1;
            if (cost == 0)
                cost = 1;

            if (found && cost > 0)
            {
                switch (obj.ItemType)
                {
                    case ItemTypes.Armor:
                        if (obj.Value[0] >= obj.Value[1])
                            cost = -2;
                        else
                            cost *= (obj.Value[1] - obj.Value[0]);
                        break;
                    case ItemTypes.Weapon:
                        if (Program.INIT_WEAPON_CONDITION == obj.Value[0])
                            cost = -2;
                        else
                            cost *= (Program.INIT_WEAPON_CONDITION - obj.Value[0]);
                        break;
                    case ItemTypes.Wand:
                    case ItemTypes.Staff:
                        if (obj.Value[2] >= obj.Value[1])
                            cost = -2;
                        else
                            cost *= (obj.Value[1] - obj.Value[2]);
                        break;
                }
            }

            return cost;
        }

        public static void repair_one_obj(CharacterInstance ch, CharacterInstance keeper, ObjectInstance obj,
                                          string arg, int maxgold, string fixstr, string fixstr2)
        {
            int cost;
            string buffer;

            if (!ch.CanDrop(obj))
                color.ch_printf(ch, "You can't let go of %s.\r\n", obj.Name);
            else if ((cost = get_repaircost(keeper, obj)) < 0)
            {
                if (cost < 0)
                {
                    comm.act(ATTypes.AT_TELL,
                             cost != -2
                                 ? "$n tells you, 'Sorry, I can't do anything with $p.'"
                                 : "$n tells you, '$p looks fine to me!'", keeper, obj, ch, ToTypes.Victim);
                }
            }

            // repair all gets a 10% surcharge
            else if ((cost = arg.Equals("all") ? cost : 11 * (cost / 10)) > ch.CurrentCoin)
            {
                buffer = string.Format("$N tells you, 'It will cost {0} piece{1} of gold to {2} {3}...'",
                                         cost, cost == 1 ? "" : "s", fixstr, obj.Name);
                comm.act(ATTypes.AT_TELL, buffer, ch, null, keeper, ToTypes.Character);
                comm.act(ATTypes.AT_TELL, "$n tells you, 'Which I see you can't afford.'", ch, null, keeper, ToTypes.Character);
            }
            else
            {
                buffer = string.Format("$n gives $p to $N, who quickly {0} it.", fixstr2);
                comm.act(ATTypes.AT_ACTION, buffer, ch, obj, keeper, ToTypes.Room);

                buffer = string.Format("$n charges you {0} gold piece{1} to {2} $p.", cost, cost == 1 ? "" : "s", fixstr);
                comm.act(ATTypes.AT_ACTION, buffer, ch, obj, keeper, ToTypes.Character);

                ch.CurrentCoin -= cost;
                keeper.CurrentCoin += cost;

                if (keeper.CurrentCoin < 0)
                    keeper.CurrentCoin = 0;
                else if (keeper.CurrentCoin > maxgold)
                {
                    keeper.CurrentRoom.Area.BoostEconomy(keeper.CurrentCoin - maxgold / 2);
                    keeper.CurrentCoin = maxgold / 2;
                    comm.act(ATTypes.AT_ACTION, "$n puts some gold into a large safe.", keeper, null, null, ToTypes.Room);
                }

                switch (obj.ItemType)
                {
                    default:
                        color.send_to_char("For some reason, you think you got ripped off...\r\n", ch);
                        break;
                    case ItemTypes.Armor:
                        obj.Value[0] = obj.Value[1];
                        break;
                    case ItemTypes.Weapon:
                        obj.Value[0] = Program.INIT_WEAPON_CONDITION;
                        break;
                    case ItemTypes.Wand:
                    case ItemTypes.Staff:
                        obj.Value[2] = obj.Value[1];
                        break;
                }

                mud_prog.oprog_repair_trigger(ch, obj);
            }
        }

        public static void appraise_all(CharacterInstance ch, CharacterInstance keeper, string fixstr)
        {
            int total = 0, cost = 0;
            string buffer;

            foreach (ObjectInstance obj in ch.Carrying.Where(obj => obj.WearLocation == WearLocations.None
                                                                    && ch.CanSee(obj)
                                                                    && (obj.ItemType == ItemTypes.Armor
                                                                        || obj.ItemType == ItemTypes.Weapon
                                                                        || obj.ItemType == ItemTypes.Wand
                                                                        || obj.ItemType == ItemTypes.Staff)))
            {
                if (!ch.CanDrop(obj))
                    color.ch_printf(ch, "You can't let go of %s.\r\n", obj.Name);
                else if ((cost = get_repaircost(keeper, obj)) > 0)
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
