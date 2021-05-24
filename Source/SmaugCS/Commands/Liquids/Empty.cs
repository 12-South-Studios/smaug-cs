using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using System.Linq;

namespace SmaugCS.Commands.Liquids
{
    public static class Empty
    {
        public static void do_empty(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Empty what?")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;

            var obj = ch.GetCarriedObject(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You aren't carrying that.")) return;

            if (obj.Count > 1)
                obj.Split();

            var secondArg = argument.SecondWord();
            if (secondArg.EqualsIgnoreCase("into"))
                secondArg = argument.ThirdWord();

            switch (obj.ItemType)
            {
                case ItemTypes.Pipe:
                    EmptyPipe(ch, obj);
                    break;
                case ItemTypes.DrinkContainer:
                    EmptyDrinkContainer(ch, obj);
                    break;
                case ItemTypes.Quiver:
                case ItemTypes.Container:
                    EmptyContainerOrQuiver(ch, obj);
                    break;
                case ItemTypes.KeyRing:
                    EmptyKeyRing(ch, obj, secondArg);
                    break;
                default:
                    comm.act(ATTypes.AT_ACTION, "You shake $p in an attempt to empty it...", ch, obj, null, ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, "$n begins to shake $p in an attempt to empty it...", ch, obj, null, ToTypes.Room);
                    break;
            }
        }

        private static void EmptyKeyRing(CharacterInstance ch, ObjectInstance obj, string arg)
        {
            if (CheckFunctions.CheckIfTrue(ch, !obj.Contents.Any(), "It's already empty.")) return;

            if (string.IsNullOrEmpty(arg))
                EmptyToGround(ch, obj);
            else
                EmptyIntoObject(ch, obj, arg);
        }

        private static void EmptyIntoObject(CharacterInstance ch, ObjectInstance obj, string arg)
        {
            var destObj = ch.GetObjectOnMeOrInRoom(arg);
            if (CheckFunctions.CheckIfNullObject(ch, destObj, "You can't find it.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, destObj, obj, "You can't empty something into itself!")) return;
            if (CheckFunctions.CheckIfTrue(ch,
                destObj.ItemType != ItemTypes.Container && destObj.ItemType != ItemTypes.KeyRing
                && destObj.ItemType != ItemTypes.Quiver, "That's not a container!")) return;

            if (((int)destObj.Values.Flags).IsSet(ContainerFlags.Closed))
            {
                comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, destObj.Name, ToTypes.Character);
                return;
            }

            destObj.Split();

            if (obj.Empty(destObj))
            {
                comm.act(ATTypes.AT_ACTION, "You empty $p into $P.", ch, obj, destObj, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n empties $p into $P.", ch, obj, destObj, ToTypes.Room);

                if (destObj.CarriedBy == null && GetSaveFlags().IsSet(AutoSaveFlags.Empty))
                    save.save_char_obj(ch);
            }
            else
                comm.act(ATTypes.AT_ACTION, "$P is too full.", ch, obj, destObj, ToTypes.Character);
        }

        private static int GetSaveFlags()
        {
            return GameManager.Instance.SystemData.SaveFlags;
        }

        private static void EmptyToGround(CharacterInstance ch, ObjectInstance obj)
        {
            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDrop) || ch.Act.IsSet((int)PlayerFlags.Litterbug))
            {
                ch.SendTo("&[magic]A magical force stops you!");
                ch.SendTo("&[tell]Someone tells you, 'No littering here!");
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch,
                ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDropAll) || ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom),
                "You can't seem to do that here...")) return;

            if (CheckFunctions.CheckIfTrue(ch, !obj.Empty(null, ch.CurrentRoom), "Hmmm... didn't work.")) return;

            comm.act(ATTypes.AT_ACTION, "You empty $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n empties $p.", ch, obj, null, ToTypes.Room);

            if (GetSaveFlags().IsSet(AutoSaveFlags.Empty))
                save.save_char_obj(ch);
        }

        private static void EmptyPipe(CharacterInstance ch, ObjectInstance obj)
        {
            comm.act(ATTypes.AT_ACTION, "You gently tap $p and empty it out.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n gently taps $p and empties it out.", ch, obj, null, ToTypes.Room);
            obj.Values.Flags = obj.Values.Flags.RemoveBit(PipeFlags.FullOfAsh);
            obj.Values.Flags = obj.Values.Flags.RemoveBit(PipeFlags.Lit);
            obj.Values.NumberOfDraws = 0;
        }

        private static void EmptyDrinkContainer(CharacterInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfTrue(ch, obj.Values.Quantity < 1, "It's already empty.")) return;

            comm.act(ATTypes.AT_ACTION, "You empty $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n empties $p.", ch, obj, null, ToTypes.Room);
            obj.Values.Quantity = 0;
        }

        private static void EmptyContainerOrQuiver(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Values.Flags.IsSet(ContainerFlags.Closed))
                comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, obj, ToTypes.Character);
        }
    }
}
