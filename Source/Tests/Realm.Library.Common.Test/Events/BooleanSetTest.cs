using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using Realm.Library.Common.Events;
using Xunit;

namespace Realm.Library.Common.Test.Events
{
    public class BooleanSetTest
    {
        private EventCallback<RealmEventArgs> _eventCallback;

        public BooleanSetTest()
        {
            _eventCallback = args => { };
        }

        [Fact]
        public void ConstructorTest()
        {
            var set = new BooleanSet(new EventTable(), _eventCallback);
            set.IsComplete.Should().BeTrue();
        }

        [Fact]
        public void AddHasTest()
        {
            var set = new BooleanSet("Test", _eventCallback);
            set.Should().NotBeNull();

            set.AddItem("Testing123");
            set.HasItem("Testing123").Should().BeTrue();
            set.HasItem("Tester").Should().BeFalse();
        }

        [Fact]
        public void CompleteItemInvalidItemTest()
        {
            var set = new BooleanSet("Test", _eventCallback);

            Action act = () => set.CompleteItem("Testing123");
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void CompleteItemInvalidCallbackTest()
        {
            var set = new BooleanSet("Test", null);

            set.AddItem("Testing123");

            Action act = () => set.CompleteItem("Testing123");
            act.Should().Throw<NoNullAllowedException>();
        }

        [Fact]
        public void CompleteItemCountGreaterThan1Test()
        {
            var set = new BooleanSet("Test", _eventCallback);

            set.AddItem("Testing123");
            set.AddItem("Testing1234");

            set.CompleteItem("Testing123");

            set.IsComplete.Should().BeFalse();
        }

        [Fact]
        public void CompleteItemCountIs0Test()
        {
            var callback = false;

            _eventCallback = args => { callback = true; };

            var set = new BooleanSet("Test", _eventCallback);

            set.AddItem("Testing123");

            set.CompleteItem("Testing123");

            callback.Should().BeTrue();
        }
    }
}
