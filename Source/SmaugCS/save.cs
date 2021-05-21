using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.IO;

namespace SmaugCS
{
    public static class save
    {
        public static void kill_timer()
        {
            // TODO
        }

        public static void de_equip_char(CharacterInstance ch)
        {
            // TODO
        }

        public static void re_equip_char(CharacterInstance ch)
        {
            // TODO
        }

        public static short find_old_age(CharacterInstance ch)
        {
            // TODO
            return 0;
        }

        public static void save_char_obj(CharacterInstance ch)
        {
            // TODO
        }

        public static void fwrite_char(CharacterInstance ch, FileStream fs)
        {
            // TODO
        }

        public static void fwrite_obj(CharacterInstance ch, ObjectInstance obj,
                                      FileStream fs, int iNest, short os_type, bool hotboot)
        {
            // TODO
        }

        public static void load_char_obj(DescriptorData d, string name,
                                         bool preload, bool copyover)
        {
            // TODO
        }

        public static void fread_char(CharacterInstance ch, FileStream fs, bool preload, bool copyover)
        {
            // TODO
        }

        public static void fread_obj(CharacterInstance ch, FileStream fs, short os_type)
        {
            // TODO
        }

        public static void set_alarm(long seconds)
        {
            // TODO
        }

        public static void do_lst(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void write_corpses(CharacterInstance ch, string name, ObjectInstance objrem)
        {
            // TODO
        }

        public static void load_corpses()
        {
            // TODO
        }

        public static void fwrite_mobile(FileStream fs, CharacterInstance mob)
        {
            // TODO
        }

        public static CharacterInstance fread_mobile(FileStream fs)
        {
            // TODO
            return null;
        }

        public static void write_char_mobile(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void read_char_mobile(string argument)
        {
            // TODO
        }
    }
}
