using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Common;

namespace SmaugCS
{
    public static class magic
    {
        public static int ch_slookup(CharacterInstance ch, string name)
        {
            SkillData skill = db.GetSkill(name);
            if (skill == null)
                return -1;

            if (ch.IsNpc())
                return skill.ID;
            if (ch.PlayerData.Learned[skill.ID] > 0
                && (ch.Level >= skill.skill_level[(int) ch.CurrentClass]
                    || ch.Level >= skill.race_level[(int) ch.CurrentRace]))
                return skill.ID;
            return -1;
        }

        public static int personal_lookup(CharacterInstance ch, string name)
        {
            if (ch.PlayerData == null)
                return -1;
            foreach (SkillData skill in ch.PlayerData.special_skills.Where(skill => (char.ToLower(name[0]) == char.ToLower(skill.Name[0]))
                                                                                    && name.StartsWithIgnoreCase(skill.Name)))
            {
                return skill.ID;
            }
            return -1;
        }

        public static int find_skill_of_type(CharacterInstance ch, string name, bool know, int expectedType)
        {
            SkillData skill = null;

            if (ch == null || ch.IsNpc() || !know)
                skill = db.GetSkill(name);
            else
                skill = db.GetSkill(ch_slookup(ch, name));

            if (skill == null)
                return -1;
            return (int)skill.Type == expectedType ? skill.ID : -1;
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
            if (slot <= 0)
                return -1;
            foreach (SkillData skill in db.SKILLS.Where(skill => skill.slot == slot))
            {
                return skill.ID;
            }
            return -1;
        }

        /// <summary>
        /// Handlers to tell the victim which spell is being effected
        /// </summary>
        /// <param name="paf"></param>
        /// <param name="ch"></param>
        /// <param name="victim"></param>
        /// <param name="affect"></param>
        /// <param name="dispel"></param>
        /// <returns></returns>
        public static int dispel_casting(AffectData paf, CharacterInstance ch, CharacterInstance victim, int affect, bool dispel)
        {
            ExtendedBitvector ext_bv = misc.meb(affect);
            bool isMage = false;
            bool hasDetect = false;

            if (ch.IsNpc() || ch.CurrentClass == ClassTypes.Mage)
                isMage = true;
            if (ch.IsAffected(AffectedByTypes.DetectMagic))
                hasDetect = true;

            string spell;
            if (paf != null)
            {
                SkillData skill = db.GetSkill((int) paf.Type);
                if (skill == null)
                    return 0;
                spell = skill.Name;
            }
            else
                spell = handler.affect_bit_name(ext_bv);

            color.set_char_color(ATTypes.AT_MAGIC, ch);
            color.set_char_color(ATTypes.AT_HITME, victim);

            string buffer = !handler.can_see(ch, victim)
                         ? "Someone"
                         : (victim.IsNpc()
                                ? victim.ShortDescription
                                : victim.Name).CapitalizeFirst();

            if (dispel)
            {
                color.ch_printf(victim, "Your %s vanishes.\r\n", spell);
                if (isMage && hasDetect)
                    color.ch_printf(ch, "%s's %s vanishes.\r\n", buffer, spell);
                else
                    return 0;
            }
            else
            {
                if (isMage && hasDetect)
                    color.ch_printf(ch, "%s's %s wavers but holds.\r\n", buffer, spell);
                else
                    return 0;
            }

            return 1;
        }

        public static void successful_casting(SkillData skill, CharacterInstance ch, CharacterInstance victim, ObjectInstance obj)
        {
            ATTypes chItRoom = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_ACTION;
            ATTypes chIt = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_HIT;

            if (skill.Target != TargetTypes.OffensiveCharacter)
            {
                chIt = chItRoom;
            }

            if (ch != null && ch != victim)
            {
                if (!skill.HitCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.HitCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (skill.Type == SkillTypes.Spell)
                    comm.act(ATTypes.AT_MAGIC, "Ok.", ch, null, null, ToTypes.Character);
            }

            if (ch != null && !skill.HitRoomMessage.IsNullOrEmpty()
                && !skill.HitRoomMessage.Equals(Program.SPELL_SILENT_MARKER))
                comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, obj, victim, ToTypes.NotVictim);

            if (ch != null && victim != null && !skill.HitVictimMessage.IsNullOrEmpty())
            {
                if (skill.HitVictimMessage.Equals(Program.SPELL_SILENT_MARKER))
                {
                    comm.act(ATTypes.AT_MAGIC, skill.HitVictimMessage, ch, obj, victim,
                             ch != victim ? ToTypes.Victim : ToTypes.Character);
                }
            }
            else if (ch != null && ch == victim && skill.Type == SkillTypes.Spell)
                comm.act(ATTypes.AT_MAGIC, "Ok.", ch, null, null, ToTypes.Character);
            else if (ch != null && ch == victim && skill.Type == SkillTypes.Skill)
            {
                if (!skill.HitCharacterMessage.IsNullOrEmpty())
                    comm.act(chIt, skill.HitCharacterMessage, ch, obj, victim, ToTypes.Character);
                else 
                    comm.act(chIt, "Ok.", ch, null, null, ToTypes.Character);
            }
        }

