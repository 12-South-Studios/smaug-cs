using System;
using System.Management.Instrumentation;
using System.Threading;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Events
{
    [TestFixture]
    [Category("Events")]
    public class EventHandlerTests
    {
        private class FakeObject
        {
            public string Name;
        }

        private class FakeEvent : EventBase { }

        private class BuggyFakeEvent : EventBase
        {
            public BuggyFakeEvent() { throw new Exception("Fail!"); }
        }

        private Mock<ILogWrapper> _mockLogger;
        private EventCallback<RealmEventArgs> _eventCallback;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogWrapper>();
            _eventCallback = args => { };
        }

        [Test]
        public void RegisterListenerToObjectToType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof (FakeEvent), _eventCallback));
        }

        [Test]
        public void RegisterListenerTwoObjectsToObjectToType()
        {
            var objectListener1 = new FakeObject();
            var objectListener2 = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListener1, objectActing, typeof(FakeEvent), _eventCallback));
            handler.RegisterListener(new EventListener(objectListener2, objectActing, typeof(FakeEvent), _eventCallback));
        }

        [Test]
        public void RegisterListenerToType()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));
        }

        [Test]
        public void RegisterListenerTwoObjectsToType()
        {
            var objectListener1 = new FakeObject();
            var objectListener2 = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListener1, null, typeof(FakeEvent), _eventCallback));
            handler.RegisterListener(new EventListener(objectListener2, null, typeof(FakeEvent), _eventCallback));
        }

        [Test]
        public void IsListeningToType()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            var result = handler.IsListening(objectListening, typeof(FakeEvent));

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsListeningToObjectToType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));

            var result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));

            Assert.AreEqual(true, result);
        }

        [Test]
        public void StopListeningToObjectType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));
            Assert.AreEqual(true, result);

            handler.StopListeningTo(objectListening, objectActing, typeof(FakeEvent));
            result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));
            Assert.AreEqual(false, result);
        }

        [Test]
        public void StopListeningType()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, typeof(FakeEvent));
            Assert.AreEqual(true, result);

            handler.StopListening(objectListening, typeof(FakeEvent));
            result = handler.IsListening(objectListening, typeof(FakeEvent));
            Assert.AreEqual(false, result);
        }

        [Test]
        public void StopListening()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, typeof(FakeEvent));
            Assert.AreEqual(true, result);

            handler.StopListening(objectListening);
            result = handler.IsListening(objectListening, typeof(FakeEvent));
            Assert.AreEqual(false, result);
        }

        [Test, Timeout(10000)]
        public void ThrowEventWithSenderAndEvent()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent());

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof(FakeObject)), "Unit test expected Sender to be a FakeObject");
            Assert.That(resultArgs.Sender.CastAs<FakeObject>().Name, Is.EqualTo("Actor"), "Unit test expected Sender's Name to be 'Actor'");
        }

        [Test, Timeout(10000)]
        public void ThrowEventWithSenderAndEventAndEventArgs()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var parameterArgs = new RealmEventArgs("TestType");
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent(), parameterArgs);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof(FakeObject)), "Unit test expected Sender to be a FakeObject");
            Assert.That(resultArgs.Sender.CastAs<FakeObject>().Name, Is.EqualTo("Actor"), "Unit test expected Sender's Name to be 'Actor'");
            Assert.That(resultArgs.Type, Is.EqualTo("TestType"), "Unit test expected the 'Type' to be 'TestType'");
        }

        [Test, Timeout(10000)]
        public void ThrowEventWithSenderAndEventAndEventTable()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var table = new EventTable { { "Value", "TestValue" } };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent(), table);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof(FakeObject)), "Unit test expected Sender to be a FakeObject");
            Assert.That(resultArgs.Sender.CastAs<FakeObject>().Name, Is.EqualTo("Actor"), "Unit test expected Sender's Name to be 'Actor'");
            Assert.That(resultArgs.GetValue("Value"), Is.EqualTo("TestValue"), "Unit test expected the 'Value' to have a value of 'TestValue'");
        }

        [Test, Timeout(10000)]
        public void ThrowEventOfTypeWithSenderAndEventTable()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var table = new EventTable { { "Value", "TestValue" } };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent<FakeEvent>(objectActing, table);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof(FakeObject)), "Unit test expected Sender to be a FakeObject");
            Assert.That(resultArgs.Sender.CastAs<FakeObject>().Name, Is.EqualTo("Actor"), "Unit test expected Sender's Name to be 'Actor'");
            Assert.That(resultArgs.GetValue("Value"), Is.EqualTo("TestValue"), "Unit test expected the 'Value' to have a value of 'TestValue'");
        }

        [Test, Timeout(10000)]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void ThrowEventOfTypeWithSenderAndEventTableWithUnknownEventType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var table = new EventTable { { "Value", "TestValue" } };

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent<BuggyFakeEvent>(objectActing, table);

            Assert.Fail("Unit test expected an InstanceNotFoundException to be thrown!");
        }

        [Test, Timeout(10000)]
        public void ThrowEventOfTypeWithSender()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent<FakeEvent>(objectActing);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof(FakeObject)), "Unit test expected Sender to be a FakeObject");
            Assert.That(resultArgs.Sender.CastAs<FakeObject>().Name, Is.EqualTo("Actor"), "Unit test expected Sender's Name to be 'Actor'");
        }

        [Test, Timeout(10000)]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void ThrowEventOfTypeWithSenderWithUnknownEventType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent<BuggyFakeEvent>(objectActing);

            Assert.Fail("Unit test expected an InstanceNotFoundException to be thrown!");
        }
    }
}
