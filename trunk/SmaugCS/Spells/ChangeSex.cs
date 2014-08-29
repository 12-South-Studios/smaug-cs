using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;
using SmaugCS.Common;

namespace SmaugCS.Spells
{
    public static class ChangeSex
    {
        public static ReturnTypes spell_change_sex(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance)vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            if (victim.IsImmune(ResistanceTypes.Magic))
            {
                ch.ImmuneCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            if (victim.IsAffectedBy(sn))
            {
                ch.FailedCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            AffectData af = AffectData.Create();
            af.SkillNumber = sn;
            af.Duration = (10 * level * GameConstants.GetConstant<int>("AffectDurationConversionValue"));
            af.Location = ApplyTypes.Gender;

            do
            {
                af.Modifier = SmaugRandom.Between(0, 2) - (int) victim.Gender;
            } while (af.Modifier == 0);

            //af.BitVector.ClearBits();

            victim.AddAffect(af);
            ch.SuccessfulCast(skill, victim);

            return ReturnTypes.None;
        }
    }
}
