using System;
using System.Collections.Generic;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Repositories
{
    public class ObjInstanceRepository : Repository<long, ObjectInstance>, IInstanceRepository<ObjectInstance>
    {
        private static long _idSpace = 1;
        private static long GetNextId { get { return _idSpace++; } }

        public ObjectInstance Create(Template parent, params object[] args)
        {
            Validation.IsNotNull(parent, "parent");
            Validation.Validate(parent is ObjectTemplate, "Invalid Template Type");

            ObjectTemplate objParent = parent.CastAs<ObjectTemplate>();

            string name = parent.Name;
            if (args != null && args.Length > 1)
                name = args[1].ToString();

            ObjectInstance obj = ObjectInstance.Create(GetNextId, name, 99, 99);
            obj.Parent = parent;
            obj.Level = args == null || args.Length == 0 ? 1 : (int)args[0];
            obj.WearLocation = WearLocations.None;
            obj.Count = 1;
            obj.ShortDescription = objParent.ShortDescription;
            obj.Description = parent.Description;
            obj.Action = objParent.Action;
            obj.ItemType = objParent.Type;
            obj.ExtraFlags = objParent.ExtraFlags;
            obj.Weight = objParent.Weight;
            obj.Cost = objParent.Cost;

            foreach (var wearLoc in objParent.GetWearFlags())
                obj.WearFlags += (int) wearLoc;

            Array.Copy(objParent.Value, obj.Value, 5);

            if (ObjectActionTable.ContainsKey(obj.ItemType))
                ObjectActionTable[obj.ItemType].Invoke(obj);

            Add(obj.ID, obj);
            return obj;
        }

        private static readonly Dictionary<ItemTypes, Action<ObjectInstance>> ObjectActionTable = new Dictionary<ItemTypes, Action<ObjectInstance>>
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
                obj.Value[0] = GameConstants.GetConstant<int>("InitWeaponCondition");
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
