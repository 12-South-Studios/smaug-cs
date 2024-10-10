using FluentAssertions;
using Library.Common.Events;
using Xunit;

namespace Library.Common.Tests.Events;

public class EventListenerTest
{
  private readonly EventCallback<RealmEventArgs> _eventCallback = _ => { };

  [Fact]
  public void ToStringTest()
  {
    EventListener listener = new("tester", "testee", typeof(string), _eventCallback);

    listener.Should().NotBeNull();

    const string expected = "Listener tester, ListenTo testee, EventType System.String, " +
                            "CallbackFunction Library.Common.Events.EventCallback`1[Library.Common.Events.RealmEventArgs]";

    listener.ToString().Should().Be(expected);
  }
}