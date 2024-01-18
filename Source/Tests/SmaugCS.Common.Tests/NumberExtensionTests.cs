using FluentAssertions;
using System;
using Xunit;

namespace SmaugCS.Common.Tests
{

    public class NumberExtensionTests
    {
        private enum FakeEnumWithoutFlags { One, Two }

        [Flags]
        private enum FakeEnumWithFlags { One = 1, Two = 2, Four = 4 }

        [Fact]
        public void ToPercentIntegerReturnsValidValue()
        {
            const int value = 25;
            const int total = 100;
            const string expected = "25.00%";

            value.ToPercent(total).Should().Be(expected);
        }

        [Fact]
        public void ToPercentDoubleReturnsValidValue()
        {
            const double value = 82.5D;
            const double total = 102.56D;
            const string expected = "80.44%";

            value.ToPercent(total).Should().Be(expected);
        }

        [Theory]
        [InlineData(8 | 4, 4, true)]
        [InlineData(8 | 4, 2, false)]
        public void IsSetInt32(int val, int bit, bool expected)
        {
            val.IsSet(bit).Should().Be(expected);
        }

        [Theory]
        [InlineData(2, FakeEnumWithoutFlags.One, false)]
        [InlineData(2, FakeEnumWithFlags.Two, true)]
        public void IsSetEnum(int val, Enum bit, bool expected)
        {
            val.IsSet(bit).Should().Be(expected);
        }

        [Fact]
        public void SetBitInt32()
        {
            const int val = 8 | 4;

            val.SetBit(2).Should().Be(14);
        }

        [Fact]
        public void SetBitEnum()
        {
            const int val = 8 | 4;

            val.SetBit(FakeEnumWithFlags.Two).Should().Be(14);
        }

        [Fact]
        public void RemoveBitInt32()
        {
            const int val = 8 | 4;

            val.RemoveBit(4).Should().Be(8);
        }

        [Fact]
        public void RemoveBitEnum()
        {
            const int val = 8 | 4;

            val.RemoveBit(FakeEnumWithFlags.Four).Should().Be(8);
        }

        [Fact]
        public void ToggleBitInt32()
        {
            const int val = 8 | 4;

            val.ToggleBit(2).Should().Be(14);
        }

        [Fact]
        public void ToggleBitEnum()
        {
            const int val = 8 | 4;

            val.ToggleBit(FakeEnumWithFlags.Two).Should().Be(14);
        }

        [Fact]
        public void GetFlagString()
        {
            const int val = 8 | 4;

            val.GetFlagString(new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }).Should().Be("cd");
        }

        [Fact]
        public void Interpolate()
        {
            const int number = 2;
            const int value00 = 100;
            const int value32 = -100;
            const int expected = 88;

            var actual = number.Interpolate(value00, value32);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 5, 1)]
        [InlineData(1, -5, -5)]
        public void GetLowestOfTwoNumbersInt32(int value, int lowValue, int expectedValue)
        {
            value.GetLowestOfTwoNumbers(lowValue).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(1, 5, 1)]
        [InlineData(1, -5, -5)]
        public void GetLowestOfTwoNumbersLong(long value, long lowValue, long expectedValue)
        {
            value.GetLowestOfTwoNumbers(lowValue).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(5, 1, 5)]
        [InlineData(-5, 1, 1)]
        public void GetHighestOfTwoNumbersInt32(int value, int hiValue, int expectedValue)
        {
            value.GetHighestOfTwoNumbers(hiValue).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(5, 1, 5)]
        [InlineData(-5, 1, 1)]
        public void GetHighestOfTwoNumbersLong(long value, long hiValue, long expectedValue)
        {
            value.GetHighestOfTwoNumbers(hiValue).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(1, 5, 10, 5)]
        [InlineData(1, -5, 10, 1)]
        [InlineData(1, 15, 10, 10)]
        public void GetNumberThatIsBetweenInt32(int lowValue, int value, int hiValue, int expectedValue)
        {
            value.GetNumberThatIsBetween(lowValue, hiValue).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(1, 5, 10, 5)]
        [InlineData(1, -5, 10, 1)]
        [InlineData(1, 15, 10, 10)]
        public void GetNumberThatIsBetweenLong(long lowValue, long value, long hiValue, long expectedValue)
        {
            value.GetNumberThatIsBetween(lowValue, hiValue).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(1, 5, 4)]
        [InlineData(5, 1, 4)]
        [InlineData(5, -5, 10)]
        public void GetAbsoluteDifference(int value1, int value2, int expected)
        {
            value1.GetAbsoluteDifference(value2).Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(1001, "1,001")]
        [InlineData(1001001, "1,001,001")]
        public void ToPunctuation(int value, string expected)
        {
            value.ToPunctuation().Should().Be(expected);
        }
    }
}
