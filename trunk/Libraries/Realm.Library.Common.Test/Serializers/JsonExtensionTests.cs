using System;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Serializers
{
    [TestFixture]
    public class JsonExtensionTests
    {
        [Test]
        [Category("Serializer Tests")]
        public void ToJsonNullObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() => JSONExtensions.ToJSON<SerializableFakeObject>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Serializer Tests")]
        public void ToJsonTest()
        {
            var obj = new SerializableFakeObject
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
        [Category("Serializer Tests")]
        public void FromJsonNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => JSONExtensions.FromJSON<SerializableFakeObject>(""),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Serializer Tests")]
        public void FromJsonTest()
        {
            const string json = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

            Assert.That(json.FromJSON<SerializableFakeObject>(), Is.Not.Null);
        }
    }
}
