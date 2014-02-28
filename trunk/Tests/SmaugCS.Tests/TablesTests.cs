using NUnit.Framework;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class TablesTests
    {
        [Test]
        public void ConvertStringSyllablesTest()
        {
            const string inputString = "Protect Evil";
            const string outputString = "sfainfra zzur";

            Assert.That(tables.ConvertStringSyllables(inputString), Is.EqualTo(outputString));
        }
    }
}
