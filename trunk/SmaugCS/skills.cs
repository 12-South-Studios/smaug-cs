using SmaugCS.Objects;

namespace SmaugCS
{
    public static class skills
    {
        public static void free_skill(SkillData skill)
        {
            // TODO
        }

        public static void free_skills()
        {
            // TODO
        }

        public static void skill_notfound(CharacterInstance ch, string argument)
        {
            color.send_to_char("Huh?\r\n", ch);
        }

        public static int get_ssave(string name)
        {
            // TODO
            return -1;
        }

        public static int get_starget(string name)
        {
            // TODO
            return -1;
        }

        public static int get_sdamage(string name)
        {
            // TODO
            return -1;
        }

        public static int get_saction(string name)
        {
            // TODO
            return -1;
        }

        public static int get_ssave_effect(string name)
        {
            // TODO
            return -1;
        }

        public static int get_sflag(string name)
        {
            // TODO
            return -1;
        }

        public static int get_spower(string name)
        {
            // TODO
            return -1;
        }

        public static int get_sclass(string name)
        {
            // TODO
            return -1;
        }

        public static bool is_legal_kill(CharacterInstance ch, CharacterInstance vch)
        {
            if (ch.IsNpc() || vch.IsNpc())
                return true;
            if (!ch.IsPKill() || !vch.IsPKill())
                return false;
            return ch.PlayerData.Clan == null || ch.PlayerData.Clan != vch.PlayerData.Clan;
        }

        public static bool check_ability(CharacterInstance ch, string command, string argument)
        {
            // TODO 
            return false;
        }

        public static bool check_skill(CharacterInstance ch, string command, string argument)
        {
            // TODO 
            return false;
        }

        public static void do_skin(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_slookup(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_sset(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void ability_learn_from_success(CharacterInstance ch, int sn)
        {
            // TODO
        }

        public static void learn_from_success(CharacterInstance ch, int sn)
        {
            // TODO
        }

        public static void learn_from_failure(CharacterInstance ch, int sn)
        {
            // TODO
        }

        public static void do_gouge(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_detrap(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_dig(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_search(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_steal(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_backstab(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_rescue(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_kick(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_punch(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_bite(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_claw(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_sting(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_tail(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_bash(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_stun(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_bloodlet(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_feed(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void disarm(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static void do_disarm(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void trip(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static void do_mistwalk(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_broach(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_pick(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_sneak(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_hide(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_visible(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_recall(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_aid(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mount(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_dismount(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool check_parry(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static bool check_dodge(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static bool check_tumble(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static void do_poison_weapon(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_scribe(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_brew(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool check_grip(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static void do_circle(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_berserk(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_hitall(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool check_illegal_psteal(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static void do_scan(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static CharacterInstance scan_for_victim(CharacterInstance ch, ExitData pexit, string name)
        {
            // TODO
            return null;
        }

        public static ObjectInstance find_projectile(CharacterInstance ch, int type)
        {
            // TODO
            return null;
        }

        public static int ranged_got_target(CharacterInstance ch, CharacterInstance victim,
                                            ObjectInstance weapon, ObjectInstance projectile, short dist, short dt, string stxt,
                                            short color)
        {
            // TODO
            return 0;
        }

        public static int ranged_attack(CharacterInstance ch, string argument, ObjectInstance weapon,
                                        ObjectInstance projectile, short dt, short range)
        {
            // TODO
            return 0;
        }

        public static void do_fire(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool mob_fire(CharacterInstance ch, string name)
        {
            // TODO
            return false;
        }

        public static void do_slice(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_style(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool can_use_skill(CharacterInstance ch, int percent, int gsn)
        {
            // TODO
            return false;
        }

        public static void do_cook(CharacterInstance ch, string argument)
        {
            // TODO
        }
    }
}
