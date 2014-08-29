using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class Curse
    {
        public static ReturnTypes spell_curse(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance) vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            if (victim.Immunity.IsSet(ResistanceTypes.Magic))
            {
                ch.ImmuneCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            if (victim.IsAffected(AffectedByTypes.Curse) || victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
            {
                ch.FailedCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            AffectData af = AffectData.Create();
            af.SkillNumber = sn;
            af.Duration = ((4 * level) * GameConstants.GetConstant<int>("AffectDurationConversionValue"));
            af.Location = ApplyTypes.HitRoll;
            af.Modifier = -1;
            victim.AddAffect(af);
            
            af = AffectData.Create();
            af.SkillNumber = sn;
            af.Duration = ((4 * level) * GameConstants.GetConstant<int>("AffectDurationConversionValue"));
            af.Location = ApplyTypes.SaveVsSpell;
            af.Modifier = 1;
            victim.AddAffect(af);

            color.set_char_color(ATTypes.AT_MAGIC, victim);
            color.send_to_char("You feel unclean.", victim);

            if (ch != victim)
            {
                comm.act(ATTypes.AT_MAGIC, "You utter a curse upon $N.", ch, null, victim, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n utters a curse upon $N.", ch, null, victim, ToTypes.NotVictim);
            }

            return ReturnTypes.None;
        }
    }
}
