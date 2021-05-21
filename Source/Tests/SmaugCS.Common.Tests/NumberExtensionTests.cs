using NUnit.Framework;
using System;

namespace SmaugCS.Common.Tests
{
    [TestFixture]
    public class NumberExtensionTests
    {
        private enum FakeEnumWithoutFlags { One, Two }

        [Flags]
        private enum FakeEnumWithFlags { One = 1, Two = 2, Four = 4 }

        [Test]
        public void ToPercentIntegerReturnsValidValue()
        {
            const int value = 25;
            const int total = 100;
            const string expected = "25.00%";

            Assert.That(value.ToPercent(total), Is.EqualTo(expected));
        }

        [Test]
        public void ToPercentDoubleReturnsValidValue()
        {
            const double value = 82.5D;
            const double total = 102.56D;
            const string expected = "80.44%";

            Assert.That(value.ToPercent(total), Is.EqualTo(expected));
        }

        [TestCase(8 | 4, 4, true)]
        [TestCase(8 | 4, 2, false)]
        public void IsSetInt32(int val, int bit, bool expected)
        {
            Assert.That(val.IsSet(bit), Is.EqualTo(expected));
        }

        [TestCase(2, FakeEnumWithoutFlags.One, false)]
        [TestCase(2, FakeEnumWithFlags.Two, true)]
        public void IsSetEnum(int val, Enum bit, bool expected)
        {
            Assert.That(val.IsSet(bit), Is.EqualTo(expected));
        }

        [Test]
        public void SetBitInt32()
        {
            const int val = 8 | 4;

            Assert.That(val.SetBit(2), Is.EqualTo(14));
        }

        [Test]
        public void SetBitEnum()
        {
            const int val = 8 | 4;

            Assert.That(val.SetBit(FakeEnumWithFlags.Two), Is.EqualTo(14));
        }

        [Test]
        public void RemoveBitInt32()
        {
            const int val = 8 | 4;

            Assert.That(val.RemoveBit(4), Is.EqualTo(8));
        }

        [Test]
        public void RemoveBitEnum()
        {
            const int val = 8 | 4;

            Assert.That(val.RemoveBit(FakeEnumWithFlags.Four), Is.EqualTo(8));
        }

        [Test]
        public void ToggleBitInt32()
        {
            const int val = 8 | 4;

            Assert.That(val.ToggleBit(2), Is.EqualTo(14));
        }

        [Test]
        public void ToggleBitEnum()
        {
            const int val = 8 | 4;

            Assert.That(val.ToggleBit(FakeEnumWithFlags.Two), Is.EqualTo(14));
        }

        [Test]
        public void GetFlagString()
        {
            const int val = 8 | 4;

            Assert.That(val.GetFlagString(new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }), Is.EqualTo("cd"));
        }

        [Test]
        public void Interpolate()
        {
            const int number = 2;
            const int value00 = 100;
            const int value32 = -100;
            const int expected = 88;

            var actual = number.Interpolate(value00, value32);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(1, 5, 1)]
        [TestCase(1, -5, -5)]
        public void GetLowestOfTwoNumbersInt32(int value, int lowValue, int expectedValue)
        {
            Assert.That(value.GetLowestOfTwoNumbers(lowValue), Is.EqualTo(expectedValue));
        }

        [TestCase(1, 5, 1)]
        [TestCase(1, -5, -5)]
        public void GetLowestOfTwoNumbersLong(long value, long lowValue, long expectedValue)
        {
            Assert.That(value.GetLowestOfTwoNumbers(lowValue), Is.EqualTo(expectedValue));
        }

        [TestCase(5, 1, 5)]
        [TestCase(-5, 1, 1)]
        public void GetHighestOfTwoNumbersInt32(int value, int hiValue, int expectedValue)
        {
            Assert.That(value.GetHighestOfTwoNumbers(hiValue), Is.EqualTo(expectedValue));
        }

        [TestCase(5, 1, 5)]
        [TestCase(-5, 1, 1)]
        public void GetHighestOfTwoNumbersLong(long value, long hiValue, long expectedValue)
        {
            Assert.That(value.GetHighestOfTwoNumbers(hiValue), Is.EqualTo(expectedValue));
        }

        [TestCase(1, 5, 10, 5)]
        [TestCase(1, -5, 10, 1)]
        [TestCase(1, 15, 10, 10)]
        public void GetNumberThatIsBetweenInt32(int lowValue, int value, int hiValue, int expectedValue)
        {
            Assert.That(value.GetNumberThatIsBetween(lowValue, hiValue), Is.EqualTo(expectedValue));
        }

        [TestCase(1, 5, 10, 5)]
        [TestCase(1, -5, 10, 1)]
        [TestCase(1, 15, 10, 10)]
        public void GetNumberThatIsBetweenLong(long lowValue, long value, long hiValue, long expectedValue)
        {
            Assert.That(value.GetNumberThatIsBetween(lowValue, hiValue), Is.EqualTo(expectedValue));
        }

        [TestCase(1, 5, 4)]
        [TestCase(5, 1, 4)]
        [TestCase(5, -5, 10)]
        public void GetAbsoluteDifference(int value1, int value2, int expected)
        {
            Assert.That(value1.GetAbsoluteDifference(value2), Is.EqualTo(expected));
        }

        [TestCase(1, "1")]
        [TestCase(1001, "1,001")]
        [TestCase(1001001, "1,001,001")]
        public void ToPunctuation(int value, string expected)
        {
            Assert.That(value.ToPunctuation(), Is.EqualTo(expected));
        }
    }
}
