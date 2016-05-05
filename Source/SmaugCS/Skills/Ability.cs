using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;
using SmaugCS.Logging;
using SmaugCS.Repository;
using SmaugCS.Spells;

namespace SmaugCS.Skills
{
    public static class Ability
    {
        public static bool CheckAbility(CharacterInstance ch, string command, string argument, 
            IRepositoryManager databaseManager = null)
        {
            var sn = ch.GetIDOfSkillCharacterKnows(command);
            if (sn == -1)
                return false;

            var skill = (databaseManager ?? RepositoryManager.Instance).GetEntity<SkillData>(sn);
            if (skill.SkillFunction == null || skill.SpellFunction == null
                || ch.CanUseSkill(0, sn))
                return false;

            if (!interp.check_pos(ch, skill.MinimumPosition))
                return true;

            if (CheckFunctions.CheckIf(ch, HelperFunctions.IsCharmedOrPossessed,
                "For some reason, you seem unable to perform that...",
                new List<object> { ch }))
            {
                comm.act(ATTypes.AT_GREY, "$n wanders around aimlessly.", ch, null, null, ToTypes.Room);
                return true;
            }

            //// Check if mana is required
            int mana = 0, blood = 0;

            if (skill.MinimumMana > 0)
            {
                mana = ch.IsNpc() ? 0 : skill.MinimumMana
                        .GetHighestOfTwoNumbers(100 / (2 + ch.Level - skill.RaceLevel.ToList()[(int)ch.CurrentRace]));

                if (CheckFunctions.CheckIf(ch, HelperFunctions.HasSufficientBloodPower,
                    "You don't have enough blood power.",
                    new List<object> { ch, blood }))
                    return true;

                if (CheckFunctions.CheckIf(ch, HelperFunctions.HasSufficientMana, "You don't have enough mana.",
                    new List<object> { ch, mana }))
                    return true;
            }

            DateTime start, end;

            //// Is this a real d-fun or just a spell?
            if (skill.SkillFunction == null)
            {
                CharacterInstance victim = null;
                ObjectInstance obj = null;
                var targetName = string.Empty;
                object vo;

                switch (skill.Target)
                {
                    default:
                        LogManager.Instance.Bug("Bad target to Skill {0}", sn);
                        ch.SendTo("Something went wrong...");
                        return true;

                    case TargetTypes.Ignore:
                        vo = null;
                        victim = ch.GetMyTarget();
                        targetName = argument.IsNullOrEmpty() && victim != null ? victim.Name : argument;
                        break;
                    case TargetTypes.OffensiveCharacter:
                        victim = ch.GetMyTarget();

                        if (argument.IsNullOrEmpty() && victim == null)
                        {
                            ch.Printf("Confusion overcomes you as your '%s' has no target.\r\n", skill.Name);
                            return true;
                        }

                        victim = ch.GetCharacterInRoom(argument);
                        if (CheckFunctions.CheckIfTrue(ch, !argument.IsNullOrEmpty() && victim == null,
                            "They aren't here.")) return true;

                        if (fight.is_safe(ch, victim, true))
                            return true;

                        if (CheckFunctions.CheckIfTrue(ch, ch == victim && skill.Flags.IsSet(SkillFlags.NoSelf),
                            "You can't target yourself!")) return true;

                        if (!ch.IsNpc())
                        {
                            if (!victim.IsNpc())
                            {
                                if (CheckFunctions.CheckIfNotNullObject(ch, ch.GetTimer(TimerTypes.PKilled),
                                    "You have been killed in the last five minutes.")) return true;
                                if (CheckFunctions.CheckIfNotNullObject(ch, victim.GetTimer(TimerTypes.PKilled),
                                    "This player has been killed in the last five minutes.")) return true;
                                if (CheckFunctions.CheckIfEquivalent(ch, ch, victim,
                                    "You really shouldn't do this to another player...")) return true;
                            }

                            if (CheckFunctions.CheckIfTrue(ch,
                                ch.IsAffected(AffectedByTypes.Charm) && ch.Master == victim,
                                "You can't do that on your own follower.")) return true;
                        }

                        if (CheckFunctions.CheckIfTrue(ch, fight.check_illegal_pk(ch, victim),
                            "You can't do that to another player!")) return true;

                        vo = victim;
                        break;
                    case TargetTypes.DefensiveCharacter:
                        victim = ch.GetCharacterInRoom(argument);
                        if (CheckFunctions.CheckIfTrue(ch, !argument.IsNullOrEmpty() && victim == null,
                            "They aren't here.")) return true;

                        if (CheckFunctions.CheckIfTrue(ch, ch == victim && skill.Flags.IsSet(SkillFlags.NoSelf),
                            "You can't target yourself!")) return true;

                        vo = victim;
                        break;
                    case TargetTypes.Self:
                        victim = ch;
                        vo = ch;
                        break;
                    case TargetTypes.InventoryObject:
                        obj = ch.GetCarriedObject(argument);
                        if (CheckFunctions.CheckIfNullObject(ch, obj, "You can't find that.")) return true;

                        vo = obj;
                        break;
                }

                Macros.WAIT_STATE(ch, skill.Rounds);

                //// Check for failure
                if (SmaugRandom.D100() + skill.difficulty * 5 > (ch.IsNpc() ? 75 : Macros.LEARNED(ch, (int)skill.ID)))
                {
                    ch.FailedCast(skill, victim, obj);
                    skill.LearnFromFailure((PlayerInstance)ch);
                    if (mana > 0)
                    {
                        if (ch.IsVampire())
                            ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, -blood / 2);
                        else
                            ch.CurrentMana -= mana / 2;
                    }
                    return true;
                }
                if (mana > 0)
                {
                    if (ch.IsVampire())
                        ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, -blood);
                    else
                        ch.CurrentMana -= mana;
                }

                start = DateTime.Now;
                var retcode = skill.SpellFunction.Value.Invoke((int)skill.ID, ch.Level, ch, vo);
                end = DateTime.Now;
                skill.UseHistory.Use(ch, end.Subtract(start));

                if (retcode == ReturnTypes.CharacterDied || retcode == ReturnTypes.Error || ch.CharDied())
                    return true;

                if (retcode == ReturnTypes.SpellFailed)
                {
                    skill.LearnFromFailure((PlayerInstance)ch);
                    retcode = ReturnTypes.None;
                }
                else
                   skill.AbilityLearnFromSuccess((PlayerInstance)ch);

                if (skill.Target == TargetTypes.OffensiveCharacter
                    && victim != ch
                    && !victim.CharDied())
                {
                    if (ch.CurrentRoom.Persons.Any(vch => victim == vch && victim.CurrentFighting == null && victim.Master != ch))
                    {
                        retcode = fight.multi_hit(victim, ch, Program.TYPE_UNDEFINED);
                    }
                }

                return true;
            }

            if (mana > 0)
            {
                if (ch.IsVampire())
                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, -blood);
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
    }
}
