using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands;
using SmaugCS.Commands.Shop;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data;

using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class skills
    {
        public static void skill_notfound(CharacterInstance ch, string argument)
        {
            color.send_to_char("Huh?\r\n", ch);
        }

        public static int get_ssave(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellSaves"));
        }

        public static int get_starget(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("TargetTypes"));
        }

        public static int get_sdamage(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellDamageTypes"));
        }

        public static int get_saction(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellActionTypes"));
        }

        public static int get_ssave_effect(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellSaveEffects"));
        }

        public static int get_sflag(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellFlags"));
        }

        public static int get_spower(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellPowerTypes"));
        }

        public static int get_sclass(string name)
        {
            return GameConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellClassTypes"));
        }

        public static bool is_legal_kill(CharacterInstance ch, CharacterInstance vch)
        {
            if (ch.IsNpc() || vch.IsNpc())
                return true;
            if (!ch.IsPKill() || !vch.IsPKill())
                return false;
            return ch.PlayerData.Clan == null || ch.PlayerData.Clan != vch.PlayerData.Clan;
        }


        /// <summary>
        /// Racial skill handling
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="command"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static bool check_ability(CharacterInstance ch, string command, string argument)
        {
            int sn = magic.ch_slookup(ch, command);
            if (sn == -1)
                return false;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            if (skill.SkillFunction == null || skill.SpellFunction == null
                || can_use_skill(ch, 0, sn))
                return false;

            if (!interp.check_pos(ch, skill.MinimumPosition))
                return true;

            if (Helpers.CheckFunctions.CheckIf(ch, Helpers.HelperFunctions.IsCharmedOrPossessed,
                "For some reason, you seem unable to perform that...",
                new List<object> {ch}))
            {
                comm.act(ATTypes.AT_GREY, "$n wanders around aimlessly.", ch, null, null, ToTypes.Room);
                return true;
            }

            //// Check if mana is required
            int mana = 0, blood = 0;

            if (skill.MinimumMana > 0)
            {
                mana = ch.IsNpc() ? 0 : skill.MinimumMana
                        .GetHighestOfTwoNumbers(100/(2 + ch.Level - skill.RaceLevel[(int) ch.CurrentRace]));

                if (Helpers.CheckFunctions.CheckIf(ch, Helpers.HelperFunctions.HasSufficientBloodPower,
                    "You don't have enough blood power.",
                    new List<object> {ch, blood}))
                    return true;

                if (Helpers.CheckFunctions.CheckIf(ch, Helpers.HelperFunctions.HasSufficientMana, "You don't have enough mana.",
                    new List<object> {ch, mana}))
                    return true;
            }

            DateTime start, end;

            //// Is this a real d-fun or just a spell?
            if (skill.SkillFunction == null)
            {
                CharacterInstance victim = null;
                ObjectInstance obj = null;
                string targetName = string.Empty;
                object vo;

                switch (skill.Target)
                {
                    default:
                        LogManager.Instance.Bug("Bad target to Skill {0}", sn);
                        color.send_to_char("Something went wrong...\r\n", ch);
                        return true;

                    case TargetTypes.Ignore:
                        vo = null;
                        victim = fight.who_fighting(ch);
                        targetName = (argument.IsNullOrEmpty() && victim != null) ? victim.Name : argument;
                        break;
                    case TargetTypes.OffensiveCharacter:
                        victim = fight.who_fighting(ch);

                        if (argument.IsNullOrEmpty() && victim == null)
                        {
                            color.ch_printf(ch, "Confusion overcomes you as your '%s' has no target.\r\n", skill.Name);
                            return true;
                        }

                        victim = handler.get_char_room(ch, argument);
                        if (!argument.IsNullOrEmpty() && victim == null)
                        {
                            color.send_to_char("They aren't here.\r\n", ch);
                            return true;
                        }

                        if (fight.is_safe(ch, victim, true))
                            return true;

                        if (ch == victim && skill.Flags.IsSet(SkillFlags.NoSelf))
                        {
                            color.send_to_char("You can't target yourself!\r\n", ch);
                            return true;
                        }

                        if (!ch.IsNpc())
                        {
                            if (!victim.IsNpc())
                            {
                                if (handler.get_timer(ch, (short)TimerTypes.PKilled) > 0)
                                {
                                    color.send_to_char("You have been killed in the last 5 minutes.\r\n", ch);
                                    return true;
                                }
                                if (handler.get_timer(victim, (short)TimerTypes.PKilled) > 0)
                                {
                                    color.send_to_char("This player has been killed in the last 5 minutes.\r\n", ch);
                                    return true;
                                }
                                if (victim != ch)
                                    color.send_to_char("You really shouldn't do this to another player...\r\n", ch);
                            }

                            if (ch.IsAffected(AffectedByTypes.Charm) && ch.Master == victim)
                            {
                                color.send_to_char("You can't do that on your own follower.\r\n", ch);
                                return true;
                            }
                        }

                        if (fight.check_illegal_pk(ch, victim))
                        {
                            color.send_to_char("You can't do that to another player!\r\n", ch);
                            return true;
                        }

                        vo = victim;
                        break;
                    case TargetTypes.DefensiveCharacter:
                        victim = handler.get_char_room(ch, argument);
                        if (!argument.IsNullOrEmpty() && victim == null)
                        {
                            color.send_to_char("They aren't here.\r\n", ch);
                            return true;
                        }

                        if (ch == victim && skill.Flags.IsSet(SkillFlags.NoSelf))
                        {
                            color.send_to_char("You can't target yourself!\r\n", ch);
                            return true;
                        }

                        vo = victim;
                        break;
                    case TargetTypes.Self:
                        victim = ch;
                        vo = ch;
                        break;
                    case TargetTypes.InventoryObject:
                        obj = handler.get_obj_carry(ch, argument);
                        if (obj == null)
                        {
                            color.send_to_char("You can't find that.\r\n", ch);
                            return true;
                        }

                        vo = obj;
                        break;
                }

                Macros.WAIT_STATE(ch, skill.Rounds);

                //// Check for failure
                if ((SmaugRandom.Percent() + skill.difficulty * 5) > (ch.IsNpc() ? 75 : Macros.LEARNED(ch, (int)skill.ID)))
                {
                    magic.failed_casting(skill, ch, victim, obj);
                    learn_from_failure(ch, (int)skill.ID);
                    if (mana > 0)
                    {
                        if (ch.IsVampire())
                            update.gain_condition(ch, ConditionTypes.Bloodthirsty, -blood / 2);
                        else
                            ch.CurrentMana -= mana / 2;
                    }
                    return true;
                }
                if (mana > 0)
                {
                    if (ch.IsVampire())
                        update.gain_condition(ch, ConditionTypes.Bloodthirsty, -blood);
                    else
                        ch.CurrentMana -= mana;
                }

                start = DateTime.Now;
                ReturnTypes retcode = skill.SpellFunction.Value.Invoke((int)skill.ID, ch.Level, ch, vo);
                end = DateTime.Now;
                skill.UseHistory.Use(ch, end.Subtract(start));

                if (retcode == ReturnTypes.CharacterDied || retcode == ReturnTypes.Error || ch.CharDied())
                    return true;

                if (retcode == ReturnTypes.SpellFailed)
                {
                    learn_from_failure(ch, (int)skill.ID);
                    retcode = ReturnTypes.None;
                }
                else
                    ability_learn_from_success(ch, (int)skill.ID);

                if (skill.Target == TargetTypes.OffensiveCharacter
                    && victim != ch
                    && !victim.CharDied())
                {
                    foreach (CharacterInstance vch in ch.CurrentRoom.Persons)
                    {
                        if (victim == vch && victim.CurrentFighting == null && victim.Master != ch)
                        {
                            retcode = fight.multi_hit(victim, ch, Program.TYPE_UNDEFINED);
                            break;
                        }
                    }
                }

                return true;
            }

            if (mana > 0)
            {
                if (ch.IsVampire())
                    update.gain_condition(ch, ConditionTypes.Bloodthirsty, -blood);
                else
                    ch.CurrentMana -= mana;
            }

            ch.LastCommand = skill.SkillFunction;
            start = DateTime.Now;
            skill.SkillFunction.Value.Invoke(ch, argument);
            end = DateTime.Now;
            skill.UseHistory.Use(ch, end.Subtract(start));

            // TODO: Tail chain?

            return true;
        }

        public static bool check_skill(CharacterInstance ch, string command, string argument)
        {
            return check_ability(ch, command, argument);
        }

        public static void ability_learn_from_success(CharacterInstance ch, int sn)
        {
            if (ch.IsNpc() || ch.PlayerData.Learned[sn] <= 0)
                return;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            int adept = skill.RaceAdept[(int)ch.CurrentRace];
            int skillLevel = skill.RaceLevel[(int)ch.CurrentRace];

            if (skillLevel == 0)
                skillLevel = ch.Level;
            if (ch.PlayerData.Learned[sn] < adept)
            {
                int schance = ch.PlayerData.Learned[sn] + (5 * skill.difficulty);
                int percent = SmaugRandom.Percent();

                int learn = 1;
                if (percent >= schance)
                    learn = 2;
                else if (schance - percent > 25)
                    return;

                ch.PlayerData.Learned[sn] = adept.GetLowestOfTwoNumbers(ch.PlayerData.Learned[sn] + learn);

                int gain = 0;
                if (ch.PlayerData.Learned[sn] == adept)
                {
                    gain = 1000 * skillLevel;
                    color.set_char_color(ATTypes.AT_WHITE, ch);
                    color.ch_printf(ch, "You are now an adept of %s!  You gain %d bonus experience!\r\n", skill.Name,
                                    gain);
                }
                else
                {
                    gain = 20 * skillLevel;
                    if (ch.CurrentFighting == null) // TODO: Check gsn_hide && gsn_sneak
                    {
                        color.set_char_color(ATTypes.AT_WHITE, ch);
                        color.ch_printf(ch, "You gain %d experience points from your success!\r\n", gain);
                    }
                }
                ch.GainXP(gain);
            }
        }

        public static void learn_from_success(CharacterInstance ch, int sn)
        {
            // TODO
        }

        public static void learn_from_failure(CharacterInstance ch, int sn)
        {
            // TODO
        }

        public static void disarm(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static void trip(CharacterInstance ch, CharacterInstance victim)
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

        public static bool check_grip(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static bool check_illegal_psteal(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
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

        public static ReturnTypes ranged_attack(CharacterInstance ch, string argument, ObjectInstance weapon,
                                        ObjectInstance projectile, int sn, int range)
        {
            // TODO
            return 0;
        }

        public static bool can_use_skill(CharacterInstance ch, int percent, int gsn)
        {
            // TODO
            return false;
        }
    }
}
