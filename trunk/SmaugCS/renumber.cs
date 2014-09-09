using SmaugCS.Data;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class renumber
    {
        public static void do_renumber(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool check_vnums(CharacterInstance ch, AreaData tarea, renumber_areas r_area)
        {
            // TODO
            return false;
        }

        public static renumber_areas gather_renumber_data(AreaData area, int new_base, bool fill_gaps)
        {
            // TODO
            return null;
        }

        public static renumber_data gather_one_list(short type, int low, int high,
                                                    int new_base, bool fill_gaps, int max_vnum)
        {
            // TODO
            return null;
        }

        public static void renumber_area(CharacterInstance ch, AreaData area, renumber_areas r_area, bool area_is_proto,
                                         bool verbose)
        {
            // TODO
        }

        public static void translate_exits(CharacterInstance ch, AreaData area, renumber_areas r_area, bool verbose)
        {
            // TODO
        }

        public static void translate_objvals(CharacterInstance ch, AreaData area, renumber_areas r_area, bool verbose)
        {
            // TODO
        }

        public static void warn_progs(CharacterInstance ch, int low, int high, AreaData area, renumber_areas r_area)
        {
            // TODO
        }

        public static void warn_in_prog(CharacterInstance ch, int low, int high, string where,
            int vnum, MudProgData mprog, renumber_areas r_area)
        {
            // TODO
        }

        public static void translate_reset(ResetData reset, renumber_areas r_data)
        {
            // TODO
        }

        public static int find_translation(int vnum, renumber_data r_data)
        {
            // TODO
            return 0;
        }

        public static AreaData find_area(string filename, bool p_is_proto)
        {
            // TODO
            return null;
        }
    }
}
