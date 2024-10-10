using System;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Events;
using Library.Common.Exceptions;
using Library.Common.Logging;
using Xunit;
using EventHandler = Library.Common.Events.EventHandler;

namespace Library.Common.Tests.Events;

public class EventHandlerTests
{
  private class FakeObject
  {
    public string Name;
  }

  private class FakeEvent : EventBase;

  private class BuggyFakeEvent : EventBase
  {
    public BuggyFakeEvent()
    {
      throw new Exception("Fail!");
    }
  }

  private readonly ILogWrapper _mockLogger = A.Fake<ILogWrapper>();
  private EventCallback<RealmEventArgs> _eventCallback = _ => { };

  [Fact]
  public void RegisterListenerToObjectToType()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));
  }

  [Fact]
  public void RegisterListenerTwoObjectsToObjectToType()
  {
    FakeObject objectListener1 = new();
    FakeObject objectListener2 = new();
    FakeObject objectActing = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListener1, objectActing, typeof(FakeEvent), _eventCallback));
    handler.RegisterListener(new EventListener(objectListener2, objectActing, typeof(FakeEvent), _eventCallback));
  }

  [Fact]
  public void RegisterListenerToType()
  {
    FakeObject objectListening = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
  }

  [Fact]
  public void RegisterListenerTwoObjectsToType()
  {
    FakeObject objectListener1 = new();
    FakeObject objectListener2 = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListener1, null, typeof(FakeEvent), _eventCallback));
    handler.RegisterListener(new EventListener(objectListener2, null, typeof(FakeEvent), _eventCallback));
  }

  [Fact]
  public void IsListeningToType()
  {
    FakeObject objectListening = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    bool result = handler.IsListening(objectListening, typeof(FakeEvent));

    result.Should().BeTrue();
  }

  [Fact]
  public void IsListeningToObjectToType()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));

    bool result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));

    result.Should().BeTrue();
  }

  [Fact]
  public void StopListeningToObjectType()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));
    bool result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));
    result.Should().BeTrue();

    handler.StopListeningTo(objectListening, objectActing, typeof(FakeEvent));
    result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));
    result.Should().BeFalse();
  }

  [Fact]
  public void StopListeningType()
  {
    FakeObject objectListening = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
    bool result = handler.IsListening(objectListening, typeof(FakeEvent));
    result.Should().BeTrue();

    handler.StopListening(objectListening, typeof(FakeEvent));
    result = handler.IsListening(objectListening, typeof(FakeEvent));
    result.Should().BeFalse();
  }

  [Fact]
  public void StopListening()
  {
    FakeObject objectListening = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
    bool result = handler.IsListening(objectListening, typeof(FakeEvent));
    result.Should().BeTrue();

    handler.StopListening(objectListening);
    result = handler.IsListening(objectListening, typeof(FakeEvent));
    result.Should().BeFalse();
  }

  [Fact]
  public void ThrowEventWithSenderAndEvent()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };
    RealmEventArgs resultArgs = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);
    _eventCallback = args => { resultArgs = args; };

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    handler.ThrowEvent(objectActing, new FakeEvent());

    Thread.Sleep(250);

    resultArgs.Sender.Should().NotBeNull();
    resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

    FakeObject sender = resultArgs.Sender as FakeObject;
    sender?.Name.Should().Be("Actor");
  }

  [Fact]
  public void ThrowEventWithSenderAndEventAndEventArgs()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };
    RealmEventArgs parameterArgs = new("TestType");
    RealmEventArgs resultArgs = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);
    _eventCallback = args => { resultArgs = args; };

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    handler.ThrowEvent(objectActing, new FakeEvent(), parameterArgs);

    Thread.Sleep(250);

    resultArgs.Sender.Should().NotBeNull();
    resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

    FakeObject sender = resultArgs.Sender as FakeObject;
    sender?.Name.Should().Be("Actor");

    resultArgs.Type.Should().Be("TestType");
  }

  [Fact]
  public void ThrowEventWithSenderAndEventAndEventTable()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };
    EventTable table = new() { { "Value", "TestValue" } };
    RealmEventArgs resultArgs = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);
    _eventCallback = args => { resultArgs = args; };

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    handler.ThrowEvent(objectActing, new FakeEvent(), table);

    Thread.Sleep(250);

    resultArgs.Sender.Should().NotBeNull();
    resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

    FakeObject sender = resultArgs.Sender as FakeObject;
    sender?.Name.Should().Be("Actor");

    resultArgs.GetValue("Value").Should().Be("TestValue");
  }

  [Fact]
  public void ThrowEventOfTypeWithSenderAndEventTable()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };
    EventTable table = new() { { "Value", "TestValue" } };
    RealmEventArgs resultArgs = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);
    _eventCallback = args => { resultArgs = args; };

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    handler.ThrowEvent<FakeEvent>(objectActing, table);

    Thread.Sleep(250);

    resultArgs.Sender.Should().NotBeNull();
    resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

    FakeObject sender = resultArgs.Sender as FakeObject;
    sender?.Name.Should().Be("Actor");

    resultArgs.GetValue("Value").Should().Be("TestValue");
  }

  [Fact]
  public void ThrowEvent_ThrowsException_WhenUnknownEventTypeIsSent()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };
    EventTable table = new() { { "Value", "TestValue" } };

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    Action act = () => handler.ThrowEvent<BuggyFakeEvent>(objectActing, table);
    act.Should().Throw<InstanceNotFoundException>();
  }

  [Fact]
  public void ThrowEventOfTypeWithSender()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };
    RealmEventArgs resultArgs = new();

    EventHandler handler = new(new CommonTimer(), _mockLogger);
    _eventCallback = args => { resultArgs = args; };

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    handler.ThrowEvent<FakeEvent>(objectActing);

    Thread.Sleep(250);

    resultArgs.Sender.Should().NotBeNull();
    resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

    FakeObject sender = resultArgs.Sender as FakeObject;
    sender?.Name.Should().Be("Actor");
  }

  [Fact]
  public void ThrowEvent_ThrowsException_WhenUnknownEventTypeIsPassed()
  {
    FakeObject objectListening = new();
    FakeObject objectActing = new() { Name = "Actor" };

    EventHandler handler = new(new CommonTimer(), _mockLogger);

    handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

    Action act = () => handler.ThrowEvent<BuggyFakeEvent>(objectActing);
    act.Should().Throw<InstanceNotFoundException>();
  }
}