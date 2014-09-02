using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Commands
{
    public static class Cast
    {
        public static string TargetName;
        public static string RangedTargetName;

        private static SkillData _skill;
        private static bool _dontWait = false;
        private static int _blood = 0;
        private static int _mana = 0;

        [Descriptor(new[] { " is here chanting." })]
        public static void do_cast(CharacterInstance ch, string argument)
        {
            switch (ch.SubState)
            {
                case CharacterSubStates.TimerDoAbort:
                    CastAbortTimer(ch, argument);
                    break;
                case CharacterSubStates.Pause:
                    CastPause(ch, argument);
                    break;
                default:
                    DefaultCast(ch, argument);
                    break;
            }

            if (_skill != null && _skill.Name.EqualsIgnoreCase("ventriloquate"))
                magic.say_spell(ch, (int)_skill.ID);

            if (!_dontWait)
                Macros.WAIT_STATE(ch, _skill.Rounds);

            if (!magic.process_spell_components(ch, (int)_skill.ID))
            {
                if (ch.IsVampire())
                    ch.GainCondition(ConditionTypes.Bloodthirsty, -1 * 1.GetHighestOfTwoNumbers(_blood / 2));
                else if (ch.Level < LevelConstants.ImmortalLevel)
                    ch.CurrentMana -= _mana/2;
                //skills.learn_from_failure(ch, (int)_skill.ID);
                return;
            }

            if (!ch.IsNpc() && (SmaugRandom.D100() + _skill.difficulty*5) > ch.PlayerData.Learned[_skill.ID])
            {

            }
            else
            {
                
            }
        }

        private static void DefaultCast(CharacterInstance ch, string argument)
        {
            
        }

        private static void CastAbortTimer(CharacterInstance ch, string argument)
        {
            int sn = ch.tempnum;
            if (Macros.IS_VALID_SN(sn))
            {
                _skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
                if (_skill == null)
                {
                    
                }

                _mana = ch.IsNpc()
                            ? 0
                            : _skill.MinimumMana.GetHighestOfTwoNumbers(100/
                                                                        (2 + ch.Level -
                                                                         _skill.SkillLevels.ToList()[(int) ch.CurrentClass]));

            }

            color.set_char_color(ATTypes.AT_MAGIC, ch);
            color.send_to_char("You stop chanting...\r\n", ch);
            
            // TODO: Add a chance to backfire here
        }

        private static void CastPause(CharacterInstance ch, string argument)
        {
            
        }

    }
}
