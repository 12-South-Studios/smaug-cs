using System;
using System.Diagnostics.CodeAnalysis;

namespace SmaugCS.Common
{
    [ExcludeFromCodeCoverage]
    public static class SmaugRandom
    {
        #region Smaug Specific
        public static int Fuzzy(int number)
        {
            int bits = Bits(2);
            return 1.GetHighestOfTwoNumbers((bits == 0) ? number - 1 : (bits == 3) ? number + 1 : number);
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
        #endregion

        #region Dice
        public static int Between(int from, int to)
        {
            return Realm.Library.Common.Random.Between(from, to);
        }

        public static int D4(int times = 1)
        {
            return Realm.Library.Common.Random.D4(times);
        }

        public static int D6(int times = 1)
        {
            return Realm.Library.Common.Random.D6(times);
        }

        public static int D8(int times = 1)
        {
            return Realm.Library.Common.Random.D8(times);
        }

        public static int D10(int times = 1)
        {
            return Realm.Library.Common.Random.D10(times);
        }

        public static int D12(int times = 1)
        {
            return Realm.Library.Common.Random.D12(times);
        }

        public static int D20(int times = 1)
        {
            return Realm.Library.Common.Random.D20(times);
        }

        public static int D100(int times = 1)
        {
            return Realm.Library.Common.Random.D100(times);
        }

        public static int Roll(int size, int times)
        {
            return Realm.Library.Common.Random.Roll(size, times);
        }
        #endregion
    }
}
