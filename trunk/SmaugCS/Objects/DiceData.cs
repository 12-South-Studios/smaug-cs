using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Objects
{
    public class DiceData
    {
        public int NumberOf { get; set; }
        public int SizeOf { get; set; }
        public int Bonus { get; set; }

        public DiceData() {}

        public DiceData(DiceData dice)
        {
            NumberOf = dice.NumberOf;
            SizeOf = dice.SizeOf;
            Bonus = dice.Bonus;
        }
    }
}
