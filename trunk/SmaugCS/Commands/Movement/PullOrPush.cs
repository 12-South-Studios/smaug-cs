using System;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Movement
{
    public static class PullOrPush
    {
        public static void pullorpush(CharacterInstance ch, ObjectInstance obj, bool pull)
        {
            bool isUp = obj.Value[0].IsSet((int)TriggerFlags.Up);
            if (CheckForValidObjectState(ch, obj, pull, isUp)) return;

            if (CheckAndFirePullProg(pull, obj, ch)) return;
            if (CheckAndFirePushProg(pull, obj, ch)) return;
            if (CheckAndFireOProgUseTrigger(pull, ch, obj)) return;

            if (!obj.Value[0].IsSet((int) TriggerFlags.AutoReturn))
            {
                if (pull)
                    obj.Value[0].RemoveBit((int) TriggerFlags.Up);
                else
                    obj.Value[0].SetBit((int) TriggerFlags.Up);
            }

            if (CheckAndFireTeleportTrigger(ch, obj)) return;
            if (CheckAndFireRandom(ch, obj)) return;
            if (CheckAndFireDeathTrigger(ch, obj)) return;
            if (CheckAndFireObjectLoading(ch, obj)) return;
            if (CheckAndFireMobLoading(ch, obj)) return;
            if (CheckAndFireSpell(ch, obj)) return;
            if (CheckAndFireContainer(ch, obj)) return;
            if (CheckAndFireDoor(ch, obj)) return;
        }

        private static bool CheckAndFireDoor(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.Door))
            {
                RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]) ?? obj.InRoom;
                if (room == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                Tuple<DirectionTypes, string> exitDir = GetExitDirectionAndText(obj);

                ExitData exit = room.GetExit(exitDir.Item1);
                if (exit == null)
                {
                    CreateNewPassage(ch, obj, exitDir.Item1);
                    return true;
                }

                if (obj.Value[0].IsSet((int) TriggerFlags.Unlock)
                    && exit.Flags.IsSet((int) ExitFlags.Locked))
                {
                    UnlockExit(ch, exit, exitDir.Item2);
                    return true;
                }

                if (obj.Value[0].IsSet((int) TriggerFlags.Lock)
                    && !exit.Flags.IsSet((int) ExitFlags.Locked))
                {
                    LockExit(ch, exit, exitDir.Item2);
                    return true;
                }

                if (obj.Value[0].IsSet((int) TriggerFlags.Open)
                    && exit.Flags.IsSet((int) ExitFlags.Closed))
                {
                    OpenExit(ch, obj, exit, exitDir.Item2);
                    return true;
                }

                if (obj.Value[0].IsSet((int) TriggerFlags.Close)
                    && !exit.Flags.IsSet((int) ExitFlags.Closed))
                {
                    CloseExit(ch, obj, exit, exitDir.Item2);
                    return true;
                }
            }
            return false;
        }

        private static Tuple<DirectionTypes, string> GetExitDirectionAndText(ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int)TriggerFlags.D_North))
                return new Tuple<DirectionTypes, string>(DirectionTypes.North, "to the north");
            
            if (obj.Value[0].IsSet((int)TriggerFlags.D_South))
                return new Tuple<DirectionTypes, string>(DirectionTypes.South, "to the south");
            
            if (obj.Value[0].IsSet((int)TriggerFlags.D_East))
                return new Tuple<DirectionTypes, string>(DirectionTypes.East, "to the east");
            
            if (obj.Value[0].IsSet((int)TriggerFlags.D_West))
                return new Tuple<DirectionTypes, string>(DirectionTypes.West, "to the west");
            
            if (obj.Value[0].IsSet((int)TriggerFlags.D_Up))
                return new Tuple<DirectionTypes, string>(DirectionTypes.Up, "from above");
            
            if (obj.Value[0].IsSet((int)TriggerFlags.D_Down))
                return new Tuple<DirectionTypes, string>(DirectionTypes.Down, "from below");
            
            // TODO Exception, log it!
            return null;
        }

        private static void CreateNewPassage(CharacterInstance ch, ObjectInstance obj, DirectionTypes exitDir)
        {
            if (!obj.Value[0].IsSet((int) TriggerFlags.Passage))
            {
                // TODO Exception, log it!
                return;
            }

            RoomTemplate sourceRoom = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]);
            if (sourceRoom == null)
            {
                // TODO Exception, log it!
                return;
            }
            
            RoomTemplate destRoom = DatabaseManager.Instance.ROOMS.Get(obj.Value[2]);
            if (destRoom == null)
            {
                // TODO Exception, log it!
                return;
            }

            ExitData exit = db.make_exit(sourceRoom, destRoom, (int)exitDir);
            exit.Key = -1;
            comm.act(ATTypes.AT_PLAIN, "A passage opens!", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_PLAIN, "A passage opens!", ch, null, null, ToTypes.Room);
        }

        private static void UnlockExit(CharacterInstance ch, ExitData exit, string txt)
        {
            exit.Flags.RemoveBit((int) ExitFlags.Locked);
            comm.act(ATTypes.AT_PLAIN, "You hear a faint click $T.", ch, null, txt, ToTypes.Character);
            comm.act(ATTypes.AT_PLAIN, "You hear a faint click $T.", ch, null, txt, ToTypes.Room);
            exit.RemoveFlagFromSelfAndReverseExit(ExitFlags.Locked);
        }

        private static void LockExit(CharacterInstance ch, ExitData exit, string txt)
        {
            exit.Flags.SetBit((int)ExitFlags.Locked);
            comm.act(ATTypes.AT_PLAIN, "You hear a faint click $T.", ch, null, txt, ToTypes.Character);
            comm.act(ATTypes.AT_PLAIN, "You hear a faint click $T.", ch, null, txt, ToTypes.Room);
            exit.SetFlagOnSelfAndReverseExit(ExitFlags.Locked);
        }

        private static void OpenExit(CharacterInstance ch, ObjectInstance obj, ExitData exit, string txt)
        {
            exit.Flags.RemoveBit((int) ExitFlags.Closed);

            RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]);
            if (room == null)
            {
                // TODO Exception, log it!
                return;
            }

            foreach(CharacterInstance rch in room.Persons)
                comm.act(ATTypes.AT_ACTION, "The $d opens.", rch, null, exit.Keywords, ToTypes.Character);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit.Destination == exit.Destination)
            {
                reverseExit.Flags.RemoveBit((int) ExitFlags.Closed);

                RoomTemplate destRoom = exit.GetDestination(DatabaseManager.Instance);
                foreach(CharacterInstance rch in destRoom.Persons)
                    comm.act(ATTypes.AT_ACTION, "The $d opens.", rch, null, reverseExit.Keywords, ToTypes.Character);
            }

            // TODO Check room for traps
        }

        private static void CloseExit(CharacterInstance ch, ObjectInstance obj, ExitData exit, string txt)
        {
            exit.Flags.SetBit((int) ExitFlags.Closed);

            RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]);
            if (room == null)
            {
                // TODO Exception, log it!
                return;
            }

            foreach (CharacterInstance rch in room.Persons)
                comm.act(ATTypes.AT_ACTION, "The $d closes.", rch, null, exit.Keywords, ToTypes.Character);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit.Destination == exit.Destination)
            {
                reverseExit.Flags.SetBit((int)ExitFlags.Closed);

                RoomTemplate destRoom = exit.GetDestination(DatabaseManager.Instance);
                foreach (CharacterInstance rch in destRoom.Persons)
                    comm.act(ATTypes.AT_ACTION, "The $d closes.", rch, null, reverseExit.Keywords, ToTypes.Character);
            }

            // TODO Check room for traps
        }

        private static bool CheckAndFireContainer(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.Container))
            {
                RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]) ?? obj.InRoom;
                if (room == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                ObjectInstance container =
                    ch.CurrentRoom.Contents.FirstOrDefault(foundObj => foundObj.ObjectIndex.ID == obj.Value[2]);

                if (container == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                if (container.ItemType != ItemTypes.Container)
                {
                    // TODO Exception, log it!
                    return true;
                }

                if (obj.Value[3].IsSet((int) ContainerFlags.Closeable))
                    container.Value[1].ToggleBit((int) ContainerFlags.Closeable);
                if (obj.Value[3].IsSet((int)ContainerFlags.PickProof))
                    container.Value[1].ToggleBit((int)ContainerFlags.PickProof);
                if (obj.Value[3].IsSet((int)ContainerFlags.Closed))
                    container.Value[1].ToggleBit((int)ContainerFlags.Closed);
                if (obj.Value[3].IsSet((int)ContainerFlags.Locked))
                    container.Value[1].ToggleBit((int)ContainerFlags.Locked);
                if (obj.Value[3].IsSet((int)ContainerFlags.EatKey))
                    container.Value[1].ToggleBit((int)ContainerFlags.EatKey);

                return true;
            }
            return false;
        }

        private static bool CheckAndFireSpell(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.Cast))
            {
                if (obj.Value[1] <= 0 || !Macros.IS_VALID_SN(obj.Value[1]))
                {
                    // TODO Exception, log it!
                    return true;
                }

                int minLevel = obj.Value[2] > 0 ? obj.Value[2] : ch.Level;
                magic.obj_cast_spell(obj.Value[1],
                    1.GetNumberThatIsBetween(minLevel, GameConstants.GetIntegerConstant("MaximumLevel")), ch, ch, null);
                return true;
            }
            return false;
        }

        private static bool CheckAndFireMobLoading(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.MobileLoad))
            {
                MobTemplate template = DatabaseManager.Instance.MOBILE_INDEXES.Get(obj.Value[1]);
                if (template == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                RoomTemplate room = ch.CurrentRoom;
                if (obj.Value[2] > 0)
                {
                    room = DatabaseManager.Instance.ROOMS.Get(obj.Value[2]);
                    if (room == null)
                    {
                        // TODO Exception, log it!
                        return true;
                    }
                }

                CharacterInstance instance = DatabaseManager.Instance.CHARACTERS.Create(template);
                if (instance == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                room.ToRoom(instance);
                return true;
            }
            return false;
        }

        private static bool CheckForValidObjectState(CharacterInstance ch, ObjectInstance obj, bool pull, bool isUp)
        {
            if (obj.ItemType == ItemTypes.Switch
                || obj.ItemType == ItemTypes.Lever
                || obj.ItemType == ItemTypes.PullChain
                || obj.ItemType == ItemTypes.Button)
            {
                if ((!pull && isUp) || (pull && !isUp))
                {
                    string state = string.Empty;
                    if (obj.ItemType == ItemTypes.Button)
                        state = isUp ? "in" : "out";
                    else
                        state = isUp ? "up" : "down";
                    color.ch_printf(ch, "It is already %s.", state);
                    return true;
                }
            }
            else
            {
                color.ch_printf(ch, "You can't %s that!", pull ? "pull" : "push");
                return true;
            }
            return true;
        }

        private static bool CheckAndFireObjectLoading(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.ObjectLoad))
            {
                ObjectTemplate template = DatabaseManager.Instance.OBJECT_INDEXES.Get(obj.Value[1]);
                if (template == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                RoomTemplate room = null;
                if (obj.Value[2] > 0)
                {
                    room = DatabaseManager.Instance.ROOMS.Get(obj.Value[2]);
                    if (room == null)
                    {
                        // TODO Exception, log it!
                        return true;
                    }
                }

                int level = 0.GetNumberThatIsBetween(obj.Value[3], GameConstants.GetIntegerConstant("MaximumLevel"));
                ObjectInstance instance = DatabaseManager.Instance.OBJECTS.Create(template, level);
                if (instance == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                if (room != null)
                    room.ToRoom(instance);
                else
                {
                    if (Macros.CAN_WEAR(obj, (int) ItemWearFlags.Take))
                        instance.ToCharacter(ch);
                    else
                        ch.CurrentRoom.ToRoom(instance);
                }
                return true;
            }
            return false;
        }

        private static bool CheckAndFireDeathTrigger(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.Death))
            {
                comm.act(ATTypes.AT_DEAD, "$n falls prey to a terrible death!", ch, null, null, ToTypes.Room);
                comm.act(ATTypes.AT_DEAD, "Oopsie... you're dead!", ch, null, null, ToTypes.Character);

                // TODO Log a death trigger to Monitor channel?

                handler.set_cur_char(ch);
                fight.raw_kill(ch, ch);
                return true;
            }
            return false;
        }

        private static bool CheckAndFireRandom(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.Rand4)
                || obj.Value[0].IsSet((int) TriggerFlags.Rand6))
            {
                RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]);
                if (room == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                int maxd = obj.Value[0].IsSet((int) TriggerFlags.Rand4) ? 3 : 5;

                db.randomize_exits(room, maxd);
                foreach (CharacterInstance rch in room.Persons)
                {
                    color.send_to_char("You hear a loud rumbling sound.", rch);
                    color.send_to_char("Something seems different...", rch);
                }
                return true;
            }
            return false;
        }

        private static bool CheckAndFireTeleportTrigger(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Value[0].IsSet((int) TriggerFlags.Teleport)
                || obj.Value[0].IsSet((int) TriggerFlags.TeleportAll))
            {
                RoomTemplate room = DatabaseManager.Instance.ROOMS.Get(obj.Value[1]);
                if (room == null)
                {
                    // TODO Exception, log it!
                    return true;
                }

                int flags = 0;
                if (obj.Value[0].IsSet((int) TriggerFlags.ShowRoomDescription))
                    flags.SetBit((int) TeleportTriggerFlags.ShowDescription);
                if (obj.Value[0].IsSet((int) TriggerFlags.TeleportAll))
                    flags.SetBit((int) TeleportTriggerFlags.TransportAll);
                if (obj.Value[0].IsSet((int) TriggerFlags.TeleportPlus))
                    flags.SetBit((int) TeleportTriggerFlags.TransportAllPlus);

               act_move.teleport(ch, obj.Value[1], flags);
                return true;
            }
            return false;
        }

        private static bool CheckAndFirePullProg(bool pull, ObjectInstance obj, CharacterInstance ch)
        {
            if (pull && obj.ObjectIndex.HasProg(MudProgTypes.Pull))
            {
                if (obj.Value[0].IsSet((int) TriggerFlags.AutoReturn))
                    obj.Value[0].RemoveBit((int) TriggerFlags.Up);
                mud_prog.oprog_pull_trigger(ch, obj);
                return true;
            }
            return false;
        }

        private static bool CheckAndFirePushProg(bool push, ObjectInstance obj, CharacterInstance ch)
        {
            if (!push && obj.ObjectIndex.HasProg(MudProgTypes.Push))
            {
                if (obj.Value[0].IsSet((int) TriggerFlags.AutoReturn))
                    obj.Value[0].SetBit((int) TriggerFlags.Up);
                mud_prog.oprog_push_trigger(ch, obj);
                return true;
            }
            return false;
        }

        private static bool CheckAndFireOProgUseTrigger(bool pull, CharacterInstance ch, ObjectInstance obj)
        {
            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, string.Format("$n {0} $p.", pull ? "pulls" : "pushes"), ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, string.Format("You {0} $p.", pull ? "pull" : "push"), ch, obj, null, ToTypes.Character);
                return true;
            }
            return false;
        }
    }
}
