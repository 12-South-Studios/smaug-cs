using System.Collections.Generic;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class magic
    {
        public static bool is_immune(CharacterInstance ch, short damtype)
        {
            // TODO
            return false;
        }

        public static int ch_slookup(CharacterInstance ch, string name)
        {
            // TODO
            return 0;
        }

        public static int personal_lookup(CharacterInstance ch, string name)
        {
            // TODO
            return 0;
        }

        public static int find_skill_of_type(CharacterInstance ch, string name, bool know, int expectedType)
        {
            // TODO
            return 0;
        }

        public static int find_spell(CharacterInstance ch, string name, bool know)
        {
            return find_skill_of_type(ch, name, know, (int)SkillTypes.Spell);
        }

        public static int find_skill(CharacterInstance ch, string name, bool know)
        {
            return find_skill_of_type(ch, name, know, (int)SkillTypes.Skill);
        }

        public static int find_ability(CharacterInstance ch, string name, bool know)
        {
            return find_skill_of_type(ch, name, know, (int)SkillTypes.Racial);
        }

        public static int find_weapon(CharacterInstance ch, string name, bool know)
        {
            return find_skill_of_type(ch, name, know, (int)SkillTypes.Weapon);
        }

        public static int find_tongue(CharacterInstance ch, string name, bool know)
        {
            return find_skill_of_type(ch, name, know, (int)SkillTypes.Tongue);
        }

        public static int slot_lookup(int slot)
        {
            // TODO
            return 0;
        }

        public static int dispel_casting(AffectData paf, CharacterInstance ch, CharacterInstance victim, int affect, bool dispel)
        {
            // TODO
            return 0;
        }

        public static void successful_casting(SkillData skill, CharacterInstance ch, CharacterInstance victim, ObjectInstance @object)
        {
            // TODO
        }

        public static void failed_casting(SkillData skill, CharacterInstance ch, CharacterInstance victim, ObjectInstance @object)
        {
            // TODO
        }

        public static void immune_casting(SkillData skill, CharacterInstance ch, CharacterInstance victim, ObjectInstance @object)
        {
            // TODO
        }

        public static void say_spell(CharacterInstance ch, int sn)
        {
            // TODO
        }

        public static int ris_save(CharacterInstance ch, int schance, int ris)
        {
            // TODO
            return 0;
        }

        public static int rd_parse(CharacterInstance ch, int level, string texp)
        {
            // TODO
            return 0;
        }

        public static int dice_parse(CharacterInstance ch, int level, string texp)
        {
            // TODO
            return 0;
        }

        public static bool process_spell_components(CharacterInstance ch, int sn)
        {
            // TODO
            return false;
        }

        public static List<object> locate_targets(CharacterInstance ch, string arg, int sn, CharacterInstance victim, ObjectInstance @object)
        {
            // TODO
            return null;
        }

        public static void do_cast(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static int obj_cast_spell(int sn, int level, CharacterInstance ch, CharacterInstance victim, ObjectInstance @object)
        {
            // TODO
            return 0;
        }

        public static int spell_acid_blast(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_blindness(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_burning_hands(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_call_lightning(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_cause_light(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_cause_critical(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_cause_serious(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_change_sex(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static bool can_charm(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static int spell_charm_person(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_chill_touch(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_colour_spray(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_control_weather(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_create_food(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_create_water(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_cure_blindness(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_sacral_divinity(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_expurgation(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_bethsaidean_touch(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }

        public static int spell_cure_poison(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }


        public static int spell_curse(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }


        public static int spell_detect_poison(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }


        public static int spell_dispel_evil(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO 
            return 0;
        }



    }
}
