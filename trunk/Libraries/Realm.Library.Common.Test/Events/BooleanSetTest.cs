using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Events
{
    [TestFixture]
    public class BooleanSetTest
    {
        private EventCallback<RealmEventArgs> _eventCallback;

        [SetUp]
        public void Setup()
        {
            _eventCallback = args => { };
        }

        [Test]
        public void ConstructorTest()
        {
            var set = new BooleanSet(new EventTable(), _eventCallback);

            Assert.That(set.IsComplete, Is.True);
        }

        [Test]
        public void AddHasTest()
        {
            var set = new BooleanSet("Test", _eventCallback);
            Assert.That(set, Is.Not.Null);

            set.AddItem("Testing123");
            Assert.That(set.HasItem("Testing123"), Is.True);
            Assert.That(set.HasItem("Tester"), Is.False);
        }

        [Test]
        public void CompleteItemInvalidItemTest()
        {
            var set = new BooleanSet("Test", _eventCallback);

            Assert.Throws<KeyNotFoundException>(() => set.CompleteItem("Testing123"),
                                                "Unit test expected a KeyNotFoundException to be thrown");
        }

        [Test]
        public void CompleteItemInvalidCallbackTest()
        {
            var set = new BooleanSet("Test", null);

            set.AddItem("Testing123");

            Assert.Throws<NoNullAllowedException>(() => set.CompleteItem("Testing123"),
                                                  "Unit test expected a NoNullAllowedException to be thrown");
        }

        [Test]
        public void CompleteItemCountGreaterThan1Test()
        {
            var set = new BooleanSet("Test", _eventCallback);

            set.AddItem("Testing123");
            set.AddItem("Testing1234");

            set.CompleteItem("Testing123");

            Assert.That(set.IsComplete, Is.False);
        }

        [Test]
        public void CompleteItemCountIs0Test()
        {
            var callback = false;

            _eventCallback = args => { callback = true; };

            var set = new BooleanSet("Test", _eventCallback);

            set.AddItem("Testing123");

            set.CompleteItem("Testing123");

            Assert.That(callback, Is.True.After(250));
        }
    }
}
