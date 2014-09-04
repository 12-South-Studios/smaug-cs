using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class magic
    {
        /// <summary>
        /// Was ch_slookup
        /// </summary>
        public static int GetIDOfSkillCharacterKnows(this CharacterInstance ch, string name)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(name);
            if (skill == null) return 0;

            if (ch.IsNpc())
                return (int)skill.ID;
            if (ch.PlayerData.Learned[skill.ID] > 0
                && (ch.Level >= skill.SkillLevels.ToList()[(int)ch.CurrentClass]
                    || ch.Level >= skill.RaceLevel[(int)ch.CurrentRace]))
                return (int)skill.ID;
            return 0;
        }

        public static int personal_lookup(CharacterInstance ch, string name)
        {
            if (ch.PlayerData == null)
                return -1;
            foreach (
                SkillData skill in
                    ch.PlayerData.special_skills.Where(skill => (char.ToLower(name[0]) == char.ToLower(skill.Name[0]))
                                                                && name.StartsWithIgnoreCase(skill.Name)))
                return (int) skill.ID;
            return -1;
        }

        public static int FindSkillOfType(CharacterInstance ch, string skillName, bool shouldCharacterKnowSkill,
            SkillTypes expectedType)
        {
            SkillData skill;

            if (ch == null || ch.IsNpc() || !shouldCharacterKnowSkill)
                skill = DatabaseManager.Instance.GetEntity<SkillData>(skillName);
            else
                skill = DatabaseManager.Instance.GetEntity<SkillData>(ch.GetIDOfSkillCharacterKnows(skillName));

            if (skill == null) return 0;
            return skill.Type == expectedType ? (int) skill.ID : 0;
        }

        public static int find_spell(CharacterInstance ch, string name, bool know)
        {
            return FindSkillOfType(ch, name, know, SkillTypes.Spell);
        }

        public static int find_skill(CharacterInstance ch, string name, bool know)
        {
            return FindSkillOfType(ch, name, know, SkillTypes.Skill);
        }

        public static int find_ability(CharacterInstance ch, string name, bool know)
        {
            return FindSkillOfType(ch, name, know, SkillTypes.Racial);
        }

        public static int find_weapon(CharacterInstance ch, string name, bool know)
        {
            return FindSkillOfType(ch, name, know, SkillTypes.Weapon);
        }

        public static int find_tongue(CharacterInstance ch, string name, bool know)
        {
            return FindSkillOfType(ch, name, know, SkillTypes.Tongue);
        }

        public static int slot_lookup(int slot)
        {
            if (slot <= 0)
                return -1;
            foreach (SkillData skill in DatabaseManager.Instance.SKILLS.Values.Where(skill => skill.Slot == slot))
                return (int) skill.ID;
            return -1;
        }

        /// <summary>
        /// Handlers to tell the victim which spell is being effected
        /// </summary>
        public static int dispel_casting(AffectData paf, CharacterInstance ch, CharacterInstance victim, int affect, bool dispel)
        {
            bool isMage = false;
            bool hasDetect = false;

            if (ch.IsNpc() || ch.CurrentClass == ClassTypes.Mage)
                isMage = true;
            if (ch.IsAffected(AffectedByTypes.DetectMagic))
                hasDetect = true;

            string spell;
            if (paf != null)
            {
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>((int) paf.Type);
                if (skill == null)
                    return 0;
                spell = skill.Name;
            }
            else
                spell = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>(affect).GetName().ToLower();

            color.set_char_color(ATTypes.AT_MAGIC, ch);
            color.set_char_color(ATTypes.AT_HITME, victim);

            string buffer = !ch.CanSee(victim)
                         ? "Someone"
                         : (victim.IsNpc()
                                ? victim.ShortDescription
                                : victim.Name).CapitalizeFirst();

            if (dispel)
            {
                color.ch_printf(victim, "Your %s vanishes.", spell);
                if (isMage && hasDetect)
                   color.ch_printf(ch, "%s's %s vanishes.", buffer, spell);
                else
                    return 0;
            }
            else
            {
                if (isMage && hasDetect)
                    color.ch_printf(ch, "%s's %s wavers but holds.", buffer, spell);
                else
                    return 0;
            }

            return 1;
        }

        public static void SuccessfulCast(this CharacterInstance ch, SkillData skill, CharacterInstance victim = null,
            ObjectInstance obj = null)
        {
            ATTypes chItRoom = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_ACTION;
            ATTypes chIt = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_HIT;

            if (skill.Target != TargetTypes.OffensiveCharacter)
                chIt = chItRoom;

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

        public static void FailedCast(this CharacterInstance ch, SkillData skill, CharacterInstance victim = null,
            ObjectInstance obj = null)
        {
            ATTypes chItRoom = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_ACTION;
            ATTypes chItMe = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_HITME;

            if (skill.Target != TargetTypes.OffensiveCharacter)
                chItMe = chItRoom;

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

        public static void ImmuneCast(this CharacterInstance ch, SkillData skill, CharacterInstance victim = null,
            ObjectInstance obj = null)
        {
            ATTypes chItRoom = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_ACTION;
            ATTypes chIt = skill.Type == SkillTypes.Spell ? ATTypes.AT_MAGIC : ATTypes.AT_HIT;

            if (skill.Target != TargetTypes.OffensiveCharacter)
                chIt = chItRoom;

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
        public static int ModifySavingThrowWithResistance(this CharacterInstance ch, int chance, ResistanceTypes ris)
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
                return chance;
            return (chance * modifier) / 10;
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
        public static int rd_parse(CharacterInstance ch, string expression)
        {
            if (expression.IsNullOrWhitespace())
                return 0;

            GameManager.CurrentCharacter = ch;
            return GameManager.Instance.ExpParser.Execute(expression);
        }

        public static int ParseDiceExpression(CharacterInstance ch, string expression)
        {
            return rd_parse(ch, expression);
        }

        /// <summary>
        /// Process the spell's required components, if any
        /// </summary>
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

            ObjectInstance obj = null;
            int val = -1;

            foreach (SpellComponent component in skill.Components)
            {
                bool found = false;
                bool fail = false;
                bool consume = true;

                switch (component.OperatorType)
                {
                    case ComponentOperatorTypes.FailIfPresent:
                        fail = true;
                        break;
                    case ComponentOperatorTypes.DoNotConsume:
                        consume = false;
                        break;
                    case ComponentOperatorTypes.DecreaseValue0:
                        val = 0;
                        break;
                    case ComponentOperatorTypes.DecreaseValue1:
                        val = 1;
                        break;
                    case ComponentOperatorTypes.DecreaseValue2:
                        val = 2;
                        break;
                    case ComponentOperatorTypes.DecreaseValue3:
                        val = 3;
                        break;
                    case ComponentOperatorTypes.DecreaseValue4:
                        val = 4;
                        break;
                    case ComponentOperatorTypes.DecreaseValue5:
                        val = 5;
                        break;
                }

                switch (component.RequiredType)
                {
                    case ComponentRequiredTypes.ItemType:
                        foreach (ObjectInstance vobj in ch.Carrying)
                        {
                            if (vobj.ItemType ==
                                Realm.Library.Common.EnumerationExtensions.GetEnumByName<ItemTypes>(
                                    component.RequiredData))
                            {
                                if (CheckFunctions.CheckIfTrue(ch, fail,
                                    "Something disrupts the casting of this spell...")) return false;
                                found = true;
                                obj = vobj;
                                break;
                            }
                        }
                        break;
                    case ComponentRequiredTypes.ItemVnum:
                        foreach (ObjectInstance vobj in ch.Carrying)
                        {
                            if (vobj.ID == component.RequiredData.ToInt32())
                            {
                                if (CheckFunctions.CheckIfTrue(ch, fail,
                                    "Something disrupts the casting of this spell...")) return false;
                                found = true;
                                obj = vobj;
                                break;
                            }
                        }
                        break;
                    case ComponentRequiredTypes.ItemKeyword:
                        foreach (ObjectInstance vobj in ch.Carrying)
                        {
                            if (vobj.Name.IsAnyEqual(component.RequiredData))
                            {
                                if (CheckFunctions.CheckIfTrue(ch, fail,
                                    "Something disrupts the casting of this spell...")) return false;
                                found = true;
                                obj = vobj;
                                break;
                            }
                        }
                        break;
                    case ComponentRequiredTypes.PlayerCoin:
                        if (ch.CurrentCoin >= component.RequiredData.ToInt32())
                        {
                            if (CheckFunctions.CheckIfTrue(ch, fail,
                                    "Something disrupts the casting of this spell...")) return false;
                            if (consume)
                            {
                                color.set_char_color(ATTypes.AT_GOLD, ch);
                                color.send_to_char("You feel a little lighter...", ch);
                                ch.CurrentCoin -= component.RequiredData.ToInt32();
                            }
                            continue;
                        }
                        break;
                    case ComponentRequiredTypes.PlayerHealth:
                        if (ch.CurrentHealth >= component.RequiredData.ToInt32())
                        {
                            if (CheckFunctions.CheckIfTrue(ch, fail,
                                    "Something disrupts the casting of this spell...")) return false;
                            if (consume)
                            {
                                color.set_char_color(ATTypes.AT_BLOOD, ch);
                                color.send_to_char("You feel a little weaker...", ch);
                                ch.CurrentHealth -= component.RequiredData.ToInt32();
                                ch.UpdatePositionByCurrentHealth();
                            }
                            continue;
                        }
                        break;
                }

                if (fail) continue;
                if (CheckFunctions.CheckIfTrue(ch, !found, "Something is missing...")) return false;
                if (obj != null)
                {
                    if (val >= 0 && val < 6)
                    {
                        obj.Split();
                        if (obj.Value[val] <= 0)
                        {
                            comm.act(ATTypes.AT_MAGIC, "$p dispapears in a puff of smoke!", ch, obj, null, ToTypes.Character);
                            comm.act(ATTypes.AT_MAGIC, "$p disappears in a puff of smoke!", ch, obj, null, ToTypes.Room);
                            obj.Extract();
                            return false;
                        }

                        if (--obj.Value[val] == 0)
                        {
                            comm.act(ATTypes.AT_MAGIC, "$p glows briefly, then disappears in a puff of smoke!", ch, obj, null, ToTypes.Character);
                            comm.act(ATTypes.AT_MAGIC, "$p glows briefly, then disappears in a puff of smoke!", ch, obj, null, ToTypes.Room);
                            obj.Extract();
                        }
                        else
                            comm.act(ATTypes.AT_MAGIC, "$p glows briefly and a whisp of smoke rises from it.", ch, obj,
                                null, ToTypes.Room);
                    }
                    else if (consume)
                    {
                        obj.Split();
                        comm.act(ATTypes.AT_MAGIC, "$p glows briefly, then disappears in a puff of smoke!", ch, obj, null, ToTypes.Character);
                        comm.act(ATTypes.AT_MAGIC, "$p glows briefly, then disappears in a puff of smoke!", ch, obj, null, ToTypes.Room);
                        obj.Extract();
                    }
                    else
                    {
                        int count = obj.Count;
                        obj.Count = 1;
                        comm.act(ATTypes.AT_MAGIC, "$p glows briefly.", ch, obj, null, ToTypes.Character);
                        obj.Count = count;
                    }
                }
            }

            return true;
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
                    vo = TargetCharacterWithOffensiveSpell(arg, ch, false, skill);
                    break;
                case TargetTypes.DefensiveCharacter:
                    vo = TargetCharacterWithDefensiveSpell(arg, ch, false, skill);
                    break;
                case TargetTypes.Self:
                    vo = TargetSelf(arg, ch, false);
                    break;
                case TargetTypes.InventoryObject:
                    vo = TargetObjectInInventory(arg, ch, false);
                    break;
            }

            return vo;
        }

        private static object TargetCharacterWithOffensiveSpell(string arg, CharacterInstance ch, bool silence, SkillData skill)
        {
            CharacterInstance victim;

            if (arg.IsNullOrEmpty())
            {
                victim = ch.GetMyTarget();
                if (CheckFunctions.CheckIfNullObject(ch, victim, !silence ? "Cast the spell on whom?" : "")) return null;
            }
            else
            {
                victim = ch.GetCharacterInRoom(arg);
                if (CheckFunctions.CheckIfNullObject(ch, victim, !silence ? "They aren't here." : "")) return null;
            }

            // Nuisance flag will pick who you are fighting for offensive spells up to 92% of the time
            if (!ch.IsNpc() && ch.CurrentFighting != null && ch.PlayerData.Nuisance != null
                && ch.PlayerData.Nuisance.Flags > 5 &&
                SmaugRandom.D100() < (((ch.PlayerData.Nuisance.Flags - 5) * 8) + 6 * ch.PlayerData.Nuisance.Power))
                victim = ch.GetMyTarget();

            if (fight.is_safe(ch, victim, true))
                return null;

            if (ch == victim)
            {
                if (CheckFunctions.CheckIfSet(ch, skill.Flags, SkillFlags.NoSelf,
                    !silence ? "You can't cast this on yourself!" : "")) return null;

                if (!silence)
                    color.send_to_char("Cast this on yourself?  Okay...", ch);
            }

            if (!ch.IsNpc())
            {
                if (!victim.IsNpc())
                {
                    if (CheckFunctions.CheckIfTrue(ch, ch.GetTimer(TimerTypes.PKilled) != null,
                        !silence ? "You have been killed in the last 5 minutes." : "")) return null;

                    if (CheckFunctions.CheckIfTrue(ch, victim.GetTimer(TimerTypes.PKilled) != null,
                        !silence ? "This player has been killed in the last 5 minutes." : "")) return null;

                    if (CheckFunctions.CheckIfTrue(ch, ch.Act.IsSet(PlayerFlags.Nice) && ch != victim,
                        !silence ? "You are too nice to attack another player." : "")) return null;

                    if (victim != ch)
                    {
                        if (!silence)
                            color.send_to_char("You really shouldn't do this to another player...", ch);
                        else if (victim.GetMyTarget() != ch)
                            return null;
                    }
                }

                if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Charm) && ch.Master == victim,
                    !silence ? "You can't do that to your own follower." : "")) return null;
            }

            fight.check_illegal_pk(ch, victim);
            return victim;
        }

        private static object TargetCharacterWithDefensiveSpell(string arg, CharacterInstance ch, bool silence, 
            SkillData skill)
        {
            CharacterInstance victim;

            if (arg.IsNullOrEmpty())
                victim = ch;
            else
            {
                victim = ch.GetCharacterInRoom(arg);
                if (CheckFunctions.CheckIfNullObject(ch, victim, !silence ? "They aren't here." : "")) return null;
            }

            // Nuisance flag will pick who you are fighting for defensive spells up to 36% of the time
            if (!ch.IsNpc() && ch.CurrentFighting != null && ch.PlayerData.Nuisance != null
                && ch.PlayerData.Nuisance.Flags > 5 &&
                SmaugRandom.D100() < (((ch.PlayerData.Nuisance.Flags - 5) * 8) + 6 * ch.PlayerData.Nuisance.Power))
                victim = ch.GetMyTarget();

            if (CheckFunctions.CheckIfTrue(ch, ch == victim && skill.Flags.IsSet(SkillFlags.NoSelf),
                !silence ? "You can't cast this on yourself!" : "")) return null;

            return victim;
        }

        private static object TargetSelf(string arg, CharacterInstance ch, bool silence)
        {
            if (CheckFunctions.CheckIfTrue(ch, !arg.IsNullOrEmpty() && !arg.EqualsIgnoreCase(ch.Name),
                !silence ? "You cannot cast this spell on another." : "")) return null;

            return ch;
        }

        private static object TargetObjectInInventory(string arg, CharacterInstance ch, bool silence)
        {
            if (CheckFunctions.CheckIfEmptyString(ch, arg, !silence ? "What should the spell be cast upon?" : ""))
                return null;

            ObjectInstance obj = ch.GetCarriedObject(arg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, !silence ? "You are not carrying that." : "")) return null;

            return obj;
        }

        public static ReturnTypes ObjectCastSpell(this CharacterInstance ch, int sn, int level,
            CharacterInstance victim = null, ObjectInstance obj = null)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            if (skill == null || skill.SpellFunction == null)
                return ReturnTypes.Error;

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoMagic)
                || (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe)
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
                && ch.Chance(95 + levelDiff))
            {
                switch (SmaugRandom.Bits(2))
                {
                    case 0:
                    case 2:
                        ch.FailedCast(skill, victim);
                        break;
                    case 1:
                    case 3:
                        comm.act(ATTypes.AT_MAGIC, "The $t spell backfires!", ch, skill.Name, victim, ToTypes.Character);
                        if (victim != null)
                            comm.act(ATTypes.AT_MAGIC, "$n's $t spell backfires!", ch, skill.Name, victim,
                                ToTypes.Victim);
                        comm.act(ATTypes.AT_MAGIC, "$n's $t spell backfires!", ch, skill.Name, victim, ToTypes.Room);
                        return ch.CauseDamageTo(ch, SmaugRandom.Between(1, level), Program.TYPE_UNDEFINED);
                }

                return ReturnTypes.None;
            }

            object vo;

            switch (skill.Target)
            {
                default:
                    LogManager.Instance.Bug("Bad target for sn {0}", sn);
                    return ReturnTypes.Error;

                case TargetTypes.Ignore:
                    vo = null;
                    if (victim != null)
                        Cast.TargetName = victim.Name;
                    else if (obj != null)
                        Cast.TargetName = obj.Name;
                    break;
                case TargetTypes.OffensiveCharacter:
                    if (victim != ch)
                    {
                        if (victim == null)
                            victim = ch.GetMyTarget();
                        if (CheckFunctions.CheckIfTrue(ch, victim == null || (!victim.IsNpc() && !victim.IsInArena()),
                            "You can't do that.")) return ReturnTypes.None;
                    }
                    if (ch != victim && fight.is_safe(ch, victim, true))
                        return ReturnTypes.None;
                    vo = victim;
                    break;
                case TargetTypes.DefensiveCharacter:
                    if (victim == null)
                        victim = ch;
                    vo = victim;
                    if (CheckFunctions.CheckIfTrueCasting(
                        skill.Type != SkillTypes.Herb && victim.Immunity.IsSet(ResistanceTypes.Magic),
                        skill, ch, CastingFunctionType.Immune, victim)) return ReturnTypes.None;
                    break;
                case TargetTypes.Self:
                    vo = ch;
                    if (CheckFunctions.CheckIfTrueCasting(
                        skill.Type != SkillTypes.Herb && ch.Immunity.IsSet(ResistanceTypes.Magic),
                        skill, ch, CastingFunctionType.Immune, victim)) return ReturnTypes.None;
                    break;
                case TargetTypes.InventoryObject:
                    if (CheckFunctions.CheckIfNullObject(ch, obj, "You can't do that!")) return ReturnTypes.None;

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
