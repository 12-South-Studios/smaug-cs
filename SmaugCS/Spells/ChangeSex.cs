using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data;

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
                magic.immune_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (victim.IsAffectedBy(sn))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            AffectData af = new AffectData
            {
                SkillNumber = sn,
                Duration = (10*level*GameConstants.GetIntegerConstant("AffectDurationConversionValue")),
                Location = ApplyTypes.Gender
            };

            do
            {
                af.Modifier = SmaugRandom.Between(0, 2) - (int) victim.Gender;
            } while (af.Modifier == 0);

            //af.BitVector.ClearBits();

            victim.AddAffect(af);
            magic.successful_casting(skill, ch, victim, null);

            return ReturnTypes.None;
        }
    }
}
