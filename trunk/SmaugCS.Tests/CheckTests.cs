using NUnit.Framework;
using SmaugCS.Common;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class CheckTests
    {
        [TestCase(1, 5, 1)]
        [TestCase(1, -5, -5)]
        public void CheckMinimumTest(int value, int lowValue, int expectedValue)
        {
            Assert.That(value.GetLowestOfTwoNumbers(lowValue), Is.EqualTo(expectedValue));
        }

        [TestCase(5, 1, 5)]
        [TestCase(-5, 1, 1)]
        public void CheckMaximumTest(int value, int hiValue, int expectedValue)
        {
            Assert.That(value.GetHighestOfTwoNumbers(hiValue), Is.EqualTo(expectedValue));
        }

        [TestCase(1, 5, 10, 5)]
        [TestCase(1, -5, 10, 1)]
        [TestCase(1, 15, 10, 10)]
        public void CheckRangeTest(int lowValue, int value, int hiValue, int expectedValue)
        {
            Assert.That(value.GetNumberThatIsBetween(lowValue, hiValue), Is.EqualTo(expectedValue));
        }
    }
}
