using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Extensions
{
    public static class SkillDataExtensions
    {
        public static bool CheckSave(this SkillData skill, int level, CharacterInstance ch, CharacterInstance victim)
        {
            bool saved = false;
            int localLevel = level;

            if (skill.Flags.IsSet(SkillFlags.PKSensitive) && !ch.IsNpc() && !victim.IsNpc())
                localLevel /= 2;

            switch (skill.SaveVs)
            {
                case SaveVsTypes.PoisonOrDeath:
                    saved = victim.SavingThrows.CheckSaveVsPoisonDeath(localLevel, victim);
                    break;
                case SaveVsTypes.RodsOrWands:
                    saved = victim.SavingThrows.CheckSaveVsWandRod(localLevel, victim);
                    break;
                case SaveVsTypes.ParalysisOrPetrification:
                    saved = victim.SavingThrows.CheckSaveVsParalysisPetrify(localLevel, victim);
                    break;
                case SaveVsTypes.Breath:
                    saved = victim.SavingThrows.CheckSaveVsBreath(localLevel, victim);
                    break;
                case SaveVsTypes.SpellsOrStaves:
                    saved = victim.SavingThrows.CheckSaveVsSpellStaff(localLevel, victim);
                    break;
            }

            return saved;
        }
    }
}
