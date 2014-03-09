using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class magic
    {
        public static int ch_slookup(CharacterInstance ch, string name)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(name);
            if (skill == null)
                return -1;

            if (ch.IsNpc())
                return (int)skill.ID;
            if (ch.PlayerData.Learned[skill.ID] > 0
                && (ch.Level >= skill.skill_level[(int)ch.CurrentClass]
                    || ch.Level >= skill.RaceLevel[(int)ch.CurrentRace]))
                return (int)skill.ID;
            return -1;
        }

        public static int personal_lookup(CharacterInstance ch, string name)
        {
            if (ch.PlayerData == null)
                return -1;
            foreach (SkillData skill in ch.PlayerData.special_skills.Where(skill => (char.ToLower(name[0]) == char.ToLower(skill.Name[0]))
                                                                                    && name.StartsWithIgnoreCase(skill.Name)))
            {
                return (int)skill.ID;
            }
            return -1;
        }

        public static int find_skill_of_type(CharacterInstance ch, string name, bool know, int expectedType)
        {
            SkillData skill = null;

            if (ch == null || ch.IsNpc() || !know)
                skill = DatabaseManager.Instance.GetEntity<SkillData>(name);
            else
                skill = DatabaseManager.Instance.GetEntity<SkillData>(ch_slookup(ch, name));

            if (skill == null)
                return -1;
            return (int)skill.Type == expectedType ? (int)skill.ID : -1;
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
            foreach (SkillData skill in DatabaseManager.Instance.SKILLS.Values.Where(skill => skill.Slot == slot))
            {
                return (int)skill.ID;
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
            //ExtendedBitvector ext_bv = ExtendedBitvector.Meb(affect);
            bool isMage = false;
            bool hasDetect = false;

            if (ch.IsNpc() || ch.CurrentClass == ClassTypes.Mage)
                isMage = true;
            if (ch.IsAffected(AffectedByTypes.DetectMagic))
                hasDetect = true;

            string spell;
            if (paf != null)
            {
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>((int)paf.Type);
                if (skill == null)
                    return 0;
                spell = skill.Name;
            }
           // else
               // spell = handler.affect_bit_name(ext_bv);

            color.set_char_color(ATTypes.AT_MAGIC, ch);
            color.set_char_color(ATTypes.AT_HITME, victim);

            string buffer = !ch.CanSee(victim)
                         ? "Someone"
                         : (victim.IsNpc()
                                ? victim.ShortDescription
                                : victim.Name).CapitalizeFirst();

            if (dispel)
            {
                //color.ch_printf(victim, "Your %s vanishes.\r\n", spell);
                //if (isMage && hasDetect)
                //    color.ch_printf(ch, "%s's %s vanishes.\r\n", buffer, spell);
                //else
                    return 0;
            }
            else
            {
                //if (isMage && hasDetect)
               //     color.ch_printf(ch, "%s's %s wavers but holds.\r\n", buffer, spell);
               // else
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
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
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
            return (schance * modifier) / 10;
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
            if (expression.IsNullOrWhitespace())
                return 0;

            GameManager.CurrentCharacter = ch;
            return GameManager.Instance.ExpParser.Execute(expression);
        }

        public static int dice_parse(CharacterInstance ch, int level, string texp)
        {
            return rd_parse(ch, level, texp);
        }

        /// <summary>
        /// Process the spell's required components, if any
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        /// <remarks>
        /// T###		check for item of type ###
        /// V#####	check for item of vnum #####
        /// Kword	check for item with keyword 'word'
        /// G#####	check if player has ##### amount of gold
        /// H####	check if player has #### amount of hitpoints
        /// 
        /// Special operators:
        /// ! spell fails if player has this
        /// + don't consume this component
        /// @ decrease component's value[0], and extract if it reaches 0
        /// # decrease component's value[1], and extract if it reaches 0
        /// $ decrease component's value[2], and extract if it reaches 0
        /// % decrease component's value[3], and extract if it reaches 0
        /// ^ decrease component's value[4], and extract if it reaches 0
        /// & decrease component's value[5], and extract if it reaches 0
        /// </remarks>
        public static bool process_spell_components(CharacterInstance ch, int sn)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            if (skill == null || skill.Components.Count == 0)
                return true;

            // TODO: how to handle this?

            return false;
        }

        public static object locate_targets(CharacterInstance ch, string arg, int sn, CharacterInstance victim, ObjectInstance obj)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            if (skill == null)
                return null;

            object vo = null;

            switch (skill.Target)
            {
                default:
                    LogManager.Instance.Bug("Bad target for SN {0}", sn);
                    return -1;
                case TargetTypes.Ignore:
                    break;
                case TargetTypes.OffensiveCharacter:
                    vo = TarCharOffensive(arg, ch, false, skill);
                    break;
                case TargetTypes.DefensiveCharacter:
                    vo = TarCharDefensive(arg, ch, false, skill);
                    break;
                case TargetTypes.Self:
                    vo = TarCharSelf(arg, ch, false);
                    break;
                case TargetTypes.InventoryObject:
                    vo = TarObjInv(arg, ch, false);
                    break;
            }

            return vo;
        }

        private static object TarCharOffensive(string arg, CharacterInstance ch, bool silence, SkillData skill)
        {
            CharacterInstance victim = null;

            if (arg.IsNullOrEmpty())
            {
                victim = fight.who_fighting(ch);
                if (victim == null)
                {
                    if (!silence)
                        color.send_to_char("Cast the spell on whom?\r\n", ch);
                    return null;
                }
            }
            else
            {
                victim = handler.get_char_room(ch, arg);
                if (victim == null)
                {
                    if (!silence)
                        color.send_to_char("They aren't here.\r\n", ch);
                    return null;
                }
            }

            // Nuisance flag will pick who you are fighting for offensive spells up to 92% of the time
            if (!ch.IsNpc() && ch.CurrentFighting != null && ch.PlayerData.Nuisance != null
                && ch.PlayerData.Nuisance.Flags > 5 &&
                SmaugRandom.Percent() < (((ch.PlayerData.Nuisance.Flags - 5) * 8) + 6 * ch.PlayerData.Nuisance.Power))
                victim = fight.who_fighting(ch);

            if (fight.is_safe(ch, victim, true))
                return null;

            if (ch == victim)
            {
                if (Macros.SPELL_FLAG(skill, (int)SkillFlags.NoSelf))
                {
                    if (!silence)
                        color.send_to_char("You can't cast this on yourself!\r\n", ch);
                    return null;
                }

                if (!silence)
                    color.send_to_char("Cast this on yourself?  Okay...\r\n", ch);
            }

            if (!ch.IsNpc())
            {
                if (!victim.IsNpc())
                {
                    if (handler.get_timer(ch, (int)TimerTypes.PKilled) > 0)
                    {
                        if (!silence)
                            color.send_to_char("You have been killed in the last 5 minutes.\r\n", ch);
                        return null;
                    }

                    if (handler.get_timer(victim, (int)TimerTypes.PKilled) > 0)
                    {
                        if (!silence)
                            color.send_to_char("This player has been killed in the last 5 minutes.\r\n", ch);
                        return null;
                    }

                    if (ch.Act.IsSet((int)PlayerFlags.Nice) && ch != victim)
                    {
                        if (!silence)
                            color.send_to_char("You are too nice to attack another player.\r\n", ch);
                        return null;
                    }

                    if (victim != ch)
                    {
                        if (!silence)
                            color.send_to_char("You really shouldn't do this to another player...\r\n", ch);
                        else if (fight.who_fighting(victim) != ch)
                            return null;
                    }
                }

                if (ch.IsAffected(AffectedByTypes.Charm) && ch.Master == victim)
                {
                    if (!silence)
                        color.send_to_char("You can't do that to your own follower.\r\n", ch);
                    return null;
                }
            }

            fight.check_illegal_pk(ch, victim);
            return victim;
        }
        private static object TarCharDefensive(string arg, CharacterInstance ch, bool silence, SkillData skill)
        {
            CharacterInstance victim;

            if (arg.IsNullOrEmpty())
                victim = ch;
            else
            {
                victim = handler.get_char_room(ch, arg);
                if (victim == null)
                {
                    if (!silence)
                        color.send_to_char("They aren't here.\r\n", ch);
                    return null;
                }
            }

            // Nuisance flag will pick who you are fighting for defensive spells up to 36% of the time
            if (!ch.IsNpc() && ch.CurrentFighting != null && ch.PlayerData.Nuisance != null
                && ch.PlayerData.Nuisance.Flags > 5 &&
                SmaugRandom.Percent() < (((ch.PlayerData.Nuisance.Flags - 5) * 8) + 6 * ch.PlayerData.Nuisance.Power))
                victim = fight.who_fighting(ch);

            if (ch == victim && Macros.SPELL_FLAG(skill, (int)SkillFlags.NoSelf))
            {
                if (!silence)
                    color.send_to_char("You can't cast this on yourself!\r\n", ch);
                return null;
            }

            return victim;
        }
        private static object TarCharSelf(string arg, CharacterInstance ch, bool silence)
        {
            if (!arg.IsNullOrEmpty() && !arg.EqualsIgnoreCase(ch.Name))
            {
                if (!silence)
                    color.send_to_char("You cannot cast this spell on another.\r\n", ch);
                return null;
            }
            return ch;
        }
        private static object TarObjInv(string arg, CharacterInstance ch, bool silence)
        {
            if (arg.IsNullOrEmpty())
            {
                if (!silence)
                    color.send_to_char("What should the spell be cast upon?\r\n", ch);
                return null;
            }

            ObjectInstance obj = handler.get_obj_carry(ch, arg);
            if (obj == null)
            {
                if (!silence)
                    color.send_to_char("You are not carrying that.\r\n", ch);
                return null;
            }

            return obj;
        }

        public static ReturnTypes obj_cast_spell(int sn, int level, CharacterInstance ch, CharacterInstance victim, ObjectInstance obj)
        {
            if (sn == -1)
                return (int)ReturnTypes.None;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            if (skill == null || skill.SpellFunction == null)
                return ReturnTypes.Error;

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.NoMagic)
                || (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Safe)
                && skill.Target == TargetTypes.OffensiveCharacter))
            {
                color.set_char_color(ATTypes.AT_MAGIC, ch);
                color.send_to_char("Nothing seems to happen...\r\n", ch);
                return ReturnTypes.None;
            }

            // Reduces the number of level 5 players using level 40 scrolls in battle
            int levelDiff = ch.Level - level;
            if ((skill.Target == TargetTypes.OffensiveCharacter || SmaugRandom.Bits(7) == 1)
                && skill.Type != SkillTypes.Herb
                && handler.chance(ch, 95 + levelDiff))
            {
                switch (SmaugRandom.Bits(2))
                {
                    case 0:
                    case 2:
                        failed_casting(skill, ch, victim, null);
                        break;
                    case 1:
                    case 3:
                        comm.act(ATTypes.AT_MAGIC, "The $t spell backfires!", ch, skill.Name, victim, ToTypes.Character);
                        if (victim != null)
                            comm.act(ATTypes.AT_MAGIC, "$n's $t spell backfires!", ch, skill.Name, victim, ToTypes.Victim);
                        comm.act(ATTypes.AT_MAGIC, "$n's $t spell backfires!", ch, skill.Name, victim, ToTypes.Room);
                        return fight.damage(ch, ch, SmaugRandom.Between(1, level), Program.TYPE_UNDEFINED);
                }

                return ReturnTypes.None;
            }

            object vo = null;
            string targetName = string.Empty;

            switch (skill.Target)
            {
                default:
                    LogManager.Instance.Bug("Bad target for sn {0}", sn);
                    return ReturnTypes.Error;

                case TargetTypes.Ignore:
                    vo = null;
                    if (victim != null)
                        targetName = victim.Name;
                    else if (obj != null)
                        targetName = obj.Name;
                    break;
                case TargetTypes.OffensiveCharacter:
                    if (victim != ch)
                    {
                        if (victim == null)
                            victim = fight.who_fighting(ch);
                        if (victim == null || (!victim.IsNpc() && !victim.IsInArena()))
                        {
                            color.send_to_char("You can't do that.\r\n", ch);
                            return ReturnTypes.None;
                        }
                    }
                    if (ch != victim && fight.is_safe(ch, victim, true))
                        return ReturnTypes.None;
                    vo = victim;
                    break;
                case TargetTypes.DefensiveCharacter:
                    if (victim == null)
                        victim = ch;
                    vo = victim;
                    if (skill.Type != SkillTypes.Herb && victim.Immunity.IsSet((int)ResistanceTypes.Magic))
                    {
                        immune_casting(skill, ch, victim, null);
                        return ReturnTypes.None;
                    }
                    break;
                case TargetTypes.Self:
                    vo = ch;
                    if (skill.Type != SkillTypes.Herb && ch.Immunity.IsSet((int)ResistanceTypes.Magic))
                    {
                        immune_casting(skill, ch, victim, null);
                        return ReturnTypes.None;
                    }
                    break;
                case TargetTypes.InventoryObject:
                    if (obj == null)
                    {
                        color.send_to_char("You can't do that!\r\n", ch);
                        return ReturnTypes.None;
                    }
                    vo = obj;
                    break;
            }

            DateTime start = DateTime.Now;
            ReturnTypes retcode = skill.SpellFunction.Value.Invoke(sn, level, ch, vo);
            skill.UseHistory.Use(ch, DateTime.Now.Subtract(start));

            if (retcode == ReturnTypes.SpellFailed)
                retcode = ReturnTypes.None;

            if (retcode == ReturnTypes.CharacterDied || retcode == ReturnTypes.Error)
                return retcode;

            if (ch.CharDied())
                return ReturnTypes.CharacterDied;

            if (skill.Target == TargetTypes.OffensiveCharacter
                && victim != ch
                && !victim.CharDied())
            {
                foreach (
                    CharacterInstance vch in
                        ch.CurrentRoom.Persons.Where(
                            vch => victim == vch && vch.CurrentFighting == null && vch.Master != ch))
                {
                    retcode = fight.multi_hit(vch, ch, Program.TYPE_UNDEFINED);
                    break;
                }
            }

            return retcode;
        }
    }
}
