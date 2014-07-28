using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Lock
    {
        public static void do_lock(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();

            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Lock what?")) return;

            ExitData exit = act_move.find_door(ch, firstArg, true);
            if (exit != null)
            {
                LockDoor(ch, exit, firstArg);
                return;
            }

            ObjectInstance obj = handler.get_obj_here(ch, firstArg);
            if (obj != null)
            {
                LockObject(ch, obj, firstArg);
                return;
            }

            color.ch_printf(ch, "You see no %s here.", firstArg);
        }

        private static void LockObject(CharacterInstance ch, ObjectInstance obj, string arg)
        {
            if (CheckFunctions.CheckIf(ch, args => ((ObjectInstance) args[0]).ItemType != ItemTypes.Container,
                "That's not a container.", new List<object> {obj})) return;
            if (CheckFunctions.CheckIfNotSet(ch, obj.Value[1], (int) ContainerFlags.Closed, "It's not closed.")) return;
            if (CheckFunctions.CheckIf(ch, args => ((ObjectInstance) args[0]).Value[2] < 0, "It can't be locked.",
                new List<object> {obj})) return;

            ObjectInstance key = ch.HasKey(obj.Value[2]);
            if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
            if (CheckFunctions.CheckIfSet(ch, obj.Value[1], (int) ContainerFlags.Locked, "It's already locked."))
                return;

            obj.Value[1].SetBit((int) ContainerFlags.Locked);
            color.send_to_char("*Click*", ch);
            int count = key.Count;
            key.Count = 1;
            comm.act(ATTypes.AT_ACTION, "$n locks $p with $P.", ch, obj, key, ToTypes.Room);
            key.Count = count;
        }

        private static void LockDoor(CharacterInstance ch, ExitData exit, string arg)
        {
            if (exit.Flags.IsSet((int) ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(arg))
            {
                color.ch_printf(ch, "You see no %s here.", arg);
                return;
            }

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.IsDoor, "You can't do that.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.Closed, "It's not closed.")) return;
            if (CheckFunctions.CheckIf(ch, args => ((ExitData) args[0]).Key <= 0,
                "It can't be locked.", new List<object> {exit})) return;

            ObjectInstance key = ch.HasKey(exit.Key);
            if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.Locked, "It's already locked.")) return;

            if (!exit.Flags.IsSet((int) ExitFlags.Secret)
                || exit.Keywords.IsAnyEqual(arg))
            {
                color.send_to_char("*Click*", ch);
                int count = key.Count;
                key.Count++;
                comm.act(ATTypes.AT_ACTION, "$n locks the $d with $p.", ch, key, exit.Keywords, ToTypes.Room);
                key.Count = count;
                exit.SetFlagOnSelfAndReverseExit(ExitFlags.Locked);
            }

        }
    }
}
