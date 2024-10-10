using System;
using FluentAssertions;
using Library.Common.Serializers;
using Xunit;

namespace Library.Common.Tests.Serializers;

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
    Action act = () => JsonExtensions.ToJson<SerializableObjectFake>(null);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ToJsonTest()
  {
    SerializableObjectFake obj = new()
    {
      IntegerProp = 5,
      StringProp = "Test"
    };

    string result = obj.ToJson();

    const string value = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

    string.IsNullOrEmpty(result).Should().BeFalse();
    result.Should().Be(value);
  }

  [Fact]
  public void FromJsonNullStringTest()
  {
    Action act = () => "".FromJson<SerializableObjectFake>();
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void FromJsonTest()
  {
    const string json = "{\"<IntegerProp>k__BackingField\":5,\"<StringProp>k__BackingField\":\"Test\"}";

    SerializableObjectFake result = json.FromJson<SerializableObjectFake>();
    result.Should().NotBeNull();
  }
}