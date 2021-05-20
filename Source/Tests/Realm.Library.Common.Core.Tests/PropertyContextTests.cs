using System;
using System.Linq;
using FluentAssertions;
using Realm.Library.Common.Contexts;
using Realm.Library.Common.Objects;
using Xunit;

namespace Realm.Library.Common.Fact
{
    public class PropertyContextFacts
    {
        private class FakeEntity : Entity
        {
            public FakeEntity(long id, string name) : base(id, name)
            {
            }
        }

        [Flags]
        private enum FactEnum
        {
            Fact1,
            Fact2
        };

        [Fact]
        public void SetProperty_StringName_NoOptions_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty("NullProperty", "Nothing");

            var result = ctx.GetProperty<string>("NullProperty");
            result.Should().Be("Nothing");
        }

        [Fact]
        public void SetProperty_StringName_Volatile_SetExisting_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty("NullProperty", "Nothing", PropertyTypeOptions.Volatile);

            var result = ctx.GetProperty<string>("NullProperty");
            result.Should().Be("Nothing");

            ctx.SetProperty("NullProperty", "Still Nothing");

            result = ctx.GetProperty<string>("NullProperty");
            result.Should().Be("Still Nothing");
        }

        [Fact]
        public void SetProperty_Enum_NoOptions_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing");

            var result = ctx.GetProperty<string>("Fact1");
            result.Should().Be("Nothing");
        }

        [Fact]
        public void HasProperty_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing");

            var result = ctx.HasProperty("Fact1");
            result.Should().BeTrue();
        }

        [Fact]
        public void IsPersistable_True_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing", PropertyTypeOptions.Persistable);

            var result = ctx.IsPersistable("Fact1");
            result.Should().BeTrue();
        }

        [Fact]
        public void IsPersistable_False_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing");

            var result = ctx.IsPersistable("Fact1");
            result.Should().BeFalse();
        }

        [Fact]
        public void IsVisible_True_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing", PropertyTypeOptions.Visible);

            var result = ctx.IsVisible("Fact1");
            result.Should().BeTrue();
        }

        [Fact]
        public void IsVisible_False_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing");

            var result = ctx.IsVisible("Fact1");
            result.Should().BeFalse();
        }

        [Fact]
        public void IsVolatile_True_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing", PropertyTypeOptions.Volatile);

            var result = ctx.IsVolatile("Fact1");
            result.Should().BeTrue();
        }

        [Fact]
        public void IsVolatile_False_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing");

            var result = ctx.IsVolatile("Fact1");
            result.Should().BeFalse();
        }

        [Fact]
        public void RemoveProperty_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing");

            var result = ctx.HasProperty("Fact1");
            result.Should().BeTrue();

            ctx.RemoveProperty("Fact1");

            result = ctx.HasProperty("Fact1");
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(PropertyTypeOptions.None, "")]
        [InlineData(PropertyTypeOptions.Persistable, "p")]
        [InlineData(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible, "pi")]
        [InlineData(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible | PropertyTypeOptions.Volatile, "pvi")]
        public void GetPropertyBits_Fact(PropertyTypeOptions options, string expectedValue)
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Nothing", options);

            var result = ctx.GetPropertyBits("Fact1");
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void Count_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Fact #1");
            ctx.SetProperty(FactEnum.Fact2, "Fact #2");
            
            ctx.Count.Should().Be(2);
        }

        [Fact]
        public void PropertyKeys_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(FactEnum.Fact1, "Fact #1");
            ctx.SetProperty(FactEnum.Fact2, "Fact #2");

            var keys = ctx.PropertyKeys;

            keys.Should().NotBeNull();
            keys.Count().Should().Be(2);
            keys.Contains("Fact1").Should().BeTrue();
            keys.Contains("Fact2").Should().BeTrue();
        }
    }
}
