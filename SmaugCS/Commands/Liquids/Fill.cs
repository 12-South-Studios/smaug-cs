using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Liquids
{
    public static class Fill
    {
        public static void do_fill(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Fill what?")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;

            ObjectInstance obj = ch.GetCarriedObject(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You do not have that item.")) return;

            if (obj.ItemType == ItemTypes.Container)
            {
                if (obj.Values.Flags.IsSet(ContainerFlags.Closed))
                {
                    comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, obj.Name, ToTypes.Character);
                    return;
                }
                if (CheckFunctions.CheckIfTrue(ch, (obj.GetRealObjectWeight()/obj.Count) >= obj.Values.Capacity,
                    "It's already full as it can be.")) return;
            }
            else
            {
                if (CheckFunctions.CheckIfTrue(ch,
                    (GetMaximumCondition() < 1) || (obj.Values.Quantity >= obj.Values.Capacity),
                    "It's already full as it can be.")) return;
            }

            if (CheckFunctions.CheckIfTrue(ch,
                obj.ItemType == ItemTypes.Pipe && obj.Values.Flags.IsSet(PipeFlags.FullOfAsh),
                "It's full of ashes, and needs to be emptied first.")) return;

            IEnumerable<ItemTypes> sourceItemTypes = ChooseSourceItemTypes(ch, obj);

            string secondArg = argument.SecondWord();
            if (secondArg.EqualsIgnoreCase("from")
                || secondArg.EqualsIgnoreCase("with"))
                secondArg = argument.ThirdWord();

            ObjectInstance source = null;
            bool all = false;

            if (!secondArg.IsNullOrEmpty())
            {
                if (obj.ItemType == ItemTypes.Container &&
                    (secondArg.EqualsIgnoreCase("all") || secondArg.StartsWithIgnoreCase("all.")))
                    all = true;
                else if (obj.ItemType == ItemTypes.Pipe)
                {
                    source =ch.GetCarriedObject(secondArg);
                    if (CheckFunctions.CheckIfNullObject(ch, source, "You don't have that item.")) return;
                    if (sourceItemTypes.All(x => x != source.ItemType))
                    {
                        comm.act(ATTypes.AT_PLAIN, "You cannot fill $p with $P!", ch, obj, source, ToTypes.Character);
                        return;
                    }
                }
                else
                {
                    source = ch.GetObjectOnMeOrInRoom(secondArg);
                    if (CheckFunctions.CheckIfNullObject(ch, source, "You cannot find that item.")) return;
                }

                if (CheckFunctions.CheckIf(ch,
                    args => ((ObjectInstance) args[0]) == null && ((ObjectInstance) args[1]).ItemType == ItemTypes.Pipe,
                    "Fill it with what?", new List<object> {source, obj})) return;

                // TODO Finish this (source function is VERY MESSY!)
            }
        }

        private static int GetMaximumCondition()
        {
            return GameConstants.GetConstant<int>("MaximumConditionValue");
        }

        private static IEnumerable<ItemTypes> ChooseSourceItemTypes(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.ItemType == ItemTypes.Container)
                return new List<ItemTypes> {ItemTypes.Container, ItemTypes.NpcCorpse, ItemTypes.PlayerCorpse};
            if (obj.ItemType == ItemTypes.DrinkContainer)
                return new List<ItemTypes> {ItemTypes.Fountain, ItemTypes.Blood};
            if (obj.ItemType == ItemTypes.HerbContainer)
                return new List<ItemTypes> {ItemTypes.Herb, ItemTypes.HerbContainer};
            if (obj.ItemType == ItemTypes.Pipe)
                return new List<ItemTypes> {ItemTypes.Herb, ItemTypes.HerbContainer};

            comm.act(ATTypes.AT_ACTION, "$n tries to fill $p... (Don't ask me how)", ch, obj, null, ToTypes.Room);
            color.send_to_char("You cannot fill that.", ch);
            return new List<ItemTypes>();
        }
    }
}
