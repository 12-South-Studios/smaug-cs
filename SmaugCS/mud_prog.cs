using SmaugCS.Data;
using SmaugCS.Data;

namespace SmaugCS
{
    public static class mud_prog
    {
        //http://www.realmsofdespair.com/smaug/herne/smaug/olc-ifchecks.html
        /* MudProg functions
         * $i mob
         * $n actor
         * $t character
         * $r random
         * $o object
         * $p object
         * rand
         * economy
         * mobinarea
         * mobinroom
         * mobinworld
         * timeskilled
         * objinworld
         * ovnumhere
         * otypehere
         * ovnumroom
         * otyperoom
         * ovnumcarry
         * otypecarry
         * ovnumwear
         * otypewear
         * ovnuminv
         * otypeinv
         * ispacifist
         * ismobinvis
         * mobinvislevel
         * ispc
         * isnpc
         * cansee
         * isriding
         * ispassage
         * isopen
         * islocked
         * isdevoted
         * canpkill
         * ismounted
         * ismorphed
         * isnuisance
         * isgood
         * isneutral
         * isevil
         * isfight
         * isimmort
         * ischarmed
         * isflying
         * isthief
         * isattacker
         * iskiller
         * isfollow
         * isaffected
         * numfighting
         * hitprcnt
         * inroom
         * wasinroom
         * norecall
         * sex
         * position
         * doingquest
         * ishelled
         * level
         * goldamt
         * class
         * weight
         * hostdesc
         * multi
         * race
         * morph
         * nuisance
         * clan
         * isleader
         * wearing
         * wearingvnum
         * carryingvnum
         * isclanleader
         * isclan1
         * isclan2
         * council
         * deity
         * guild
         * clantype
         * isflagged
         * istagged
         * waistate
         * asupressed
         * favor
         * hps
         * mana
         * str
         * wis
         * int
         * dex
         * con
         * cha
         * lck
         * objtype
         * leverpos
         * up
         * objval0
         * objval1
         * objval2
         * objval3
         * objval4
         * objval5
         * number
         * time
         * name
         * rank
         * mortinworld
         * mortinroom
         * mortinarea
         * mortcount
         * mobcount
         * charcount
         * drunk
         * haspet
         * isafk
         * isidle
         * ishunting
         * ishating
         * isdead
         * norecall
         * noastral
         * nosummon
         * nomagic
         * nosupplicate
         * indoors
         * ishot
         * iscold
         * iswiny
         * issnowing
         * israining
         * iscalm
         * 
     
          
             */

