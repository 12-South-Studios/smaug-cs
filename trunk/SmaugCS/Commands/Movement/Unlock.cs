using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Unlock
    {
        public static void do_unlock(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Unlock what?")) return;

            ExitData exit = act_move.find_door(ch, firstArg, true);
            if (exit != null)
            {
                UnlockDoor(ch, exit, firstArg);
                return;
            }

            ObjectInstance obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj != null)
            {
                UnlockObject(ch, obj, firstArg);
                return;
            }

            color.ch_printf(ch, "You see no %s here.", firstArg);
        }

        private static void UnlockObject(CharacterInstance ch, ObjectInstance obj, string firstArg)
        {
            if (CheckFunctions.CheckIf(ch, args => ((ObjectInstance) args[0]).ItemType != ItemTypes.Container,
                "That's not a container.", new List<object> {obj})) return;
            if (CheckFunctions.CheckIfNotSet(ch, obj.Value[1], (int) ContainerFlags.Closed, "It's not closed.")) return;
            if (CheckFunctions.CheckIf(ch, args => ((ObjectInstance) args[0]).Value[2] < 0, "It can't be unlocked.",
                new List<object> {obj})) return;

            ObjectInstance key = ch.HasKey(obj.Value[2]);
            if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, obj.Value[1], (int)ExitFlags.Locked, "It's already unlocked.")) return;

            obj.Value[1].RemoveBit((int) ContainerFlags.Locked);
            color.send_to_char("*Click*", ch);
            int count = key.Count;
            key.Count = 1;
            comm.act(ATTypes.AT_ACTION, "$n unlocks $p with $P.", ch, obj, key, ToTypes.Room);
            key.Count = count;

            if (obj.Value[1].IsSet((int)ContainerFlags.EatKey))
            {
                handler.separate_obj(key);
                handler.extract_obj(key);
            }
        }

        private static void UnlockDoor(CharacterInstance ch, ExitData exit, string firstArg)
        {
            if (exit.Flags.IsSet((int) ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(firstArg))
            {
                color.ch_printf(ch, "You see no %s here.", firstArg);
                return;
            }

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.IsDoor, "You can't do that.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.Closed, "It's not closed.")) return;

            if (exit.Key < 0)
            {
                color.send_to_char("It can't be unlocked.", ch);
                return;
            }

            ObjectInstance key = ch.HasKey(exit.Key);
            if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, (int) ExitFlags.Locked, "It's already unlocked.")) return;

            if (!exit.Flags.IsSet((int) ExitFlags.Secret) || exit.Keywords.IsAnyEqual(firstArg))
            {
                color.send_to_char("*Click*", ch);
                int count = key.Count;
                key.Count = 1;
                comm.act(ATTypes.AT_ACTION, "$n unlocks the $d with $p.", ch, key, exit.Keywords, ToTypes.Room);
                key.Count = count;

                if (exit.Flags.IsSet((int) ExitFlags.EatKey))
                {
                    handler.separate_obj(key);
                    handler.extract_obj(key);
                }

                exit.RemoveFlagFromSelfAndReverseExit(ExitFlags.Locked);
            }
        }
    }
}
