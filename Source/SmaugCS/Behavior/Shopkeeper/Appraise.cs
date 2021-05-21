using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Shops;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using System.Linq;

namespace SmaugCS.Behavior.Shopkeeper
{
    public static class Appraise
    {
        public static int GetAdjustedCost(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj, bool buy)
        {
            if (obj == null || keeper.MobIndex.Shop == null)
                return 0;

            var cost = buy ? GetCostIfBuying(ch, keeper, obj) : GetCostIfSelling(ch, keeper, obj);

            if (obj.ItemType == ItemTypes.Staff || obj.ItemType == ItemTypes.Wand)
                cost *= (obj.Values.Charges / obj.Values.MaxCharges);

            return cost;
        }

        private static int GetCostIfBuying(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj)
        {
            var richCustomer = ch.CurrentCoin > ch.Level * ch.Level * 100000;
            var profitMod = 13 - ch.GetCurrentCharisma() + (richCustomer ? 15 : 0)
                            + (ch.Level.GetNumberThatIsBetween(5, LevelConstants.AvatarLevel) - 20) / 2;

            var shop = keeper.MobIndex.Shop.CastAs<ItemShopData>();
            var cost = obj.Cost * (shop.ProfitSell + 1).GetHighestOfTwoNumbers(shop.ProfitBuy + profitMod) / 100;
            return cost;
        }

        private static int GetCostIfSelling(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj)
        {
            var richCustomer = ch.CurrentCoin > ch.Level * ch.Level * 100000;
            var profitMod = 13 - ch.GetCurrentCharisma() + (richCustomer ? 15 : 0);

            var shop = keeper.MobIndex.Shop.CastAs<ItemShopData>();
            var cost = 0;
            if (shop.ItemTypes.ToList().Contains(obj.ItemType))
                cost = obj.Cost * (shop.ProfitBuy - 1).GetLowestOfTwoNumbers(shop.ProfitSell + profitMod) / 100;

            if (keeper.Carrying.Any(carriedObj => obj.ObjectIndex == carriedObj.ObjectIndex))
                cost = 0;

            return cost;
        }

        public static int GetAdjustedRepairCost(MobileInstance keeper, ObjectInstance obj)
        {
            if (obj == null || keeper.MobIndex.Shop == null) return 0;

            var shop = keeper.MobIndex.RepairShop;
            var cost = shop.ItemTypes.ToList().Any(x => x == obj.ItemType) ? obj.Cost * shop.ProfitFix / 1000 : 0;
            if (cost <= 0)
                cost = 1;

            switch (obj.ItemType)
            {
                case ItemTypes.Armor:
                    cost = obj.GetArmorRepairCost(cost);
                    break;
                case ItemTypes.Weapon:
                    cost = obj.GetWeaponRepairCost(cost, GameConstants.GetConstant<int>("InitWeaponCondition"));
                    break;
                case ItemTypes.Wand:
                case ItemTypes.Staff:
                    cost = obj.GetImplementRepairCost(cost);
                    break;
            }

            return cost;
        }
    }
}
