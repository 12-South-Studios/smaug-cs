using System.IO;
using SmaugCS.Common;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class misc
    {
        public static void do_eat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_quaff(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_recite(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void pullorpush(CharacterInstance ch, ObjectInstance @object, bool pull)
        {
            // TODO
        }

        public static void do_pull(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_push(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_rap(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_tamp(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_smoke(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_light(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_apply(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void actiondesc(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static ExtendedBitvector fread_bitvector(FileStream fs)
        {
            // TODO
            return null;
        }

        public static string print_bitvector(ExtendedBitvector bits)
        {
            // TODO
            return string.Empty;
        }

        public static void fwrite_bitvector(ExtendedBitvector bits, FileStream fs)
        {
            // TODO
        }

        public static ExtendedBitvector meb(int bit)
        {
            ExtendedBitvector bits = new ExtendedBitvector();
            bits.ClearBits();
            if (bit >= 0)
                bits.SetBit(bit);
            return bits;
        }

        public static ExtendedBitvector multimeb(int bit, params object[] args)
        {
            // TODO
            return null;
        }
    }
}
