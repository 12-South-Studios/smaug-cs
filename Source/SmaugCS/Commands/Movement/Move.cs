using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using System.Collections.Generic;

namespace SmaugCS.Commands
{
    public static class Move
    {
        public static ReturnTypes move_char(CharacterInstance ch, ExitData pexit, int fall)
        {
            var drunk = false;
            var nuisance = false;
            string txt;

            if (!ch.IsNpc())
            {
                var pch = (PlayerInstance)ch;

                if (ch.IsDrunk(2) && ch.CurrentPosition != PositionTypes.Shove
                    && ch.CurrentPosition != PositionTypes.Drag)
                    drunk = true;

                if (pch.PlayerData.Nuisance != null && pch.PlayerData.Nuisance.Flags > 8
                    && ch.CurrentPosition != PositionTypes.Shove
                    && ch.CurrentPosition != PositionTypes.Drag
                    && SmaugRandom.D100() > pch.PlayerData.Nuisance.Flags * pch.PlayerData.Nuisance.Power)
                    nuisance = true;
            }

            int door;
            ExitData exit = null;

            // Nuisance flag, makes them walk in random directions 50% of the time
            if ((nuisance || drunk) && fall == 0)
            {
                door = db.number_door();
                exit = ch.CurrentRoom.GetExit(door);
            }

#if DEBUG
            if (exit != null)
            {
                Program.LogManager.Info("{0} to door {1}", ch.Name, pexit.Direction);
            }
#endif

            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Mounted))
                return ReturnTypes.None;

            var inRoom = ch.CurrentRoom;
            var fromRoom = inRoom;
            RoomTemplate toRoom = null;

            if (exit?.Destination == null)
            {
                if (drunk && ch.CurrentPosition != PositionTypes.Mounted
                    && ch.CurrentRoom.SectorType != SectorTypes.ShallowWater
                    && ch.CurrentRoom.SectorType != SectorTypes.DeepWater
                    && ch.CurrentRoom.SectorType != SectorTypes.Underwater
                    && ch.CurrentRoom.SectorType != SectorTypes.OceanFloor)
                {
                    switch (SmaugRandom.Bits(4))
                    {
                        default:
                            comm.act(ATTypes.AT_ACTION, "You drunkenly stumble into some obstacle.", ch, null, null, ToTypes.Character);
                            comm.act(ATTypes.AT_ACTION, "$n drunkenly stumbles into a nearby obstacle.", ch, null, null, ToTypes.Room);
                            break;
                        case 3:
                            comm.act(ATTypes.AT_ACTION, "In your drunken stupor you trip over your own feet and tumble to the ground.", ch, null, null, ToTypes.Character);
                            comm.act(ATTypes.AT_ACTION, "$n stumbles drunkenly, trips and tumbles to the ground.", ch, null, null, ToTypes.Room);
                            ch.CurrentPosition = PositionTypes.Resting;
                            break;
                        case 4:
                            comm.act(ATTypes.AT_SOCIAL, "You utter a string of slurred obscenities.", ch, null, null, ToTypes.Character);
                            comm.act(ATTypes.AT_ACTION, "Something blurry and immovable has intercepted you as you stagger along.", ch, null, null, ToTypes.Character);
                            comm.act(ATTypes.AT_HURT, "Oh geez... THAT really hurt.  Everything slowly goes dark and numb...", ch, null, null, ToTypes.Character);
                            comm.act(ATTypes.AT_ACTION, "$n drunkenly staggers into something.", ch, null, null, ToTypes.Room);
                            comm.act(ATTypes.AT_SOCIAL, "$n utters a string of slurred obscenities: @*&^%@*&!", ch, null, null, ToTypes.Room);
                            comm.act(ATTypes.AT_ACTION, "$n topples to the ground with a thud.", ch, null, null, ToTypes.Room);
                            ch.CurrentPosition = PositionTypes.Incapacitated;
                            break;
                    }
                }
                else if (nuisance)
                    comm.act(ATTypes.AT_ACTION, "You stare around trying to remember where you where going.", ch, null, null, ToTypes.Character);
                else if (drunk)
                    comm.act(ATTypes.AT_ACTION, "You stare around trying to make sense of things through your drunken stupor.", ch, null, null, ToTypes.Character);
                else
                    ch.SendTo("Alas, you cannot go that way.\r\n");
                return ReturnTypes.None;
            }

