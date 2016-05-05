using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class ChangeSex
    {
        public static ReturnTypes spell_change_sex(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance)vo;
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            if (CheckFunctions.CheckIfTrueCasting(victim.IsImmune(ResistanceTypes.Magic), skill, ch,
                CastingFunctionType.Immune, victim)) return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(victim.IsAffectedBy(sn), skill, ch, CastingFunctionType.Failed, victim))
                return ReturnTypes.SpellFailed;

            var af = new AffectData
            {
                SkillNumber = sn,
                Duration = GetDuration(level),
                Location = ApplyTypes.Gender
            };

            do
            {
                af.Modifier = SmaugRandom.Between(0, 2) - (int) victim.Gender;
            } while (af.Modifier == 0);

            victim.AddAffect(af);
            ch.SuccessfulCast(skill, victim);

            return ReturnTypes.None;
        }

        private static int GetDuration(int level)
        {
            checked
            {
                return 10*level*GameConstants.GetConstant<int>("AffectDurationConversionValue");
            }
        }
    }
}
