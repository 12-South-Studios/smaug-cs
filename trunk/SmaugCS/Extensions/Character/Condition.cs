using System;
using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Extensions
{
    public static class Condition
    {
        public static int GetCondition(this CharacterInstance ch, ConditionTypes condition)
        {
            return ch.PlayerData != null ? ch.PlayerData.ConditionTable[condition] : 0;
        }

        public static void GainCondition(this CharacterInstance ch, ConditionTypes condition, int value)
        {
            if (value == 0 || ch.IsNpc() || ch.Level >= LevelConstants.ImmortalLevel
                || ch.IsNotAuthorized())
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

        private static readonly Dictionary<ConditionTypes, Func<CharacterInstance, int, ReturnTypes>> ConditionTable = new Dictionary
            <ConditionTypes, Func<CharacterInstance, int, ReturnTypes>>
        {
            {ConditionTypes.Full, ConditionFull},
            {ConditionTypes.Thirsty, ConditionThirsty},
            {ConditionTypes.Bloodthirsty, ConditionBloodthirsty},
            {ConditionTypes.Drunk, ConditionDrunk}
        };

        private static ReturnTypes ConditionFull(CharacterInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level < LevelConstants.AvatarLevel && ch.CurrentClass != ClassTypes.Vampire)
            {
                color.set_char_color(ATTypes.AT_HUNGRY, ch);
                DescriptorAttribute attrib = ConditionTypes.Full.GetAttribute<DescriptorAttribute>();

                color.send_to_char(attrib.Messages[conditionValue * 2], ch);
                if (conditionValue < 2)
                {
                    comm.act(ATTypes.AT_HUNGRY, attrib.Messages[(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
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
                }
            }

            return retcode;
        }

        private static ReturnTypes ConditionThirsty(CharacterInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level < LevelConstants.AvatarLevel && ch.CurrentClass != ClassTypes.Vampire)
            {
                color.set_char_color(ATTypes.AT_THIRSTY, ch);
                DescriptorAttribute attrib = ConditionTypes.Thirsty.GetAttribute<DescriptorAttribute>();

                color.send_to_char(attrib.Messages[conditionValue * 2], ch);
                if (conditionValue < 2)
                {
                    comm.act(ATTypes.AT_THIRSTY, attrib.Messages[(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
                    if (conditionValue == 0)
                    {
                        ch.WorsenMentalState(ch.IsPKill() ? 1 : 2);
                        retcode = ch.CauseDamageTo(ch, 2, (int)SkillNumberTypes.Undefined);
                    }
                    else
                        ch.WorsenMentalState(1);
                }
            }

            return retcode;
        }

        private static ReturnTypes ConditionBloodthirsty(CharacterInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level < LevelConstants.AvatarLevel)
            {
                color.set_char_color(ATTypes.AT_BLOOD, ch);
                DescriptorAttribute attrib = ConditionTypes.Bloodthirsty.GetAttribute<DescriptorAttribute>();

                color.send_to_char(attrib.Messages[conditionValue * 2], ch);
                if (conditionValue < 2)
                {
                    comm.act(ATTypes.AT_HUNGRY, attrib.Messages[(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
                    if (conditionValue == 0)
                    {
                        ch.WorsenMentalState(2);
                        retcode = ch.CauseDamageTo(ch, ch.MaximumHealth / 20, (int)SkillNumberTypes.Undefined);
                    }
                    else
                        ch.WorsenMentalState(1);
                }
            }

            return retcode;
        }

        private static ReturnTypes ConditionDrunk(CharacterInstance ch, int conditionValue)
        {
            if (conditionValue == 0 || conditionValue == 1)
            {
                color.set_char_color(ATTypes.AT_SOBER, ch);

                DescriptorAttribute attrib = ConditionTypes.Drunk.GetAttribute<DescriptorAttribute>();
                color.send_to_char(attrib.Messages[conditionValue], ch);
            }

            return ReturnTypes.None;
        }
    }
}
