using System;
using FluentAssertions;
using Realm.Library.Common.Extensions;
using Xunit;

namespace Realm.Library.Common.Test.Extensions
{
    public class NumberExtensionsTest
    {
        [Theory]
        [InlineData(5, 1, 10, true, true)]
        [InlineData(5, 1, 4, true, false)]
        [InlineData(5, 1, 10, false, true)]
        [InlineData(5, 1, 5, false, false)]
        public void InRangeInt32Test(int value, int min, int max, bool inclusive, bool expected)
        {
            var result = value.InRange(min, max, inclusive);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 1, 10, true, true)]
        [InlineData(5, 1, 4, true, false)]
        [InlineData(5, 1, 10, false, true)]
        [InlineData(5, 1, 5, false, false)]
        public void InRangeInt64Test(long value, long min, long max, bool inclusive, bool expected)
        {
            var result = value.InRange(min, max, inclusive);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5.2, 1.5, 10.25, true, true)]
        [InlineData(5.2, 1.5, 4.5, true, false)]
        [InlineData(5.2, 1.5, 10.25, false, true)]
        [InlineData(5.2, 1.5, 5.2, false, false)]
        public void InRangeDoubleTest(double value, double min, double max, bool inclusive, bool expected)
        {
            var result = value.InRange(min, max, inclusive);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("10", true)]
        [InlineData("10.000", true)]
        [InlineData("test", false)]
        public void IsNumericTest(string value, bool expected)
        {
            value.IsNumeric().Should().Be(expected);
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(-5, "minus five")]
        [InlineData(1000005, "one million and five")]
        [InlineData(105, "one hundred and five")]
        [InlineData(1024, "one thousand and twenty-four")]
        public void ToWordsTest(int number, string expected)
        {
            number.ToWords().Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "one")]
        [InlineData(2, "two")]
        [InlineData(3, "three")]
        [InlineData(4, "four")]
        [InlineData(5, "five")]
        [InlineData(6, "six")]
        [InlineData(7, "seven")]
        [InlineData(8, "eight")]
        [InlineData(9, "nine")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(12, "twelve")]
        [InlineData(15, "three")]
        public void ConvertHourTest(int number, string expected)
        {
            number.ConvertHour().Should().Be(expected);
        }

        [Fact]
        public void ConvertHour_ThrowsException_WhenHourIsTooLow()
        {
            Action act = () => (-1).ConvertHour();
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ConvertHour_ThrowsException_WhenHourIsTooHigh()
        {
            Action act = () => 25.ConvertHour();
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(21, "21st")]
        [InlineData(32, "32nd")]
        [InlineData(103, "103rd")]
        [InlineData(12, "12th")]
        public void GetOrdinalTest(int number, string expected)
        {
            number.GetOrdinal().Should().Be(expected);
        }

        [Theory]
        [InlineData(22, "at night")]
        [InlineData(3, "at night")]
        [InlineData(9, "in the morning")]
        [InlineData(15, "in the afternoon")]
        [InlineData(19, "in the evening")]
        public void ToPeriodOfDayTest(int number, string expected)
        {
            number.ToPeriodOfDay().Should().Be(expected);
        }
    }
}
