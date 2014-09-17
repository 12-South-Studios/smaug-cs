using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.SpecFuns
{
    public static class Janitor
    {
        public static bool DoSpecJanitor(CharacterInstance ch)
        {
            if (!ch.IsAwake())
                return false;

            foreach (ObjectInstance trash in ch.CurrentRoom.Contents
                .Where(x => x.WearFlags.IsSet(ItemWearFlags.Take))
                .Where(x => !x.ExtraFlags.IsSet(ItemExtraFlags.Buried)))
            {
                if (trash.ItemType != ItemTypes.DrinkContainer && trash.ItemType != ItemTypes.Trash && trash.Cost >= 10 &&
                    (trash.ObjectIndex.Vnum != GameConstants.GetVnum("ObjectShoppingBag") ||
                     trash.Contents.First() != null)) continue;

                comm.act(ATTypes.AT_ACTION, "$n picks up some trash.", ch, null, null, ToTypes.Room);
                ch.CurrentRoom.FromRoom(trash);
                trash.ToCharacter(ch);
                return true;
            }

            return false;
        }
    }
}
