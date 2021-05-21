using FluentAssertions;
using System.Linq;
using Xunit;

namespace Realm.Library.Common.Test
{
    public class RandomTest
    {
        private static void AssertBetween(int actual, int min, int max)
        {
            actual.Should().BeGreaterOrEqualTo(min);
            actual.Should().BeLessOrEqualTo(max);
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

        [Theory]
        [InlineData(100, 2)]
        [InlineData(20, 2)]
        [InlineData(12, 2)]
        [InlineData(10, 2)]
        [InlineData(8, 2)]
        [InlineData(6, 2)]
        [InlineData(4, 2)]
        public void RollTimesTest(int size, int times)
        {
            AssertBetween(RollTimes(size, times), times, size * times);
        }

        [Fact]
        public void BetweenTest()
        {
            const int minimum = 1;
            const int maximum = 4;

            AssertBetween(Random.Between(minimum, maximum), minimum, maximum);
        }

        [Fact]
        public void RollTest()
        {
            const int times = 2;
            const int size = 5;

            AssertBetween(Random.Roll(size, times), times, size * times);
        }

        [Fact]
        public void RollSeriesCountTest()
        {
            const int times = 3;
            const int size = 5;

            var seriesList = Random.RollSeries(size, times);
            seriesList.Count.Should().Be(3);
        }

        [Fact]
        public void RollSeriesTotalTest()
        {
            const int times = 3;
            const int size = 5;

            var seriesList = Random.RollSeries(size, times);
            AssertBetween(seriesList.Sum(), times, size * times);
        }
    }
}
