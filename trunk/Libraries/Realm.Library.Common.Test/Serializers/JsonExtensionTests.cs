using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Serializers
{
    [TestFixture]
    public class JsonExtensionTests
    {
        [Serializable]
        private class SerializableObjectFake
        {
            public int IntegerProp { get; set; }
            public string StringProp { get; set; }
        }

        [Test]
        public void ToJsonNullObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() => JSONExtensions.ToJSON<SerializableObjectFake>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ToJsonTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var result = obj.ToJSON();

            const string value = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

            Assert.That(string.IsNullOrEmpty(result), Is.False);
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void FromJsonNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => JSONExtensions.FromJSON<SerializableObjectFake>(""),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void FromJsonTest()
        {
            const string json = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

            Assert.That(json.FromJSON<SerializableObjectFake>(), Is.Not.Null);
        }
    }
}
