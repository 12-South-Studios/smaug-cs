using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Events
{
    [TestFixture]
    public class EventListenerTest
    {
        private EventCallback<RealmEventArgs> _eventCallback;

        [SetUp]
        public void Setup()
        {
            _eventCallback = args => { };
        }

        [Test]
        public void ToStringTest()
        {
            var listener = new EventListener("tester", "testee", typeof(String), _eventCallback);

            Assert.That(listener, Is.Not.Null);

            const string expected = "Listener tester, ListenTo testee, EventType System.String, " +
                "CallbackFunction Realm.Library.Common.EventCallback`1[Realm.Library.Common.RealmEventArgs]";

            Assert.That(listener.ToString(), Is.EqualTo(expected));
        }
    }
}
