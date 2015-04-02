using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Extensions
{
    public static class SavingThrowDataExtensions
    {
        public static bool CheckSaveVsPoisonDeath(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            var save = 50 + (victim.Level - level - savingThrow.SaveVsPoisonDeath) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsWandRod(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            if (victim.Immunity.IsSet(ResistanceTypes.Magic))
                return true;

            var save = 50 + (victim.Level - level - savingThrow.SaveVsWandRod) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsParalysisPetrify(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            var save = 50 + (victim.Level - level - savingThrow.SaveVsParalysisPetrify) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsBreath(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            var save = 50 + (victim.Level - level - savingThrow.SaveVsBreath) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }

        public static bool CheckSaveVsSpellStaff(this SavingThrowData savingThrow, int level, CharacterInstance victim)
        {
            if (victim.Immunity.IsSet(ResistanceTypes.Magic))
                return true;
            if (victim.IsNpc() && level > 10)
                level -= 5;

            var save = 50 + (victim.Level - level - savingThrow.SaveVsSpellStaff) * 5;
            save = save.GetNumberThatIsBetween(5, 95);
            return victim.Chance(save);
        }
    }
}
