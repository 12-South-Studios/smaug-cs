﻿using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Lock
    {
        public static void do_lock(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();

            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Lock what?")) return;

            var exit = ch.FindExit(firstArg, true);
            if (exit != null)
            {
                LockDoor(ch, exit, firstArg);
                return;
            }

            var obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj != null)
            {
                LockObject(ch, obj, firstArg);
                return;
            }

            ch.Printf("You see no %s here.", firstArg);
        }

        private static void LockObject(CharacterInstance ch, ObjectInstance obj, string arg)
        {
            if (CheckFunctions.CheckIfTrue(ch, obj.ItemType != ItemTypes.Container, "That's not a container.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, obj.Values.Flags, ContainerFlags.Closed, "It's not closed.")) return;
            if (CheckFunctions.CheckIfTrue(ch, obj.Values.KeyID <= 0, "It can't be locked.")) return;

            var key = ch.HasKey((int)obj.Values.KeyID);
            if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
            if (CheckFunctions.CheckIfSet(ch, obj.Values.Flags, ContainerFlags.Locked, "It's already locked."))
                return;

            obj.Values.Flags = obj.Values.Flags.SetBit(ContainerFlags.Locked);
            ch.SendTo("*Click*");
            var count = key.Count;
            key.Count = 1;
            comm.act(ATTypes.AT_ACTION, "$n locks $p with $P.", ch, obj, key, ToTypes.Room);
            key.Count = count;
        }

        private static void LockDoor(CharacterInstance ch, ExitData exit, string arg)
        {
            if (exit.Flags.IsSet(ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(arg))
            {
                ch.Printf("You see no %s here.", arg);
                return;
            }

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsDoor, "You can't do that.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Closed, "It's not closed.")) return;
            if (CheckFunctions.CheckIfTrue(ch, exit.Key <= 0, "It can't be locked.")) return;

            var key = ch.HasKey(exit.Key);
            if (CheckFunctions.CheckIfNullObject(ch, key, "You lack the key.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Locked, "It's already locked.")) return;

            if (!exit.Flags.IsSet(ExitFlags.Secret) || exit.Keywords.IsAnyEqual(arg))
            {
                ch.SendTo("*Click*");
                var count = key.Count;
                key.Count++;
                comm.act(ATTypes.AT_ACTION, "$n locks the $d with $p.", ch, key, exit.Keywords, ToTypes.Room);
                key.Count = count;
                exit.SetFlagOnSelfAndReverseExit(ExitFlags.Locked);
            }
        }
    }
}