        /*
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

        /// <summary>
        /// Recursive function used by the carryingvnum ifcheck.  Loops through all objects
        /// belonging to an instance (in nested containers) and returns true if it finds a 
        /// matching vnum.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="obj"></param>
        /// <param name="vnum"></param>
        /// <returns></returns>
        public static bool carryingvnum_visit(CharacterInstance ch, ObjectInstance obj, int vnum)
        {
            if (obj.WearLocation == WearLocations.None
                && obj.ObjectIndex.Vnum == vnum)
                return true;

            return obj.Contents.Any(vobj => carryingvnum_visit(ch, vobj, vnum));
        }

        public static void init_supermob()
        {
            db.Supermob = DatabaseManager.Instance.CHARACTERS.Create(DatabaseManager.Instance.MOBILE_INDEXES.Get(3));
            RoomTemplate office = DatabaseManager.Instance.ROOMS.Get(3);
            office.ToRoom(db.Supermob);
        }

        public static string mprog_next_command(string clist)
        {
            // TODO
            return string.Empty;
        }

        /// <summary>
        /// These two functions do the basic evaluation of ifcheck operators. 
        /// It is important to note that the string operations are not what you probably expect.  
        /// Equality is exact and division is substring. remember that lhs has been stripped 
        /// of leading space, but can still have trailing spaces so be careful when editing since: 
        /// "guard" and "guard " are not equal.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="opr"></param>
        /// <param name="rhs"></param>
        /// <param name="mob"></param>
        /// <returns></returns>
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

            progbug(string.Format("Improper MOBprog operator '{0}'", opr), mob);
            return false;
        }

        /// <summary>
        /// These two functions do the basic evaluation of ifcheck operators. 
        /// It is important to note that the string operations are not what you probably expect.  
        /// Equality is exact and division is substring. remember that lhs has been stripped 
        /// of leading space, but can still have trailing spaces so be careful when editing since: 
        /// "guard" and "guard " are not equal.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="opr"></param>
        /// <param name="rhs"></param>
        /// <param name="mob"></param>
        /// <returns></returns>
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

            progbug(string.Format("Improper MOBprog operator '{0}'", opr), mob);
            return false;
        }

        /// <summary>
        ///This function performs the evaluation of the if checks.  It is here that you can add any 
        /// ifchecks which you so desire. Hopefully it is clear from what follows how one would 
        /// go about adding your own. The syntax for an if check is: ifcheck ( arg ) [opr val]
        /// where the parenthesis are required and the opr and val fields are optional but if one 
        /// is there then both must be. The spaces are all optional. The evaluation of the opr 
        /// expressions is farmed out to reduce the redundancy of the mammoth if statement list.
        ///  If there are errors, then return BERR otherwise return boolean 1,0 Redone by Altrag.. 
        /// kill all that big copy-code that performs the same action on each variable..
        /// </summary>
        /// <param name="ifcheck"></param>
        /// <param name="mob"></param>
        /// <param name="actor"></param>
        /// <param name="object"></param>
        /// <param name="vo"></param>
        /// <param name="rndm"></param>
        /// <returns></returns>
        public static int mprog_do_ifcheck(string ifcheck, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                           CharacterInstance rndm)
        {
            if (ifcheck.IsNullOrEmpty())
            {
                progbug("Null ifcheck", mob);
                return Program.BERR;
            }

            string buffer = ifcheck;
            string opr = string.Empty;

            return 0;
        }

        public static void mprog_translate(char ch, string t, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                           CharacterInstance rndm)
        {
            // TODO
        }

        public static void mprog_driver(string com_list, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                        bool single_step)
        {
            // TODO
        }

        public static int mprog_do_command(string cmnd, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
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

        public static bool mprog_wordlist_check(string arg, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                                int type)
        {
            // TODO
            return false;
        }

        public static void mprog_percent_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                               int type)
        {
            // TODO
        }

        public static void mprog_time_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                            int type)
        {
            // TODO
        }

        public static void mob_act_add(CharacterInstance mob)
        {
            // TODO
        }*/

        public static void mprog_act_trigger(string buf, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo)
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

        public static void mprog_give_trigger(CharacterInstance mob, CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void mprog_sell_trigger(CharacterInstance mob, CharacterInstance ch, ObjectInstance obj)
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

        public static void oprog_script_trigger(ObjectInstance obj)
        {
            // TODO
        }

        public static void rprog_script_trigger(RoomTemplate room)
        {
            // TODO
        }

        public static void set_supermob(ObjectInstance obj)
        {
            // TODO
        }

        public static void release_supermob()
        {
            // TODO
        }

        public static bool oprog_percent_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo, int type)
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

        public static void oprog_random_trigger(ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_wear_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static bool oprog_use_trigger(CharacterInstance ch, ObjectInstance obj, CharacterInstance vict, ObjectInstance targ)
        {
            // TODO
            return false;
        }

        public static void oprog_remove_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_sac_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_get_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_damage_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_repair_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_drop_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_examine_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_zap_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_pull_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_push_trigger(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
        }

        public static void oprog_act_trigger(string buf, ObjectInstance mobj, CharacterInstance ch, ObjectInstance obj, object vo)
        {
            // TODO
        }

        public static bool oprog_wordlist_check(string arg, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                                int type, ObjectInstance iobj)
        {
            // TODO
            return false;
        }

        public static void rset_supermob(RoomTemplate room)
        {
            // TODO
        }

        public static bool rprog_percent_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo, int type)
        {
            // TODO
            return false;
        }

        public static void rprog_act_trigger(string buf, RoomTemplate room, CharacterInstance ch, ObjectInstance obj, object vo)
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

        public static bool rprog_wordlist_check(string arg, CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo,
                                        int type, RoomTemplate room)
        {
            // TODO
            return false;
        }

        public static void rprog_time_check(CharacterInstance mob, CharacterInstance actor, ObjectInstance obj, object vo, int type)
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

        public static void obj_act_add(ObjectInstance obj)
        {
            // TODO
        }

        public static void obj_act_update()
        {
            // TODO
        }
    }
}
