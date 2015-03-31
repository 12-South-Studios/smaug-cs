using System.IO;
using NUnit.Framework;
using Realm.Library.Common;

namespace SmaugCS.Common.Tests
{
    [TestFixture]
    public class TextReaderProxyExtensionTests
    {
        [TestCase("2&4&8~", 2, true)]
        [TestCase("2&4&8~", 4, true)]
        [TestCase("2&4&8~", 8, true)]
        [TestCase("2&4&8~", 16, false)]
        public void ReadBitvectorReturnsValidObject(string stringToRead, int valueToCheck, bool expectedValue)
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader(stringToRead));

            ExtendedBitvector result = proxy.ReadBitvector();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSet((ulong)valueToCheck), Is.EqualTo(expectedValue));
        }

        [TestCase("2 & 4 & 8~", 2, true)]
        [TestCase("2 & 4 & 8~", 4, true)]
        [TestCase("2 & 4 & 8~", 8, true)]
        [TestCase("2 & 4 & 8~", 16, false)]
        public void ReadBitvectorWithSpacesReturnsValidObject(string stringToRead, int valueToCheck, bool expectedValue)
        {
            TextReaderProxy proxy = new TextReaderProxy(new StringReader("2 & 4 & 8~"));

            ExtendedBitvector result = proxy.ReadBitvector();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSet((ulong)valueToCheck), Is.EqualTo(expectedValue));
        }
    }
}
