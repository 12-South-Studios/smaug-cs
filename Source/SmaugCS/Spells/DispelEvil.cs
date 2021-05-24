using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class DispelEvil
    {
        public static ReturnTypes spell_dispel_evil(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance)vo;
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            if (!ch.IsNpc() && ch.IsEvil())
                victim = ch;

            if (victim.IsGood())
            {
                comm.act(ATTypes.AT_MAGIC, "Thoric protects $N.", ch, null, victim, ToTypes.Room);
                return ReturnTypes.SpellFailed;
            }

            if (victim.IsNeutral())
            {
                comm.act(ATTypes.AT_MAGIC, "$N does not seem to be affected.", ch, null, victim, ToTypes.Character);
                return ReturnTypes.SpellFailed;
            }

            if (CheckFunctions.CheckIfTrueCasting(victim.IsImmune(ResistanceTypes.Magic), skill, ch,
                CastingFunctionType.Immune, victim)) return ReturnTypes.SpellFailed;

            var damage = SmaugRandom.Roll(level, 4);
            if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                damage /= 2;

            return ch.CauseDamageTo(victim, damage, sn);
        }
    }
}
