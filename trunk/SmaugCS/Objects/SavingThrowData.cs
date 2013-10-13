
using SmaugCS.Common;
using SmaugCS.Enums;

namespace SmaugCS.Objects
{
    public class SavingThrowData
    {
        public int SaveVsPoisonDeath { get; set; }
        public int SaveVsWandRod { get; set; }
        public int SaveVsParalysisPetrify { get; set; }
        public int SaveVsBreath { get; set; }
        public int SaveVsSpellStaff { get; set; }

        public SavingThrowData() { }

        public SavingThrowData(SavingThrowData savingThrows)
        {
            SaveVsBreath = savingThrows.SaveVsBreath;
            SaveVsParalysisPetrify = savingThrows.SaveVsParalysisPetrify;
            SaveVsPoisonDeath = savingThrows.SaveVsPoisonDeath;
            SaveVsSpellStaff = savingThrows.SaveVsSpellStaff;
            SaveVsWandRod = savingThrows.SaveVsWandRod;
        }

        public static bool CheckSaveVsPoisonDeath(int level, CharacterInstance victim)
        {
            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsPoisonDeath) * 5;
            save = Check.Range(5, save, 95);
            return handler.chance(victim, (short)save);
        }

        public static bool CheckSaveVsWandRod(int level, CharacterInstance victim)
        {
            if (victim.Immunity.IsSet((int)ResistanceTypes.Magic))
                return true;

            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsWandRod) * 5;
            save = Check.Range(5, save, 95);
            return handler.chance(victim, (short)save);
        }

        public static bool CheckSaveVsParalysisPetrify(int level, CharacterInstance victim)
        {
            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsParalysisPetrify) * 5;
            save = Check.Range(5, save, 95);
            return handler.chance(victim, (short)save);
        }

        public static bool CheckSaveVsBreath(int level, CharacterInstance victim)
        {
            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsBreath) * 5;
            save = Check.Range(5, save, 95);
            return handler.chance(victim, (short)save);
        }

        public static bool CheckSaveVsSpellStaff(int level, CharacterInstance victim)
        {
            if (victim.Immunity.IsSet((int)ResistanceTypes.Magic))
                return true;
            if (victim.IsNpc() && level > 10)
                level -= 5;

            int save = 50 + (victim.Level - level - victim.SavingThrows.SaveVsSpellStaff) * 5;
            save = Check.Range(5, save, 95);
            return handler.chance(victim, (short)save);
        }
    }
}
