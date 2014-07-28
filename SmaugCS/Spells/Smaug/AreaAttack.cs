using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Spells.Smaug
{
    public static class AreaAttack
    {
        public static ReturnTypes spell_area_attack(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                magic.failed_casting(skill, ch, null, null);
                return ReturnTypes.SpellFailed;
            }

            if (string.IsNullOrEmpty(skill.HitCharacterMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, null, ToTypes.Character);
            if (string.IsNullOrEmpty(skill.HitRoomMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, null, ToTypes.Room);

            foreach (CharacterInstance vch in ch.CurrentRoom.Persons)
            {
                if (!vch.IsNpc() && vch.Act.IsSet(PlayerFlags.WizardInvisibility)
                    && vch.PlayerData.WizardInvisible >= LevelConstants.GetLevel("Immortal"))
                    continue;

                if (vch.Equals(ch))
                    continue;

                if (fight.is_safe(ch, vch, false))
                    continue;

                if (!ch.IsNpc() && !vch.IsNpc() && !ch.IsInArena()
                    && (!ch.IsPKill() || !vch.IsPKill()))
                    continue;

                bool saved = skill.CheckSave(level, ch, vch);
                if (saved && Macros.SPELL_SAVE(skill) == (int) SpellSaveEffectTypes.Negate)
                {
                    magic.failed_casting(skill, ch, vch, null);
                    continue;
                }

                int damage = !string.IsNullOrEmpty(skill.Dice) ? magic.dice_parse(ch, level, skill.die_char) : SmaugRandom.Between(1, level/2);

                if (saved)
                {
                    SpellSaveEffectTypes spellSaveType =
                        Realm.Library.Common.EnumerationExtensions.GetEnum<SpellSaveEffectTypes>(Macros.SPELL_SAVE(skill));
                    switch (spellSaveType)
                    {
                        case SpellSaveEffectTypes.ThreeQuartersDamage:
                            damage = (damage*3)/4;
                            break;
                        case SpellSaveEffectTypes.HalfDamage:
                            damage >>= 1;
                            break;
                        case SpellSaveEffectTypes.QuarterDamage:
                            damage >>= 2;
                            break;
                        case SpellSaveEffectTypes.EighthDamage:
                            damage >>= 3;
                            break;
                        case SpellSaveEffectTypes.Absorb:
                            comm.act(ATTypes.AT_MAGIC, "$N absorbs your $t!", ch, skill.DamageMessage, vch, ToTypes.Character);
                            comm.act(ATTypes.AT_MAGIC, "You absorb $N's $t!", vch, skill.DamageMessage, ch, ToTypes.Character);
                            comm.act(ATTypes.AT_MAGIC, "$N absorbs $n's $t!", ch, skill.DamageMessage, vch, ToTypes.NotVictim);

                            vch.CurrentHealth = 0.GetNumberThatIsBetween(vch.CurrentHealth + damage, vch.MaximumHealth);
                            fight.update_pos(vch);

                            if (damage > 0 && ((ch.CurrentFighting != null && ch.CurrentFighting.Who.Equals(vch))
                                || (vch.CurrentFighting != null && vch.CurrentFighting.Who.Equals(ch))))
                            {
                                int xp = ch.CurrentFighting != null
                                    ? ch.CurrentFighting.Experience
                                    : vch.CurrentFighting.Experience;
                                int xpGain = xp*damage*2/vch.MaximumHealth;

                                ch.GainXP(0 - xpGain);
                            }
                            continue;
                        case SpellSaveEffectTypes.Reflect:
                            Attack.spell_attack(sn, level, vch, ch);
                            if (ch.CharDied())
                                break;
                            continue;
                    }
                }

                ReturnTypes retcode = fight.damage(ch, vch, damage, sn);
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
    }
}
