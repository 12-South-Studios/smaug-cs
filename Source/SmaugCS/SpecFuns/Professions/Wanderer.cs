using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Repository;

namespace SmaugCS.SpecFuns.Professions
{
    public static class Wanderer
    {
        public static bool Execute(CharacterInstance ch, IManager dbManager)
        {
            if (!ch.IsAwake()) return false;

            var thrown = false;
            var noExit = true;

            var exit = ch.CurrentRoom.Exits.First();
            if (exit != null) noExit = false;

            if (SmaugRandom.D100() <= 50) return false;

            foreach (var obj in ch.CurrentRoom.Contents
                .Where(obj => obj.WearFlags.IsSet(ItemWearFlags.Take) && !obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                .Where(obj => obj.ItemType == ItemTypes.Weapon || obj.ItemType == ItemTypes.Armor || obj.ItemType == ItemTypes.Light))
            {
                obj.Split();
                comm.act(ATTypes.AT_ACTION, "$n leans over and gets $p.", ch, obj, null, ToTypes.Room);
                ch.CurrentRoom.RemoveFrom(obj);

                var trash = obj.AddTo(ch);
                if (ch.Level < trash.Level)
                {
                    comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is too inexperienced.", ch, trash, null, ToTypes.Room);
                    thrown = true;
                }

                if (!thrown)
                    ch.WearItem(trash, false, ItemWearFlags.None);

                var found = false;
                if (!thrown)
                {
                    foreach (var obj2 in ch.Carrying.Where(obj2 => obj2.WearLocation == WearLocations.None))
                    {
                        Say.do_say(ch, "Hmm, I can't use this.");
                        trash = obj2;
                        thrown = true;
                    }
                }

                if (thrown && !noExit)
                {
                    while (!found && !noExit)
                    {
                        var door = db.number_door();
                        exit = ch.CurrentRoom.GetExitNumber(door);

                        var destRoom = exit?.GetDestination(RepositoryManager.Instance);
                        if (destRoom == null || exit.Flags.IsSet(ExitFlags.Closed) ||
                            destRoom.Flags.IsSet(RoomFlags.NoDrop)) continue;
                        if (destRoom.Persons.OfType<MobileInstance>().Any(x => x.SpecialFunctionName.EqualsIgnoreCase("spec_wanderer")))
                            return false;
                        found = true;
                    }
                }

                if (noExit || !thrown) return true;

                handler.set_cur_obj(trash);
                if (trash.CauseDamageTo() != ReturnTypes.ObjectScrapped)
                    ThrowScrapAtDirection(ch, trash, exit);
                else
                {
                    Say.do_say(ch, "This thing is junk!");
                    comm.act(ATTypes.AT_ACTION, "$n growls and breaks $p.", ch, trash, null, ToTypes.Room);
                }
                return true;
            }

            return false;
        }

        private static void ThrowScrapAtDirection(CharacterInstance ch, ObjectInstance trash, ExitData exit)
        {
            trash.Split();
            comm.act(ATTypes.AT_ACTION, "$n growls and throws $p $T.", ch, trash, exit.Direction.GetName(),
                ToTypes.Room);
            trash.RemoveFrom();

            var oldRoom = ch.CurrentRoom;
            var room = exit.GetDestination(RepositoryManager.Instance);
            room.AddTo(trash);
            ch.CurrentRoom.RemoveFrom(ch);
            room.AddTo(ch);
            comm.act(ATTypes.AT_CYAN, "$p thrown by $n lands in the room.", ch, trash, ch, ToTypes.Room);
            ch.CurrentRoom.RemoveFrom(ch);
            oldRoom.AddTo(ch);
        }
    }
}
