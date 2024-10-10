namespace SmaugCS.Data
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
  }
}