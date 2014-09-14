using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
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

            ObjectInstance firstObj = ch.GetCarriedObject(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, firstObj, "You aren't carrying that.")) return;

            ObjectInstance secondObj = ch.GetCarriedObject(secondArg);
            if (CheckFunctions.CheckIfNullObject(ch, secondObj, "You aren't carrying that.")) return;

            if (CheckFunctions.CheckIfTrue(ch,
                firstObj.ItemType != ItemTypes.DrinkContainer && firstObj.ItemType != ItemTypes.DrinkMixture &&
                secondObj.ItemType != ItemTypes.DrinkContainer && secondObj.ItemType != ItemTypes.DrinkMixture,
                "You can't mix that!")) return;

            if (CheckFunctions.CheckIfTrue(ch, firstObj.Values.Quantity <= 0 || secondObj.Values.Quantity <= 0,
                "It's empty.")) return;

            bool success = CheckMixture(firstObj, secondObj);
            if (CheckFunctions.CheckIfTrue(ch, !success, "Those two don't mix well together.")) return;

            color.send_to_char("&cYou mix them together.&g", ch);
        }

        private static bool CheckMixture(ObjectInstance firstObj, ObjectInstance secondObj)
        {
            if (firstObj.ItemType == ItemTypes.DrinkContainer
                && secondObj.ItemType == ItemTypes.DrinkContainer)
                return liquids.MixFullyWith(firstObj, secondObj) != null;
            if (firstObj.ItemType == ItemTypes.DrinkMixture
                && secondObj.ItemType == ItemTypes.DrinkContainer)
                return liquids.MixPartiallyWith(secondObj, firstObj) != null;
            if (firstObj.ItemType == ItemTypes.DrinkContainer
                && secondObj.ItemType == ItemTypes.DrinkMixture)
                return liquids.MixPartiallyWith(firstObj, secondObj) != null;
            return false;
        }
    }
}
