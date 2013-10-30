using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Enums;

namespace SmaugCS.Objects
{
    public class CharacterMorph
    {
        public MorphData Morph { get; set; }

        public ExtendedBitvector AffectedBy { get; set; }
        public ExtendedBitvector NotAffectedBy { get; set; }

        public int no_immune { get; set; }
        public int no_resistant { get; set; }
        public int no_suscept { get; set; }
        public int immune { get; set; }
        public int resistant { get; set; }
        public int suscept { get; set; }
        public int timer { get; set; }

        public Dictionary<StatisticTypes, int> Statistics { get; set; }
        public SavingThrowData SavingThrows { get; set; }
        public short tumble { get; set; }

        public CharacterMorph()
        {
            Statistics = new Dictionary<StatisticTypes, int>();
            SavingThrows = new SavingThrowData();
        }
    }
}
