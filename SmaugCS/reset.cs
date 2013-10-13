using System.Collections.Generic;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class reset
    {
        public static ObjectInstance get_obj_type(ObjectTemplate pObjIndex)
        {
            // TODO
            return null;
        }

        public static ObjectInstance get_obj_room(ObjectTemplate pObjIndex, RoomTemplate pRoomIndex)
        {
            // TODO
            return null;
        }

        public static string sprint_reset(ResetData pReset, short num)
        {
            // TODO
            return string.Empty;
        }

        public static ResetData make_reset(char letter, int extra, int arg1, int arg2, int arg3)
        {
            // TODO
            return null;
        }

        public static void add_obj_reset(RoomTemplate room, char cm, ObjectInstance obj, int v2, int v3)
        {
            // TODO
        }

        public static void delete_reset(ResetData pReset)
        {
            // TODO
        }

        public static void instaroom(RoomTemplate pRoom, bool dodoors)
        {
            // TODO
        }

        public static void wipe_resets(RoomTemplate room)
        {
            // TODO
        }

        public static void wipe_area_resets(AreaData area)
        {
            // TODO
        }

        public static void do_instaroom(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_instazone(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static int generate_itemlevel(AreaData pArea, ObjectTemplate pObjIndex)
        {
            // TODO
            return 0;
        }

        public static int count_obj_list(ObjectTemplate pObjIndex, List<ObjectInstance> list)
        {
            // TODO
            return 0;
        }

        public static void reset_room(RoomTemplate room)
        {
            // TODO
        }

        public static void reset_area(AreaData area)
        {
            // TODO
        }

        public static void renumber_put_resets(RoomTemplate room)
        {
            // TODO
        }

        public static ResetData add_reset(RoomTemplate room, char letter, int extra, int arg1, int arg2, int arg3)
        {
            // TODO
            return null;
        }

        public static ResetData find_oreset(RoomTemplate room, string oname)
        {
            // TODO
            return null;
        }

        public static void do_reset(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void update_room_reset(CharacterInstance ch, bool setting)
        {
            // TODO
        }
    }
}
