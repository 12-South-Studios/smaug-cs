using System;
using System.Diagnostics.CodeAnalysis;

namespace SmaugCS.Common
{
    [ExcludeFromCodeCoverage]
    public static class SmaugRandom
    {
        public static int Fuzzy(int number)
        {
            int bits = Bits(2);
            return 1.GetHighestOfTwoNumbers((bits == 0) ? number - 1 : (bits == 3) ? number + 1 : number);
        }

        public static int Between(int from, int to)
        {
            return Realm.Library.Common.Random.Between(from, to);
        }

        public static int Percent()
        {
            return Realm.Library.Common.Random.D100(1);
        }

        public static int Bits(int width)
        {
            return RandomMM() & ((1 << width) - 1);
        }

        public static int RandomMM()
        {
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var random = new Random(Convert.ToInt32(t.TotalSeconds));
            return random.Next();
        }

        public static int RollDice(int number, int size)
        {
            return Realm.Library.Common.Random.Roll(size, number);
        }
    }
}
