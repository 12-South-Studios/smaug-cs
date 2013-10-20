using NUnit.Framework;
using SmaugCS.Common;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class CharExtensionTests
    {
        [TestCase('a', false)]
        [TestCase('1', true)]
        public void IsDigitTest(char value, bool expected)
        {
            Assert.That(value.IsDigit(), Is.EqualTo(expected));
        }

        [TestCase('\0', true)]
        [TestCase('a', false)]
        public void IsSpaceTest(char value, bool expected)
        {
            Assert.That(value.IsSpace(), Is.EqualTo(expected));
        }
    }
}
