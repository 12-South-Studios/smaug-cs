using FluentAssertions;
using System;
using Xunit;

namespace SmaugCS.Common.Tests;

public class ObjectExtensionTests
{
  private class FakeAttribute : Attribute;

  private class Fake2Attribute : Attribute;

  private class FakeObject
  {
    [Fake] public bool TestProp { get; set; }

    [Fake]
    public bool TestMethod()
    {
      return true;
    }
  }

  [Fact]
  public void GetAttribute_NoMethodFound()
  {
    FakeObject obj = new();

    obj.GetAttribute<FakeAttribute>("IncorrectMethod").Should().BeNull();
  }

  [Fact]
  public void GetAttribute_NoAttributeFound()
  {
    FakeObject obj = new();

    obj.GetAttribute<Fake2Attribute>("TestMethod").Should().BeNull();
  }

  [Fact]
  public void GetAttribute_AttributeFound()
  {
    FakeObject obj = new();

    obj.GetAttribute<FakeAttribute>("TestMethod").Should().NotBeNull();
  }

  [Fact]
  public void HasAttribute_NoMethodFound()
  {
    FakeObject obj = new();

    obj.HasAttribute<FakeAttribute>("IncorrectMethod").Should().BeFalse();
  }

  [Fact]
  public void HasAttribute_NoAttributeFound()
  {
    FakeObject obj = new();

    obj.HasAttribute<Fake2Attribute>("TestMethod").Should().BeFalse();
  }

  [Fact]
  public void HasAttribute_AttributeFound()
  {
    FakeObject obj = new();

    obj.HasAttribute<FakeAttribute>("TestMethod").Should().BeTrue();
  }
}