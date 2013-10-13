using System.IO;
using NUnit.Framework;
using Realm.Library.Common;
using SmaugCS.Common;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class TextReaderProxyExtensionTests
    {
        [Test]
        public void ReadBitvectorReturnsValidObject()
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader("2&4&8~"));

            ExtendedBitvector result = proxy.ReadBitvector();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Bits[0], Is.EqualTo(2));
            Assert.That(result.Bits[1], Is.EqualTo(4));
            Assert.That(result.Bits[2], Is.EqualTo(8));
        }

        [Test]
        public void ReadBitvectorWithSpacesReturnsValidObject()
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader("2 & 4 & 8~"));

            ExtendedBitvector result = proxy.ReadBitvector();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Bits[0], Is.EqualTo(2));
            Assert.That(result.Bits[1], Is.EqualTo(4));
            Assert.That(result.Bits[2], Is.EqualTo(8));
        }
    }
}
