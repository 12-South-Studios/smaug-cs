using System.Linq;
using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Shops;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;

namespace SmaugCS.Behavior.Shopkeeper;

public static class Appraise
{
  public static int GetAdjustedCost(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj, bool buy)
  {
    if (obj == null || keeper.MobIndex.Shop == null)
      return 0;

    int cost = buy ? GetCostIfBuying(ch, keeper, obj) : GetCostIfSelling(ch, keeper, obj);

    if (obj.ItemType is ItemTypes.Staff or ItemTypes.Wand)
      cost *= obj.Values.Charges / obj.Values.MaxCharges;

    return cost;
  }

  private static int GetCostIfBuying(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj)
  {
    bool richCustomer = ch.CurrentCoin > ch.Level * ch.Level * 100000;
    int profitMod = 13 - ch.GetCurrentCharisma() + (richCustomer ? 15 : 0)
                                                 + (ch.Level.GetNumberThatIsBetween(5, LevelConstants.AvatarLevel) -
                                                    20) / 2;

    ItemShopData shop = keeper.MobIndex.Shop.CastAs<ItemShopData>();
    int cost = obj.Cost * (shop.ProfitSell + 1).GetHighestOfTwoNumbers(shop.ProfitBuy + profitMod) / 100;
    return cost;
  }

  private static int GetCostIfSelling(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj)
  {
    bool richCustomer = ch.CurrentCoin > ch.Level * ch.Level * 100000;
    int profitMod = 13 - ch.GetCurrentCharisma() + (richCustomer ? 15 : 0);

    ItemShopData shop = keeper.MobIndex.Shop.CastAs<ItemShopData>();
    int cost = 0;
    if (shop.ItemTypes.ToList().Contains(obj.ItemType))
      cost = obj.Cost * (shop.ProfitBuy - 1).GetLowestOfTwoNumbers(shop.ProfitSell + profitMod) / 100;

    if (keeper.Carrying.Any(carriedObj => obj.ObjectIndex == carriedObj.ObjectIndex))
      cost = 0;

    return cost;
  }

  public static int GetAdjustedRepairCost(MobileInstance keeper, ObjectInstance obj)
  {
    if (obj == null || keeper.MobIndex.Shop == null) return 0;

    RepairShopData shop = keeper.MobIndex.RepairShop;
    int cost = shop.ItemTypes.ToList().Any(x => x == obj.ItemType) ? obj.Cost * shop.ProfitFix / 1000 : 0;
    if (cost <= 0)
      cost = 1;

    cost = obj.ItemType switch
    {
      ItemTypes.Armor => obj.GetArmorRepairCost(cost),
      ItemTypes.Weapon => obj.GetWeaponRepairCost(cost, GameConstants.GetConstant<int>("InitWeaponCondition")),
      ItemTypes.Wand or ItemTypes.Staff => obj.GetImplementRepairCost(cost),
      _ => cost
    };

    return cost;
  }
}