            door = (int)exit.Direction;
            var distance = exit.Distance;

            // Exit is only a "window", there is no way to travel in that direction unless it's a door with a window in it 
            if (CheckFunctions.CheckIfTrue(ch, exit.Flags.IsSet(ExitFlags.Window) && !exit.Flags.IsSet(ExitFlags.IsDoor),
                "Alas, you cannot go that way.")) return ReturnTypes.None;

            if (ch.IsNpc())
            {
                if (exit.Flags.IsSet(ExitFlags.Portal))
                {
                    comm.act(ATTypes.AT_PLAIN, "Mobs can't use portals.", ch, null, null, ToTypes.Character);
                    return ReturnTypes.None;
                }

                if (exit.Flags.IsSet(ExitFlags.NoMob) || toRoom.Flags.IsSet(RoomFlags.NoMob))
                {
                    comm.act(ATTypes.AT_PLAIN, "Mobs can't enter there.", ch, null, null, ToTypes.Character);
                    return ReturnTypes.None;
                }
            }

            if (exit.Flags.IsSet(ExitFlags.Closed) && (!ch.IsAffected(AffectedByTypes.PassDoor) || exit.Flags.IsSet(ExitFlags.NoPassDoor)))
            {
                if (!exit.Flags.IsSet(ExitFlags.Secret) && !exit.Flags.IsSet(ExitFlags.Dig))
                {
                    if (drunk)
                    {
                        comm.act(ATTypes.AT_PLAIN, "$n runs into the $d in $s drunken state.", ch, null, null,
                                 ToTypes.Room);
                        comm.act(ATTypes.AT_PLAIN, "You run into the $d in your drunken state.", ch, null, null,
                                 ToTypes.Character);
                    }
                    else
                        comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, exit.Keywords,
                                 ToTypes.Character);
                }
                else
                {
                    ch.SendTo(drunk
                        ? "You stagger around in your drunken state.\r\n"
                        : "Alas, you cannot go that way.\r\n");
                }

