using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Movement
{
    public static class Open
    {
        public static void do_open(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Open what?")) return;

            var exit = ch.FindExit(firstArg, true);
            if (exit != null)
            {
                OpenDoor(ch, exit, firstArg);
                return;
            }

            var obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj != null)
            {
                OpenObject(ch, obj, firstArg);
                return;
            }

            ch.Printf("You see no %s here.", firstArg);
        }

        private static void OpenObject(CharacterInstance ch, ObjectInstance obj, string arg)
        {
            if (obj.ItemType != ItemTypes.Container)
            {
                ch.Printf("%s is not a container.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (!obj.Values.Flags.IsSet(ContainerFlags.Closed))
            {
                ch.Printf("%s is already open.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (!obj.Values.Flags.IsSet(ContainerFlags.Closeable))
            {
                ch.Printf("%s cannot be opened or closed.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (obj.Values.Flags.IsSet(ContainerFlags.Locked))
            {
                ch.Printf("%s is locked.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            obj.Values.Flags = obj.Values.Flags.RemoveBit(ContainerFlags.Closed);
            comm.act(ATTypes.AT_ACTION, "You open $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n opens $p.", ch, obj, null, ToTypes.Room);
            ch.CheckObjectForTrap(obj, TrapTriggerTypes.Open);
        }

        private static void OpenDoor(CharacterInstance ch, ExitData exit, string arg)
        {
            if (exit.Flags.IsSet(ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(arg))
            {
                ch.Printf("You see no %s here.", arg);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, !exit.Flags.IsSet(ExitFlags.IsDoor)
                || exit.Flags.IsSet(ExitFlags.Dig), "You can't do that.")) return;

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Closed, "It's already open.")) return;

            if (CheckFunctions.CheckIfTrue(ch, exit.Flags.IsSet(ExitFlags.Locked) &&
                    exit.Flags.IsSet(ExitFlags.Bolted), "The bolt is locked.")) return;

            if (CheckFunctions.CheckIfSet(ch, exit.Flags, ExitFlags.Bolted, "It's bolted.")) return;
            if (CheckFunctions.CheckIfSet(ch, exit.Flags, ExitFlags.Locked, "It's locked.")) return;

            if (!exit.Flags.IsSet(ExitFlags.Secret) || exit.Keywords.IsAnyEqual(arg))
            {
                comm.act(ATTypes.AT_ACTION, "$n opens the $d.", ch, null, exit.Keywords, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You open the $d.", ch, null, exit.Keywords, ToTypes.Character);

                var reverseExit = exit.GetReverse();
                if (reverseExit != null)
                {
                    var room = exit.GetDestination(DatabaseManager.Instance);
                    foreach(var vch in room.Persons)
                        comm.act(ATTypes.AT_ACTION, "The $d opens.", vch, null, reverseExit.Keywords, ToTypes.Character);
                }

                exit.RemoveFlagFromSelfAndReverseExit(ExitFlags.Closed);

                // TODO Check for traps
            }
        }
    }
}
