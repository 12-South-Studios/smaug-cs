using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class RandomTest
    {
        private static void AssertBetween(int actual, int min, int max)
        {
            Assert.That(actual >= min && actual <= max, Is.True);
        }

        private static int RollTimes(int size, int times)
        {
            switch (size)
            {
                case 100: return Random.D100(times);
                case 20: return Random.D20(times);
                case 12: return Random.D12(times);
                case 10: return Random.D10(times);
                case 8: return Random.D8(times);
                case 6: return Random.D6(times);
                case 4: return Random.D4(times);
            }
            return 0;
        }

        [TestCase(100, 2)]
        [TestCase(20, 2)]
        [TestCase(12, 2)]
        [TestCase(10, 2)]
        [TestCase(8, 2)]
        [TestCase(6, 2)]
        [TestCase(4, 2)]
        [Category("Random Tests")]
        public void RollTimes_TakesTimesAndSize_ReturnsValueBetweenValueRange(int size, int times)
        {
            AssertBetween(RollTimes(size, times), times, size * times);
        }

        [Test]
        [Category("Random Tests")]
        public void Between_TakesMinAndMax_ReturnsResultBetweenValues()
        {
            const int minimum = 1;
            const int maximum = 4;

            AssertBetween(Random.Between(minimum, maximum), minimum, maximum);
        }

        [Test]
        [Category("Random Tests")]
        public void Roll_TakesTimesAndSize_ReturnsResultBetweenValueRange()
        {
            const int times = 2;
            const int size = 5;

            AssertBetween(Random.Roll(size, times), times, size * times);
        }

        [Test]
        [Category("Random Tests")]
        public void RollSeries_TakesTimesAndSize_ReturnsListWithRowCountEqualToTimes()
        {
            const int times = 3;
            const int size = 5;

            var seriesList = Random.RollSeries(size, times);
            Assert.That(seriesList.Count, Is.EqualTo(times));
        }

        [Test]
        [Category("Random Tests")]
        public void RollSeries_TakesTimesAndSize_ReturnsListWithSumBetweenValueRange()
        {
            const int times = 3;
            const int size = 5;

            var seriesList = Random.RollSeries(size, times);
            AssertBetween(seriesList.Sum(), times, size * times);
        }
    }
}
