﻿using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class act_move
    {
        public static string wordwrap(string txt, short wrap)
        {
            if (string.IsNullOrEmpty(txt))
                return string.Empty;

            // TODO do some crazy line wrap

            return string.Empty;
        }

        private static string GetDecorateRoom_PreAndPost_1(int iRand, int nRand, SectorTypes sector, int x)
        {
            string pre, post;
            int result = SmaugRandom.Between(1, 2 * (iRand == (nRand - 1) ? 1 : 2));
            if (result <= 2)
            {
                post = ".";
                pre = result == 1 ? "You notice " : "You see ";
            }
            else
            {
                post = ", and ";
                pre = result == 3 ? "You see " : "You notice ";
            }

            return string.Format("{0}{1}{2}", pre, LookupConstants.RoomSents[(int)sector][x], post);
        }
        private static string GetDecorateRoom_PreAndPost_2(SectorTypes sector, int x)
        {
            string pre, post;
            int random = SmaugRandom.Between(0, 3);
            if (random == 0 || random == 2)
            {
                post = ".";
                pre = random == 0 ? "you notice " : "you see ";
            }
            else
            {
                post = ", and ";
                pre = random == 1 ? "you see " : "over yonder ";
            }

            return string.Format("{0}{1}{2}", pre, LookupConstants.RoomSents[(int)sector][x], post);
        }
        private static string GetDecorateRoom_PreAndPost_3(SectorTypes sector, int x)
        {
            string[] outputArray = { ".", " not too far away.", ", and ", " nearby." };

            return string.Format("{0}{1}{2}", string.Empty, LookupConstants.RoomSents[(int)sector][x], outputArray[SmaugRandom.Between(0, 3)]);
        }

        public static void decorate_room(RoomTemplate room)
        {
            string buf = string.Empty;
            string buf2 = string.Empty;
            int[] previous = new int[8];

            //room.Name = "In a virtual room";
            room.Description = "You're on a pathway.\r\n";

            SectorTypes sector = room.SectorType;
            //room.Name = GameConstants.SectorNames[(int)sector].Key;
            int nRand = SmaugRandom.Between(1, 8.GetLowestOfTwoNumbers(LookupConstants.SentTotals[(int)sector]));

            for (int iRand = 0; iRand < nRand; iRand++)
                previous[iRand] = -1;

            for (int iRand = 0; iRand < nRand; iRand++)
            {
                while (previous[iRand] == -1)
                {
                    int x = SmaugRandom.Between(0, LookupConstants.SentTotals[(int)sector] - 1);

                    int z;
                    for (z = 0; z < iRand; z++)
                        if (previous[z] == x)
                            break;

                    if (z < iRand)
                        continue;

                    previous[iRand] = x;
                    int len = buf.Length;
                    if (len == 0)
                        buf2 = GetDecorateRoom_PreAndPost_1(iRand, nRand, sector, x);
                    else if (iRand != (nRand - 1))
                        buf2 = buf.EndsWith(".")
                            ? GetDecorateRoom_PreAndPost_2(sector, x)
                            : GetDecorateRoom_PreAndPost_3(sector, x);
                    else
                        buf2 = string.Format("{0}.", LookupConstants.RoomSents[(int)sector][x]);

                    if (len > 5 && buf2.EndsWith("."))
                    {
                        buf += "  ";
                        buf2 = buf2.CapitalizeFirst();
                    }
                    else if (len == 0)
                        buf2 = buf2.CapitalizeFirst();

                    buf += buf2;
                }
            }

            room.Description = string.Format("{0}\r\n", wordwrap(buf, 78));
        }

        public static string rev_exit(int vdir)
        {
            return LookupManager.Instance.GetLookup("ReverseDirectionNames", vdir);
        }

        public static RoomTemplate generate_exit(RoomTemplate room, ExitData exit)
        {
            long serial;
            long roomnum;
            long brvnum;
            long distance = -1;
            RoomTemplate backroom;
            int vdir = (int)exit.Direction;

            if (room.Vnum > 32767)
            {
                serial = room.Vnum;
                roomnum = room.TeleportToVnum;
                if ((serial & 65535) == exit.vnum)
                {
                    brvnum = serial >> 16;
                    --roomnum;
                    distance = roomnum;
                }
                else
                {
                    brvnum = serial & 65535;
                    ++roomnum;
                    distance = exit.Distance - 1;
                }
                backroom = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(brvnum);
            }
            else
            {
                long r1 = room.Vnum;
                long r2 = exit.vnum;

                brvnum = r1;
                backroom = room;
                serial = (r1.GetHighestOfTwoNumbers(r2) << 16) | r1.GetLowestOfTwoNumbers(r2);
                distance = exit.Distance - 1;
                roomnum = r1 < r2 ? 1 : distance;
            }

            bool found = false;

            RoomTemplate foundRoom =
                DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values.FirstOrDefault(
                    x => x.Vnum == serial && x.TeleportToVnum == roomnum);
            if (foundRoom != null)
                found = true;

            // Create room in direction
            RoomTemplate newRoom = null;
            if (!found)
            {
                newRoom = RoomTemplate.Create(serial, "New room");
                newRoom.Area = room.Area;
                newRoom.TeleportToVnum = roomnum;
                newRoom.SectorType = room.SectorType;
                newRoom.Flags = room.Flags;
                decorate_room(newRoom);
                DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Add(newRoom.Vnum, newRoom);
            }

            ExitData xit = newRoom.GetExit(vdir);
            if (!found || xit == null)
            {
                //xit = db.make_exit(newRoom, exit.GetDestination(), vdir);
                xit.Key = -1;
                xit.Distance = (int)distance;
            }

            if (!found)
            {
                //ExitData bxit = db.make_exit(newRoom, backroom, GameConstants.rev_dir[vdir]);
                ExitData bxit = null;
                bxit.Key = -1;
                if ((serial & 65536) != exit.vnum)
                    bxit.Distance = (int)roomnum;
                else
                {
                    ExitData tmp = backroom.GetExit(vdir);
                    bxit.Distance = tmp.Distance - (int)distance;
                }
            }

            return newRoom;
        }

        private static readonly Dictionary<string, int> DoorDirectionMap = new Dictionary<string, int>()
            {
                {"n;north", 0},
                {"e;east", 1},
                {"s;south", 2},
                {"w;west", 3},
                {"u;up", 4},
                {"d;down", 5},
                {"ne;northeast", 6},
                {"nw;northwest", 7},
                {"se;southeast", 8},
                {"sw;southwest", 9}
            };

        public static ExitData find_door(CharacterInstance ch, string arg, bool quiet)
        {
            if (string.IsNullOrEmpty(arg))
                return null;

            int door = (from key in DoorDirectionMap.Keys let words = key.Split(';') where words.Any(x => x.EqualsIgnoreCase(arg)) select DoorDirectionMap[key]).FirstOrDefault();

            if (door == 0)
            {
                foreach (ExitData pexit in ch.CurrentRoom.Exits)
                {
                    if ((quiet || pexit.Flags.IsSet((int)ExitFlags.IsDoor))
                        && !string.IsNullOrEmpty(pexit.Keywords)
                        && arg.IsAnyEqual(pexit.Keywords))
                        return pexit;
                }

                if (!quiet)
                    comm.act(ATTypes.AT_PLAIN, "You see no $T here.", ch, null, arg, ToTypes.Character);
                return null;
            }

            ExitData exit = ch.CurrentRoom.GetExit(door);
            if (exit == null)
            {
                if (!quiet)
                    comm.act(ATTypes.AT_PLAIN, "You see no $T here.", ch, null, arg, ToTypes.Character);
                return null;
            }

            if (quiet)
                return exit;

            if (exit.Flags.IsSet((int)ExitFlags.Secret))
            {
                comm.act(ATTypes.AT_PLAIN, "You see no $T here.", ch, null, arg, ToTypes.Character);
                return null;
            }

            if (!exit.Flags.IsSet((int)ExitFlags.IsDoor))
            {
                color.send_to_char("You can't do that.\r\n", ch);
                return null;
            }

            return exit;
        }

        public static void teleportch(CharacterInstance ch, RoomTemplate room, bool show)
        {
            if (room.IsPrivate())
                return;

            comm.act(ATTypes.AT_ACTION, "$n disappears suddenly!", ch, null, null, ToTypes.Room);
            ch.CurrentRoom.FromRoom(ch);
            room.ToRoom(ch);
            comm.act(ATTypes.AT_ACTION, "$n arrives suddenly!", ch, null, null, ToTypes.Room);

            if (show)
                Look.do_look(ch, "auto");

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Death)
                && !ch.IsImmortal())
            {
                comm.act(ATTypes.AT_DEAD, "$n falls prey to a terrible death!", ch, null, null, ToTypes.Room);
                color.set_char_color(ATTypes.AT_DEAD, ch);
                color.send_to_char("Oopsie... you're dead!\r\n", ch);

                string buffer = string.Format("{0} hit a DEATH TRAP in room {1}!", ch.Name, ch.CurrentRoom.Vnum);
                //log_string(buffer);
                ChatManager.to_channel(buffer, ChannelTypes.Monitor, "Monitor", (short)LevelConstants.ImmortalLevel);
                handler.extract_char(ch, false);
            }
        }

        public static void teleport(CharacterInstance ch, int room, int flags)
        {
            RoomTemplate dest = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(room);
            if (dest == null)
            {
                LogManager.Instance.Bug("bad room vnum {0}", room);
                return;
            }

            bool show = flags.IsSet((int)TeleportTriggerFlags.ShowDescription);

            if (!flags.IsSet((int)TeleportTriggerFlags.TransportAll))
            {
                teleportch(ch, dest, show);
                return;
            }

            RoomTemplate start = ch.CurrentRoom;
            foreach (CharacterInstance nch in start.Persons)
                teleportch(nch, dest, show);

            if (flags.IsSet((int)TeleportTriggerFlags.TransportAllPlus))
            {
                foreach (ObjectInstance obj in start.Contents)
                {
                    obj.InRoom.FromRoom(obj);
                    dest.ToRoom(obj);
                }
            }
        }

        public static ReturnTypes pullcheck(CharacterInstance ch, int pulse)
        {
            if (ch.CurrentRoom == null)
            {
                LogManager.Instance.Bug("{0} not in a room?!?", ch.Name);
                return ReturnTypes.None;
            }

            ExitData xit = null;
            foreach (ExitData exit in ch.CurrentRoom.Exits)
            {
                if (exit.Pull > 0 && exit.Destination != null
                    && (xit == null || Math.Abs(exit.Pull) > Math.Abs(xit.Pull)))
                    xit = exit;
            }

            if (xit == null)
                return ReturnTypes.None;

            int pull = xit.Pull;
            int pullfact = (20 - (Math.Abs(pull) / 5)).GetNumberThatIsBetween(1, 20);

            if ((pulse % pullfact) != 0)
            {
                foreach (ExitData exit in ch.CurrentRoom.Exits
                    .Where(exit => exit.Pull > 0 && exit.Destination != null))
                {
                    pull = exit.Pull;
                    pullfact = (20 - (Math.Abs(pull) / 5)).GetNumberThatIsBetween(1, 20);
                    if ((pulse % pullfact) == 0)
                        break;
                }
            }

            if (xit.Flags.IsSet((int)ExitFlags.Closed))
                return ReturnTypes.None;

            if (xit.GetDestination().Tunnel > 0)
            {
                int count = ch.CurrentMount != null ? 1 : 0;

                if (xit.GetDestination().Persons.Any(ctmp => ++count >= xit.GetDestination().Tunnel))
                    return ReturnTypes.None;
            }

            if (pull < 0)
            {
                xit = ch.CurrentRoom.GetExit(LookupConstants.rev_dir[(int)xit.Direction]);
                if (xit == null)
                    return ReturnTypes.None;
            }

            string dtxt = rev_exit((int)xit.Direction);

            // First determine if the player should be moved or not Check various flags, spells, 
            // the players position and strength vs. the pull, etc... any kind of checks you like.
            bool move = false;
            switch (xit.PullType)
            {
                case DirectionPullTypes.Current:
                case DirectionPullTypes.Whirlpool:
                    if (xit.PullType == DirectionPullTypes.Current)
                        break;

                    switch (ch.CurrentRoom.SectorType)
                    {
                        // allow whirlpool to be in any sector type
                        case SectorTypes.ShallowWater:
                        case SectorTypes.DeepWater:
                            if ((ch.CurrentMount != null
                                 && !ch.CurrentMount.IsFloating())
                                || (ch.CurrentMount == null
                                    && !ch.IsFloating()))
                                move = true;
                            break;
                        case SectorTypes.Underwater:
                        case SectorTypes.OceanFloor:
                            move = true;
                            break;
                    }
                    break;
                case DirectionPullTypes.Geyser:
                case DirectionPullTypes.Wave:
                    move = true;
                    break;
                case DirectionPullTypes.Wind:
                case DirectionPullTypes.Storm:
                    // if not flying, check weight, position and strength
                    move = true;
                    break;
                case DirectionPullTypes.ColdWind:
                    // if not flying, check weight, position and strength
                    // also check for damage due to bitter cold
                    move = true;
                    break;
                case DirectionPullTypes.HotAir:
                    // if not flying, check weight, position and strength
                    // also check for damage due to heat
                    move = true;
                    break;
                case DirectionPullTypes.Breeze:
                    move = false;
                    break;
                case DirectionPullTypes.Earthquake:
                case DirectionPullTypes.Sinkhole:
                case DirectionPullTypes.Quicksand:
                case DirectionPullTypes.Landslide:
                case DirectionPullTypes.Slip:
                case DirectionPullTypes.Lava:
                    if ((ch.CurrentMount != null && !ch.CurrentMount.IsFloating())
                        || (ch.CurrentMount == null && !ch.IsFloating()))
                        move = true;
                    break;
                case DirectionPullTypes.Undefined:
                    // as if player moved in that direction him/herself
                    return Move.move_char(ch, xit, 0);
                default:
                    move = true;
                    break;
            }

            bool showroom = xit.PullType == DirectionPullTypes.Mysterious;
            PullcheckMessages msg = GetPullcheckMessages(pull, xit.PullType);

            if (move)
            {
                if (!string.IsNullOrEmpty(msg.ToChar))
                {
                    comm.act(ATTypes.AT_PLAIN, msg.ToChar, ch, null,
                             LookupManager.Instance.GetLookup("DirectionNames", (int) xit.Direction), ToTypes.Character);
                    color.send_to_char("\r\n", ch);
                }
                if (!string.IsNullOrEmpty(msg.ToRoom))
                    comm.act(ATTypes.AT_PLAIN, msg.ToRoom, ch, null,
                             LookupManager.Instance.GetLookup("DirectionNames", (int) xit.Direction), ToTypes.Room);

                if (!string.IsNullOrEmpty(msg.DestRoom)
                                          && xit.GetDestination().Persons.Any())
                {
                    comm.act(ATTypes.AT_PLAIN, msg.DestRoom, xit.GetDestination().Persons.First(), null, dtxt, ToTypes.Character);
                    comm.act(ATTypes.AT_PLAIN, msg.DestRoom, xit.GetDestination().Persons.First(), null, dtxt, ToTypes.Room);
                }

                if (xit.PullType == DirectionPullTypes.Slip)
                    return Move.move_char(ch, xit, 1);

                ch.CurrentRoom.FromRoom(ch);
                xit.GetDestination().ToRoom(ch);

                if (showroom)
                    Look.do_look(ch, "auto");

                if (ch.CurrentMount != null)
                {
                    ch.CurrentMount.CurrentRoom.FromRoom(ch.CurrentMount);
                    xit.GetDestination().ToRoom(ch.CurrentMount);
                    if (showroom)
                        Look.do_look(ch.CurrentMount, "auto");
                }
            }

            foreach (ObjectInstance obj in ch.CurrentRoom.Contents)
            {
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Buried)
                    || !obj.WearFlags.IsSet(ItemWearFlags.Take))
                    continue;

                int resistance = obj.GetObjectWeight();
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Metallic))
                    resistance = (resistance * 6) / 5;

                switch (obj.ItemType)
                {
                    case ItemTypes.Scroll:
                    case ItemTypes.Note:
                    case ItemTypes.Trash:
                        resistance >>= 2;
                        break;
                    case ItemTypes.Scraps:
                    case ItemTypes.Container:
                        resistance >>= 1;
                        break;
                    case ItemTypes.Pen:
                    case ItemTypes.Wand:
                        resistance = (resistance * 5) / 6;
                        break;
                    case ItemTypes.PlayerCorpse:
                    case ItemTypes.NpcCorpse:
                    case ItemTypes.Fountain:
                        resistance <<= 2;
                        break;
                }

                if ((Math.Abs(pull) * 10) > resistance)
                {
                    if (!string.IsNullOrEmpty(msg.ObjMsg)
                        && ch.CurrentRoom.Persons.Any())
                    {
                        comm.act(ATTypes.AT_PLAIN, msg.ObjMsg, ch.CurrentRoom.Persons.First(), obj,
                                 LookupManager.Instance.GetLookup("DirectionNames", (int) xit.Direction),
                                 ToTypes.Character);
                        comm.act(ATTypes.AT_PLAIN, msg.ObjMsg, ch.CurrentRoom.Persons.First(), obj,
                                 LookupManager.Instance.GetLookup("DirectionNames", (int) xit.Direction), ToTypes.Room);
                    }

                    if (!string.IsNullOrEmpty(msg.DestObj) && ch.CurrentRoom.Persons.Any())
                    {
                        comm.act(ATTypes.AT_PLAIN, msg.DestObj, xit.GetDestination().Persons.First(), obj, dtxt, ToTypes.Character);
                        comm.act(ATTypes.AT_PLAIN, msg.DestObj, xit.GetDestination().Persons.First(), obj, dtxt, ToTypes.Room);
                    }

                    obj.InRoom.FromRoom(obj);
                    xit.GetDestination().ToRoom(obj);
                }
            }

            return ReturnTypes.None;
        }

        private static PullcheckMessages GetPullcheckMessages(int pull, DirectionPullTypes pulltype)
        {
            PullcheckAttribute attrib = pulltype.GetAttribute<PullcheckAttribute>();
            if (attrib != null)
                return new PullcheckMessages
                {
                    ToChar = attrib.ToChar,
                    ToRoom = attrib.ToRoom,
                    DestRoom = attrib.DestRoom,
                    ObjMsg = attrib.ObjMsg,
                    DestObj = attrib.DestObj
                };

            return new PullcheckMessages
            {
                ToChar = pull > 0 ? "You are pulled $T!" : "You are pushed $T!",
                ToRoom = pull > 0 ? "$n is pulled $T." : "$n is pushed $T.",
                DestRoom = pull > 0 ? "$n is pulled in from $T." : "$n is pushed in from $T.",
                ObjMsg = pull > 0 ? "$p is pulled $T." : "$p is pushed $T.",
                DestObj = pull > 0 ? "$p is pulled in from $T." : "$p is pushed in from $T."
            };
        }

        private class PullcheckMessages
        {
            public string ToChar { get; set; }
            public string ToRoom { get; set; }
            public string DestRoom { get; set; }
            public string ObjMsg { get; set; }
            public string DestObj { get; set; }
        }
    }
}