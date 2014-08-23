using System;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Serializers
{
    [TestFixture]
    public class BinaryExtensionTests
    {
        [Test]
        [Category("Serializer Tests")]
        public void ToBinaryNullObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() => BinaryExtensions.ToBinary<SerializableFakeObject>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Serializer Tests")]
        public void ToBinaryTest()
        {
            var obj = new SerializableFakeObject
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            Assert.That(obj.ToBinary(), Is.Not.Null);
        }

        [Test]
        [Category("Serializer Tests")]
        public void FromBinaryNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => BinaryExtensions.FromBinary<SerializableFakeObject>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Serializer Tests")]
        public void FromBinaryTest()
        {
            var obj = new SerializableFakeObject
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var encoded = obj.ToBinary();

            var result = encoded.FromBinary<SerializableFakeObject>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IntegerProp, Is.EqualTo(obj.IntegerProp));
            Assert.That(result.StringProp, Is.EqualTo(obj.StringProp));
        }
    }
}
