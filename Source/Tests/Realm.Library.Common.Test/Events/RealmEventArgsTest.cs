using System;
using FluentAssertions;
using Realm.Library.Common.Events;
using Xunit;

namespace Realm.Library.Common.Test.Events
{
    public class RealmEventArgsTest
    {
        [Fact]
        public void RealmEventArgsConstructorTest()
        {
            var result = new RealmEventArgs();

            result.Data.Should().NotBeNull();

            var result2 = typeof(EventTable) == result.Data.GetType();
            result2.Should().BeTrue();
        }

        [Fact]
        public void Constructor_ReturnsValidData_WhenEventTableIsPassed()
        {
            var result = new RealmEventArgs(new EventTable());
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ThrowsException_WhenCreateEventTableIsFalse()
        {
            Assert.Throws<ArgumentNullException>(() => new RealmEventArgs((EventTable)null));
        }

        [Fact]
        public void Constructor_TypeIsAssignedProperly_WHenValidParameterIsPassed()
        {
            var result = new RealmEventArgs("test");
            result.Type.Should().Be("test");
        }

        [Fact]
        public void Constructor_ThrowsException_WhenArgTypeIsNotProvided()
        {
            Action act = () => new RealmEventArgs(string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetValueEmptyKeyTest()
        {
            var args = new RealmEventArgs();

            Action act = () => args.GetValue(string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetValueNullTableTest()
        {
            var args = new RealmEventArgs("test");

            args.GetValue("key").Should().BeNull();
        }

        [Fact]
        public void GetValueNoKeyTest()
        {
            var table = new EventTable { { "key", 25 } };

            var args = new RealmEventArgs(table);

            args.GetValue("key2").Should().BeNull();
        }

        [Fact]
        public void GetValueSuccessTest()
        {
            const int value = 25;
            var table = new EventTable { { "key", value } };

            var args = new RealmEventArgs(table);

            args.GetValue("key").Should().Be(value);
        }

        [Fact]
        public void HasValueNullTableTest()
        {
            var args = new RealmEventArgs("test");

            args.HasValue("key").Should().BeFalse();
        }

        [Fact]
        public void HasValueSuccessTest()
        {
            const int value = 25;
            var table = new EventTable { { "key", value } };

            var args = new RealmEventArgs(table);

            args.HasValue("key").Should().BeTrue();
        }
    }
}
