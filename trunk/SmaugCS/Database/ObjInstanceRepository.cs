using System;
using System.Collections.Generic;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjInstanceRepository : Repository<long, ObjectInstance>
    {
        private static long _idSpace = 1;
        private static long GetNextId { get { return _idSpace++; } }

        private readonly int _maxWear;
        private readonly int _maxLayers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxWear"></param>
        /// <param name="maxLayers"></param>
        public ObjInstanceRepository(int maxWear, int maxLayers)
        {
            _maxWear = maxWear;
            _maxLayers = maxLayers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public ObjectInstance Create(ObjectTemplate parent, int level)
        {
            Validation.IsNotNull(parent, "parent");
            Validation.Validate(level > 0, "Invalid level {0}", level);

            ObjectInstance obj = new ObjectInstance(GetNextId, parent.Name, _maxWear, _maxLayers)
                {
                    Parent = parent,
                    Level = level,
                    WearLocation = WearLocations.None,
                    Count = 1,
                    ShortDescription = parent.ShortDescription,
                    Description = parent.Description,
                    Action = parent.Action,
                    ItemType = parent.Type,
                    ExtraFlags = new ExtendedBitvector(parent.ExtraFlags),
                    Weight = parent.Weight,
                    Cost = parent.Cost
                };

            foreach (var wearLoc in parent.GetWearFlags())
                obj.WearFlags += (int) wearLoc;

            Array.Copy(parent.Value, obj.Value, 5);

            if (ObjectActionTable.ContainsKey(obj.ItemType))
                ObjectActionTable[obj.ItemType].Invoke(obj);

            Add(obj.ID, obj);
            return obj;
        }

        private static readonly Dictionary<ItemTypes, Action<ObjectInstance>> ObjectActionTable = new Dictionary<ItemTypes, Action<ObjectInstance>>()
            {
                {ItemTypes.Food, UpdateFood},
                {ItemTypes.Cook, UpdateFood},
                {ItemTypes.Salve, UpdateSalve},
                {ItemTypes.Scroll, UpdateScroll},
                {ItemTypes.Wand, UpdateMagicalImplement},
                {ItemTypes.Staff, UpdateMagicalImplement},
                {ItemTypes.Weapon, UpdateWeapon},
                {ItemTypes.MissileWeapon, UpdateWeapon},
                {ItemTypes.Projectile, UpdateWeapon},
                {ItemTypes.Armor, UpdateArmor},
                {ItemTypes.Potion, UpdatePotion},
                {ItemTypes.Pill, UpdatePotion},
                {ItemTypes.Money, UpdateMoney}
            };

        private static void UpdateFood(ObjectInstance obj)
        {
            obj.Timer = (obj.Value[4] > 0) ? obj.Value[4] : obj.Value[1];
        }
        private static void UpdateSalve(ObjectInstance obj)
        {
            obj.Value[3] = SmaugRandom.Fuzzy(obj.Value[3]);
        }
        private static void UpdateScroll(ObjectInstance obj)
        {
            obj.Value[0] = SmaugRandom.Fuzzy(obj.Value[0]);
        }
        private static void UpdateMagicalImplement(ObjectInstance obj)
        {
            obj.Value[0] = SmaugRandom.Fuzzy(obj.Value[0]);
            obj.Value[1] = SmaugRandom.Fuzzy(obj.Value[1]);
            obj.Value[2] = obj.Value[1];
        }
        private static void UpdateWeapon(ObjectInstance obj)
        {
            if (obj.Value[1] > 0 && obj.Value[2] > 0)
                obj.Value[2] = obj.Value[1];
            else
            {
                obj.Value[1] = SmaugRandom.Fuzzy(1 * obj.Level / 4 + 2);
                obj.Value[2] = SmaugRandom.Fuzzy(3 * obj.Level / 4 + 6);
            }
            if (obj.Value[0] == 0)
                obj.Value[0] = Program.INIT_WEAPON_CONDITION;
        }
        private static void UpdateArmor(ObjectInstance obj)
        {
            if (obj.Value[0] == 0)
                obj.Value[0] = SmaugRandom.Fuzzy(obj.Level / 4 + 2);
            if (obj.Value[1] == 0)
                obj.Value[1] = obj.Value[0];
        }
        private static void UpdatePotion(ObjectInstance obj)
        {
            obj.Value[0] = SmaugRandom.Fuzzy(obj.Value[0]);
        }
        private static void UpdateMoney(ObjectInstance obj)
        {
            obj.Value[0] = obj.Cost > 0 ? obj.Cost : 1;
        }
    }
}
