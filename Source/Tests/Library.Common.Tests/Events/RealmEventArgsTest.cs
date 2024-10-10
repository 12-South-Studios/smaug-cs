using System;
using FluentAssertions;
using Library.Common.Events;
using Xunit;

namespace Library.Common.Tests.Events;

public class RealmEventArgsTest
{
  [Fact]
  public void RealmEventArgsConstructorTest()
  {
    RealmEventArgs result = new();

    result.Data.Should().NotBeNull();

    bool result2 = typeof(EventTable) == result.Data.GetType();
    result2.Should().BeTrue();
  }

  [Fact]
  public void Constructor_ReturnsValidData_WhenEventTableIsPassed()
  {
    RealmEventArgs result = new(new EventTable());
    result.Data.Should().NotBeNull();
  }

  [Fact]
  public void Constructor_ThrowsException_WhenCreateEventTableIsFalse()
  {
    Assert.Throws<ArgumentNullException>(() => new RealmEventArgs((EventTable)null));
  }

  [Fact]
  public void Constructor_TypeIsAssignedProperly_WHenValidParameterIsPassed()
  {
    RealmEventArgs result = new("test");
    result.Type.Should().Be("test");
  }

  [Fact]
  public void Constructor_ThrowsException_WhenArgTypeIsNotProvided()
  {
    Action act = () => new RealmEventArgs(string.Empty);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetValueEmptyKeyTest()
  {
    RealmEventArgs args = new();

    Action act = () => args.GetValue(string.Empty);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetValueNullTableTest()
  {
    RealmEventArgs args = new("test");

    args.GetValue("key").Should().BeNull();
  }

  [Fact]
  public void GetValueNoKeyTest()
  {
    EventTable table = new() { { "key", 25 } };

    RealmEventArgs args = new(table);

    args.GetValue("key2").Should().BeNull();
  }

  [Fact]
  public void GetValueSuccessTest()
  {
    const int value = 25;
    EventTable table = new() { { "key", value } };

    RealmEventArgs args = new(table);

    args.GetValue("key").Should().Be(value);
  }

  [Fact]
  public void HasValueNullTableTest()
  {
    RealmEventArgs args = new("test");

    args.HasValue("key").Should().BeFalse();
  }

  [Fact]
  public void HasValueSuccessTest()
  {
    const int value = 25;
    EventTable table = new() { { "key", value } };

    RealmEventArgs args = new(table);

    args.HasValue("key").Should().BeTrue();
  }
}