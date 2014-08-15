namespace SmaugCS.Data
{
    public class SavingThrowData
    {
        public static SavingThrowData Clone(SavingThrowData savingThrows)
        {
            return new SavingThrowData(savingThrows);
        }

        public static SavingThrowData Create()
        {
            return new SavingThrowData();
        }

        public int SaveVsPoisonDeath { get; set; }
        public int SaveVsWandRod { get; set; }
        public int SaveVsParalysisPetrify { get; set; }
        public int SaveVsBreath { get; set; }
        public int SaveVsSpellStaff { get; set; }

        private SavingThrowData() { }

        private SavingThrowData(SavingThrowData savingThrows)
        {
            SaveVsBreath = savingThrows.SaveVsBreath;
            SaveVsParalysisPetrify = savingThrows.SaveVsParalysisPetrify;
            SaveVsPoisonDeath = savingThrows.SaveVsPoisonDeath;
            SaveVsSpellStaff = savingThrows.SaveVsSpellStaff;
            SaveVsWandRod = savingThrows.SaveVsWandRod;
        }
    }
}
