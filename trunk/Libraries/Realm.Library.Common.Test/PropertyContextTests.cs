using System;
using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class PropertyContextTests
    {
        private class FakeEntity : Entity
        {
            public FakeEntity(long id, string name) : base(id, name)
            {
            }
        }

        [Flags]
        private enum TestEnum
        {
            Test1,
            Test2
        };

        [Test]
        public void SetProperty_StringName_NoOptions_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty("NullProperty", "Nothing");

            Assert.That(ctx.GetProperty<string>("NullProperty"), Is.EqualTo("Nothing"));
        }

        [Test]
        public void SetProperty_StringName_Volatile_SetExisting_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty("NullProperty", "Nothing", PropertyTypeOptions.Volatile);

            Assert.That(ctx.GetProperty<string>("NullProperty"), Is.EqualTo("Nothing"));

            ctx.SetProperty("NullProperty", "Still Nothing");

            Assert.That(ctx.GetProperty<string>("NullProperty"), Is.EqualTo("Still Nothing"));
        }

        [Test]
        public void SetProperty_Enum_NoOptions_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.GetProperty<string>("Test1"), Is.EqualTo("Nothing"));
        }

        [Test]
        public void HasProperty_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.HasProperty("Test1"), Is.True);
        }

        [Test]
        public void IsPersistable_True_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing", PropertyTypeOptions.Persistable);

            Assert.That(ctx.IsPersistable("Test1"), Is.True);
        }

        [Test]
        public void IsPersistable_False_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.IsPersistable("Test1"), Is.False);
        }

        [Test]
        public void IsVisible_True_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing", PropertyTypeOptions.Visible);

            Assert.That(ctx.IsVisible("Test1"), Is.True);
        }

        [Test]
        public void IsVisible_False_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.IsVisible("Test1"), Is.False);
        }

        [Test]
        public void IsVolatile_True_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing", PropertyTypeOptions.Volatile);

            Assert.That(ctx.IsVolatile("Test1"), Is.True);
        }

        [Test]
        public void IsVolatile_False_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.IsVolatile("Test1"), Is.False);
        }

        [Test]
        public void RemoveProperty_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.HasProperty("Test1"), Is.True);

            ctx.RemoveProperty("Test1");

            Assert.That(ctx.HasProperty("Test1"), Is.False);
        }

        [TestCase(PropertyTypeOptions.None, "")]
        [TestCase(PropertyTypeOptions.Persistable, "p")]
        [TestCase(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible, "pi")]
        [TestCase(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible | PropertyTypeOptions.Volatile, "pvi")]
        public void GetPropertyBits_Test(PropertyTypeOptions options, string expectedValue)
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Nothing", options);

            Assert.That(ctx.GetPropertyBits("Test1"), Is.EqualTo(expectedValue));
        }

        [Test]
        public void Count_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Test #1");
            ctx.SetProperty(TestEnum.Test2, "Test #2");
            
            Assert.That(ctx.Count, Is.EqualTo(2));
        }

        [Test]
        public void PropertyKeys_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);
            ctx.SetProperty(TestEnum.Test1, "Test #1");
            ctx.SetProperty(TestEnum.Test2, "Test #2");

            var keys = ctx.PropertyKeys;

            Assert.That(keys, Is.Not.Null);
            Assert.That(keys.Count(), Is.EqualTo(2));
            Assert.That(keys.Contains("Test1"), Is.True);
            Assert.That(keys.Contains("Test2"), Is.True);
        }
    }
}
