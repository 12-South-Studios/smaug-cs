using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS.Commands
{
    public static class Close
    {
        public static void do_close(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Close what?")) return;

            var exit = ch.FindExit(firstArg, true);
            if (exit != null)
            {
                CloseDoor(ch, exit, firstArg);
                return;
            }

            var obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj != null)
            {
                CloseObject(ch, obj);
                return;
            }

            ch.Printf("You see no %s here.", firstArg);
        }

        private static void CloseObject(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.ItemType != ItemTypes.Container)
            {
                ch.Printf("%s is not a container.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (obj.Values.Flags.IsSet(ContainerFlags.Closed))
            {
                ch.Printf("%s is already closed.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            if (!obj.Values.Flags.IsSet(ContainerFlags.Closeable))
            {
                ch.Printf("%s cannot be opened or closed.", obj.ShortDescription.CapitalizeFirst());
                return;
            }

            obj.Values.Flags = obj.Values.Flags.SetBit(ContainerFlags.Closed);
            comm.act(ATTypes.AT_ACTION, "You close $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n closes $p.", ch, obj, null, ToTypes.Room);
            ch.CheckObjectForTrap(obj, TrapTriggerTypes.Close);
        }

        private static void CloseDoor(CharacterInstance ch, ExitData exit, string firstArg)
        {
            if (exit.Flags.IsSet(ExitFlags.Secret)
                && !exit.Keywords.IsAnyEqual(firstArg))
            {
                ch.Printf("You see no %s here.", firstArg);
                return;
            }

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsDoor, "You can't do that.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Closed, "It's already closed.")) return;

            comm.act(ATTypes.AT_ACTION, "$n closes the $d.", ch, null, exit.Keywords, ToTypes.Room);
            comm.act(ATTypes.AT_ACTION, "You close the $d.", ch, null, exit.Keywords, ToTypes.Character);

            var reverseExit = exit.GetReverse();
            if (reverseExit != null)
            {
                reverseExit.Flags.SetBit(ExitFlags.Closed);
                foreach (var vch in exit.GetDestination(RepositoryManager.Instance).Persons)
                    comm.act(ATTypes.AT_ACTION, "The $d closes.", vch, null, reverseExit.Keywords, ToTypes.Character);
            }

            exit.SetFlagOnSelfAndReverseExit(ExitFlags.Closed);

            // TODO Check room for traps
        }
    }
}
