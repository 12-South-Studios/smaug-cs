using System.Management.Instrumentation;
using System.Threading;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Events
{
    [TestFixture]
    public class EventHandlerTests
    {
        private Mock<ILogWrapper> _mockLogger;
        private EventCallback<RealmEventArgs> _eventCallback;

        private static FakeEntity GetEntity()
        {
            return new FakeEntity(1, "Test");
        }

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogWrapper>();
            _eventCallback = args => { };
        }

        [Test]
        [Category("Event Tests")]
        public void RegisterListenerToObjectToType()
        {
            var objectListening = GetEntity();
            var objectActing = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof (FakeEvent), _eventCallback));
        }

        [Test]
        [Category("Event Tests")]
        public void RegisterListenerTwoObjectsToObjectToType()
        {
            var objectListener1 = GetEntity();
            var objectListener2 = GetEntity();
            var objectActing = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListener1, objectActing, typeof (FakeEvent), _eventCallback));
            handler.RegisterListener(new EventListener(objectListener2, objectActing, typeof (FakeEvent), _eventCallback));
        }

        [Test]
        [Category("Event Tests")]
        public void RegisterListenerToType()
        {
            var objectListening = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));
        }

        [Test]
        [Category("Event Tests")]
        public void RegisterListenerTwoObjectsToType()
        {
            var objectListener1 = GetEntity();
            var objectListener2 = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListener1, null, typeof (FakeEvent), _eventCallback));
            handler.RegisterListener(new EventListener(objectListener2, null, typeof (FakeEvent), _eventCallback));
        }

        [Test]
        [Category("Event Tests")]
        public void IsListeningToType()
        {
            var objectListening = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            var result = handler.IsListening(objectListening, typeof (FakeEvent));

            Assert.AreEqual(true, result);
        }

        [Test]
        [Category("Event Tests")]
        public void IsListeningToObjectToType()
        {
            var objectListening = GetEntity();
            var objectActing = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof (FakeEvent), _eventCallback));

            var result = handler.IsListening(objectListening, objectActing, typeof (FakeEvent));

            Assert.AreEqual(true, result);
        }

        [Test]
        [Category("Event Tests")]
        public void StopListeningToObjectType()
        {
            var objectListening = GetEntity();
            var objectActing = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof (FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, objectActing, typeof (FakeEvent));
            Assert.AreEqual(true, result);

            handler.StopListeningTo(objectListening, objectActing, typeof (FakeEvent));
            result = handler.IsListening(objectListening, objectActing, typeof (FakeEvent));
            Assert.AreEqual(false, result);
        }

        [Test]
        [Category("Event Tests")]
        public void StopListeningType()
        {
            var objectListening = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, typeof (FakeEvent));
            Assert.AreEqual(true, result);

            handler.StopListening(objectListening, typeof (FakeEvent));
            result = handler.IsListening(objectListening, typeof (FakeEvent));
            Assert.AreEqual(false, result);
        }

        [Test]
        [Category("Event Tests")]
        public void StopListening()
        {
            var objectListening = GetEntity();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, typeof (FakeEvent));
            Assert.AreEqual(true, result);

            handler.StopListening(objectListening);
            result = handler.IsListening(objectListening, typeof (FakeEvent));
            Assert.AreEqual(false, result);
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventWithSenderAndEvent()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent());

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof (FakeEntity)),
                "Unit test expected Sender to be a FakeEntity");
            Assert.That(resultArgs.Sender.CastAs<FakeEntity>().Name, Is.EqualTo("Actor"),
                "Unit test expected Sender's Name to be 'Actor'");
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventWithSenderAndEventAndEventArgs()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");
            var parameterArgs = new RealmEventArgs("TestType");
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent(), parameterArgs);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof (FakeEntity)),
                "Unit test expected Sender to be a FakeEntity");
            Assert.That(resultArgs.Sender.CastAs<FakeEntity>().Name, Is.EqualTo("Actor"),
                "Unit test expected Sender's Name to be 'Actor'");
            Assert.That(resultArgs.Type, Is.EqualTo("TestType"), "Unit test expected the 'Type' to be 'TestType'");
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventWithSenderAndEventAndEventTable()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");
            var table = new EventTable {{"Value", "TestValue"}};
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent(), table);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof (FakeEntity)),
                "Unit test expected Sender to be a FakeEntity");
            Assert.That(resultArgs.Sender.CastAs<FakeEntity>().Name, Is.EqualTo("Actor"),
                "Unit test expected Sender's Name to be 'Actor'");
            Assert.That(resultArgs.GetValue("Value"), Is.EqualTo("TestValue"),
                "Unit test expected the 'Value' to have a value of 'TestValue'");
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventOfTypeWithSenderAndEventTable()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");
            var table = new EventTable {{"Value", "TestValue"}};
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            handler.ThrowEvent<FakeEvent>(objectActing, table);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof (FakeEntity)),
                "Unit test expected Sender to be a FakeEntity");
            Assert.That(resultArgs.Sender.CastAs<FakeEntity>().Name, Is.EqualTo("Actor"),
                "Unit test expected Sender's Name to be 'Actor'");
            Assert.That(resultArgs.GetValue("Value"), Is.EqualTo("TestValue"),
                "Unit test expected the 'Value' to have a value of 'TestValue'");
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventOfTypeWithSenderAndEventTableWithUnknownEventType()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");
            var table = new EventTable {{"Value", "TestValue"}};

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            Assert.Throws<InstanceNotFoundException>(() => handler.ThrowEvent<BuggyFakeEvent>(objectActing, table),
                "Unit test expected an InstanceNotFoundException to be thrown!");
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventOfTypeWithSender()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            handler.ThrowEvent<FakeEvent>(objectActing);

            Thread.Sleep(250);
            Assert.That(resultArgs.Sender, Is.Not.Null, "Unit test expected Sender to not be null");
            Assert.That(resultArgs.Sender.GetType(), Is.EqualTo(typeof (FakeEntity)),
                "Unit test expected Sender to be a FakeEntity");
            Assert.That(resultArgs.Sender.CastAs<FakeEntity>().Name, Is.EqualTo("Actor"),
                "Unit test expected Sender's Name to be 'Actor'");
        }

        [Test, Timeout(10000)]
        [Category("Event Tests")]
        public void ThrowEventOfTypeWithSenderWithUnknownEventType()
        {
            var objectListening = GetEntity();
            var objectActing = new FakeEntity(1, "Actor");

            var handler = new EventHandler(new CommonTimer(), 50, _mockLogger.Object);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));

            Assert.Throws<InstanceNotFoundException>(() => handler.ThrowEvent<BuggyFakeEvent>(objectActing),
                "Unit test expected an InstanceNotFoundException to be thrown!");
        }
    }
}
