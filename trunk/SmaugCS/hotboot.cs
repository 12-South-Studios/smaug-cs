using System.IO;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class hotboot
    {
        public static int MAX_NEST = 100;
        public static string EXE_FILE = "../src/smaug";
        public static string MOB_FILE = "mobs.dat";

        public static void save_mobile(FileStream fs, CharacterInstance mob)
        {
            // TODO
        }

        public static void save_world()
        {
            // TODO
        }

        public static CharacterInstance load_mobile(FileStream fs)
        {
            // TODO
            return null;
        }

        public static void read_obj_file(string dirname, string filename)
        {
            // TODO
        }

        public static void load_obj_files()
        {
            // TODO
        }

        public static void load_world()
        {
            // TODO
        }

        public static void do_hotboot(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void hotboot_recover()
        {
            // TODO
        }
    }
}
