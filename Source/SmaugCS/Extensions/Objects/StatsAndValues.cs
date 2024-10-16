﻿using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions.Objects;

public static class StatsAndValues
{
  public static int GetResistance(this ObjectInstance obj)
  {
    int resist = SmaugRandom.Fuzzy(Program.MAX_ITEM_IMPACT);

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Magical))
      resist += SmaugRandom.Fuzzy(12);

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Metallic))
      resist += SmaugRandom.Fuzzy(5);

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Organic))
      resist -= SmaugRandom.Fuzzy(5);

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Blessed))
      resist += SmaugRandom.Fuzzy(5);

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Inventory))
      resist += 20;

    resist += obj.Level / 10 - 2;

    if (obj.ItemType == ItemTypes.Armor || obj.ItemType == ItemTypes.Weapon)
      resist += obj.Value.ToList()[0] / 2 - 2;

    return resist.GetNumberThatIsBetween(10, 99);
  }

  public static int GetWeight(this ObjectInstance obj)
  {
    int weight = obj.Count * obj.Weight;
    if (obj.ItemType != ItemTypes.Container || !obj.ExtraFlags.IsSet((int)ItemExtraFlags.Magical))
      weight += obj.Contents.Sum(o => o.GetWeight());

    return weight;
  }

  public static int GetRealWeight(this ObjectInstance obj)
  {
    int weight = obj.Count * obj.Weight;

    weight += obj.Contents.Sum(o => o.GetRealWeight());

    return weight;
  }

  public static int GetArmorRepairCost(this ObjectInstance obj, int baseCost)
  {
    int cost = baseCost;
    if (obj.Values.CurrentAC >= obj.Values.OriginalAC)
      cost = -2;
    else
      cost *= obj.Values.OriginalAC - obj.Values.CurrentAC;
    return cost;
  }

  public static int GetWeaponRepairCost(this ObjectInstance obj, int baseCost, int weaponCondition)
  {
    int cost = baseCost;
    if (weaponCondition == obj.Values.Condition)
      cost = -2;
    else
      cost *= weaponCondition - obj.Values.Condition;
    return cost;
  }

  public static int GetImplementRepairCost(this ObjectInstance obj, int baseCost)
  {
    int cost = baseCost;
    if (obj.Value.ToList()[2] >= obj.Value.ToList()[1])
      cost = -2;
    else
      cost *= obj.Value.ToList()[1] - obj.Value.ToList()[2];
    return cost;
  }
}