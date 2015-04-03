using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
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
    public static class AreaAttack
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "vo")]
        public static ReturnTypes spell_area_attack(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.SKILLS.Get(sn);

            if (CheckFunctions.CheckIfTrueCasting(ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe), skill, ch))
                return ReturnTypes.SpellFailed;

            if (string.IsNullOrEmpty(skill.HitCharacterMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, null, ToTypes.Character);
            if (string.IsNullOrEmpty(skill.HitRoomMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, null, ToTypes.Room);

            foreach (var vch in ch.CurrentRoom.Persons
                .Where(x => x.IsNpc() || !x.Act.IsSet(PlayerFlags.WizardInvisibility) ||
                    ((PlayerInstance)x).PlayerData.WizardInvisible < LevelConstants.ImmortalLevel)
                .Where(x => x != ch)
                .Where(x => !fight.is_safe(ch, x, false))
                .Where(x => ch.IsNpc() || x.IsNpc() || ch.IsInArena() || (ch.IsPKill() && x.IsPKill())))
            {
                var saved = skill.CheckSave(level, ch, vch);
                if (saved &&
                    CheckFunctions.CheckIfTrueCasting(Macros.SPELL_SAVE(skill) == (int) SpellSaveEffectTypes.Negate,
                        skill, ch, CastingFunctionType.Failed, vch))
                    continue;

                var damage = GetBaseDamage(level, ch, skill);

                if (saved)
                    damage = GetDamageIfSaved(sn, level, ch, skill, vch, damage);

                var retcode = ch.CauseDamageTo(vch, damage, sn);
                if (retcode == ReturnTypes.None
                    && !ch.CharDied() && !vch.CharDied()
                    &&
                    (!vch.IsAffectedBy(sn) || skill.Flags.IsSet(SkillFlags.Accumulative) ||
                     skill.Flags.IsSet(SkillFlags.ReCastable)))
                    retcode = AffectCharacter.spell_affectchar(sn, level, ch, vch);

                if (retcode == ReturnTypes.CharacterDied || ch.CharDied())
                    break;
            }
            return ReturnTypes.None;
        }

        private static int GetDamageIfSaved(int sn, int level, CharacterInstance ch, SkillData skill, CharacterInstance vch,
            int damage)
        {
            var spellSaveType =
                EnumerationExtensions.GetEnum<SpellSaveEffectTypes>(
                    Macros.SPELL_SAVE(skill));
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
                    return damage;
                case SpellSaveEffectTypes.Reflect:
                    if (ReflectDamage(sn, level, ch, vch)) break;
                    return damage;
            }
            return damage;
        }

        private static int GetBaseDamage(int level, CharacterInstance ch, SkillData skill)
        {
            return !string.IsNullOrEmpty(skill.Dice)
                ? magic.ParseDiceExpression(ch, skill.DieCharacterMessage)
                : SmaugRandom.Between(1, level/2);
        }

        private static bool ReflectDamage(int sn, int level, CharacterInstance ch, CharacterInstance vch)
        {
            Attack.spell_attack(sn, level, vch, ch);
            return ch.CharDied();
        }

        private static void AbsorbDamage(CharacterInstance ch, SkillData skill, CharacterInstance vch, int damage)
        {
            comm.act(ATTypes.AT_MAGIC, "$N absorbs your $t!", ch, skill.DamageMessage, vch, ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "You absorb $N's $t!", vch, skill.DamageMessage, ch, ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "$N absorbs $n's $t!", ch, skill.DamageMessage, vch, ToTypes.NotVictim);

            vch.CurrentHealth = 0.GetNumberThatIsBetween(vch.CurrentHealth + damage, vch.MaximumHealth);
            vch.UpdatePositionByCurrentHealth();

            if (damage > 0 && ((ch.CurrentFighting != null && ch.CurrentFighting.Who == vch)
                               || (vch.CurrentFighting != null && vch.CurrentFighting.Who == ch)))
            {
                var xp = ch.CurrentFighting != null
                    ? ch.CurrentFighting.Experience
                    : vch.CurrentFighting.Experience;
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
            return (damage * 3) / 4;
        }
    }
}