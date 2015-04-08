using NUnit.Framework;

namespace SmaugCS.Language.Tests
{
    [TestFixture]
    public class LanguageConversionDataTests
    {
        [Test]
        public void ConstructorSplitTest()
        {
            var lcv = new LanguageConversionData("OldWord NewWord");

            Assert.That(lcv.NewValue, Is.EqualTo("NewWord"));
            Assert.That(lcv.OldValue, Is.EqualTo("OldWord"));
        }
    }
}
