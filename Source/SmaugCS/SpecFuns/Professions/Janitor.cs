using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using System.Linq;

namespace SmaugCS.SpecFuns.Professions
{
    public static class Janitor
    {
        public static bool Execute(CharacterInstance ch, IManager dbManager)
        {
            if (!ch.IsAwake()) return false;

            var shoppingBagId = GameConstants.GetVnum("ObjectShoppingBag");

            foreach (var trash in ch.CurrentRoom.Contents
                .Where(x => x.WearFlags.IsSet(ItemWearFlags.Take))
                .Where(x => !x.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                .Where(trash => trash.ItemType == ItemTypes.DrinkContainer || trash.ItemType == ItemTypes.Trash
                    || trash.Cost < 10 || (trash.ObjectIndex.ID == shoppingBagId && trash.Contents.First() == null)))
            {
                comm.act(ATTypes.AT_ACTION, "$n picks up some trash.", ch, null, null, ToTypes.Room);
                ch.CurrentRoom.RemoveFrom(trash);
                trash.AddTo(ch);
                return true;
            }

            return false;
        }
    }
}
