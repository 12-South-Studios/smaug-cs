using System;
using System.Management.Instrumentation;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Events;
using Realm.Library.Common.Logging;
using Xunit;
using EventHandler = Realm.Library.Common.Events.EventHandler;

namespace Realm.Library.Common.Test.Events
{
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

        private ILogWrapper _mockLogger;
        private EventCallback<RealmEventArgs> _eventCallback;

        public EventHandlerTests()
        {
            _mockLogger = A.Fake<ILogWrapper>();
            _eventCallback = args => { };
        }

        [Fact]
        public void RegisterListenerToObjectToType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof (FakeEvent), _eventCallback));
        }

        [Fact]
        public void RegisterListenerTwoObjectsToObjectToType()
        {
            var objectListener1 = new FakeObject();
            var objectListener2 = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListener1, objectActing, typeof(FakeEvent), _eventCallback));
            handler.RegisterListener(new EventListener(objectListener2, objectActing, typeof(FakeEvent), _eventCallback));
        }

        [Fact]
        public void RegisterListenerToType()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, null, typeof (FakeEvent), _eventCallback));
        }

        [Fact]
        public void RegisterListenerTwoObjectsToType()
        {
            var objectListener1 = new FakeObject();
            var objectListener2 = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListener1, null, typeof(FakeEvent), _eventCallback));
            handler.RegisterListener(new EventListener(objectListener2, null, typeof(FakeEvent), _eventCallback));
        }

        [Fact]
        public void IsListeningToType()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            var result = handler.IsListening(objectListening, typeof(FakeEvent));

            result.Should().BeTrue();
        }

        [Fact]
        public void IsListeningToObjectToType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));

            var result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));

            result.Should().BeTrue();
        }

        [Fact]
        public void StopListeningToObjectType()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, objectActing, typeof(FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));
            result.Should().BeTrue();

            handler.StopListeningTo(objectListening, objectActing, typeof(FakeEvent));
            result = handler.IsListening(objectListening, objectActing, typeof(FakeEvent));
            result.Should().BeFalse();
        }

        [Fact]
        public void StopListeningType()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, typeof(FakeEvent));
            result.Should().BeTrue();

            handler.StopListening(objectListening, typeof(FakeEvent));
            result = handler.IsListening(objectListening, typeof(FakeEvent));
            result.Should().BeFalse();
        }

        [Fact]
        public void StopListening()
        {
            var objectListening = new FakeObject();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));
            var result = handler.IsListening(objectListening, typeof(FakeEvent));
            result.Should().BeTrue();

            handler.StopListening(objectListening);
            result = handler.IsListening(objectListening, typeof(FakeEvent));
            result.Should().BeFalse();
        }

        [Fact]
        public void ThrowEventWithSenderAndEvent()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent());

            Thread.Sleep(250);

            resultArgs.Sender.Should().NotBeNull();
            resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

            var sender = resultArgs.Sender as FakeObject;
            sender.Name.Should().Be("Actor");
        }

        [Fact]
        public void ThrowEventWithSenderAndEventAndEventArgs()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var parameterArgs = new RealmEventArgs("TestType");
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent(), parameterArgs);

            Thread.Sleep(250);

            resultArgs.Sender.Should().NotBeNull();
            resultArgs.Sender.Should().BeAssignableTo<FakeObject>();
            
            var sender = resultArgs.Sender as FakeObject;
            sender.Name.Should().Be("Actor");

            resultArgs.Type.Should().Be("TestType");
        }

        [Fact]
        public void ThrowEventWithSenderAndEventAndEventTable()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var table = new EventTable { { "Value", "TestValue" } };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent(objectActing, new FakeEvent(), table);

            Thread.Sleep(250);

            resultArgs.Sender.Should().NotBeNull();
            resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

            var sender = resultArgs.Sender as FakeObject;
            sender.Name.Should().Be("Actor");

            resultArgs.GetValue("Value").Should().Be("TestValue");
         }

        [Fact]
        public void ThrowEventOfTypeWithSenderAndEventTable()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var table = new EventTable { { "Value", "TestValue" } };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent<FakeEvent>(objectActing, table);

            Thread.Sleep(250);

            resultArgs.Sender.Should().NotBeNull();
            resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

            var sender = resultArgs.Sender as FakeObject;
            sender.Name.Should().Be("Actor");

            resultArgs.GetValue("Value").Should().Be("TestValue");
        }

        [Fact]
        public void ThrowEvent_ThrowsException_WhenUnknownEventTypeIsSent()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var table = new EventTable { { "Value", "TestValue" } };

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            Action act = () => handler.ThrowEvent<BuggyFakeEvent>(objectActing, table);
            act.Should().Throw<InstanceNotFoundException>();
        }

        [Fact]
        public void ThrowEventOfTypeWithSender()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };
            var resultArgs = new RealmEventArgs();

            var handler = new EventHandler(new CommonTimer(), _mockLogger);
            _eventCallback = args => { resultArgs = args; };

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            handler.ThrowEvent<FakeEvent>(objectActing);

            Thread.Sleep(250);

            resultArgs.Sender.Should().NotBeNull();
            resultArgs.Sender.Should().BeAssignableTo<FakeObject>();

            var sender = resultArgs.Sender as FakeObject;
            sender.Name.Should().Be("Actor");
        }

        [Fact]
        public void ThrowEvent_ThrowsException_WhenUnknownEventTypeIsPassed()
        {
            var objectListening = new FakeObject();
            var objectActing = new FakeObject { Name = "Actor" };

            var handler = new EventHandler(new CommonTimer(), _mockLogger);

            handler.RegisterListener(new EventListener(objectListening, null, typeof(FakeEvent), _eventCallback));

            Action act = () => handler.ThrowEvent<BuggyFakeEvent>(objectActing);
            act.Should().Throw<InstanceNotFoundException>();
        }
    }
}
