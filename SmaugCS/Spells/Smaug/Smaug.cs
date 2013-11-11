using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Spells.Smaug
{
    class Smaug
    {
        public static bool check_save(int sn, int level, CharacterInstance ch, CharacterInstance victim)
        {
            SkillData skill = db.GetSkill(sn);
            bool saved = false;

            if (Macros.SPELL_FLAG(skill, (int)SkillFlags.PKSensitive)
                && !ch.IsNpc() && !victim.IsNpc())
                level /= 2;

            switch (skill.SaveVs)
            {
                case SaveVsTypes.PoisonOrDeath:
                    saved = victim.SavingThrows.CheckSaveVsPoisonDeath(level, victim);
                    break;
                case SaveVsTypes.RodsOrWands:
                    saved = victim.SavingThrows.CheckSaveVsWandRod(level, victim);
                    break;
                case SaveVsTypes.ParalysisOrPetrification:
                    saved = victim.SavingThrows.CheckSaveVsParalysisPetrify(level, victim);
                    break;
                case SaveVsTypes.Breath:
                    saved = victim.SavingThrows.CheckSaveVsBreath(level, victim);
                    break;
                case SaveVsTypes.SpellsOrStaves:
                    saved = victim.SavingThrows.CheckSaveVsSpellStaff(level, victim);
                    break;
            }

            return saved;
        }

        public static ReturnTypes spell_smaug(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO
            return ReturnTypes.None;
        }
    }
}
