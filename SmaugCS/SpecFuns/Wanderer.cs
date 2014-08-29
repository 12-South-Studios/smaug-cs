using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Wanderer
    {
        public static bool DoSpecWanderer(CharacterInstance ch)
        {
            if (!ch.IsAwake())
                return false;

            bool thrown = false;
            bool noExit = true;

            ExitData exit = ch.CurrentRoom.Exits.First();
            if (exit != null)
                noExit = false;

            if (SmaugRandom.D100() <= 50)
                return false;

            foreach (ObjectInstance obj in ch.CurrentRoom.Contents)
            {
                if (!obj.WearFlags.IsSet(ItemWearFlags.Take)
                    || obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                    continue;

                if (obj.ItemType != ItemTypes.Weapon
                    && obj.ItemType != ItemTypes.Armor
                    && obj.ItemType != ItemTypes.Light)
                    continue;

                handler.separate_obj(obj);
                comm.act(ATTypes.AT_ACTION, "$n leans over and gets $p.", ch, obj, null, ToTypes.Room);
                ch.CurrentRoom.FromRoom(obj);

                ObjectInstance trash = obj.ToCharacter(ch);
                if (ch.Level < trash.Level)
                {
                    comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is too inexperienced.", ch, trash, null, ToTypes.Room);
                    thrown = true;
                }

                if (!thrown)
                    act_obj.wear_obj(ch, trash, false, -1);

                bool found = false;
                if (!thrown)
                {
                    foreach (ObjectInstance obj2 in ch.Carrying.Where(obj2 => obj2.WearLocation == WearLocations.None))
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
                        int door = db.number_door();
                        exit = ch.CurrentRoom.GetExitNumber(door);
                        
                        if (exit != null)
                        {
                            RoomTemplate destRoom = exit.GetDestination(DatabaseManager.Instance);
                            if (destRoom != null && !exit.Flags.IsSet(ExitFlags.Closed)
                                && !destRoom.Flags.IsSet(RoomFlags.NoDrop))
                            {
                                if (destRoom.Persons.Any(x => x.SpecialFunctionName.EqualsIgnoreCase("spec_wanderer")))
                                    return false;
                                found = true;
                            }
                        }    
                    }
                }

                if (!noExit && thrown)
                {
                    handler.set_cur_obj(trash);
                    if (act_obj.damage_obj(trash) != ReturnTypes.ObjectScrapped)
                    {
                        handler.separate_obj(trash);
                        comm.act(ATTypes.AT_ACTION, "$n growls and throws $p $T.", ch, trash, exit.Direction.GetName(),
                            ToTypes.Room);
                        trash.FromCharacter();

                        RoomTemplate oldRoom = ch.CurrentRoom;
                        RoomTemplate room = exit.GetDestination(DatabaseManager.Instance);
                        room.ToRoom(trash);
                        ch.CurrentRoom.FromRoom(ch);
                        room.ToRoom(ch);
                        comm.act(ATTypes.AT_CYAN, "$p thrown by $n lands in the room.", ch, trash, ch, ToTypes.Room);
                        ch.CurrentRoom.FromRoom(ch);
                        oldRoom.ToRoom(ch);
                    }
                    else
                    {
                        Say.do_say(ch, "This thing is junk!");
                        comm.act(ATTypes.AT_ACTION, "$n growls and breaks $p.", ch, trash, null, ToTypes.Room);
                    }
                    return true;
                }

                return true;
            }

            return false;
        }
    }
}
