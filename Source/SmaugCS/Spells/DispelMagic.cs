using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System.Linq;

namespace SmaugCS.Spells
{
    public static class DispelMagic
    {
        public static ReturnTypes spell_dispel_magic(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance)vo;
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            ch.SetColor(ATTypes.AT_MAGIC);

            if (CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), skill, ch, CastingFunctionType.Immune, victim))
                return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(victim.IsNpc() && victim.IsAffected(AffectedByTypes.Possess), skill,
                ch, CastingFunctionType.Immune, victim))
                return ReturnTypes.SpellFailed;

            if (ch == victim)
                return DispelSelf(ch, victim);

            var isMage = ch.IsNpc() || ch.CurrentClass == ClassTypes.Mage;
            if (!isMage && !ch.IsAffected(AffectedByTypes.DetectMagic))
            {
                ch.SendTo("You don't sense a magical aura to dispel.");
                return ReturnTypes.Error;
            }

            int chance = ch.GetCurrentIntelligence() - victim.GetCurrentIntelligence();
            if (isMage)
                chance += 5;
            else
                chance -= 15;

            bool twice = false, three = false;
            if (SmaugRandom.D100() > 75 - chance)
            {
                twice = true;
                if (SmaugRandom.D100() > 75 - chance)
                    three = true;
            }

            bool continueOuterLoop = true;
            int affectedBy;
            bool found = false;
            int cnt = 0, times = 0;

            while (continueOuterLoop)
            {
                AffectData paf = null;

                // grab affected_by from mobs first
                if (victim.IsNpc() && victim.AffectedBy.IsEmpty())
                {
                    for (; ; )
                    {
                        affectedBy = SmaugRandom.Between(0, EnumerationFunctions.MaximumEnumValue<AffectedByTypes>() - 1);
                        if (victim.IsAffectedBy(affectedBy))
                        {
                            found = true;
                            break;
                        }
                        if (cnt++ > 30)
                        {
                            found = false;
                            break;
                        }
                    }

                    // is it a spell?
                    if (found)
                    {
                        foreach (var af in victim.Affects)
                        {
                            paf = af;
                            if (paf.Type.IsSet(affectedBy))
                                break;
                        }

                        // its a spell, remove it
                        if (paf != null)
                        {
                            if (level < victim.Level || victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                            {
                                if (magic.dispel_casting(paf, ch, victim, 0, false) != 0)
                                    ch.FailedCast(skill, victim);
                                return ReturnTypes.SpellFailed;
                            }
                            if (skill.Flags.IsSet(SkillFlags.NoDispel))
                            {
                                if (magic.dispel_casting(paf, ch, victim, 0, false) != 0 && times == 0)
                                    ch.FailedCast(skill, victim);
                                return ReturnTypes.SpellFailed;
                            }

                            if (magic.dispel_casting(paf, ch, victim, 0, true) != 0 && times == 0)
                                ch.SuccessfulCast(skill, victim);
                            victim.RemoveAffect(paf);

                            if ((twice && times < 1) || (three && times < 2))
                            {
                                times++;
                                continue;
                            }
                            return ReturnTypes.None;
                        }

                        // not a spell, just remove the bit (for mobs only)
                        else
                        {
                            if (level < victim.Level || victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                            {
                                if (magic.dispel_casting(null, ch, victim, affectedBy, false) != 0)
                                    ch.FailedCast(skill, victim);
                                return ReturnTypes.SpellFailed;
                            }

                            if (magic.dispel_casting(null, ch, victim, affectedBy, true) != 0 && times == 0)
                                ch.SuccessfulCast(skill, victim);
                            victim.AffectedBy.RemoveBit(affectedBy);

                            if ((twice && times < 1) || (three && times < 2))
                            {
                                times++;
                                continue;
                            }
                            return ReturnTypes.None;
                        }
                    }
                }

                // mob has no affectedBys or we didn't catch them
                if (!victim.Affects.Any())
                {
                    ch.FailedCast(skill, victim);
                    return ReturnTypes.SpellFailed;
                }

                // randomize the affects
                cnt = victim.Affects.Count;
                paf = victim.Affects.First();

                int affectNum;
                int i = 0;
                for (affectNum = SmaugRandom.Between(0, cnt - 1); affectNum > 0; affectNum--)
                {
                    paf = victim.Affects.ToList()[i];
                }

                if (level < victim.Level || victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                {
                    if (magic.dispel_casting(paf, ch, victim, 0, false) != 0)
                        ch.FailedCast(skill, victim);
                    return ReturnTypes.SpellFailed;
                }

                // make sure we have an affect and it isn't a dispel
                if (paf == null || paf.Type.IsSet(SkillFlags.NoDispel))
                {
                    if (magic.dispel_casting(paf, ch, victim, 0, false) != 0)
                        ch.FailedCast(skill, victim);
                    return ReturnTypes.SpellFailed;
                }

                if (magic.dispel_casting(null, ch, victim, 0, true) != 0 && times == 0)
                    ch.SuccessfulCast(skill, victim);
                victim.RemoveAffect(paf);

                if ((twice && times < 1) || (three && times < 2))
                {
                    times++;
                    continue;
                }
            }

            if (!victim.IsNpc())
                victim.update_aris();
            return ReturnTypes.None;
        }

        private static ReturnTypes DispelSelf(CharacterInstance ch, CharacterInstance victim)
        {
            ch.SendTo("You pass your hands around your body...");
            if (ch.Affects.Any())
                return ReturnTypes.None;

            while (ch.Affects.Any())
            {
                var affect = ch.Affects.First();
                ch.RemoveAffect(affect);
            }

            if (!ch.IsNpc())
                victim.update_aris();
            return ReturnTypes.None;
        }
    }
}
