using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Liquids
{
    public static class Mix
    {
        public static void do_mix(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "What would you like to mix together?")) return;

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "What would you like to mix together?")) return;

            ObjectInstance firstObj = handler.get_obj_carry(ch, firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, firstObj, "You aren't carrying that.")) return;

            ObjectInstance secondObj = handler.get_obj_carry(ch, secondArg);
            if (CheckFunctions.CheckIfNullObject(ch, secondObj, "You aren't carrying that.")) return;

            if (CheckFunctions.CheckIfTrue(ch,
                firstObj.ItemType != ItemTypes.DrinkContainer && firstObj.ItemType != ItemTypes.DrinkMixture &&
                secondObj.ItemType != ItemTypes.DrinkContainer && secondObj.ItemType != ItemTypes.DrinkMixture,
                "You can't mix that!")) return;

            if (CheckFunctions.CheckIfTrue(ch, firstObj.Value[1] <= 0 || secondObj.Value[1] <= 0, "It's empty.")) return;

            bool success = CheckMixture(firstObj, secondObj);
            if (CheckFunctions.CheckIfTrue(ch, !success, "Those two don't mix well together.")) return;

            color.send_to_char("&cYou mix them together.&g", ch);
        }

        private static bool CheckMixture(ObjectInstance firstObj, ObjectInstance secondObj)
        {
            if (firstObj.ItemType == ItemTypes.DrinkContainer
                && secondObj.ItemType == ItemTypes.DrinkContainer)
                return liquids.liq_can_mix(firstObj, secondObj) != null;
            if (firstObj.ItemType == ItemTypes.DrinkMixture
                && secondObj.ItemType == ItemTypes.DrinkContainer)
                return liquids.liqobj_can_mix(secondObj, firstObj) != null;
            if (firstObj.ItemType == ItemTypes.DrinkContainer
                && secondObj.ItemType == ItemTypes.DrinkMixture)
                return liquids.liqobj_can_mix(firstObj, secondObj) != null;
            return false;
        }
    }
}
