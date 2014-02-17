using System.Linq;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.SpecFuns
{
    public static class Wanderer
    {
        public static bool DoSpecWanderer(CharacterInstance ch)
        {
            if (!ch.IsAwake())
                return false;

            bool thrown = false;
            bool found = false;
            bool noExit = true;

            ExitData exit = ch.CurrentRoom.Exits.First();
            if (exit != null)
                noExit = false;

            if (SmaugRandom.Percent() <= 50)
                return false;

            foreach (ObjectInstance obj in ch.CurrentRoom.Contents)
            {
                if (!obj.WearFlags.IsSet((int)ItemWearFlags.Take)
                    || Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Buried))
                    continue;

                if (obj.ItemType != ItemTypes.Weapon
                    && obj.ItemType != ItemTypes.Armor
                    && obj.ItemType != ItemTypes.Light)
                    continue;

                handler.separate_obj(obj);
                comm.act(ATTypes.AT_ACTION, "$n leans over and gets $p.", ch, obj, null, ToTypes.Room);
                ch.CurrentRoom.FromRoom(obj);

                ObjectInstance trash = obj.ToCharacter(ch);
                if (ch.Level < trash.Level)
                {
                    comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is too inexperienced.", ch, trash, null, ToTypes.Room);
                    thrown = true;
                }

                if (!thrown)
                    act_obj.wear_obj(ch, trash, false, -1);

                found = false;
                if (!thrown)
                {
                    foreach (ObjectInstance obj2 in ch.Carrying.Where(obj2 => obj2.WearLocation == WearLocations.None))
                    {
                        Say.do_say(ch, "Hmm, I can't use this.");
                        trash = obj2;
                        thrown = true;
                    }
                }

                // TODO: Finish this
            }

            return false;
        }
    }
}
