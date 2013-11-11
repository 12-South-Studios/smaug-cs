using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class track
    {
        public static int BFS_ERROR = -1;
        public static int BFS_ALREADY_THERE = -2;
        public static int BFS_NO_PATH = -3;
        public static int BFS_MARK = (int)RoomFlags.BfsMark;

        public static Queue<BFSQueueData> BFS_DATA = new Queue<BFSQueueData>();
        public static List<BFSQueueData> room_queue = new List<BFSQueueData>();

        public static void MARK(RoomTemplate room)
        {
            room.Flags.IsSet(BFS_MARK);
        }

        public static void UNMARK(RoomTemplate room)
        {
            room.Flags.RemoveBit(BFS_MARK);
        }

        public static bool IS_MARKED(RoomTemplate room)
        {
            return room.Flags.IsSet(BFS_MARK);
        }

        public static bool valid_edge(ExitData exit)
        {
            return exit.Destination != null
#if !TRACK_THROUGH_DOORS
 && !exit.Flags.IsSet((int)ExitFlags.Closed)
#endif
 && !IS_MARKED(exit.GetDestination());
        }

        public static void bfs_enqueue(RoomTemplate room, char dir)
        {
            BFS_DATA.Enqueue(new BFSQueueData { Room = room, Dir = dir });
        }

        public static void bfs_dequeue()
        {
            BFS_DATA.Dequeue();
        }

        public static void bfs_clear_queue()
        {
            BFS_DATA.Clear();
        }

        public static void room_enqueue(RoomTemplate room)
        {
            room_queue.Add(new BFSQueueData { Room = room });
        }

        public static void clean_room_queue()
        {
            room_queue.Clear();
        }

        public static int find_first_step(RoomTemplate src, RoomTemplate target, int maxdist)
        {
            if (src == null || target == null)
            {
                LogManager.Bug("Illegal value passed to find_first_step");
                return BFS_ERROR;
            }

            if (src == target)
                return BFS_ALREADY_THERE;

            if (src.Area != target.Area)
                return BFS_NO_PATH;

            room_enqueue(src);
            MARK(src);

            int curr_dir = 0;
            foreach (ExitData exit in src.Exits.Where(valid_edge))
            {
                curr_dir = exit.vdir;
                MARK(exit.GetDestination());
                room_enqueue(exit.GetDestination());
                bfs_enqueue(exit.GetDestination(), Convert.ToChar(curr_dir));
            }

            int count = 0;

            BFSQueueData queueHead = BFS_DATA.Peek();
            while (queueHead != null)
            {
                if (++count > maxdist)
                {
                    bfs_clear_queue();
                    clean_room_queue();
                    return BFS_NO_PATH;
                }
                if (queueHead.Room == target)
                {
                    curr_dir = queueHead.Dir;
                    bfs_clear_queue();
                    clean_room_queue();
                    return curr_dir;
                }

                foreach (ExitData exit in queueHead.Room.Exits.Where(valid_edge))
                {
                    curr_dir = exit.vdir;
                    MARK(exit.GetDestination());
                    room_enqueue(exit.GetDestination());
                    bfs_enqueue(exit.GetDestination(), queueHead.Dir);
                }
                bfs_dequeue();
            }

            clean_room_queue();
            return BFS_NO_PATH;
        }

        public static void do_track(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void found_prey(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static void hunt_victim(CharacterInstance ch)
        {
            // TODO
        }
    }
}
