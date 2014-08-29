using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Movement
{
    public static class Open
    {
        public static void do_open(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Open what?")) return;

            ExitData exit = act_move.find_door(ch, firstArg, true);
            if (exit != null)
            {
                OpenDoor(ch, exit, firstArg);
                return;
            }

            ObjectInstance obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj != null)
            {
                OpenObject(ch, obj, firstArg);
                return;
            }

            color.ch_printf(ch, "You see no %s here.", firstArg);
        }

        private static void OpenObject(CharacterInstance ch, ObjectInstance obj, string arg)
        {
            if (obj.ItemType != ItemTypes.Container)
            {
                color.ch_printf(ch, "%s is not a container.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (!obj.Value[1].IsSet((int) ContainerFlags.Closed))
            {
                color.ch_printf(ch, "%s is already open.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (!obj.Value[1].IsSet((int) ContainerFlags.Closeable))
            {
                color.ch_printf(ch, "%s cannot be opened or closed.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (obj.Value[1].IsSet((int) ContainerFlags.Locked))
            {
                color.ch_printf(ch, "%s is locked.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            obj.Value[1].RemoveBit((int) ContainerFlags.Closed);
            comm.act(ATTypes.AT_ACTION, "You open $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n opens $p.", ch, obj, null, ToTypes.Room);
            handler.check_for_trap(ch, obj, TrapTriggerTypes.Open);
        }

        private static void OpenDoor(CharacterInstance ch, ExitData exit, string arg)
        {
            if (exit.Flags.IsSet((int) ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(arg))
            {
                color.ch_printf(ch, "You see no %s here.", arg);
                return;
            }

            if (CheckFunctions.CheckIf(ch, args => !((ExitData) args[0]).Flags.IsSet((int) ExitFlags.IsDoor)
                                                   || ((ExitData) args[0]).Flags.IsSet((int) ExitFlags.Dig),
                "You can't do that.", new List<object> {exit})) return;

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.Closed, "It's already open.")) return;

            if (CheckFunctions.CheckIf(ch,
                args =>
                    ((ExitData) args[0]).Flags.IsSet((int) ExitFlags.Locked) &&
                    ((ExitData) args[0]).Flags.IsSet((int) ExitFlags.Bolted), "The bolt is locked.",
                new List<object> {exit})) return;

            if (CheckFunctions.CheckIfSet(ch, exit.Flags, (int) ExitFlags.Bolted, "It's bolted.")) return;
            if (CheckFunctions.CheckIfSet(ch, exit.Flags, (int) ExitFlags.Locked, "It's locked.")) return;

            if (!exit.Flags.IsSet((int) ExitFlags.Secret) || exit.Keywords.IsAnyEqual(arg))
            {
                comm.act(ATTypes.AT_ACTION, "$n opens the $d.", ch, null, exit.Keywords, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You open the $d.", ch, null, exit.Keywords, ToTypes.Character);

                ExitData reverseExit = exit.GetReverseExit();
                if (reverseExit != null)
                {
                    RoomTemplate room = exit.GetDestination(DatabaseManager.Instance);
                    foreach(CharacterInstance vch in room.Persons)
                        comm.act(ATTypes.AT_ACTION, "The $d opens.", vch, null, reverseExit.Keywords, ToTypes.Character);
                }

                exit.RemoveFlagFromSelfAndReverseExit(ExitFlags.Closed);

                // TODO Check for traps
            }
        }
    }
}
