using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class SavingThrowDataExtensions
    {
        public static bool CheckSaveVsPoisonDeath(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsPoisonDeath) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsWandRod(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            if (victim.Immunity.IsSet(ResistanceTypes.Magic))
                return true;

            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsWandRod) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsParalysisPetrify(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsParalysisPetrify) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsBreath(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsBreath) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsSpellStaff(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            if (victim.Immunity.IsSet(ResistanceTypes.Magic))
                return true;
            if (victim.IsNpc() && level > 10)
                level -= 5;

            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsSpellStaff) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }
    }
}
