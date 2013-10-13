using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class mud_prog
    {
        public static int COMMANDOK = 1;
        public static int IFTRUE = 2;
        public static int IFFALSE = 3;
        public static int ORTRUE = 4;
        public static int ORFALSE = 5;
        public static int FOUNDELSE = 6;
        public static int FOUNDENDIF = 7;
        public static int IFIGNORED = 8;
        public static int ORIGNORED = 9;

        public static int MAX_IF_ARGS = 6;

        private static readonly char[] operators = new[] {'=', '<', '>', '!', '&', '|'};

        public static bool isoperator(char c)
        {
            return operators.Contains(c);
        }

        public static void uphold_supermob(int curr_serial, int serial, RoomTemplate supermob_room,
                                           ObjectInstance trueSupermobObject)
        {
            // TODO
        }

        public static bool carryingvnum_visit(CharacterInstance ch, ObjectInstance @object, int vnum)
        {
            // TODO
            return false;
        }

        public static void init_supermob()
        {
            // TODO
        }

        public static string mprog_next_command(string clist)
        {
            // TODO
            return string.Empty;
        }

        public static bool mprog_seval(string lhs, string opr, string rhs, CharacterInstance mob)
        {
            if (opr.Equals("=="))
                return lhs.Equals(rhs);
            if (opr.Equals("!="))
                return !lhs.Equals(rhs);
            if (opr.Equals("/"))
                return !lhs.Contains(rhs);
            if (opr.Equals("!/"))
                return lhs.Contains(rhs);

            // TODO Error
            return false;
        }

        public static bool mprog_veval(int lhs, string opr, int rhs, CharacterInstance mob)
        {
            if (opr.Equals("=="))
                return lhs == rhs;
            if (opr.Equals("!="))
                return lhs != rhs;
            if (opr.Equals(">"))
                return lhs > rhs;
            if (opr.Equals("<"))
                return lhs < rhs;
            if (opr.Equals("<="))
                return lhs <= rhs;
            if (opr.Equals(">="))
                return lhs >= rhs;
            if (opr.Equals("&"))
                return (lhs & rhs) > 0;
            if (opr.Equals("|"))
                return (lhs | rhs) > 0;

            // TODO Error
            return false;
        }

        public static int mprog_do_ifcheck(string ifcheck, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                           CharacterInstance rndm)
        {
            // TODO
            return 0;
        }

        public static void mprog_translate(char ch, string t, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                           CharacterInstance rndm)
        {
            // TODO
        }

        public static void mprog_driver(string com_list, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                        bool single_step)
        {
            // TODO
        }

        public static int mprog_do_command(string cmnd, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                           CharacterInstance rndm, bool ignore, bool ignore_ors)
        {
            // TODO
            return 0;
        }

        public static void mpsleep_update()
        {
            // TODO
        }

        public static bool mprog_keyword_check(string argu, string arg1)
        {
            // TODO
            return false;
        }

        public static bool mprog_wordlist_check(string arg, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                                int type)
        {
            // TODO
            return false;
        }

        public static void mprog_percent_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                               int type)
        {
            // TODO
        }

        public static void mprog_time_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                            int type)
        {
            // TODO
        }

        public static void mob_act_add(CharacterInstance mob)
        {
            // TODO
        }

        public static void mprog_act_trigger(string buf, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo)
        {
            // TODO
        }

        public static void mprog_bribe_trigger(CharacterInstance mob, CharacterInstance ch, int amount)
        {
            // TODO
        }

        public static void mprog_death_trigger(CharacterInstance killer, CharacterInstance mob)
        {
            // TODO
        }

        public static void mprog_entry_trigger(CharacterInstance mob)
        {
            // TODO
        }

        public static void mprog_fight_trigger(CharacterInstance mob, CharacterInstance ch)
        {
            // TODO
        }

        public static void mprog_give_trigger(CharacterInstance mob, CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void mprog_sell_trigger(CharacterInstance mob, CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void mprog_greet_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void mprog_hitprcnt_trigger(CharacterInstance mob, CharacterInstance ch)
        {
            // TODO
        }

        public static void mprog_random_trigger(CharacterInstance mob)
        {
            // TODO
        }

        public static void mprog_time_trigger(CharacterInstance mob)
        {
            // TODO
        }

        public static void mprog_hour_trigger(CharacterInstance mob)
        {
            // TODO
        }

        public static void mprog_speech_trigger(string txt, CharacterInstance actor)
        {
            // TODO
        }

        public static void mprog_tell_trigger(string txt, CharacterInstance actor)
        {
            // TODO
        }

        public static void mprog_command_trigger(CharacterInstance actor, string txt)
        {
            // TODO
        }

        public static void mprog_script_trigger(CharacterInstance mob)
        {
            // TODO
        }

        public static void oprog_script_trigger(ObjectInstance @object)
        {
            // TODO
        }

        public static void rprog_script_trigger(RoomTemplate room)
        {
            // TODO
        }

        public static void set_supermob(ObjectInstance @object)
        {
            // TODO
        }

        public static void release_supermob()
        {
            // TODO
        }

        public static bool oprog_percent_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo, int type)
        {
            // TODO
            return false;
        }

        public static void oprog_greet_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void oprog_speech_trigger(string txt, CharacterInstance actor)
        {
            // TODO
        }

        public static void oprog_command_trigger(CharacterInstance actor, string txt)
        {
            // TODO
        }

        public static void oprog_random_trigger(ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_wear_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static bool oprog_use_trigger(CharacterInstance ch, ObjectInstance @object, CharacterInstance vict, ObjectInstance targ)
        {
            // TODO
            return false;
        }

        public static void oprog_remove_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_sac_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_get_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_damage_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_repair_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_drop_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_examine_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_zap_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_pull_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_push_trigger(CharacterInstance ch, ObjectInstance @object)
        {
            // TODO
        }

        public static void oprog_act_trigger(string buf, ObjectInstance mobj, CharacterInstance ch, ObjectInstance @object, object vo)
        {
            // TODO
        }

        public static bool oprog_wordlist_check(string arg, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                                int type, ObjectInstance iobj)
        {
            // TODO
            return false;
        }

        public static void rset_supermob(RoomTemplate room)
        {
            // TODO
        }

        public static bool rprog_percent_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo, int type)
        {
            // TODO
            return false;
        }

        public static void rprog_act_trigger(string buf, RoomTemplate room, CharacterInstance ch, ObjectInstance @object, object vo)
        {
            // TODO
        }

        public static void rprog_leave_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_enter_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_sleep_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_rest_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_rfight_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_death_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_speech_trigger(string txt, CharacterInstance ch)
        {
            // TODO
        }
        public static void rprog_command_trigger(CharacterInstance ch, string txt)
        {
            // TODO
        }
        public static void rprog_random_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static bool rprog_wordlist_check(string arg, CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo,
                                        int type, RoomTemplate room)
        {
            // TODO
            return false;
        }

        public static void rprog_time_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance @object, object vo, int type)
        {
            // TODO
        }

        public static void rprog_time_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void rprog_hour_trigger(CharacterInstance ch)
        {
            // TODO
        }

        public static void progbug(string str, CharacterInstance mob)
        {
            // TODO
        }

        public static void room_act_add(RoomTemplate room)
        {
            // TODO
        }

        public static void room_act_update()
        {
            // TODO
        }

        public static void obj_act_add(ObjectInstance @object)
        {
            // TODO
        }

        public static void obj_act_update()
        {
            // TODO
        }
    }
}
