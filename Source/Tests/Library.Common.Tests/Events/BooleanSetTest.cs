using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Library.Common.Events;
using Xunit;

namespace Library.Common.Tests.Events;

public class BooleanSetTest
{
  private EventCallback<RealmEventArgs> _eventCallback = _ => { };

  [Fact]
  public void ConstructorTest()
  {
    BooleanSet set = new(new EventTable(), _eventCallback);
    set.IsComplete.Should().BeTrue();
  }

  [Fact]
  public void AddHasTest()
  {
    BooleanSet set = new("Test", _eventCallback);
    set.Should().NotBeNull();

    set.AddItem("Testing123");
    set.HasItem("Testing123").Should().BeTrue();
    set.HasItem("Tester").Should().BeFalse();
  }

  [Fact]
  public void CompleteItemInvalidItemTest()
  {
    BooleanSet set = new("Test", _eventCallback);

    Action act = () => set.CompleteItem("Testing123");
    act.Should().Throw<KeyNotFoundException>();
  }

  [Fact]
  public void CompleteItemInvalidCallbackTest()
  {
    BooleanSet set = new("Test", null);

    set.AddItem("Testing123");

    Action act = () => set.CompleteItem("Testing123");
    act.Should().Throw<NoNullAllowedException>();
  }

  [Fact]
  public void CompleteItemCountGreaterThan1Test()
  {
    BooleanSet set = new("Test", _eventCallback);

    set.AddItem("Testing123");
    set.AddItem("Testing1234");

    set.CompleteItem("Testing123");

    set.IsComplete.Should().BeFalse();
  }

  [Fact]
  public void CompleteItemCountIs0Test()
  {
    bool callback = false;

    _eventCallback = _ => { callback = true; };

    BooleanSet set = new("Test", _eventCallback);

    set.AddItem("Testing123");

    set.CompleteItem("Testing123");

    callback.Should().BeTrue();
  }
}