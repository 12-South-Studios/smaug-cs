using NUnit.Framework;
using SmaugCS.Common;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class NumberExtensionTests
    {
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
        public void IsSetTest(int val, int bit, bool expected)
        {
            Assert.That(val.IsSet(bit), Is.EqualTo(expected));
        }

        [Test]
        public void SetBit()
        {
            const int val = 8 | 4;

            Assert.That(val.SetBit(2), Is.EqualTo(14));
        }

        [Test]
        public void RemoveBit()
        {
            const int val = 8 | 4;

            Assert.That(val.RemoveBit(4), Is.EqualTo(8));
        }

        [Test]
        public void ToggleBit()
        {
            const int val = 8 | 4;

            Assert.That(val.ToggleBit(2), Is.EqualTo(14));
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
    }
}
