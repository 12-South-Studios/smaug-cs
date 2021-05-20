using System;
using FluentAssertions;
using Realm.Library.Common.Serializers;
using Xunit;

namespace Realm.Library.Common.Test.Serializers
{
    public class BinaryExtensionTests
    {
        [Serializable]
        private class SerializableObjectFake
        {
            public int IntegerProp { get; set; }
            public string StringProp { get; set; }
        }

        [Fact]
        public void ToBinaryNullObjectTest()
        {
            Action Act = () => BinaryExtensions.ToBinary<SerializableObjectFake>(null);
            Act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToBinaryTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var result = obj.ToBinary();
            result.Should().NotBeNull();
        }

        [Fact]
        public void FromBinaryNullStringTest()
        {
            Action act = () => BinaryExtensions.FromBinary<SerializableObjectFake>(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FromBinaryTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var encoded = obj.ToBinary();

            var result = encoded.FromBinary<SerializableObjectFake>();

            result.Should().NotBeNull();
            result.IntegerProp.Should().Be(obj.IntegerProp);
            result.StringProp.Should().Be(obj.StringProp);
        }
    }
}
