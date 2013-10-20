using NUnit.Framework;
using Realm.Library.Common.Extensions;

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
    }
}
