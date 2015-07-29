using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Templates
{
    public static class ObjectTemplateValueFunctions
    {
        private static readonly Dictionary<ItemTypes, Action<dynamic, IEnumerable<int>>> DynamicValuesTable = new Dictionary
            <ItemTypes, Action<dynamic, IEnumerable<int>>>
        {
            {ItemTypes.Armor, SetArmorDynamicValues},
            {ItemTypes.Container, SetContainerDynamicValues},
            {ItemTypes.DrinkContainer, SetDrinkContainerDynamicValues},
            {ItemTypes.Food, SetFoodDynamicValues},
            {ItemTypes.Furniture, SetFurnitureDynamicValues},
            {ItemTypes.Herb, SetHerbDynamicValues},
            {ItemTypes.Key, SetKeyDynamicValues},
            {ItemTypes.KeyRing, SetKeyRingDynamicValues},
            {ItemTypes.Lever, SetLeverDynamicValues},
            {ItemTypes.Light, SetLightDynamicValues},
            {ItemTypes.Missile, SetMissileDynamicValues},
            {ItemTypes.Money, SetMoneyDynamicValues},
            {ItemTypes.Pill, SetPillDynamicValues},
            {ItemTypes.Pipe, SetPipeDynamicValues},
            {ItemTypes.Potion, SetPotionDynamicValues},
            {ItemTypes.Projectile, SetProjectileDynamicValues},
            {ItemTypes.Quiver, SetQuiverDynamicValues},
            {ItemTypes.Salve, SetSalveDynamicValues},
            {ItemTypes.Scroll, SetScrollDynamicValues},
            {ItemTypes.Staff, SetStaffDynamicValues},
            {ItemTypes.Switch, SetSwitchDynamicValues},
            {ItemTypes.Trap, SetTrapDynamicValues},
            {ItemTypes.Treasure, SetTreasureDynamicValues},
            {ItemTypes.Wand, SetWandDynamicValues},
            {ItemTypes.Weapon, SetWeaponDynamicValues}
        };

        public static void SetObjectTemplateValues(ObjectTemplate template, IEnumerable<int> values)
        {
            Action<dynamic, IEnumerable<int>> action;
            DynamicValuesTable.TryGetValue(template.Type, out action);

            if (action == null) return;

            action.Invoke(template.Values, values);
        }

        private static void SetArmorDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.CurrentAC = list[0];
            valueProperty.OriginalAC = list[1];
        }

        private static void SetContainerDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Capacity = list[0];
            valueProperty.Flags = list[1];
            valueProperty.KeyID = list[2];
            valueProperty.Condition = list[3];
        }

        private static void SetDrinkContainerDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Capacity = list[0];
            valueProperty.Quantity = list[1];
            valueProperty.LiquidID = list[2];
            valueProperty.Poison = list[3];
        }

        private static void SetFoodDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.FoodValue = list[0];
            valueProperty.Condition = list[1];
            valueProperty.Poison = list[2];
        }

        private static void SetHerbDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Charges = list[0];
            valueProperty.HerbID = list[1];
        }

        private static void SetKeyDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.LockID = list[0];
        }

        private static void SetKeyRingDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Capacity = list[0];
            valueProperty.Condition = list[3];
        }

        private static void SetLeverDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Flags = list[0];
            valueProperty.SkillID = list[1];
            valueProperty.ID = list[2];
            valueProperty.Val = list[3];
        }

        private static void SetLightDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.CurrentAC = list[0];
            valueProperty.Lightable = list[1];
            valueProperty.HoursLeft = list[2];
            valueProperty.Flags = list[3];
        }

        private static void SetMissileDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Condition = list[0];
            valueProperty.DamageBonus = list[1];
            valueProperty.WeaponType = list[2];
            valueProperty.Range = list[3];
        }

        private static void SetMoneyDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.NumberOfCoins = list[0];
            valueProperty.CoinType = list[1];
        }

        private static void SetPillDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.SpellLevel = list[0];
            valueProperty.Skill1ID = list[1];
            valueProperty.Skill2ID = list[2];
            valueProperty.Skill3ID = list[3];
            valueProperty.FoodValue = list[4];
        }

        private static void SetPipeDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Capacity = list[0];
            valueProperty.NumberOfDraws = list[1];
            valueProperty.HerbSkillID = list[2];
            valueProperty.Flags = list[3];
        }

        private static void SetPotionDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.SpellLevel = list[0];
            valueProperty.Skill1ID = list[1];
            valueProperty.Skill2ID = list[2];
            valueProperty.Skill3ID = list[3];
        }

        private static void SetProjectileDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.MinimumBonus = list[1];
            valueProperty.MaximumBonus = list[2];
            valueProperty.SkillID = list[3];
            valueProperty.ProjectileType = list[4];
        } 
        private static void SetQuiverDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Capacity = list[0];
            valueProperty.Flags = list[1];
            valueProperty.KeyID = list[2];
            valueProperty.Condition = list[3];
        }

        private static void SetSalveDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.SpellLevel = list[0];
            valueProperty.Charges = list[1];
            valueProperty.MaxCharges = list[2];
            valueProperty.Delay = list[3];
            valueProperty.Skill1ID = list[4];
            valueProperty.Skill2ID = list[5];
        }

        private static void SetScrollDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.SpellLevel = list[0];
            valueProperty.Skill1ID = list[1];
            valueProperty.Skill2ID = list[2];
            valueProperty.Skill3ID = list[3];
        }

        private static void SetStaffDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.SpellLevel = list[0];
            valueProperty.MaxCharges = list[1];
            valueProperty.MaxCharges = list[2];
            valueProperty.SkillID = list[3];
        }

        private static void SetSwitchDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Flags = list[0];
            valueProperty.SkillID = list[1];
            valueProperty.SkillID = list[2];
            valueProperty.Val = list[3];
        }

        private static void SetTrapDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Charges = list[0];
            valueProperty.Type = list[1];
            valueProperty.Level = list[2];
            valueProperty.Flags = list[3];
        }

        private static void SetTreasureDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Type = list[0];
            valueProperty.Condition = list[1];
        }

        private static void SetWandDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Level = list[0];
            valueProperty.MaxCharges = list[1];
            valueProperty.Charges = list[2];
            valueProperty.SkillID = list[3];
        }

        private static void SetWeaponDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.Condition = list[0];
            valueProperty.NumberOfDice = list[1];
            valueProperty.SizeOfDice = list[2];
            valueProperty.WeaponType = list[3];
        }

        private static void SetFurnitureDynamicValues(dynamic valueProperty, IEnumerable<int> values)
        {
            var list = values.ToList();
            valueProperty.FurniturePositionFlags = list[0];
            valueProperty.MaxPeople = list[1];
            valueProperty.MaxWeight = list[2];
        }
    }
}
