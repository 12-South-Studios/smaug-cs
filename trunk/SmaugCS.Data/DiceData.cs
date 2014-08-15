namespace SmaugCS.Data
{
    public class DiceData
    {
        public static DiceData Clone(DiceData dice)
        {
            return new DiceData(dice);
        }

        public static DiceData Create()
        {
            return new DiceData();
        }

        public int NumberOf { get; set; }
        public int SizeOf { get; set; }
        public int Bonus { get; set; }

        private DiceData() { }

        private DiceData(DiceData dice)
        {
            NumberOf = dice.NumberOf;
            SizeOf = dice.SizeOf;
            Bonus = dice.Bonus;
        }
    }
}
