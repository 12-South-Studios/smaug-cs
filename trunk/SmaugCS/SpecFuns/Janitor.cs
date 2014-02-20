using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Common;
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

            foreach (ObjectInstance trash in ch.CurrentRoom.Contents)
            {
                if (!trash.WearFlags.IsSet((int) ItemWearFlags.Take)
                    || Macros.IS_OBJ_STAT(trash, (int) ItemExtraFlags.Buried))
                    continue;

                if (Macros.IS_OBJ_STAT(trash, (int) ItemExtraFlags.Prototype)
                    && !ch.Act.IsSet((int) ActFlags.Prototype))
                    continue;

                if (trash.ItemType == ItemTypes.DrinkContainer 
                    || trash.ItemType == ItemTypes.Trash 
                    || trash.Cost < 10
                    || (trash.ObjectIndex.Vnum == GameConstants.GetVnum("ObjectShoppingBag")  
                        && trash.Contents.First() == null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n picks up some trash.", ch, null, null, ToTypes.Room);
                    ch.CurrentRoom.FromRoom(trash);
                    trash.ToCharacter(ch);
                    return true;
                }
            }
            return false;
        }
    }
}
