using NUnit.Framework;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class TablesTests
    {
        /*[Test]
        public void load_tongues_ReturnsValidList()
        {
            var results = tables.load_tongues(new StringReader("* testing~\n\r#common\r\n'test' 'testing'~\r\nabcdefghijklmnopqrstuvwxyz~\n\r'zz' 'z'~\n\r#end"));

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Name, Is.EqualTo("common"));
            Assert.That(results[0].Alphabet, Is.EqualTo("abcdefghijklmnopqrstuvwxyz"));
            Assert.That(results[0].PreConversion, Is.Not.Null);
            Assert.That(results[0].PreConversion.Count, Is.EqualTo(1));
            Assert.That(results[0].PreConversion[0].OldValue, Is.EqualTo("test"));
            Assert.That(results[0].PreConversion[0].NewValue, Is.EqualTo("testing"));
            Assert.That(results[0].Conversion[0].OldValue, Is.EqualTo("zz"));
            Assert.That(results[0].Conversion[0].NewValue, Is.EqualTo("z"));
        }*/
    }
}
