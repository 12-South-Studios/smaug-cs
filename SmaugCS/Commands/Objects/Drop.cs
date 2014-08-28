using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Commands.Admin;
using SmaugCS.Common;
using SmaugCS.Config;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Objects
{
    public static class Drop
    {
        public static void do_drop(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Drop what?")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;
            if (!ch.IsNpc() && ch.Act.IsSet(PlayerFlags.Litterbug))
            {
                color.set_char_color(ATTypes.AT_YELLOW, ch);
                color.send_to_char("A godly force prevents you from dropping anything...", ch);
                return;
            }

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDrop) && ch != db.Supermob)
            {
                color.set_char_color(ATTypes.AT_MAGIC, ch);
                color.send_to_char("A magical force stops you!", ch);
                color.set_char_color(ATTypes.AT_TELL, ch);
                color.send_to_char("Someone tells you, 'No littering here!'", ch);
                return;
            }

            int number = 0;
            string qty = firstArg.ParseWord(1, ".");
            if (!qty.IsNullOrEmpty() && qty.IsNumber())
                number = qty.ToInt32();

            if (CheckFunctions.CheckIfTrue(ch, number < 1, "That was easy...")) return;

            firstArg = firstArg.ParseWord(2, ".");

            if (number > 0 && (firstArg.EqualsIgnoreCase("coins") || firstArg.EqualsIgnoreCase("coin")))
                DropCoins(ch, number, firstArg);
            else if (number <= 1 && !firstArg.EqualsIgnoreCase("all") && !firstArg.StartsWithIgnoreCase("all."))
                DropObject(ch, firstArg);
            else
                DropAllOrSome(ch, firstArg);

            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Drop))
                save.save_char_obj(ch);
        }

        private static void DropCoins(CharacterInstance ch, int number, string firstArg)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentCoin < number, "You haven't got that many coins.")) return;

            ch.CurrentCoin -= number;

            int num = number;
            ObjectInstance obj = ch.CurrentRoom.Contents.FirstOrDefault(x => x.ID == VnumConstants.OBJ_VNUM_MONEY_ONE);
            if (obj != null)
            {
                num += 1;
                handler.extract_obj(obj);
            }
            else
            {
                obj = ch.CurrentRoom.Contents.FirstOrDefault(x => x.ID == VnumConstants.OBJ_VNUM_MONEY_SOME);
                if (obj != null)
                {
                    num += obj.Value[0];
                    handler.extract_obj(obj);
                }
            }

            comm.act(ATTypes.AT_ACTION, "$n drops some coin.", ch, null, null, ToTypes.Room);
            ch.CurrentRoom.ToRoom(ObjectFactory.CreateMoney(num));
            color.send_to_char("You let the coin slip from your hand.", ch);

            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Drop))
                save.save_char_obj(ch);
        }

        private static void DropObject(CharacterInstance ch, string firstArg)
        {
            ObjectInstance obj = ch.GetCarriedObject(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You do not have that item.")) return;
            if (CheckFunctions.CheckIfTrue(ch, !ch.CanDrop(obj), "You can't let go of it.")) return;

            handler.separate_obj(obj);
            comm.act(ATTypes.AT_ACTION, "$n drops $p.", ch, obj, null, ToTypes.Room);
            comm.act(ATTypes.AT_ACTION, "You drop $p.", ch, obj, null, ToTypes.Character);

            obj.FromCharacter();
            obj = ch.CurrentRoom.ToRoom(obj);
            mud_prog.oprog_drop_trigger(ch, obj);

            if (ch.CharDied() || handler.obj_extracted(obj))
                return;

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom))
            {
                foreach (ClanData clan in DatabaseManager.Instance.CLANS.Values)
                {
                    if (clan.StoreRoom == ch.CurrentRoom.ID)
                        act_obj.save_clan_storeroom(ch, clan);
                }
            }

            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Drop))
                save.save_char_obj(ch);
        }

        private static void DropAllOrSome(CharacterInstance ch, string firstArg)
        {
            bool all = firstArg.EqualsIgnoreCase("all");
            string arg = all ? firstArg.Substring(4) : firstArg;

            if (CheckFunctions.CheckIfTrue(ch,
                ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDropAll) || ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom),
                "You can't seem to do that.")) return;

            bool found = false;
            foreach (ObjectInstance obj in ch.Carrying)
            {
                if ((all || obj.Name.IsAnyEqual(arg)) && ch.CanSee(obj) && obj.WearLocation == WearLocations.None &&
                    ch.CanDrop(obj))
                {
                    
                }
            }

        }
    }
}
