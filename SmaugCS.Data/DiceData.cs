namespace SmaugCS.Data
{
    public class DiceData
    {
        public int NumberOf { get; set; }
        public int SizeOf { get; set; }
        public int Bonus { get; set; }

        public DiceData() { }

        public DiceData(DiceData dice)
        {
            NumberOf = dice.NumberOf;
            SizeOf = dice.SizeOf;
            Bonus = dice.Bonus;
        }
    }
}
