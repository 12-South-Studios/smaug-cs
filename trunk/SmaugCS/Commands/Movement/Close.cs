using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Movement
{
    public static class Close
    {
        public static void do_close(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Close what?")) return;

            ExitData exit = act_move.find_door(ch, firstArg, true);
            if (exit != null)
            {
                CloseDoor(ch, exit, firstArg);
                return;
            }

            ObjectInstance obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj != null)
            {
                CloseObject(ch, obj);
                return;
            }

            color.ch_printf(ch, "You see no %s here.", firstArg);
        }

        private static void CloseObject(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.ItemType != ItemTypes.Container)
            {
                color.ch_printf(ch, "%s is not a container.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (obj.Value[1].IsSet(ContainerFlags.Closed))
            {
                color.ch_printf(ch, "%s is already closed.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (!obj.Value[1].IsSet(ContainerFlags.Closeable))
            {
                color.ch_printf(ch, "%s cannot be opened or closed.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            obj.Value[1].SetBit(ContainerFlags.Closed);
            comm.act(ATTypes.AT_ACTION, "You close $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n closes $p.", ch, obj, null, ToTypes.Room);
            handler.check_for_trap(ch, obj, TrapTriggerTypes.Close);
        }

        private static void CloseDoor(CharacterInstance ch, ExitData exit, string firstArg)
        {
            if (exit.Flags.IsSet(ExitFlags.Secret)
                && !exit.Keywords.IsAnyEqual(firstArg))
            {
                color.ch_printf(ch, "You see no %s here.", firstArg);
                return;
            }

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsDoor, "You can't do that.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Closed, "It's already closed.")) return;

            comm.act(ATTypes.AT_ACTION, "$n closes the $d.", ch, null, exit.Keywords, ToTypes.Room);
            comm.act(ATTypes.AT_ACTION, "You close the $d.", ch, null, exit.Keywords, ToTypes.Character);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null)
            {
                reverseExit.Flags.SetBit(ExitFlags.Closed);
                foreach(CharacterInstance vch in exit.GetDestination(DatabaseManager.Instance).Persons)
                    comm.act(ATTypes.AT_ACTION, "The $d closes.", vch, null, reverseExit.Keywords, ToTypes.Character);
            }

            exit.SetFlagOnSelfAndReverseExit(ExitFlags.Closed);
            
            // TODO Check room for traps
        }
    }
}
