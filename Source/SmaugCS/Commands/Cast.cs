using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using SmaugCS.Spells;
using System.Linq;

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

        [Descriptor(" is here chanting.")]
        public static void do_cast(CharacterInstance ch, string argument)
        {
            var pch = (PlayerInstance)ch;
            switch (pch.SubState)
            {
                case CharacterSubStates.TimerDoAbort:
                    if (!CastAbortTimer(pch, argument))
                        return;
                    break;
                case CharacterSubStates.Pause:
                    if (!CastPause(pch, argument))
                        return;
                    break;
                default:
                    if (!DefaultCast(ch, argument))
                        return;
                    break;
            }

            if (_skill != null && _skill.Name.EqualsIgnoreCase("ventriloquate"))
                magic.say_spell(ch, (int)_skill.ID);

            if (!_dontWait)
                Macros.WAIT_STATE(ch, _skill.Rounds);

            if (!magic.process_spell_components(ch, (int)_skill.ID))
            {
                if (ch.IsVampire())
                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, -1 * 1.GetHighestOfTwoNumbers(_blood / 2));
                else if (ch.Level < LevelConstants.ImmortalLevel)
                    ch.CurrentMana -= _mana / 2;
                //skills.learn_from_failure(ch, (int)_skill.ID);
                return;
            }

            if (!ch.IsNpc() && SmaugRandom.D100() + _skill.difficulty * 5 > pch.PlayerData.GetSkillMastery(_skill.ID))
            {


            }
            else
            {

            }
        }

        private static int GetGodLevel()
        {
            return GameConstants.GetConstant<int>("MaximumLevel") - 7;
        }

        private static bool DefaultCast(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc() && (ch.IsAffected(AffectedByTypes.Charm) || ch.IsAffected(AffectedByTypes.Possess)))
            {
                ch.SendTo("You can't seem to do that right now.");
                return false;
            }

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.NoMagic))
            {
                ch.SetColor(ATTypes.AT_MAGIC);
                ch.SendTo("You failed.");
                return false;
            }

            string arg1, arg2;
            TargetName = argument.OneArgument(out arg1);
            TargetName.OneArgument(out arg2);
            RangedTargetName = TargetName;

            if (arg1.IsNullOrEmpty())
            {
                ch.SendTo("Cast which what where?");
                return false;
            }

            var spell = LookupManager.Instance.SpellLookup.Get(arg1);
            var spellSkill = RepositoryManager.Instance.GetEntity<SkillData>(arg1);

            if (ch.Trust < GetGodLevel())
            {
                if (spell == null || spellSkill == null)
                {
                    ch.SendTo("You can't do that.");
                    return false;
                }

                var pc = ch as PlayerInstance;
                var skillMastery = spellSkill.GetMasteryLevel(pc);
                if (pc.Level < skillMastery)
                {
                    ch.SendTo("You can't do that.");
                    return false;
                }
            }
            else
            {
                if (spell == null)
                {
                    ch.SendTo("We didn't create that yet...");
                    return false;
                }

                if (spell.Value == null)
                {
                    ch.SendTo("We didn't finish that one yet...");
                    return false;
                }
            }

            // TODO Check position magic.c 1464

            return true;
        }

        private static bool CastAbortTimer(PlayerInstance ch, string argument)
        {
            var sn = ch.tempnum;
            if (Macros.IS_VALID_SN(sn))
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);
                if (skill == null)
                {
                    ch.SendTo("Something went wrong...");
                    return false;
                }

                var skillLevel = skill.SkillLevels.ToList()[(int)ch.CurrentClass];
                var mana = ch.IsNpc() ? 0 : Macros.UMAX(skill.MinimumMana, 100 / (2 + ch.Level - skillLevel));
                var blood = Macros.UMAX(1, (mana + 4) / 8);

                if (ch.IsVampire())
                    ch.GainCondition(ConditionTypes.Bloodthirsty, -1 * Macros.UMAX(1, blood / 3));
                else if (!ch.IsImmortal())
                    ch.CurrentMana -= mana / 3;
            }

            ch.SetColor(ATTypes.AT_MAGIC);
            ch.SendTo("You stop chanting...");

            return true;
        }

        private static bool CastPause(PlayerInstance ch, string argument)
        {
            var sn = ch.tempnum;
            if (Macros.IS_VALID_SN(sn))
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);
                if (skill == null)
                {
                    ch.SendTo("Something went wrong...");
                    return false;
                }

                if (skill.Type != Common.Enumerations.SkillTypes.Spell)
                {
                    ch.SendTo("Something cancels out the spell!");
                    return false;
                }

                var skillLevel = skill.SkillLevels.ToList()[(int)ch.CurrentClass];
                var mana = ch.IsNpc() ? 0 : Macros.UMAX(skill.MinimumMana, 100 / (2 + ch.Level - skillLevel));
                var blood = Macros.UMAX(1, (mana + 4) / 8);

                // TODO magic.c 1643
            }

            return true;
        }

    }
}
