using System.Linq;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Repository;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS.Spells.Smaug
{
    public static class Affect
    {
        private class TargetBooleanValues
        {
            public bool GroupSpell;
            public bool AreaSpell;
            public bool HitCharacter;
            public bool HitRoom;
            public bool HitVictim;
        }

        public static ReturnTypes spell_affect(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.SKILLS.Get(sn);

            if (!skill.Affects.Any())
                return ReturnTypes.None;
            
            var target = new TargetBooleanValues();
            if (skill.Flags.IsSet(SkillFlags.GroupSpell))
                target.GroupSpell = true;
            if (skill.Flags.IsSet(SkillFlags.Area))
                target.AreaSpell = true;

            var victim = (CharacterInstance)vo;

            if (!target.GroupSpell && !target.AreaSpell)
            {
                var retCode = CastSingleTargetSpell(skill, level, ch, victim);
                if (retCode == ReturnTypes.SpellFailed)
                    return retCode;
            }
            else
            {
                if (string.IsNullOrEmpty(skill.HitCharacterMessage))
                    target.HitCharacter = true;
                else
                    comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, null, ToTypes.Character);

                if (string.IsNullOrEmpty(skill.HitRoomMessage))
                    target.HitRoom = true;
                else
                    comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, null, ToTypes.Room);

                if (string.IsNullOrEmpty(skill.HitVictimMessage))
                    target.HitVictim = true;
                victim = victim != null ? victim.CurrentRoom.Persons.First() : ch.CurrentRoom.Persons.First();
            }

            if (CheckFunctions.CheckIfNullObjectCasting(victim, skill, ch)) return ReturnTypes.SpellFailed;

            if (!target.GroupSpell && !target.AreaSpell)
                CastTargetSpellAtVictim(ch, victim.CurrentRoom.Persons.First(), skill, target, level);
            else
                victim.CurrentRoom.Persons.ToList().ForEach(vch => CastTargetSpellAtVictim(ch, vch, skill, target, level));
            
            return ReturnTypes.None;
        }

        private static void CastTargetSpellAtVictim(CharacterInstance ch, CharacterInstance victim, SkillData skill, TargetBooleanValues targetValues, int level)
        {
            if (targetValues.GroupSpell || targetValues.AreaSpell)
            {
                var resType = EnumerationExtensions.GetEnum<ResistanceTypes>(Macros.SPELL_DAMAGE(skill));
                if ((targetValues.GroupSpell
                     && !victim.IsSameGroup(ch))
                    || victim.Immunity.IsSet(ResistanceTypes.Magic)
                    || victim.IsImmune(resType)
                    || skill.CheckSave(level, ch, victim)
                    || (!skill.Flags.IsSet(SkillFlags.ReCastable) && !victim.IsAffectedBy((int)skill.ID)))
                    return;

                if (targetValues.HitVictim && ch != victim)
                {
                    comm.act(ATTypes.AT_MAGIC, skill.HitVictimMessage, ch, null, victim, ToTypes.Victim);
                    if (targetValues.HitRoom)
                    {
                        comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, victim, ToTypes.NotVictim);
                        comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, victim, ToTypes.Character);
                    }
                }
                else if (targetValues.HitRoom)
                    comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, victim, ToTypes.Room);

                if (ch == victim)
                {
                    if (targetValues.HitVictim)
                        comm.act(ATTypes.AT_MAGIC, skill.HitVictimMessage, ch, null, ch, ToTypes.Character);
                    else if (targetValues.HitCharacter)
                        comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, ch, ToTypes.Character);
                }
                else if (targetValues.HitCharacter)
                    comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, victim, ToTypes.Character);
            }

            var retCode = AffectCharacter.spell_affectchar((int)skill.ID, level, ch, victim);
            if (!targetValues.GroupSpell && !targetValues.AreaSpell)
            {
                if (retCode == ReturnTypes.VictimImmune)
                    ch.ImmuneCast(skill, victim);
                else
                    ch.SuccessfulCast(skill, victim);
            }
        }

        private static ReturnTypes CastSingleTargetSpell(SkillData skill, int level, CharacterInstance ch, CharacterInstance victim)
        {
            if (CheckFunctions.CheckIfNullObjectCasting(victim, skill, ch)) return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(
                    (skill.Type != SkillTypes.Herb && victim.Immunity.IsSet(ResistanceTypes.Magic))
                    ||
                    victim.IsImmune(
                        EnumerationExtensions.GetEnum<ResistanceTypes>(Macros.SPELL_DAMAGE(skill))),
                    skill, ch, CastingFunctionType.Immune, victim)) return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(
                    victim.IsAffectedBy((int) skill.ID) && !skill.Flags.IsSet(SkillFlags.Accumulative)
                    && !skill.Flags.IsSet(SkillFlags.ReCastable), skill, ch, CastingFunctionType.Failed, victim))
                return ReturnTypes.SpellFailed;

            var saf = skill.Affects.FirstOrDefault();
            if (CheckFunctions.CheckIfTrueCasting(saf != null && saf.Location == (int) ApplyTypes.StripSN
                                                  && !victim.IsAffectedBy(magic.ParseDiceExpression(ch, saf.Modifier)),
                skill, ch, CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(skill.CheckSave(level, ch, victim), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;

            return ReturnTypes.None;
        }
    }
}