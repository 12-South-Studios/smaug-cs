using FluentAssertions;
using Realm.Library.Common.Objects;
using Xunit;

namespace Realm.Library.Common.Test
{
    public class PropertyTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Persistable_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test");

            prop.Persistable = setTo;

            prop.Persistable.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Visible_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test");

            prop.Visible = setTo;

            prop.Visible.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Volatile_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test");

            prop.Volatile = setTo;

            prop.Volatile.Should().Be(expectedValue);
        }
    }
}
