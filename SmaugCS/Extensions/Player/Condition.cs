using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Logging;

namespace SmaugCS.Extensions
{
    public static class Condition
    {
        public static int GetCondition(this PlayerInstance ch, ConditionTypes condition)
        {
            return ch.PlayerData != null ? ch.PlayerData.ConditionTable[condition] : 0;
        }

        public static void GainCondition(this PlayerInstance ch, ConditionTypes condition, int value)
        {
            if (value == 0 || ch.Level >= LevelConstants.ImmortalLevel || ch.IsNotAuthorized())
                return;

            int conditionValue = ch.PlayerData.GetConditionValue(condition);
            ch.PlayerData.SetConditionValue(ConditionTypes.Bloodthirsty,
                                            condition == ConditionTypes.Bloodthirsty
                                                ? (conditionValue + value).GetNumberThatIsBetween(0, 10 + ch.Level)
                                                : (conditionValue + value).GetNumberThatIsBetween(0, 48));

            if (ConditionTable.ContainsKey(condition))
                ConditionTable[condition].Invoke(ch, conditionValue);
            else
                LogManager.Instance.Bug("Invalid condition type {0}", condition);
        }

        private static readonly Dictionary<ConditionTypes, Func<PlayerInstance, int, ReturnTypes>> ConditionTable 
            = new Dictionary <ConditionTypes, Func<PlayerInstance, int, ReturnTypes>>
        {
            {ConditionTypes.Full, ConditionFull},
            {ConditionTypes.Thirsty, ConditionThirsty},
            {ConditionTypes.Bloodthirsty, ConditionBloodthirsty},
            {ConditionTypes.Drunk, ConditionDrunk}
        };

        private static ReturnTypes ConditionFull(PlayerInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level >= LevelConstants.AvatarLevel || ch.CurrentClass == ClassTypes.Vampire) return retcode;

            color.set_char_color(ATTypes.AT_HUNGRY, ch);
            DescriptorAttribute attrib = ConditionTypes.Full.GetAttribute<DescriptorAttribute>();

            color.send_to_char(attrib.Messages.ToList()[conditionValue * 2], ch);
            if (conditionValue >= 2) return retcode;

            comm.act(ATTypes.AT_HUNGRY, attrib.Messages.ToList()[(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
            if (conditionValue == 0)
            {
                if (!ch.IsPKill() || SmaugRandom.Bits(1) == 0)
                    ch.WorsenMentalState(1);
                retcode = ch.CauseDamageTo(ch, 2, (int)SkillNumberTypes.Undefined);
            }
            else
            {
                if (SmaugRandom.Bits(1) == 0)
                    ch.WorsenMentalState(1);
            }

            return retcode;
        }

        private static ReturnTypes ConditionThirsty(PlayerInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level >= LevelConstants.AvatarLevel || ch.CurrentClass == ClassTypes.Vampire) return retcode;

            color.set_char_color(ATTypes.AT_THIRSTY, ch);
            DescriptorAttribute attrib = ConditionTypes.Thirsty.GetAttribute<DescriptorAttribute>();

            color.send_to_char(attrib.Messages.ToList()[conditionValue * 2], ch);
            if (conditionValue >= 2) return retcode;

            comm.act(ATTypes.AT_THIRSTY, attrib.Messages.ToList()[(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
            if (conditionValue == 0)
            {
                ch.WorsenMentalState(ch.IsPKill() ? 1 : 2);
                retcode = ch.CauseDamageTo(ch, 2, (int)SkillNumberTypes.Undefined);
            }
            else
                ch.WorsenMentalState(1);

            return retcode;
        }

        private static ReturnTypes ConditionBloodthirsty(PlayerInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level >= LevelConstants.AvatarLevel) return retcode;

            color.set_char_color(ATTypes.AT_BLOOD, ch);
            DescriptorAttribute attrib = ConditionTypes.Bloodthirsty.GetAttribute<DescriptorAttribute>();

            color.send_to_char(attrib.Messages.ToList()[conditionValue * 2], ch);
            if (conditionValue >= 2) return retcode;

            comm.act(ATTypes.AT_HUNGRY, attrib.Messages.ToList()[(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
            if (conditionValue == 0)
            {
                ch.WorsenMentalState(2);
                retcode = ch.CauseDamageTo(ch, ch.MaximumHealth / 20, (int)SkillNumberTypes.Undefined);
            }
            else
                ch.WorsenMentalState(1);

            return retcode;
        }

        private static ReturnTypes ConditionDrunk(PlayerInstance ch, int conditionValue)
        {
            if (conditionValue < 0 || conditionValue > 1) return ReturnTypes.None;

            color.set_char_color(ATTypes.AT_SOBER, ch);

            DescriptorAttribute attrib = ConditionTypes.Drunk.GetAttribute<DescriptorAttribute>();
            color.send_to_char(attrib.Messages.ToList()[conditionValue], ch);

            return ReturnTypes.None;
        }
    }
}
