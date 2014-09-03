﻿using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class Fighting
    {
        public static CharacterInstance GetMyTarget(this CharacterInstance ch)
        {
            return ch == null || ch.CurrentFighting == null ? null : ch.CurrentFighting.Who;
        }

        public static int ModifyDamageWithResistance(this CharacterInstance ch, int dam, ResistanceTypes ris)
        {
            int modifier = 10;
            if (ch.Immunity.IsSet(ris) && !ch.NoImmunity.IsSet(ris))
                modifier -= 10;
            if (ch.Resistance.IsSet(ris) && !ch.NoResistance.IsSet(ris))
                modifier -= 2;
            if (ch.Susceptibility.IsSet(ris) && !ch.NoSusceptibility.IsSet(ris))
            {
                if (ch.IsNpc() && ch.Immunity.IsSet(ris))
                    modifier += 0;
                else
                    modifier += 2;
            }
            if (modifier <= 0)
                return -1;
            if (modifier == 10)
                return dam;
            return (dam * modifier) / 10;
        }

        public static void CheckAttackForAttackerFlag(this CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.IsNpc() || victim.Act.IsSet(PlayerFlags.Killer) || victim.Act.IsSet(PlayerFlags.Thief))
                return;

            if (!ch.IsNpc() && !victim.IsNpc() && ch.CanPKill() && victim.CanPKill())
                return;

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.Master == null)
                {
                    LogManager.Instance.Bug("{0} bad AffectedByTypes.Charm", ch.IsNpc() ? ch.ShortDescription : ch.Name);
                    // TODO affect_strip
                    ch.AffectedBy.RemoveBit(AffectedByTypes.Charm);
                    return;
                }

                return;
            }

            if (ch.IsNpc() || ch == victim || ch.Level >= LevelConstants.ImmortalLevel ||
                ch.Act.IsSet(PlayerFlags.Attacker) || ch.Act.IsSet(PlayerFlags.Killer))
                return;

            ch.Act.SetBit(PlayerFlags.Attacker);
            save.save_char_obj(ch);
        }

        public static void StopFighting(this CharacterInstance ch, bool includeMyTargetsTarget)
        {
            EndFight(ch);
            ch.UpdatePositionByCurrentHealth();

            if (!includeMyTargetsTarget) return;
            foreach (CharacterInstance fch in DatabaseManager.Instance.CHARACTERS.Values
                .Where(fch => fch.GetMyTarget() == ch))
                fch.StopFighting(false);
        }

        private static void EndFight(CharacterInstance ch)
        {
            if (ch.CurrentFighting != null && ch.CurrentFighting.Who.CharDied())
                    --ch.CurrentFighting.Who.NumberFighting;
            
            ch.CurrentFighting = null;
            ch.CurrentPosition = ch.CurrentMount != null
                ? PositionTypes.Mounted
                : PositionTypes.Standing;

            if (ch.IsAffected(AffectedByTypes.Berserk))
            {
                // TODO affect_strip
                color.set_char_color(ATTypes.AT_WEAROFF, ch);
                color.send_to_char(DatabaseManager.Instance.GetEntity<SkillData>("berserk").WearOffMessage, ch);
                color.send_to_char("\r\n", ch);
            }
        }

        public static void UpdatePositionByCurrentHealth(this CharacterInstance victim)
        {
            if (victim.CurrentHealth > 0)
            {
                if (victim.CurrentPosition <= PositionTypes.Stunned)
                    victim.CurrentPosition = PositionTypes.Standing;
                if (victim.IsAffected(AffectedByTypes.Paralysis))
                    victim.CurrentPosition = PositionTypes.Stunned;
                return;
            }

            if (victim.IsNpc() || victim.CurrentHealth <= -11)
            {
                if (victim.CurrentMount != null)
                {
                    comm.act(ATTypes.AT_ACTION, "$n falls from $N.", victim, null, victim.CurrentMount, ToTypes.Room);
                    victim.CurrentMount.Act.RemoveBit(ActFlags.Mounted);
                    victim.CurrentMount = null;
                }

                victim.CurrentPosition = PositionTypes.Dead;
                return;
            }

            if (victim.CurrentHealth <= -6)
                victim.CurrentPosition = PositionTypes.Mortal;
            else if (victim.CurrentHealth <= -3)
                victim.CurrentPosition = PositionTypes.Incapacitated;
            else
                victim.CurrentPosition = PositionTypes.Stunned;

            if (victim.CurrentPosition > PositionTypes.Stunned && victim.IsAffected(AffectedByTypes.Paralysis))
                victim.CurrentPosition = PositionTypes.Stunned;

            if (victim.CurrentMount != null)
            {
                comm.act(ATTypes.AT_ACTION, "$n falls unconcious from $N.", victim, null, victim.CurrentMount, ToTypes.Room);
                victim.CurrentMount.Act.RemoveBit(ActFlags.Mounted);
                victim.CurrentMount = null;
            }
        }

        public static int ComputeAlignmentChange(this CharacterInstance ch, CharacterInstance victim)
        {
            int align = ch.CurrentAlignment - victim.CurrentAlignment;
            int divalign = (ch.CurrentAlignment > -350 && ch.CurrentAlignment < 350) ? 4 : 20;
            int newAlign;

            if (align > 500)
                newAlign = (ch.CurrentAlignment + (align - 500) / divalign).GetLowestOfTwoNumbers(1000);
            else if (align < -500)
                newAlign = (ch.CurrentAlignment + (align + 500) / divalign).GetHighestOfTwoNumbers(-1000);
            else
                newAlign = ch.CurrentAlignment - ch.CurrentAlignment / divalign;

            return newAlign;
        }

        public static int ComputeExperienceGain(this CharacterInstance ch, CharacterInstance victim)
        {
            int xp = (victim.GetExperienceWorth() * 0.GetNumberThatIsBetween((victim.Level - ch.Level) + 10, 13)) / 10;
            int align = ch.CurrentAlignment - victim.CurrentAlignment;

            if (align > 990 || align < -990)
                xp = ModifyXPForAttackingOppositeAlignment(xp);
            else if (ch.CurrentAlignment > 300 && align < 250)
                xp = ModifyXPForGoodPlayerAttackingSameAlignment(xp);

            xp = SmaugRandom.Between((xp * 3) >> 2, (xp * 5) >> 2);

            if (!victim.IsNpc())
                xp /= 4;
            else if (!ch.IsNpc())
                xp = ReduceXPForKillingSameMobRepeatedly(ch, victim, xp);
            
            if (!ch.IsNpc() && ch.Level > 5)
                xp = ModifyXPForExperiencedVsNovicePlayer(ch, xp);

            //// Level based experience gain cap.  Cannot get more experience for
            //// a kill than the amount for your current experience level
            return 0.GetNumberThatIsBetween(xp, ch.GetExperienceLevel(ch.Level + 1) - ch.GetExperienceLevel(ch.Level));
        }

        private static int ModifyXPForAttackingOppositeAlignment(int xp)
        {
            int modXp = xp;
            return (modXp * 5) >> 2;
        }

        private static int ModifyXPForGoodPlayerAttackingSameAlignment(int xp)
        {
            int modXp = xp;
            return (modXp * 3) >> 2;
        }

        private static int ModifyXPForExperiencedVsNovicePlayer(CharacterInstance ch, int xp)
        {
            int modXp = xp;
            int xpRatio = ch.PlayedDuration/ch.Level;

            if (xpRatio > 20000)
                modXp = (modXp*5)/4; //// 5/4
            else if (xpRatio > 16000)
                modXp = (modXp*3)/4; //// 3/4
            else if (xpRatio > 10000)
                modXp >>= 1; //// 1/2
            else if (xpRatio > 5000)
                modXp >>= 2; //// 1/4th
            else if (xpRatio > 3500)
                modXp >>= 3; //// 1/8th
            else if (xpRatio > 2000)
                modXp >>= 4; //// 1/16th

            return modXp;
        }

        private static int ReduceXPForKillingSameMobRepeatedly(CharacterInstance ch, CharacterInstance victim, int xp)
        {
            int modXp = xp;
            int times = ch.TimesKilled(victim);

            if (times > 0 && times < 20)
            {
                modXp = (modXp * (20 - times)) / 20;
                if (times > 15)
                    modXp /= 3;
                else if (times > 10)
                    modXp >>= 1;
            }
            else
                modXp = 0;

            return modXp;
        }
    }
}
