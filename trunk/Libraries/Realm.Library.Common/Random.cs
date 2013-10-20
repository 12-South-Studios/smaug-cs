using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public static class Random
    {
        /// <summary>
        /// Generates a random number between the minimum and maximum
        /// </summary>
        /// <param name="aMinimum">Minimum value of the roll</param>
        /// <param name="aMaximum">Maximum value of the roll</param>
        /// <returns>Returns an integer value</returns>
        public static int Between(int aMinimum, int aMaximum)
        {
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var random = new System.Random(Convert.ToInt32(t.TotalSeconds));

            return random.Next(aMinimum, aMaximum);
        }

        /// <summary>
        /// Generates a random number that consists of a number
        /// of random rolls using the given size and times.
        /// </summary>
        /// <param name="aSize">Size of each roll</param>
        /// <param name="aTimes">Number of Rolls to make</param>
        /// <returns>Returns an integer value</returns>
        public static int Roll(int aSize, int aTimes)
        {
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var random = new System.Random(Convert.ToInt32(t.TotalSeconds));

            var total = 0;
            Enumerable.Range(0, aTimes).ToList().ForEach(x => total += random.Next(1, aSize));
            return total;
        }

        /// <summary>
        /// Generates a series of random numbers using the
        /// given size and the number of times.
        /// </summary>
        /// <param name="aSize">Size of each roll</param>
        /// <param name="aTimes">Number of Rolls to make</param>
        /// <returns>Returns a list of integers</returns>
        public static IList<int> RollSeries(int aSize, int aTimes)
        {
            var series = new List<int>();
            Enumerable.Range(0, aTimes).ToList().ForEach(x => series.Add(Between(1, aSize)));
            return series;
        }

        /// <summary>
        /// Generates a random number between 1 and 100
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D100(int aTimes)
        {
            return Roll(100, aTimes);
        }

        /// <summary>
        /// Generates a random number between 1 and 20
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D20(int aTimes)
        {
            return Roll(20, aTimes);
        }

        /// <summary>
        /// Generates a random number between 1 and 12
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D12(int aTimes)
        {
            return Roll(12, aTimes);
        }

        /// <summary>
        /// Generates a random number between 1 and 10
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D10(int aTimes)
        {
            return Roll(10, aTimes);
        }

        /// <summary>
        /// Generates a random number between 1 and 8
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D8(int aTimes)
        {
            return Roll(8, aTimes);
        }

        /// <summary>
        /// Generates a random number between 1 and 6
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D6(int aTimes)
        {
            return Roll(6, aTimes);
        }

        /// <summary>
        /// Generates a random number between 1 and 4
        /// </summary>
        /// <param name="aTimes">Number of times to roll the die</param>
        /// <returns>Returns an integer value</returns>
        public static int D4(int aTimes)
        {
            return Roll(4, aTimes);
        }
    }
}