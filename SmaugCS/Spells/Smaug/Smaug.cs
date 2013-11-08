using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Spells.Smaug
{
    class Smaug
    {
        public static bool check_save(int sn, int level, CharacterInstance ch, CharacterInstance victim)
        {
            SkillData skill = db.GetSkill(sn);
            bool saved = false;

            if (Macros.SPELL_FLAG(skill, (int) SkillFlags.PKSensitive)
                && !ch.IsNpc() && !victim.IsNpc())
                level /= 2;

            switch (skill.SaveVs)
            {
                case SaveVsTypes.PoisonOrDeath:
                    saved = SavingThrowData.CheckSaveVsPoisonDeath(level, victim);
                    break;
                case SaveVsTypes.RodsOrWands:
                    saved = SavingThrowData.CheckSaveVsWandRod(level, victim);
                    break;
                case SaveVsTypes.ParalysisOrPetrification:
                    saved = SavingThrowData.CheckSaveVsParalysisPetrify(level, victim);
                    break;
                case SaveVsTypes.Breath:
                    saved = SavingThrowData.CheckSaveVsBreath(level, victim);
                    break;
                case SaveVsTypes.SpellsOrStaves:
                    saved = SavingThrowData.CheckSaveVsSpellStaff(level, victim);
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
