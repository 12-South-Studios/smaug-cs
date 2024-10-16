﻿using Library.Common;
using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Extensions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Repository;

public class ObjInstanceRepository : Patterns.Repository.Repository<long, ObjectInstance>, IInstanceRepository<ObjectInstance>
{
  private static long _idSpace = 1;
  private static long GetNextId => _idSpace++;

  public ObjectInstance Create(Template parent, params object[] args)
  {
    Validation.IsNotNull(parent, "parent");
    Validation.Validate(parent is ObjectTemplate, "Invalid Template Type");

    ObjectTemplate objParent = parent.CastAs<ObjectTemplate>();

    long id = args is { Length: > 0 } ? Convert.ToInt64(args[0]) : GetNextId;

    string name = parent.Name;
    if (args is { Length: > 1 })
      name = args[1].ToString();

    ObjectInstance obj = new(id, name, 99, 99)
    {
      Parent = parent,
      Level = args == null || args.Length == 0 ? 1 : (int)args[0],
      WearLocation = WearLocations.None,
      Count = 1,
      ShortDescription = objParent.ShortDescription,
      Description = parent.Description,
      Action = objParent.Action,
      ItemType = objParent.Type,
      ExtraFlags = objParent.ExtraFlags,
      Weight = objParent.Weight,
      Cost = objParent.Cost,
      Values = objParent.Values
    };

    foreach (WearLocations wearLoc in objParent.GetWearFlags())
      obj.WearFlags += (int)wearLoc;

    //Array.Copy(objParent.Value, obj.Value, 5);

    if (ObjectActionTable.TryGetValue(obj.ItemType, out Action<ObjectInstance> value))
      value.Invoke(obj);

    Add(obj.Id, obj);
    return obj;
  }

  public ObjectInstance Clone(ObjectInstance source, params object[] args)
  {
    Validation.IsNotNull(source, "source");

    ObjectInstance obj = new(GetNextId, source.Name, 99, 99)
    {
      Parent = source.Parent,
      Level = source.Level,
      WearLocation = source.WearLocation,
      Count = source.Count,
      ShortDescription = source.ShortDescription,
      Description = source.Description,
      Action = source.Action,
      ItemType = source.ItemType,
      ExtraFlags = source.ExtraFlags,
      Weight = source.Weight,
      Cost = source.Cost,
      WearFlags = source.WearFlags,
      Owner = source.Owner,
      MagicFlags = source.MagicFlags,
      Timer = source.Timer,
      Values = source.Values
    };

    source.Value = [..obj.Value];

    if (ObjectActionTable.TryGetValue(obj.ItemType, out Action<ObjectInstance> value))
      value.Invoke(obj);

    Add(obj.Id, obj);
    return obj;
  }

  private static readonly Dictionary<ItemTypes, Action<ObjectInstance>> ObjectActionTable = new()
  {
      { ItemTypes.Food, UpdateFood },
      { ItemTypes.Cook, UpdateFood },
      { ItemTypes.Salve, UpdateSalve },
      { ItemTypes.Scroll, UpdateScroll },
      { ItemTypes.Wand, UpdateMagicalImplement },
      { ItemTypes.Staff, UpdateMagicalImplement },
      { ItemTypes.Weapon, UpdateWeapon },
      { ItemTypes.MissileWeapon, UpdateWeapon },
      { ItemTypes.Projectile, UpdateWeapon },
      { ItemTypes.Armor, UpdateArmor },
      { ItemTypes.Potion, UpdatePotion },
      { ItemTypes.Pill, UpdatePotion },
      { ItemTypes.Money, UpdateMoney }
    };

  private static void UpdateFood(ObjectInstance obj)
  {
    obj.Timer = obj.Value.ToList()[4] > 0 ? obj.Value.ToList()[4] : obj.Value.ToList()[1];
  }

  private static void UpdateSalve(ObjectInstance obj)
  {
    obj.Value.ToList()[3] = SmaugRandom.Fuzzy(obj.Value.ToList()[3]);
  }

  private static void UpdateScroll(ObjectInstance obj)
  {
    obj.Value.ToList()[0] = SmaugRandom.Fuzzy(obj.Value.ToList()[0]);
  }

  private static void UpdateMagicalImplement(ObjectInstance obj)
  {
    obj.Value.ToList()[0] = SmaugRandom.Fuzzy(obj.Value.ToList()[0]);
    obj.Value.ToList()[1] = SmaugRandom.Fuzzy(obj.Value.ToList()[1]);
    obj.Value.ToList()[2] = obj.Value.ToList()[1];
  }

  private static void UpdateWeapon(ObjectInstance obj)
  {
    if (obj.Value.ToList()[1] > 0 && obj.Value.ToList()[2] > 0)
      obj.Value.ToList()[2] = obj.Value.ToList()[1];
    else
    {
      obj.Value.ToList()[1] = SmaugRandom.Fuzzy(1 * obj.Level / 4 + 2);
      obj.Value.ToList()[2] = SmaugRandom.Fuzzy(3 * obj.Level / 4 + 6);
    }

    if (obj.Value.ToList()[0] == 0)
      obj.Value.ToList()[0] = GameConstants.GetConstant<int>("InitWeaponCondition");
  }

  private static void UpdateArmor(ObjectInstance obj)
  {
    if (obj.Value.ToList()[0] == 0)
      obj.Value.ToList()[0] = SmaugRandom.Fuzzy(obj.Level / 4 + 2);
    if (obj.Value.ToList()[1] == 0)
      obj.Value.ToList()[1] = obj.Value.ToList()[0];
  }

  private static void UpdatePotion(ObjectInstance obj)
  {
    obj.Value.ToList()[0] = SmaugRandom.Fuzzy(obj.Value.ToList()[0]);
  }

  private static void UpdateMoney(ObjectInstance obj)
  {
    obj.Value.ToList()[0] = obj.Cost > 0 ? obj.Cost : 1;
  }
}