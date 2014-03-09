using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class CharExtensionsTests
    {
        [TestCase('a', true)]
        [TestCase('e', true)]
        [TestCase('i', true)]
        [TestCase('o', true)]
        [TestCase('u', true)]
        [TestCase('g', false)]
        public void IsVowelTest(char letter, bool expected)
        {
            Assert.That(letter.IsVowel(), Is.EqualTo(expected));
        }

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
