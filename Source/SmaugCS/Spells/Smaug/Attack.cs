using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS.Spells.Smaug
{
    public static class Attack
    {
        public static ReturnTypes spell_attack(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.SKILLS.Get(sn);
            var vch = (CharacterInstance) vo;

            var saved = skill.CheckSave(level, ch, vch);
            if (CheckFunctions.CheckIfTrueCasting(
                saved && Macros.SPELL_SAVE(skill) == (int) SpellSaveEffectTypes.Negate, skill, ch,
                CastingFunctionType.Failed, vch)) return ReturnTypes.SpellFailed;

            var damage = GetBaseDamage(level, ch, skill);

            if (saved)
            {
                var spellSaveType =
                    EnumerationExtensions.GetEnum<SpellSaveEffectTypes>(Macros.SPELL_SAVE(skill));
                switch (spellSaveType)
                {
                    case SpellSaveEffectTypes.ThreeQuartersDamage:
                        damage = GetThreeQuartersDamage(damage);
                        break;
                    case SpellSaveEffectTypes.HalfDamage:
                        damage = GetHalfDamage(damage);
                        break;
                    case SpellSaveEffectTypes.QuarterDamage:
                        damage = GetQuarterDamage(damage);
                        break;
                    case SpellSaveEffectTypes.EighthDamage:
                        damage = GetEighthDamage(damage);
                        break;
                    case SpellSaveEffectTypes.Absorb:
                        AbsorbDamage(ch, skill, vch, damage);
                        return ReturnTypes.None;
                    case SpellSaveEffectTypes.Reflect:
                        return spell_attack(sn, level, vch, ch);
                }
            }

            var retcode = ch.CauseDamageTo(vch, damage, sn);
            if (retcode == ReturnTypes.None
                && !ch.CharDied() && !vch.CharDied()
                &&
                (!vch.IsAffectedBy(sn) || skill.Flags.IsSet(SkillFlags.Accumulative) ||
                 skill.Flags.IsSet(SkillFlags.ReCastable)))
                retcode = AffectCharacter.spell_affectchar(sn, level, ch, vch);

            return retcode;
        }

        private static void AbsorbDamage(CharacterInstance ch, SkillData skill, CharacterInstance vch, int damage)
        {
            comm.act(ATTypes.AT_MAGIC, "$N absorbs your $t!", ch, skill.DamageMessage, vch,
                ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "You absorb $N's $t!", vch, skill.DamageMessage, ch,
                ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "$N absorbs $n's $t!", ch, skill.DamageMessage, vch,
                ToTypes.NotVictim);

            vch.CurrentHealth = 0.GetNumberThatIsBetween(vch.CurrentHealth + damage, vch.MaximumHealth);
            vch.UpdatePositionByCurrentHealth();

            if (damage > 0 && ((ch.CurrentFighting != null && ch.CurrentFighting.Who == vch)
                               || (vch.CurrentFighting != null && vch.CurrentFighting.Who == ch)))
            {
                var xp = ch.CurrentFighting?.Experience ?? vch.CurrentFighting.Experience;
                var xpGain = xp*damage*2/vch.MaximumHealth;

                ((PlayerInstance)ch).GainXP(0 - xpGain);
            }
        }

        private static int GetEighthDamage(int damage)
        {
            damage >>= 3;
            return damage;
        }

        private static int GetQuarterDamage(int damage)
        {
            damage >>= 2;
            return damage;
        }

        private static int GetHalfDamage(int damage)
        {
            damage >>= 1;
            return damage;
        }

        private static int GetThreeQuartersDamage(int damage)
        {
            return damage*3/4;
        }

        private static int GetBaseDamage(int level, CharacterInstance ch, SkillData skill)
        {
            return !string.IsNullOrEmpty(skill.Dice)
                ? magic.ParseDiceExpression(ch, skill.DieCharacterMessage)
                : SmaugRandom.Between(1, level/2);
        }
    }
}