        public static void failed_casting(SkillData skill, CharacterInstance ch, CharacterInstance victim, ObjectInstance obj)
        {
            ATTypes chItRoom = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_ACTION;
            ATTypes chItMe = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_HITME;

            if (skill.Target != TargetTypes.OffensiveCharacter)
            {
                chItMe = chItRoom;
            }

            if (ch != null && ch != victim)
            {
                if (!skill.MissCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.MissCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.MissCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (skill.Type == SkillTypes.Spell)
                    comm.act(ATTypes.AT_MAGIC, "You failed.", ch, null, null, ToTypes.Character);
            }

            if (ch != null && !skill.MissRoomMessage.IsNullOrEmpty()
                && !skill.MissRoomMessage.Equals(Program.SPELL_SILENT_MARKER) 
                && !skill.MissRoomMessage.Equals("supress"))
                comm.act(ATTypes.AT_MAGIC, skill.MissRoomMessage, ch, obj, victim, ToTypes.NotVictim);
            
            if (ch != null && victim != null && !skill.MissRoomMessage.IsNullOrEmpty())
            {
                if (!skill.MissVictimMessage.Equals(Program.SPELL_SILENT_MARKER))
                {
                    comm.act(ATTypes.AT_MAGIC, skill.MissVictimMessage, ch, obj, victim,
                             ch != victim ? ToTypes.Victim : ToTypes.Character);
                }
            }
            else if (ch != null && ch == victim)
            {
                if (!skill.MissCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.MissCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.MissCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (skill.Type == SkillTypes.Spell)
                    comm.act(chItMe, "You failed.", ch, null, null, ToTypes.Character);
            }
        }

        public static void immune_casting(SkillData skill, CharacterInstance ch, CharacterInstance victim, ObjectInstance obj)
        {
            ATTypes chItRoom = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_ACTION;
            ATTypes chIt = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_HIT;

            if (skill.Target != TargetTypes.OffensiveCharacter)
            {
                chIt = chItRoom;
            }

            if (ch != null && ch != victim)
            {
                if (!skill.ImmuneCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.ImmuneCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.ImmuneCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (!skill.MissCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.MissCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.MissCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (skill.Type == SkillTypes.Spell || skill.Type == SkillTypes.Skill)
                    comm.act(chIt, "That appears to have no effect.", ch, null, null, ToTypes.Character);
            }

            if (ch != null && !skill.ImmuneRoomMessage.IsNullOrEmpty())
            {
                if (!skill.ImmuneRoomMessage.Equals(Program.SPELL_SILENT_MARKER))
                    comm.act(ATTypes.AT_MAGIC, skill.ImmuneRoomMessage, ch, obj, victim, ToTypes.Character);
            }
            else if (ch != null && !skill.MissRoomMessage.IsNullOrEmpty())
            {
                if (!skill.MissRoomMessage.Equals(Program.SPELL_SILENT_MARKER))
                    comm.act(ATTypes.AT_MAGIC, skill.MissRoomMessage, ch, obj, victim, ToTypes.Character);
            }

            if (ch != null && victim != null && !skill.ImmuneVictimMessage.IsNullOrEmpty())
            {
                if (!skill.ImmuneVictimMessage.Equals(Program.SPELL_SILENT_MARKER))
                {
                    comm.act(ATTypes.AT_MAGIC, skill.ImmuneVictimMessage, ch, obj, victim,
                             ch != victim ? ToTypes.Victim : ToTypes.Character);
                }
            }
            else if (ch != null && victim != null && !skill.MissVictimMessage.IsNullOrEmpty())
            {
                if (!skill.MissVictimMessage.Equals(Program.SPELL_SILENT_MARKER))
                {
                    comm.act(ATTypes.AT_MAGIC, skill.MissVictimMessage, ch, obj, victim,
                             ch != victim ? ToTypes.Victim : ToTypes.Character);
                }
            }
            else if (ch != null && ch == victim)
            {
                if (!skill.ImmuneCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.ImmuneCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.ImmuneCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (!skill.MissCharacterMessage.IsNullOrEmpty())
                {
                    if (!skill.MissCharacterMessage.Equals(Program.SPELL_SILENT_MARKER))
                        comm.act(ATTypes.AT_MAGIC, skill.MissCharacterMessage, ch, obj, victim, ToTypes.Character);
                }
                else if (skill.Type == SkillTypes.Spell || skill.Type == SkillTypes.Skill)
                    comm.act(chIt, "That appears to have no effect.", ch, null, null, ToTypes.Character);
            }
        }

        public static void say_spell(CharacterInstance ch, int sn)
        {
            SkillData skill = db.GetSkill(sn);
            string newString = tables.ConvertStringSyllables(skill.Name);

            foreach (CharacterInstance rch in ch.CurrentRoom.Persons.Where(x => x != ch))
            {
                comm.act(ATTypes.AT_MAGIC,
                         ch.CurrentClass == rch.CurrentClass
                             ? string.Format("$n utters the words, '{0}'.", skill.Name)
                             : string.Format("$n utters the words, '{0}'.", newString),
                         ch, null, rch, ToTypes.Victim);
            }
        }

        /// <summary>
        /// Make adjustments to saving throw based on RIS (resistance)
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="schance"></param>
        /// <param name="ris"></param>
        /// <returns></returns>
        public static int ris_save(CharacterInstance ch, int schance, int ris)
        {
            int modifier = 10;
            if (ch.Immunity.IsSet(ris))
                modifier -= 10;
            if (ch.Resistance.IsSet(ris))
                modifier -= 2;
            if (ch.Susceptibility.IsSet(ris))
            {
                if (ch.IsNpc() && ch.Immunity.IsSet(ris))
                    modifier += 0;
                else
                    modifier += 2;
            }
            if (modifier <= 0)
                return 1000;
            if (modifier == 10)
                return schance;
            return (schance*modifier)/10;
        }

        /// <summary>
        /// Fancy dice expression parsing complete with order of operations, 
        /// simple exponent support, dice support as well as a few extra variables:
        /// L = level
        /// H = hp
        /// M = mana
        /// V = move
        /// S = str
        /// X = dex 
        /// I = int
        /// W = wis
        /// C = con
        /// A = cha
        /// U = luck
        /// A = age
        /// 
        /// Used for spell dice parsing, i.e. 3d8+L-6
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="level"></param>
        /// <param name="texp"></param>
        /// <returns></returns>
        public static int rd_parse(CharacterInstance ch, int level, string expression)
        {
            char[] buffer = expression.ToCharArray();

            if (string.IsNullOrEmpty(expression))
                return 0;

            // Get rid of brackets if they surround the entire expression
            if (expression.StartsWith("(") && expression.EndsWith(")"))
                buffer = expression.Trim(new[] { '(', ')' }).ToCharArray();

            //// Check if the expression is just a number
            if (buffer.Length == 1 || buffer[0].IsVowel())
            {
                switch (buffer[0])
                {
                    case 'L':
                    case 'l':
                        return level;
                    case 'H':
                    case 'h':
                        return ch.CurrentHealth;
                    case 'M':
                    case 'm':
                        return ch.CurrentMana;
                    case 'V':
                    case 'v':
                        return ch.CurrentMovement;
                    case 'S':
                    case 's':
                        return ch.CurrentStrength;
                    case 'I':
                    case 'i':
                        return ch.CurrentIntelligence;
                    case 'W':
                    case 'w':
                        return ch.CurrentWisdom;
                    case 'X':
                    case 'x':
                        return ch.CurrentDexterity;
                    case 'C':
                    case 'c':
                        return ch.CurrentConstitution;
                    case 'A':
                    case 'a':
                        return ch.CurrentCharisma;
                    case 'U':
                    case 'u':
                        return ch.CurrentLuck;
                    case 'Y':
                    case 'y':
                        return ch.CalculateAge();
                }  
            }

            int y  = 0;
            for(int x=0; x<buffer.Length; ++x, y = x)
            {
                if (!buffer[x].IsDigit() && !buffer[x].IsSpace())
                    break;
            }
            if (y == buffer.Length)
                return Convert.ToInt32(buffer.ToString());

            int eop = 0, lop = 0, gop = 0;
            int total = 0;

            for (int x = 0; x < buffer.Length; ++x)
            {
                switch (buffer[x])
                {
                    case '^':
                        if (total == 0)
                            eop = x;
                        break;
                    case '-':
                    case '+':
                        if (total == 0)
                            lop = x;
                        break;
                    case '*':
                    case '/':
                    case '%':
                    case 'd':
                    case 'D':
                    case '<':
                    case '>':
                    case '{':
                    case '}':
                    case '=':
                        if (total == 0)
                            gop = x;
                        break;
                    case '(':
                        ++total;
                        break;
                    case ')':
                        --total;
                        break;
                }
            }

            int xp = 0;
            if (lop > 0)
                xp = lop;
            else if (gop > 0)
                xp = gop;
            else
                xp = eop;

            char operation = buffer[xp];
            
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

        public static List<object> locate_targets(CharacterInstance ch, string arg, int sn, CharacterInstance victim, ObjectInstance obj)
        {
            // TODO
            return null;
        }

        public static void do_cast(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static int obj_cast_spell(int sn, int level, CharacterInstance ch, CharacterInstance victim, ObjectInstance obj)
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
