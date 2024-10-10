using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Library.Common.Tests;

public class RandomTest
{
  private static void AssertBetween(int actual, int min, int max)
  {
    actual.Should().BeGreaterOrEqualTo(min);
    actual.Should().BeLessOrEqualTo(max);
  }

  private static int RollTimes(int size, int times)
  {
    return size switch
    {
      100 => Random.D100(times),
      20 => Random.D20(times),
      12 => Random.D12(times),
      10 => Random.D10(times),
      8 => Random.D8(times),
      6 => Random.D6(times),
      4 => Random.D4(times),
      _ => 0
    };
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

    IList<int> seriesList = Random.RollSeries(size, times);
    seriesList.Count.Should().Be(3);
  }

  [Fact]
  public void RollSeriesTotalTest()
  {
    const int times = 3;
    const int size = 5;

    IList<int> seriesList = Random.RollSeries(size, times);
    AssertBetween(seriesList.Sum(), times, size * times);
  }
}