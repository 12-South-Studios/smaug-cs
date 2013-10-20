using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Serializers
{
    [TestFixture]
    public class BinaryExtensionTests
    {
        [Serializable]
        private class SerializableObjectFake
        {
            public int IntegerProp { get; set; }
            public string StringProp { get; set; }
        }

        [Test]
        public void ToBinaryNullObjectTest()
        {
            Assert.Throws<ArgumentNullException>(() => BinaryExtensions.ToBinary<SerializableObjectFake>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ToBinaryTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            Assert.That(obj.ToBinary(), Is.Not.Null);
        }

        [Test]
        public void FromBinaryNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => BinaryExtensions.FromBinary<SerializableObjectFake>(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void FromBinaryTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var encoded = obj.ToBinary();

            var result = encoded.FromBinary<SerializableObjectFake>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IntegerProp, Is.EqualTo(obj.IntegerProp));
            Assert.That(result.StringProp, Is.EqualTo(obj.StringProp));
        }
    }
}
