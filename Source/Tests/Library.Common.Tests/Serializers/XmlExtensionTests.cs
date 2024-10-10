using System;
using FluentAssertions;
using Library.Common.Serializers;
using Xunit;

namespace Library.Common.Tests.Serializers;

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
    Action act = () => XmlExtensions.ToXml<SerializableObjectFake>(null);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ToXmlTest()
  {
    SerializableObjectFake obj = new()
    {
      IntegerProp = 5,
      StringProp = "Test"
    };

    string result = obj.ToXml();

    const string header = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

    string.IsNullOrEmpty(result).Should().BeFalse();

    string sub = result?[..header.Length];
    sub.Should().Be(header);
  }

  [Fact]
  public void FromXmlNullStringTest()
  {
    Action act = () => "".FromXml<SerializableObjectFake>();
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void FromXmlTest()
  {
    const string xml =
      "<?xml version=\"1.0\" encoding=\"utf-16\"?><SerializableObjectFake xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><IntegerProp>5</IntegerProp><StringProp>Test</StringProp></SerializableObjectFake>";

    SerializableObjectFake result = xml.FromXml<SerializableObjectFake>();
    result.Should().NotBeNull();
  }
}