                return ReturnTypes.None;
            }

            // Crazy virtual room idea, created upon demand.  
            if (distance > 1)
            {
                toRoom = act_move.generate_exit(inRoom, exit);
                if (toRoom == null)
                    ch.SendTo("Alas, you cannot go that way.");
            }

            if (CheckFunctions.CheckIfTrue(ch, fall == 0 && ch.IsAffected(AffectedByTypes.Charm)
                && ch.Master != null && inRoom == ch.Master.CurrentRoom, "What?  And leave your beloved master?"))
                return ReturnTypes.None;

            if (CheckFunctions.CheckIfTrue(ch, toRoom.IsPrivate(), "That room is private right now."))
                return ReturnTypes.None;

            if (CheckFunctions.CheckIfNotNullObject(ch, toRoom.IsDoNotDisturb(ch),
                "That room is \"do not disturb\" right now.")) return ReturnTypes.None;

            if (!ch.IsImmortal() && !ch.IsNpc()
                && ch.CurrentRoom.Area != toRoom.Area)
            {
                if (ch.Level < toRoom.Area.LowHardRange)
                {
                    ch.SetColor(ATTypes.AT_TELL);
                    switch (toRoom.Area.LowHardRange - ch.Level)
                    {
                        case 1:
                            ch.SendTo("A voice in your mind says, 'You are nearly ready to go that way...'");
                            break;
                        case 2:
                            ch.SendTo("A voice in your mind says, 'Soon you shall be ready to travel down this path... soon.'");
                            break;
                        case 3:
                            ch.SendTo("A voice in your mind says, 'You are not ready to go down that path... yet.'");
                            break;
                        default:
                            ch.SendTo("A voice in your mind says, 'You are not ready to go down that path.'");
                            break;
                    }

                    return ReturnTypes.None;
                }
                if (ch.Level > toRoom.Area.HighHardRange)
                {
                    ch.SetColor(ATTypes.AT_TELL);
                    ch.SendTo("A voice in your mind says, 'There is nothing more for you down that path.'");
                    return ReturnTypes.None;
                }
            }

            if (fall == 0 && !ch.IsNpc())
            {
                int move;

                // Prevent deadlies from entering a nopkill-flagged area from a non-flagged area, 
                // but allow them to move around if already inside a nopkill area.
                if (toRoom.Area.Flags.IsSet(AreaFlags.NoPlayerVsPlayer) && !ch.CurrentRoom.Area.Flags.IsSet(AreaFlags.NoPlayerVsPlayer)
                    && ch.IsPKill() && !ch.IsImmortal())
                {
                    ch.SetColor(ATTypes.AT_MAGIC);
                    ch.SendTo("\r\nA godly force forbids deadly characters from entering that area...");
                    return ReturnTypes.None;
                }

                if (inRoom.SectorType == SectorTypes.Air
                    || toRoom.SectorType == SectorTypes.Air
                    || exit.Flags.IsSet(ExitFlags.Fly))
                {
                    if (CheckFunctions.CheckIfTrue(ch,
                        ch.CurrentMount != null && !ch.CurrentMount.IsAffected(AffectedByTypes.Flying),
                        "Your mount can't fly.")) return ReturnTypes.None;
                    if (CheckFunctions.CheckIfTrue(ch, ch.CurrentMount == null && !ch.IsAffected(AffectedByTypes.Flying),
                        "You'd need to fly to go there.")) return ReturnTypes.None;
                }

                if (inRoom.SectorType == SectorTypes.DeepWater || toRoom.SectorType == SectorTypes.DeepWater)
                {
                    if ((ch.CurrentMount != null && !ch.CurrentMount.IsFloating()) || !ch.IsFloating())
                    {
                        // Look for a boat. We can use the boat obj for a more detailed description.
                        var boat = ch.GetObjectOfType(ItemTypes.Boat);
                        if (CheckFunctions.CheckIfNullObject(ch, boat,
                            ch.CurrentMount != null ? "Your mount would drown!" : "You'd need a boat to go there."))
                            return ReturnTypes.None;

                        txt = drunk ? "paddles unevenly" : "paddles";
                    }
                }

                if (exit.Flags.IsSet(ExitFlags.Climb))
                {
                    var found = false;
                    if (ch.CurrentMount != null && ch.CurrentMount.IsAffected(AffectedByTypes.Flying))
                        found = true;
                    else if (ch.IsAffected(AffectedByTypes.Flying))
                        found = true;

                    if (!found && ch.CurrentMount == null)
                    {
                        // TODO Climbing
                    }

                    if (CheckFunctions.CheckIfTrue(ch, !found, "You can't climb.")) return ReturnTypes.None;
                }

                if (ch.CurrentMount != null)
                {
                    if (PositionMoveMessage.ContainsKey(ch.CurrentMount.CurrentPosition))
                    {
                        ch.SendTo(PositionMoveMessage[ch.CurrentMount.CurrentPosition]);
                        return ReturnTypes.None;
                    }

                    if (!ch.CurrentMount.IsFloating())
                    {

                    }    // TODO
                }
            }

            return ReturnTypes.None;
        }

        private static readonly Dictionary<PositionTypes, string> PositionMoveMessage = new Dictionary
            <PositionTypes, string>
        {
            {PositionTypes.Dead, "Your mount is dead!\r\n"},
            {PositionTypes.Mortal, "Your mount is hurt far too badly to move.\r\n"},
            {PositionTypes.Incapacitated, "Your mount is hurt far too badly to move.\r\n"},
            {PositionTypes.Stunned, "Your mount is too stunned to do that.\r\n"},
            {PositionTypes.Sleeping, "Your mount is sleeping.\r\n"},
            {PositionTypes.Resting, "Your mount is resting.\r\n"},
            {PositionTypes.Sitting, "Your mount is sitting down.\r\n"}
        };

        public static void do_north(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.North), 0);
        }

        public static void do_east(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.East), 0);
        }

        public static void do_south(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.South), 0);
        }

        public static void do_west(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.West), 0);
        }

        public static void do_up(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Up), 0);
        }

        public static void do_down(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Down), 0);
        }

        public static void do_northeast(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Northeast), 0);
        }

        public static void do_northwest(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Northwest), 0);
        }

        public static void do_southeast(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Southeast), 0);
        }

        public static void do_southwest(CharacterInstance ch, string argument)
        {
            move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Southwest), 0);
        }
    }
}
