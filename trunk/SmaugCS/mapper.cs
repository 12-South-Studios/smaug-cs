using SmaugCS.Data;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class mapper
    {
        public static int MAPX = 10;
        public static int MAPY = 8;
        public static int MAXDEPTH = 2;

        public static bool BOUNDARY(int x, int y)
        {
            return ((x) < 0) || ((y) < 0) || ((x) > MAPX) || ((y) > MAPY);
        }

        public static coordinate get_exit_dir(int dir, int x, int y, int xorig, int yorig)
        {
            // TODO
            return null;
        }

        public static string get_exits(CharacterInstance ch)
        {
            // TODO
            return string.Empty;
        }

        public static void clear_coord(int x, int y)
        {
            // TODO
        }

        public static void clear_room(int x, int y)
        {
            // TODO
        }

        public static void map_exits(CharacterInstance ch, RoomTemplate pRoom, int x, int y, int depth)
        {
            // TODO
        }

        public static void reformat_desc(string desc)
        {
            // TODO
        }

        public static int get_line(string desc, int max_len)
        {
            // TODO
            return 0;
        }

        public static string whatColor(string str, string pos)
        {
            // TODO
            return string.Empty;
        }

        public static void show_map(CharacterInstance ch, string text)
        {
            // TODO
        }

        public static void draw_room_map(CharacterInstance ch, string desc)
        {
            // TODO
        }
    }
}
