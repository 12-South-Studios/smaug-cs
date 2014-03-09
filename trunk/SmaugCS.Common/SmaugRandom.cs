using System;
using System.Diagnostics.CodeAnalysis;

namespace SmaugCS.Common
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SmaugRandom
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Fuzzy(int number)
        {
            int bits = Bits(2);
            return 1.GetHighestOfTwoNumbers((bits == 0) ? number - 1 : (bits == 3) ? number + 1 : number);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int Between(int from, int to)
        {
            return Realm.Library.Common.Random.Between(from, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int Percent()
        {
            return Realm.Library.Common.Random.D100(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static int Bits(int width)
        {
            return RandomMM() & ((1 << width) - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int RandomMM()
        {
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var random = new Random(Convert.ToInt32(t.TotalSeconds));
            return random.Next();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int RollDice(int number, int size)
        {
            return Realm.Library.Common.Random.Roll(size, number);
        }
    }
}
