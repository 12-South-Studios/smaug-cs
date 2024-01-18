using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Logging;
using SmaugCS.MudProgs;
using SmaugCS.Repository;
using System.Linq;

namespace SmaugCS
{
    public static class act_obj
    {
        public static void get_obj(CharacterInstance ch, ObjectInstance obj, ObjectInstance container)
        {
            if (CheckFunctions.CheckIfTrue(ch, !obj.WearFlags.IsSet(ItemWearFlags.Take)
                && (ch.Level < GameManager.Instance.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelGetObjectNoTake)),
                "You can't take that.")) return;

            if (obj.MagicFlags.IsSet(ItemMagicFlags.PKDisarmed) && !ch.IsNpc())
            {
                var timer = ch.GetTimer(TimerTypes.PKilled);
                if (ch.CanPKill() && timer == null)
                {
                    if (ch.Level - obj.Value.ToList()[5] > 5 || obj.Value.ToList()[5] - ch.Level > 5)
                    {
                        ch.SendTo("\r\n&bA godly force freezes your outstretched hand.");
                        return;
                    }

                    obj.MagicFlags.RemoveBit(ItemMagicFlags.PKDisarmed);
                    obj.Value.ToList()[5] = 0;
                }
            }
            else
            {
                ch.SendTo("\r\n&BA godly force freezes your outstretched hand.");
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, obj.ExtraFlags.IsSet((int)ItemExtraFlags.Prototype) && !ch.CanTakePrototype(),
                "A godly force prevents you from getting close to it.")) return;

            if (ch.CarryNumber + obj.ObjectNumber > ch.CanCarryN())
            {
                comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that many items.", ch, null, obj.ShortDescription, ToTypes.Character);
                return;
            }

            var weight = obj.ExtraFlags.IsSet((int)ItemExtraFlags.Covering)
                             ? obj.Weight
                             : obj.GetWeight();

            if (obj.ItemType != ItemTypes.Money)
            {
                if (obj.InObject != null)
                {
                    var tObject = obj.InObject;
                    var inobj = 1;
                    var checkweight = tObject.ItemType == ItemTypes.Container
                                       && tObject.ExtraFlags.IsSet((int)ItemExtraFlags.Magical);

                    while (tObject.InObject != null)
                    {
                        tObject = tObject.InObject;
                        inobj++;

                        checkweight = tObject.ItemType == ItemTypes.Container
                                      && tObject.ExtraFlags.IsSet((int)ItemExtraFlags.Magical);
                    }

                    if (tObject.CarriedBy == null || tObject.CarriedBy != ch || checkweight)
                    {
                        if (ch.CarryWeight + weight > ch.CanCarryMaxWeight())
                        {
                            comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that much weight.", ch, null, obj.ShortDescription, ToTypes.Character);
                            return;
                        }
                    }
                }
                else if (ch.CarryWeight + weight > ch.CanCarryMaxWeight())
                {
                    comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that much weight.", ch, null, obj.ShortDescription, ToTypes.Character);
                    return;
                }
            }

