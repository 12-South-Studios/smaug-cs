using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Common.Contexts;
using Library.Common.Objects;
using Xunit;

namespace Library.Common.Tests;

public class PropertyContextFacts
{
  private class FakeEntity(long id, string name) : Entity(id, name);

  [Flags]
  private enum FactEnum
  {
    Fact1,
    Fact2
  }

  [Fact]
  public void SetProperty_StringName_NoOptions_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty("NullProperty", "Nothing");

    string result = ctx.GetProperty<string>("NullProperty");
    result.Should().Be("Nothing");
  }

  [Fact]
  public void SetProperty_StringName_Volatile_SetExisting_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty("NullProperty", "Nothing", PropertyTypeOptions.Volatile);

    string result = ctx.GetProperty<string>("NullProperty");
    result.Should().Be("Nothing");

    ctx.SetProperty("NullProperty", "Still Nothing");

    result = ctx.GetProperty<string>("NullProperty");
    result.Should().Be("Still Nothing");
  }

  [Fact]
  public void SetProperty_Enum_NoOptions_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing");

    string result = ctx.GetProperty<string>("Fact1");
    result.Should().Be("Nothing");
  }

  [Fact]
  public void HasProperty_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing");

    bool result = ctx.HasProperty("Fact1");
    result.Should().BeTrue();
  }

  [Fact]
  public void IsPersistable_True_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing", PropertyTypeOptions.Persistable);

    bool result = ctx.IsPersistable("Fact1");
    result.Should().BeTrue();
  }

  [Fact]
  public void IsPersistable_False_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing");

    bool result = ctx.IsPersistable("Fact1");
    result.Should().BeFalse();
  }

  [Fact]
  public void IsVisible_True_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing", PropertyTypeOptions.Visible);

    bool result = ctx.IsVisible("Fact1");
    result.Should().BeTrue();
  }

  [Fact]
  public void IsVisible_False_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing");

    bool result = ctx.IsVisible("Fact1");
    result.Should().BeFalse();
  }

  [Fact]
  public void IsVolatile_True_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing", PropertyTypeOptions.Volatile);

    bool result = ctx.IsVolatile("Fact1");
    result.Should().BeTrue();
  }

  [Fact]
  public void IsVolatile_False_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing");

    bool result = ctx.IsVolatile("Fact1");
    result.Should().BeFalse();
  }

  [Fact]
  public void RemoveProperty_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing");

    bool result = ctx.HasProperty("Fact1");
    result.Should().BeTrue();

    ctx.RemoveProperty("Fact1");

    result = ctx.HasProperty("Fact1");
    result.Should().BeFalse();
  }

  [Theory]
  [InlineData(PropertyTypeOptions.None, "")]
  [InlineData(PropertyTypeOptions.Persistable, "p")]
  [InlineData(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible, "pi")]
  [InlineData(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible | PropertyTypeOptions.Volatile, "pvi")]
  public void GetPropertyBits_Fact(PropertyTypeOptions options, string expectedValue)
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Nothing", options);

    string result = ctx.GetPropertyBits("Fact1");
    result.Should().Be(expectedValue);
  }

  [Fact]
  public void Count_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Fact #1");
    ctx.SetProperty(FactEnum.Fact2, "Fact #2");

    ctx.Count.Should().Be(2);
  }

  [Fact]
  public void PropertyKeys_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    PropertyContext ctx = new(fake);
    ctx.SetProperty(FactEnum.Fact1, "Fact #1");
    ctx.SetProperty(FactEnum.Fact2, "Fact #2");

    IEnumerable<string> keys = ctx.PropertyKeys;
    IEnumerable<string> enumerable = keys.ToList();
    
    enumerable.Should().NotBeNull();
    enumerable.Count().Should().Be(2);
    enumerable.Contains("Fact1").Should().BeTrue();
    enumerable.Contains("Fact2").Should().BeTrue();
  }
}