using FluentAssertions;
using Realm.Library.Common.Events;
using Xunit;

namespace Realm.Library.Common.Test.Events
{
    public class EventListenerTest
    {
        private EventCallback<RealmEventArgs> _eventCallback;

        public EventListenerTest()
        {
            _eventCallback = args => { };
        }

        [Fact]
        public void ToStringTest()
        {
            var listener = new EventListener("tester", "testee", typeof(string), _eventCallback);

            listener.Should().NotBeNull();

            const string expected = "Listener tester, ListenTo testee, EventType System.String, " +
                "CallbackFunction Realm.Library.Common.Events.EventCallback`1[Realm.Library.Common.Events.RealmEventArgs]";

            listener.ToString().Should().Be(expected);
        }
    }
}
