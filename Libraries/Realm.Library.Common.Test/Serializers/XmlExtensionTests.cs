using System;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Serializers
{
    [TestFixture]
    public class XmlExtensionTests
    {
        [Test]
        [Category("Serializer Tests")]
        public void ToXmlNullObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() => XMLExtensions.ToXML<SerializableFakeObject>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Serializer Tests")]
        public void ToXmlTest()
        {
            var obj = new SerializableFakeObject
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var result = obj.ToXML();

            const string header = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

            Assert.That(string.IsNullOrEmpty(result), Is.False);
            Assert.That(result.Substring(0, header.Length), Is.EqualTo(header));
        }

        [Test]
        [Category("Serializer Tests")]
        public void FromXmlNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => XMLExtensions.FromXML<SerializableFakeObject>(""),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Serializer Tests")]
        public void FromXmlTest()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><SerializableFakeObject xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><IntegerProp>5</IntegerProp><StringProp>Test</StringProp></SerializableFakeObject>";

            Assert.That(xml.FromXML<SerializableFakeObject>(), Is.Not.Null);
        }
    }
}
