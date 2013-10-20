using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class DelimiterTypeExtensionTest
    {
        [TestCase(DelimiterType.Backslash, "/")]
        [TestCase(DelimiterType.Colon, ":")]
        [TestCase(DelimiterType.Comma, ",")]
        [TestCase(DelimiterType.Equals, "=")]
        [TestCase(DelimiterType.Period, ".")]
        [TestCase(DelimiterType.Punctuation, ",:.=")]
        [TestCase(DelimiterType.Whitespace, "\t\n\r ")]
        public void ValueOfTest(DelimiterType type, string expected)
        {
            Assert.That(type.ValueOf(), Is.EqualTo(expected));
        }
    }
}
