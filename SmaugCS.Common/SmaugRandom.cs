using System;
using System.Diagnostics.CodeAnalysis;

namespace SmaugCS.Common
{
    [ExcludeFromCodeCoverage]
    public static class SmaugRandom
    {
        /// <summary>
        /// Fuzzifies the given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Fuzzy(int number)
        {
            int nbr = number;
            switch (Bits(2))
            {
                case 0:
                    nbr -= 1;
                    break;
                case 3:
                    nbr += 1;
                    break;
            }

            return Check.Maximum(1, nbr);
        }

        /// <summary>
        /// Generates a random number between two values
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int Between(int from, int to)
        {
            return Realm.Library.Common.Random.Between(from, to);
        }

        /// <summary>
        /// Generates a random percent value between 1 and 100
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Simulates rolling a dice of the given size a number of times and 
        /// returns the sum of all of the rolls.
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
