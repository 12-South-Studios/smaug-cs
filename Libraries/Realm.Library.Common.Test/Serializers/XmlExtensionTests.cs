using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Serializers
{
    [TestFixture]
    public class XmlExtensionTests
    {
        [Serializable]
        public class SerializableObjectFake
        {
            public int IntegerProp { get; set; }
            public string StringProp { get; set; }
        }

        [Test]
        public void ToXmlNullObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() => XMLExtensions.ToXML<SerializableObjectFake>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ToXmlTest()
        {
            var obj = new SerializableObjectFake
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
        public void FromXmlNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => XMLExtensions.FromXML<SerializableObjectFake>(""),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void FromXmlTest()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><SerializableObjectFake xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><IntegerProp>5</IntegerProp><StringProp>Test</StringProp></SerializableObjectFake>";

            Assert.That(xml.FromXML<SerializableObjectFake>(), Is.Not.Null);
        }
    }
}
