using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands.Social;
using SmaugCS.Enums;
using SmaugCS.Objects;

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
                if (db.GameTime.Hour < shop.OpenHour
                    && db.GameTime.Hour > shop.CloseHour)
                {
                    Say.do_say(keeper, "Sorry, come back later.");
                    return null;
                }
            }
            else
            {
                if (db.GameTime.Hour < shop.OpenHour)
                {
                    Say.do_say(keeper, "Sorry, come back later.");
                    return null;
                }
                if (db.GameTime.Hour > shop.CloseHour)
                {
                    Say.do_say(keeper, "Sorry, come back tomorrow.");
                    return null;
                }
            }

            if (keeper.Position == PositionTypes.Sleeping)
            {
                color.send_to_char("While they're asleep?\r\n", ch);
                return null;
            }

            if ((int)keeper.Position < (int)PositionTypes.Sleeping)
            {
                color.send_to_char("I don't think they can hear you...\r\n", ch);
                return null;
            }

            if (!handler.can_see(keeper, ch))
            {
                Say.do_say(keeper, "I don't trade with folks I can't see.");
                return null;
            }

            int speakswell = SmaugCS.Common.Check.Minimum(keeper.KnowsLanguage(ch.Speaking, ch),
                                              ch.KnowsLanguage(ch.Speaking, keeper));
            if ((SmaugCS.Common.SmaugRandom.Percent() % 65) > speakswell)
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
            int profitMod = 0;
            bool richCustomer = ch.CurrentCoin > (ch.Level * ch.Level * 100000);
            if (fBuy)
            {
                profitMod = 13 - ch.CurrentCharisma + (richCustomer ? 15 : 0)
                            + ((SmaugCS.Common.Check.Range(5, ch.Level, Program.LEVEL_AVATAR) - 20) / 2);
                cost = (obj.Cost * SmaugCS.Common.Check.Maximum(shop.ProfitSell + 1, shop.ProfitBuy + profitMod) / 100);
            }
            else
            {
                profitMod = 13 - ch.CurrentCharisma + (richCustomer ? 15 : 0);
                for (int i = 0; i < Program.MAX_TRADE; i++)
                {
                    if (shop.ItemTypes.ToList().Contains(obj.ItemType))
                    {
                        cost = (obj.Cost * SmaugCS.Common.Check.Minimum(shop.ProfitBuy - 1, shop.ProfitSell + profitMod)) / 100;
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

        public static void do_buy(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_list(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_sell(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_value(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void repair_one_obj(CharacterInstance ch, CharacterInstance keeper, ObjectInstance obj,
                                          string arg, int maxgold, string fixstr, string fixstr2)
        {

        }

        public static void do_repair(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void appraise_all(CharacterInstance ch, CharacterInstance keeper, string fixstr)
        {
            // TODO
        }

        public static void do_appraise(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_makeshop(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_shopset(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_shopstat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_shops(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_makerepair(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_repairset(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_repairstat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_repairshops(CharacterInstance ch, string argument)
        {
            // TODO
        }
    }
}
