using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class Curse
    {
        public static ReturnTypes spell_curse(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance) vo;
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            if (CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), skill, ch,
                CastingFunctionType.Immune, victim)) return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(victim.IsAffected(AffectedByTypes.Curse) 
                || victim.SavingThrows.CheckSaveVsSpellStaff(level, victim), skill, ch, 
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;

            AddAffectToTarget(ch, sn, level, ApplyTypes.HitRoll);
            AddAffectToTarget(victim, sn, level, ApplyTypes.SaveVsSpell);

           victim.SetColor(ATTypes.AT_MAGIC);
           victim.SendTo("You feel unclean.");

            if (ch != victim)
            {
                comm.act(ATTypes.AT_MAGIC, "You utter a curse upon $N.", ch, null, victim, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n utters a curse upon $N.", ch, null, victim, ToTypes.NotVictim);
            }

            return ReturnTypes.None;
        }

        private static void AddAffectToTarget(CharacterInstance ch, int sn, int level, ApplyTypes appLocation)
        {
            int duration;
            checked
            {
                duration = (4 * level) * GameConstants.GetConstant<int>("AffectDurationConversionValue");
            }

            var af = new AffectData
            {
                SkillNumber = sn,
                Duration = duration,
                Location = appLocation,
                Modifier = -1
            };
            ch.AddAffect(af);
        }
    }
}
