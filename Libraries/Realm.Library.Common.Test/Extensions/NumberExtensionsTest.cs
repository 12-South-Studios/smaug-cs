using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class NumberExtensionsTest
    {
        [TestCase("10", true)]
        [TestCase("10.000", true)]
        [TestCase("test", false)]
        public void IsNumericTest(string value, bool expected)
        {
            Assert.That(value.IsNumeric(), Is.EqualTo(expected));
        }

        [TestCase(0, "zero")]
        [TestCase(-5, "minus five")]
        [TestCase(1000005, "one million and five")]
        [TestCase(105, "one hundred and five")]
        [TestCase(1024, "one thousand and twenty-four")]
        public void ToWordsTest(int number, string expected)
        {
            Assert.That(number.ToWords(), Is.EqualTo(expected));
        }

        [TestCase(1, "one")]
        [TestCase(2, "two")]
        [TestCase(3, "three")]
        [TestCase(4, "four")]
        [TestCase(5, "five")]
        [TestCase(6, "six")]
        [TestCase(7, "seven")]
        [TestCase(8, "eight")]
        [TestCase(9, "nine")]
        [TestCase(10, "ten")]
        [TestCase(11, "eleven")]
        [TestCase(12, "twelve")]
        [TestCase(15, "three")]
        public void ConvertHourTest(int number, string expected)
        {
            Assert.That(number.ConvertHour(), Is.EqualTo(expected));
        }

        [TestCase(-1, ExpectedException = typeof(ArgumentException), ExpectedMessage = "Invalid hour (must be between 0 and 24)")]
        [TestCase(25, ExpectedException = typeof(ArgumentException), ExpectedMessage = "Invalid hour (must be between 0 and 24)")]
        public void ConvertHourInvalidTest(int number)
        {
            number.ConvertHour();

            Assert.Fail("Unit test expected an ArgumentException to be thrown");
        }

        [TestCase(21, "21st")]
        [TestCase(32, "32nd")]
        [TestCase(103, "103rd")]
        [TestCase(12, "12th")]
        public void GetOrdinalTest(int number, string expected)
        {
            Assert.That(number.GetOrdinal(), Is.EqualTo(expected));
        }

        [TestCase(22, "at night")]
        [TestCase(3, "at night")]
        [TestCase(9, "in the morning")]
        [TestCase(15, "in the afternoon")]
        [TestCase(19, "in the evening")]
        public void ToPeriodOfDayTest(int number, string expected)
        {
            Assert.That(number.ToPeriodOfDay(), Is.EqualTo(expected));
        }
    }
}
