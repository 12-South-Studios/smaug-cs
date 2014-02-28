using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class TextReaderProxyTests
    {
        [Test]
        public void ReadNextLetter_ReturnsValidChar()
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader("a word"));

            Assert.That(proxy.ReadNextLetter(), Is.EqualTo('a'));
        }

        [Test]
        public void ReadNextLetter_ThrowsExceptionAtEndOfStream()
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader(""));

            Assert.Throws<IOException>(() => proxy.ReadNextLetter(),
                                       "Unit test expected an IOException to be thrown");
        }

        [Test]
        public void ReadNextWord_ReturnsValidString()
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader("first word"));

            Assert.That(proxy.ReadNextWord(), Is.EqualTo("first"));
        }

        [Test]
        public void ReadNumber_ReturnsValidValue()
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader("100 words"));
    
            Assert.That(proxy.ReadNumber(), Is.EqualTo(100));
        }

        [Test]
        public void ReadIntoList_ReturnsValidList()
        {
            TextReaderProxy proxy =
                new TextReaderProxy(new StringReader("#common\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r"));

            List<string> results = proxy.ReadIntoList();

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results[0], Is.EqualTo("#common"));
            Assert.That(results[3], Is.EqualTo("~"));
        }

        [Test]
        public void ReadSections_HasInvalidSectionParameters()
        {
            TextReaderProxy proxy =
                new TextReaderProxy(new StringReader("#common\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r#uncommon\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r#end\n\r"));

            List<TextSection> results = proxy.ReadSections(new List<string> {"#"}, new List<string> {"*"},
                                                           new List<string> {"#"}, "#end");

            Assert.That(results.Count, Is.EqualTo(0));
        }

        [Test]
        public void ReadSections_HasFooter()
        {
            TextReaderProxy proxy =
                new TextReaderProxy(new StringReader("#common\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\r\n^end_common\n\r#uncommon\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r^end_uncommon\r\n#end\n\r"));

            List<TextSection> results = proxy.ReadSections(new List<string> { "#" }, new List<string> { "*" },
                                                           new List<string> { "^" }, "#end");

            Assert.That(results, Is.Not.Null);
            Assert.That(results[0].Footer, Is.EqualTo("end_common"));
        }

        [Test]
        public void ReadSections_ReturnsValidList()
        {
            TextReaderProxy proxy =
                new TextReaderProxy(new StringReader("#common\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r#uncommon\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r#end\n\r"));

            List<TextSection> results = proxy.ReadSections(new List<string> {"#"}, new List<string> {"*"}, null, "#end");

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results[0].Header, Is.EqualTo("common"));
            Assert.That(results[0].Lines.Count, Is.EqualTo(3));
        }

        [Test]
        public void ReadEndOfLine_DoesntReadNextLine()
        {
            TextReaderProxy proxy =
                   new TextReaderProxy(new StringReader("This is a test\n\rwith two lines."));

            Assert.That(proxy.ReadNextLetter(), Is.EqualTo('T'));
            Assert.That(proxy.ReadToEndOfLine(true), Is.EqualTo("his is a test"));
            Assert.That(proxy.ReadLine(), Is.EqualTo("with two lines."));
        }
    }
}
