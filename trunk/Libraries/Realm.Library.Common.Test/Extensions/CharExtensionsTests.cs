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
        [Category("Extension Tests")]
        public void IsVowelTest(char letter, bool expected)
        {
            Assert.That(letter.IsVowel(), Is.EqualTo(expected));
        }

        [TestCase('a', false)]
        [TestCase('1', true)]
        [Category("Extension Tests")]
        public void IsDigitTest(char value, bool expected)
        {
            Assert.That(value.IsDigit(), Is.EqualTo(expected));
        }

        [TestCase('\0', true)]
        [TestCase('a', false)]
        [Category("Extension Tests")]
        public void IsSpaceTest(char value, bool expected)
        {
            Assert.That(value.IsSpace(), Is.EqualTo(expected));
        }
    }
}
