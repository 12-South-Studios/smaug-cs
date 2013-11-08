using System.IO;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class misc
    {
        /// <summary>
        /// Generates an action description message
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="object"></param>
        public static void actiondesc(CharacterInstance ch, ObjectInstance obj)
        {
            string charbuf = obj.ActionDescription;
            string roombuf = obj.ActionDescription;

            // TODO: Replacements?

            switch (obj.ItemType)
            {
                case ItemTypes.Blood:
                case ItemTypes.Fountain:
                    comm.act(ATTypes.AT_ACTION, charbuf, ch, obj, ch, ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, roombuf, ch, obj, ch, ToTypes.Room);
                    return;

                case ItemTypes.DrinkContainer:
                    LiquidData liq = db.GetLiquid(obj.Value[2]);
                    comm.act(ATTypes.AT_ACTION, charbuf, ch, obj, liq.Name, ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, roombuf, ch, obj, liq.Name, ToTypes.Room);
                    return;

                case ItemTypes.Cook:
                case ItemTypes.Food:
                case ItemTypes.Pill:
                    comm.act(ATTypes.AT_ACTION, charbuf, ch, obj, ch, ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, roombuf, ch, obj, ch, ToTypes.Room);
                    return;
            }
        }
    }
}
