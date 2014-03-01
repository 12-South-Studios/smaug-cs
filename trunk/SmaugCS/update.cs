using System;
using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class update
    {
        public static void gain_condition(CharacterInstance ch, ConditionTypes condition, int value)
        {
            if (value == 0 || ch.IsNpc() || ch.Level >= LevelConstants.GetLevel("immortal")
                || ch.IsNotAuthorized())
                return;

            int conditionValue = ch.PlayerData.GetConditionValue(condition);
            ch.PlayerData.SetConditionValue(ConditionTypes.Bloodthirsty,
                                            condition == ConditionTypes.Bloodthirsty
                                                ? (conditionValue + value).GetNumberThatIsBetween(0, 10 + ch.Level)
                                                : (conditionValue + value).GetNumberThatIsBetween(0, 48));

            switch (condition)
            {
                case ConditionTypes.Full:
                    ConditionFull(ch, conditionValue);
                    break;
                case ConditionTypes.Thirsty:
                    ConditionThirsty(ch, conditionValue);
                    break;
                case ConditionTypes.Bloodthirsty:
                    ConditionBloodthirsty(ch, conditionValue);
                    break;
                case ConditionTypes.Drunk:
                    ConditionDrunk(ch, conditionValue);
                    break;
                default:
                    LogManager.Instance.Bug("Invalid condition type {0}", condition);
                    break;
            }
        }

        private static ReturnTypes ConditionFull(CharacterInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level < LevelConstants.GetLevel("avatar") && ch.CurrentClass != ClassTypes.Vampire)
            {
                color.set_char_color(ATTypes.AT_HUNGRY, ch);
                color.send_to_char(ConditionMessageTableTable[ConditionTypes.Full][conditionValue * 2], ch);
                if (conditionValue < 2)
                {
                    comm.act(ATTypes.AT_HUNGRY, ConditionMessageTableTable[ConditionTypes.Full][(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
                    if (conditionValue == 0)
                    {
                        if (!ch.IsPKill() || SmaugCS.Common.SmaugRandom.Bits(1) == 0)
                            ch.WorsenMentalState(1);
                        retcode = fight.damage(ch, ch, 2, (int)SkillNumberTypes.Undefined);
                    }
                    else
                    {
                        if (SmaugCS.Common.SmaugRandom.Bits(1) == 0)
                            ch.WorsenMentalState(1);
                    }
                }
            }

            return retcode;
        }
        private static ReturnTypes ConditionThirsty(CharacterInstance ch, int conditionValue)
        {
            ReturnTypes retcode = ReturnTypes.None;

            if (ch.Level < LevelConstants.GetLevel("avatar") && ch.CurrentClass != ClassTypes.Vampire)
            {
                color.set_char_color(ATTypes.AT_THIRSTY, ch);
                color.send_to_char(ConditionMessageTableTable[ConditionTypes.Thirsty][conditionValue * 2], ch);
                if (conditionValue < 2)
                {
                    comm.act(ATTypes.AT_THIRSTY, ConditionMessageTableTable[ConditionTypes.Thirsty][(conditionValue * 2) + 1], ch, null, null, ToTypes.Room);
                    if (conditionValue == 0)
                    {
                        ch.WorsenMentalState(ch.IsPKill() ? 1 : 2);
                        retcode = fight.damage(ch, ch, 2, (int)SkillNumberTypes.Undefined);
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

            if (ch.Level < LevelConstants.GetLevel("avatar"))
            {
                color.set_char_color(ATTypes.AT_BLOOD, ch);
                color.send_to_char(ConditionMessageTableTable[ConditionTypes.Bloodthirsty][conditionValue * 2], ch);
                if (conditionValue < 2)
                {
                    comm.act(ATTypes.AT_HUNGRY,
                             ConditionMessageTableTable[ConditionTypes.Bloodthirsty][(conditionValue * 2) + 1], ch, null,
                             null, ToTypes.Room);
                    if (conditionValue == 0)
                    {
                        ch.WorsenMentalState(2);
                        retcode = fight.damage(ch, ch, ch.MaximumHealth / 20, (int)SkillNumberTypes.Undefined);
                    }
                    else
                        ch.WorsenMentalState(1);
                }
            }

            return retcode;
        }
        private static int ConditionDrunk(CharacterInstance ch, int conditionValue)
        {
            if (conditionValue == 0 || conditionValue == 1)
            {
                color.set_char_color(ATTypes.AT_SOBER, ch);
                color.send_to_char(ConditionMessageTableTable[ConditionTypes.Drunk][conditionValue], ch);
            }

            return (int)ReturnTypes.None;
        }
        private static readonly Dictionary<ConditionTypes, List<string>> ConditionMessageTableTable = new Dictionary<ConditionTypes, List<string>>()
            {
                { ConditionTypes.Full, new List<string>()
                    {
                        "You are STARVING!\r\n", 
                        "$n is starved half to death!",
                        "You are really hungry.\r\n",
                        "You can hear $n's stomach growling.",
                        "You are hungry.\r\n", 
                        "",
                        "You are a mite peckish.\r\n",
                        ""
                    }},
                { ConditionTypes.Thirsty, new List<string>()
                    {
                        "You are DYING of THIRST!\r\n",
                        "$n is dying of thirst!",
                        "You are really thirsty.\r\n",
                        "$n looks a little parched.",
                        "You are thirsty.\r\n",
                        "",
                        "You could use a sip of something refreshing.\r\n",
                        ""
                    }},
                { ConditionTypes.Bloodthirsty, new List<string>()
                    {
                        "You are starved to feast on blood!\r\n",
                        "$n is suffering from lack of blood!",
                        "You have a growing need to feast on blood!\r\n",
                        "$n gets a strange look in $s eyes...",
                        "You feel an urgent need for blood.\r\n",
                        "",
                        "You feel an aching in your fangs.\r\n",
                        ""
                    }},
                { ConditionTypes.Drunk, new List<string>()
                    {
                        "You are sober.\r\n",
                        "You are feeling a little less light headed.\r\n"
                    }}
            };

        public static void mobile_update()
        {
            // TODO
        }

        public static void char_calendar_update()
        {
            // TODO
        }

        public static void char_update()
        {
            // TODO
        }

        public static void obj_update()
        {
            // TODO
        }

        public static void char_check()
        {
            // TODO
        }

        public static void aggr_update()
        {
            // TODO
        }

        public static void drunk_randoms(CharacterInstance ch)
        {
            // TODO
        }

        public static void hallucinations(CharacterInstance ch)
        {
            // TODO
        }

        public static void tele_update()
        {
            // TODO
        }

        public static void auth_update()
        {
            // TODO
        }

        public static void update_handler()
        {
            // TODO
        }

        public static void remove_portal(ObjectInstance portal)
        {
            // TODO
        }

        public static void reboot_check(DateTime reset)
        {
            // TODO
        }

        public static void auction_update()
        {
            // TODO
        }

        public static void subtract_times(DateTime etime, DateTime sttime)
        {
            // TODO
        }

        public static void time_update()
        {
            // TODO
        }

        public static void hint_update()
        {
            // TODO
        }
    }
}
