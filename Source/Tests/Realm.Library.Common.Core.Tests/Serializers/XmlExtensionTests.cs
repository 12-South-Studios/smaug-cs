using System;
using FluentAssertions;
using Realm.Library.Common.Serializers;
using Xunit;

namespace Realm.Library.Common.Test.Serializers
{
    public class XmlExtensionTests
    {
        [Serializable]
        public class SerializableObjectFake
        {
            public int IntegerProp { get; set; }
            public string StringProp { get; set; }
        }

        [Fact]
        public void ToXmlNullObjectTest()
        {
            Action act = () => XMLExtensions.ToXML<SerializableObjectFake>(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToXmlTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var result = obj.ToXML();

            const string header = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

            string.IsNullOrEmpty(result).Should().BeFalse();

            var sub = result.Substring(0, header.Length);
            sub.Should().Be(header);
        }

        [Fact]
        public void FromXmlNullStringTest()
        {
            Action act = () => XMLExtensions.FromXML<SerializableObjectFake>("");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FromXmlTest()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><SerializableObjectFake xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><IntegerProp>5</IntegerProp><StringProp>Test</StringProp></SerializableObjectFake>";

            var result = xml.FromXML<SerializableObjectFake>();
            result.Should().NotBeNull();        }
    }
}
