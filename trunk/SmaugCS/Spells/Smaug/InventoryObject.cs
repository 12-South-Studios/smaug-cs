using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Weather;

namespace SmaugCS.Spells.Smaug
{
    public static class InventoryObject
    {
        public static ReturnTypes spell_obj_inv(int sn, int level, CharacterInstance ch, object vo)
        {
            ObjectInstance obj = (ObjectInstance) vo;
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);
            WeatherCell cell = WeatherManager.Instance.GetWeather(ch.CurrentRoom.Area);

            if (CheckFunctions.CheckIfNullObjectCasting(obj, skill, ch)) return ReturnTypes.None;

            switch (Macros.SPELL_ACTION(skill))
            {
                case (int)SpellActTypes.Create:
                    if (skill.Flags.IsSet(SkillFlags.Water))
                        return CreateWaterSpellAction(skill, level, ch, obj, cell);
                    if (Macros.SPELL_DAMAGE(skill) == (int) SpellDamageTypes.Fire)
                        return BurnObjectSpellAction(skill, ch, obj);
                    if (Macros.SPELL_DAMAGE(skill) == (int) SpellDamageTypes.Poison
                        || Macros.SPELL_CLASS(skill) == (int) SpellClassTypes.Death)
                        return PoisonOrDeathSpellAction(skill, ch, obj);
                    if (Macros.SPELL_CLASS(skill) == (int) SpellClassTypes.Life
                        &&
                        (obj.ItemType == ItemTypes.Food || obj.ItemType == ItemTypes.Cook ||
                         obj.ItemType == ItemTypes.DrinkContainer))
                        return PurifySpellAction(skill, ch, obj);

                    if (CheckFunctions.CheckIfTrueCasting(Macros.SPELL_CLASS(skill) != (int) SpellClassTypes.None, skill,
                        ch, CastingFunctionType.Failed, null, obj)) return ReturnTypes.None;

                    return CloneObjectSpellAction(skill, level, ch, obj);
                case (int)SpellActTypes.Obscure:
                    return ObscureSpellAction(skill, level, ch, obj);
                case (int)SpellActTypes.Destroy:
                case (int)SpellActTypes.Resist:
                case (int)SpellActTypes.Suscept:
                case (int)SpellActTypes.Divinate:
                    return OtherSpellAction(skill, ch, obj);
            }
            return ReturnTypes.None;
        }

        private static ReturnTypes PurifySpellAction(SkillData skill, CharacterInstance ch, ObjectInstance obj)
        {
            switch (obj.ItemType)
            {
                case ItemTypes.Cook:
                case ItemTypes.Food:
                case ItemTypes.DrinkContainer:
                    handler.separate_obj(obj);
                    obj.Value[3] = 0;
                    magic.successful_casting(skill, ch, null, obj);
                    break;
                default:
                    magic.failed_casting(skill, ch, null, obj);
                    break;
            }
            return ReturnTypes.None;
        }

        private static ReturnTypes CloneObjectSpellAction(SkillData skill, int level, CharacterInstance ch, ObjectInstance obj)
        {
            switch (Macros.SPELL_POWER(skill))
            {
                case (int)SpellPowerTypes.Minor:
                    if (CheckFunctions.CheckIfTrueCasting(
                            (ch.Level - obj.Level < 20) || (obj.Cost > (ch.Level*ch.GetCurrentIntelligence()/5)),
                            skill, ch, CastingFunctionType.Failed, null, obj)) return ReturnTypes.None;
                    break;
                case (int)SpellPowerTypes.Greater:
                    if (CheckFunctions.CheckIfTrueCasting((ch.Level - obj.Level < 5) ||
                        (obj.Cost > (ch.Level*10*ch.GetCurrentIntelligence()*ch.GetCurrentWisdom())),
                        skill, ch, CastingFunctionType.Failed, null, obj)) return ReturnTypes.None;
                    break;
                case (int)SpellPowerTypes.Major:
                    if (CheckFunctions.CheckIfTrueCasting((ch.Level - obj.Level < 0) ||
                        (obj.Cost > (ch.Level * 50 * ch.GetCurrentIntelligence() * ch.GetCurrentWisdom())),
                        skill, ch, CastingFunctionType.Failed, null, obj)) return ReturnTypes.None;

                    ObjectInstance clonedObj = null;    // TODO Clone ObjectInstance
                    clonedObj.Timer = !string.IsNullOrEmpty(skill.Dice) ? magic.dice_parse(ch, level, skill.Dice) : 0;
                    clonedObj.ToCharacter(ch);
                    magic.successful_casting(skill, ch, null, obj);
                    break;
                default:
                    if (CheckFunctions.CheckIfTrueCasting((ch.Level - obj.Level < 10) ||
                        (obj.Cost > ch.Level * ch.GetCurrentIntelligence() * ch.GetCurrentWisdom()),
                        skill, ch, CastingFunctionType.Failed, null, obj)) return ReturnTypes.None;
                    break;
            }

            return ReturnTypes.None;
        }

