using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;

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
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);

            if (!skill.Affects.Any())
            {
                // TODO Exception, log it
                return ReturnTypes.None;
            }

            TargetBooleanValues target = new TargetBooleanValues();
            if (skill.Flags.IsSet(SkillFlags.GroupSpell))
                target.GroupSpell = true;
            if (skill.Flags.IsSet(SkillFlags.Area))
                target.AreaSpell = true;

            CharacterInstance victim = (CharacterInstance) vo;

            if (!target.GroupSpell && !target.AreaSpell)
            {
                ReturnTypes retCode = CastSingleTargetSpell(skill, level, ch, victim);
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

            if (victim == null)
            {
                // TODO Exception, log it
                magic.failed_casting(skill, ch, null, null);
                return ReturnTypes.SpellFailed;
            }

            foreach (CharacterInstance vch in victim.CurrentRoom.Persons)
            {
                CastTargetSpellAtVictim(ch, vch, skill, target, level);
                if (!target.GroupSpell && !target.AreaSpell)
                    break;
            }

            return ReturnTypes.None;
        }

        private static void CastTargetSpellAtVictim(CharacterInstance ch, CharacterInstance victim, SkillData skill, TargetBooleanValues targetValues, int level)
        {
            if (targetValues.GroupSpell || targetValues.AreaSpell)
            {
                ResistanceTypes resType = Realm.Library.Common.EnumerationExtensions.GetEnum<ResistanceTypes>(Macros.SPELL_DAMAGE(skill));
                if ((targetValues.GroupSpell
                     && !victim.IsSameGroup(ch))
                    || victim.Immunity.IsSet(ResistanceTypes.Magic)
                    || victim.IsImmune(resType)
                    || skill.CheckSave(level, ch, victim)
                    || (!skill.Flags.IsSet(SkillFlags.ReCastable) && !victim.IsAffectedBy((int) skill.ID)))
                    return;

                if (targetValues.HitVictim && !ch.Equals(victim))
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

                if (ch.Equals(victim))
                {
                    if (targetValues.HitVictim)
                        comm.act(ATTypes.AT_MAGIC, skill.HitVictimMessage, ch, null, ch, ToTypes.Character);
                    else if (targetValues.HitCharacter)
                        comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, ch, ToTypes.Character);
                }
                else if (targetValues.HitCharacter)
                    comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, victim, ToTypes.Character);
            }

            ReturnTypes retCode = AffectCharacter.spell_affectchar((int)skill.ID, level, ch, victim);
            if (!targetValues.GroupSpell && !targetValues.AreaSpell)
            {
                if (retCode == ReturnTypes.VictimImmune)
                    magic.immune_casting(skill, ch, victim, null);
                else 
                    magic.successful_casting(skill, ch, victim, null);
            }
        }

        private static ReturnTypes CastSingleTargetSpell(SkillData skill, int level, CharacterInstance ch, CharacterInstance victim)
        {
            if (victim == null)
            {
                magic.failed_casting(skill, ch, null, null);
                return ReturnTypes.SpellFailed;
            }

            if ((skill.Type != SkillTypes.Herb
                 && victim.Immunity.IsSet(ResistanceTypes.Magic))
                ||
                victim.IsImmune(
                    Realm.Library.Common.EnumerationExtensions.GetEnum<ResistanceTypes>(Macros.SPELL_DAMAGE(skill))))
            {
                magic.immune_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (victim.IsAffectedBy((int) skill.ID)
                && !skill.Flags.IsSet(SkillFlags.Accumulative)
                && !skill.Flags.IsSet(SkillFlags.ReCastable))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            SmaugAffect saf = skill.Affects.FirstOrDefault();
            if (saf != null && saf.Location == (int) ApplyTypes.StripSN
                && !victim.IsAffectedBy(magic.dice_parse(ch, level, saf.Modifier)))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (skill.CheckSave(level, ch, victim))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            return ReturnTypes.None;
        }
    }
}
