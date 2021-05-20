using System;
using FluentAssertions;
using Realm.Library.Common.Serializers;
using Xunit;

namespace Realm.Library.Common.Test.Serializers
{
    public class JsonExtensionTests
    {
        [Serializable]
        private class SerializableObjectFake
        {
            public int IntegerProp { get; set; }
            public string StringProp { get; set; }
        }

        [Fact]
        public void ToJsonNullObjectTest()
        {
            Action act = () => JSONExtensions.ToJSON<SerializableObjectFake>(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToJsonTest()
        {
            var obj = new SerializableObjectFake
                          {
                              IntegerProp = 5,
                              StringProp = "Test"
                          };

            var result = obj.ToJSON();

            const string value = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

            string.IsNullOrEmpty(result).Should().BeFalse();
            result.Should().Be(value);
        }

        [Fact]
        public void FromJsonNullStringTest()
        {
            Action act = () => JSONExtensions.FromJSON<SerializableObjectFake>("");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FromJsonTest()
        {
            const string json = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

            var result = json.FromJSON<SerializableObjectFake>();
            result.Should().NotBeNull();
        }
    }
}