        private static ReturnTypes PoisonOrDeathSpellAction(SkillData skill, CharacterInstance ch, ObjectInstance obj)
        {
            switch (obj.ItemType)
            {
                case ItemTypes.Cook:
                case ItemTypes.Food:
                case ItemTypes.DrinkContainer:
                    handler.separate_obj(obj);
                    obj.Value[3] = 1;
                    magic.successful_casting(skill, ch, null, obj);
                    break;
                default:
                    magic.failed_casting(skill, ch, null, obj);
                    break;
            }
            return ReturnTypes.None;
        }

        private static ReturnTypes BurnObjectSpellAction(SkillData skill, CharacterInstance ch, ObjectInstance obj)
        {
            // TODO Not implemented
            return ReturnTypes.None;
        }

        private static ReturnTypes CreateWaterSpellAction(SkillData skill, int level, CharacterInstance ch, ObjectInstance obj, WeatherCell cell)
        {
            if (CheckFunctions.CheckIfTrue(ch, obj.ItemType != ItemTypes.DrinkContainer, "It is unable to hold water."))
                return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrue(ch, obj.Value[2] != 0 && obj.Value[1] != 0, "It contains some other liquid."))
                return ReturnTypes.SpellFailed;

            int minVal = (!string.IsNullOrEmpty(skill.Dice) ? magic.dice_parse(ch, level, skill.Dice) : level)*
                         (cell.Precipitation >= 0 ? 2 : 1);
            int water = minVal.GetLowestOfTwoNumbers(obj.Value[0] - obj.Value[1]);

            if (water > 0)
            {
                handler.separate_obj(obj);
                obj.Value[2] = 0;
                obj.Value[1] += water;
                if (obj.Name.EqualsIgnoreCase("water"))
                {
                    // TODO Set name to:  string.format("{0} water", obj.Name)
                }
            }

            magic.successful_casting(skill, ch, null, obj);
            return ReturnTypes.None;
        }

        private static ReturnTypes ObscureSpellAction(SkillData skill, int level, CharacterInstance ch, ObjectInstance obj)
        {
            int percent = !string.IsNullOrEmpty(skill.Dice) ? magic.dice_parse(ch, level, skill.Dice) : 20;
            if (CheckFunctions.CheckIfTrueCasting(
                    obj.ExtraFlags.IsSet(ItemExtraFlags.Invisible) || handler.chance(ch, percent), skill, ch))
                return ReturnTypes.SpellFailed;

            magic.successful_casting(skill, ch, null, obj);
            obj.ExtraFlags.SetBit(ItemExtraFlags.Invisible);
            return ReturnTypes.None;
        }

        private static ReturnTypes OtherSpellAction(SkillData skill, CharacterInstance ch, ObjectInstance obj)
        {
            if (Macros.SPELL_DAMAGE(skill) == (int) SpellDamageTypes.Poison)
            {
                if (CheckFunctions.CheckIfTrue(ch,
                    obj.ItemType != ItemTypes.DrinkContainer && obj.ItemType != ItemTypes.Food &&
                    obj.ItemType != ItemTypes.Cook, "It doesn't look poisoned.")) return ReturnTypes.None;

                if (CheckFunctions.CheckIfTrue(ch, obj.ItemType == ItemTypes.Cook && obj.Value[2] == 0,
                    "It looks undercooked.")) return ReturnTypes.None;
                if (CheckFunctions.CheckIfTrue(ch, obj.Value[3] != 0, "You smell poisonous fumes."))
                    return ReturnTypes.None;

                color.send_to_char("It looks very delicious.", ch);
            }
            return ReturnTypes.None;
        }
    }
}
