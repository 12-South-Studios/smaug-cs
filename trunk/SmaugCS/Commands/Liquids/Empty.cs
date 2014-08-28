using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Liquids
{
    public static class Empty
    {
        public static void do_empty(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Empty what?")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;

            ObjectInstance obj = ch.GetCarriedObject(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You aren't carrying that.")) return;

            if (obj.Count > 1)
                handler.separate_obj(obj);

            string secondArg = argument.SecondWord();
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
            ObjectInstance destObj = ch.GetObjectOnMeOrInRoom(arg);
            if (CheckFunctions.CheckIfNullObject(ch, destObj, "You can't find it.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, destObj, obj, "You can't empty something into itself!")) return;
            if (CheckFunctions.CheckIfTrue(ch,
                destObj.ItemType != ItemTypes.Container && destObj.ItemType != ItemTypes.KeyRing
                && destObj.ItemType != ItemTypes.Quiver, "That's not a container!")) return;

            if (destObj.Value[1].IsSet(ContainerFlags.Closed))
            {
                comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, destObj.Name, ToTypes.Character);
                return;
            }

            handler.separate_obj(destObj);

            if (handler.empty_obj(obj, destObj, null))
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
            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDrop) || ch.Act.IsSet(PlayerFlags.Litterbug))
            {
                color.send_to_char("&[magic]A magical force stops you!", ch);
                color.send_to_char("&[tell]Someone tells you, 'No littering here!", ch);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch,
                ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDropAll) || ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom),
                "You can't seem to do that here...")) return;

            if (handler.empty_obj(obj, null, ch.CurrentRoom))
            {
                comm.act(ATTypes.AT_ACTION, "You empty $p.", ch, obj, null, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n empties $p.", ch, obj, null, ToTypes.Room);
                
                if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Empty))
                    save.save_char_obj(ch);
            }
            else
                color.send_to_char("Hmmm... didn't work.", ch);
        }

        private static void EmptyPipe(CharacterInstance ch, ObjectInstance obj)
        {
            comm.act(ATTypes.AT_ACTION, "You gently tap $p and empty it out.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n gently taps $p and empties it out.", ch, obj, null, ToTypes.Room);
            obj.Value[3].RemoveBit(PipeFlags.FullOfAsh);
            obj.Value[3].RemoveBit(PipeFlags.Lit);
            obj.Value[1] = 0;
        }

        private static void EmptyDrinkContainer(CharacterInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfTrue(ch, obj.Value[1] < 1, "It's already empty.")) return;

            comm.act(ATTypes.AT_ACTION, "You empty $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n empties $p.", ch, obj, null, ToTypes.Room);
            obj.Value[1] = 0;
        }

        private static void EmptyContainerOrQuiver(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[1].IsSet(ContainerFlags.Closed))
                comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, obj, ToTypes.Character);
        }
    }
}