            if (container != null)
                GetObjectFromContainer(ch, obj, container);
            else
                GetObjectFromRoom(ch, obj);

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom)
                && (container?.CarriedBy == null))
            {
                foreach (var clan in RepositoryManager.Instance.CLANS.Values)
                {
                    if (clan.StoreRoom == ch.CurrentRoom.ID)
                    {
                        // TODO Fix save_clan_storeroom(ch, clan);
                    }
                }
            }

            if (obj.ItemType != ItemTypes.Container)
                ch.CheckObjectForTrap(obj, TrapTriggerTypes.Get);
            if (ch.CharDied())
                return;

            if (obj.ItemType == ItemTypes.Money)
            {
                int amt = obj.Values.NumberOfCoins * obj.Count;
                ch.CurrentCoin += amt;
                obj.Extract();
            }
            else
                obj = obj.AddTo(ch);

            if (ch.CharDied() || handler.obj_extracted(obj))
                return;

            MudProgHandler.ExecuteObjectProg(MudProgTypes.Get, ch, obj);
        }

        private static void GetObjectFromRoom(CharacterInstance ch, ObjectInstance obj)
        {
            comm.act(ATTypes.AT_ACTION, "You get $p.", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n gets $p.", ch, obj, null, ToTypes.Room);
            obj.InRoom.RemoveFrom(obj);
        }

        private static void GetObjectFromContainer(CharacterInstance ch, ObjectInstance obj, ObjectInstance container)
        {
            if (container.ItemType == ItemTypes.KeyRing
                && !container.ExtraFlags.IsSet((int)ItemExtraFlags.Covering))
            {
                comm.act(ATTypes.AT_ACTION, "You remove $p from $P", ch, obj, container, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n removes $p from $P", ch, obj, container, ToTypes.Room);
            }
            else
            {
                comm.act(ATTypes.AT_ACTION, container.ExtraFlags.IsSet((int)ItemExtraFlags.Covering)
                    ? "You get $p from beneath $P."
                    : "You get $p from $P", ch, obj, container, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, container.ExtraFlags.IsSet((int)ItemExtraFlags.Covering)
                    ? "$n gets $p from beneath $P."
                    : "$n gets $p from $P", ch, obj, container, ToTypes.Room);
            }

            if (container.ExtraFlags.IsSet((int)ItemExtraFlags.ClanCorpse)
                && !ch.IsNpc() && container.Name.Contains(ch.Name))
                container.Value.ToList()[5]++;
            obj.InObject.RemoveFrom(obj);
        }

        private static int fall_count;
        private static bool is_falling;

        /// stops infinite loops from the call to obj_to_room
        public static void obj_fall(ObjectInstance obj, bool through)
        {
            if (obj.InRoom == null || is_falling)
                return;

            if (fall_count > 30)
            {
                LogManager.Instance.Bug("Object falling in loop more than 30 times");
                obj.Extract();
                fall_count = 0;
                return;
            }

            if (obj.InRoom.Flags.IsSet((int)RoomFlags.NoFloor)
                && Macros.CAN_GO(obj, (int)DirectionTypes.Down)
                && !obj.ExtraFlags.IsSet((int)ItemExtraFlags.Magical))
            {
                var exit = obj.InRoom.GetExit(DirectionTypes.Down);
                var to_room = exit.GetDestination();

                if (through)
                    fall_count++;
                else
                    fall_count = 0;

                if (obj.InRoom == to_room)
                {
                    LogManager.Instance.Bug("Object falling into same room {0}", to_room.ID);
                    obj.Extract();
                    return;
                }

                if (obj.InRoom.Persons.Any())
                {
                    comm.act(ATTypes.AT_PLAIN, "$p falls far below...", obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_PLAIN, "$p falls far below...", obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                }

                obj.InRoom.RemoveFrom(obj);
                is_falling = true;
                to_room.AddTo(obj);
                is_falling = false;

                if (obj.InRoom.Persons.Any())
                {
                    comm.act(ATTypes.AT_PLAIN, "$p falls from above...", obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_PLAIN, "$p falls from above...", obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                }

                if (!obj.InRoom.Flags.IsSet((int)RoomFlags.NoFloor) && through)
                {
                    var dam = fall_count * obj.Weight / 2;

                    // Damage players in room
                    if (obj.InRoom.Persons.Any() && SmaugRandom.D100() > 15)
                    {
                        foreach (var rch in obj.InRoom.Persons)
                        {
                            comm.act(ATTypes.AT_WHITE, "$p falls on $n!", rch, obj, null, ToTypes.Room);
                            comm.act(ATTypes.AT_WHITE, "$p falls on you!", rch, obj, null, ToTypes.Character);

                            if (rch.IsNpc() && rch.Act.IsSet((int)ActFlags.Hardhat))
                                comm.act(ATTypes.AT_WHITE, "$p bounces harmlessly off your head!", rch, obj, null,
                                         ToTypes.Character);
                            else
                                rch.CauseDamageTo(rch, dam * rch.Level, -1);
                        }
                    }

                    // Damage the falling object
                    switch (obj.ItemType)
                    {
                        case ItemTypes.Weapon:
                        case ItemTypes.Armor:
                            if (obj.Values.CurrentAC - dam <= 0)
                            {
                                if (obj.InRoom.Persons.Any())
                                {
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                                }
                                ObjectFactory.CreateScraps(obj);
                            }
                            else
                                obj.Values.CurrentAC -= dam;
                            break;
                        default:
                            if (dam * 15 > obj.GetResistance())
                            {
                                if (obj.InRoom.Persons.Any())
                                {
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                                }
                                ObjectFactory.CreateScraps(obj);
                            }
                            break;
                    }
                }
            }

            obj_fall(obj, true);
        }

        public static ObjectInstance recursive_note_find(ObjectInstance obj, string argument)
        {
            if (obj == null) return null;

            var match = true;
            var arg = string.Empty;

            switch (obj.ItemType)
            {
                case ItemTypes.Paper:
                    var subject = db.get_extra_descr("_subject_", obj.ExtraDescriptions);
                    if (string.IsNullOrEmpty(subject))
                        break;

                    subject = subject.ToLower();

                    while (match)
                    {
                        var args = argument.FirstArgument();
                        var argcopy = args.Item2;
                        arg = args.Item1;
                        if (string.IsNullOrEmpty(argcopy))
                            break;

                        if (!subject.Contains(arg))
                            match = false;
                    }

                    if (match)
                        return obj;
                    break;
                case ItemTypes.Container:
                case ItemTypes.NpcCorpse:
                case ItemTypes.PlayerCorpse:
                    if (obj.Contents.Any())
                    {
                        var returnedObj = recursive_note_find(obj.Contents.First(), argument);
                        if (returnedObj != null)
                            return returnedObj;
                    }
                    break;
            }

            return recursive_note_find(obj.Contents.First(), argument);
        }

        public static string get_chance_verb(ObjectInstance obj)
        {
            return !string.IsNullOrWhiteSpace(obj.Action) ? obj.Action : "roll$q";
        }

        public static string get_ed_number(ObjectInstance obj, int number)
        {
            var count = 1;
            foreach (var ed in obj.ExtraDescriptions)
            {
                if (count == number)
                    return ed.Description;
                count++;
            }

            return string.Empty;
        }
    }
}
