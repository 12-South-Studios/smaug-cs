using System;
using System.Linq;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class PropertyContextTests
    {
        [Flags]
        private enum TestEnum
        {
            Test1,
            Test2
        };

        private static PropertyContext GetContext()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new PropertyContext(fake);

            return ctx;
        }

        [Test]
        [Category("Context Tests")]
        public void SetProperty_StringName_NoOptions_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty("NullProperty", "Nothing");

            Assert.That(ctx.GetProperty<string>("NullProperty"), Is.EqualTo("Nothing"));
        }

        [Test]
        [Category("Context Tests")]
        public void SetProperty_StringName_Volatile_SetExisting_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty("NullProperty", "Nothing", PropertyTypeOptions.Volatile);

            Assert.That(ctx.GetProperty<string>("NullProperty"), Is.EqualTo("Nothing"));

            ctx.SetProperty("NullProperty", "Still Nothing");

            Assert.That(ctx.GetProperty<string>("NullProperty"), Is.EqualTo("Still Nothing"));
        }

        [Test]
        [Category("Context Tests")]
        public void SetProperty_Enum_NoOptions_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.GetProperty<string>("Test1"), Is.EqualTo("Nothing"));
        }

        [Test]
        [Category("Context Tests")]
        public void HasProperty_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.HasProperty("Test1"), Is.True);
        }

        [Test]
        [Category("Context Tests")]
        public void IsPersistable_True_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing", PropertyTypeOptions.Persistable);

            Assert.That(ctx.IsPersistable("Test1"), Is.True);
        }

        [Test]
        [Category("Context Tests")]
        public void IsPersistable_False_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.IsPersistable("Test1"), Is.False);
        }

        [Test]
        [Category("Context Tests")]
        public void IsVisible_True_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing", PropertyTypeOptions.Visible);

            Assert.That(ctx.IsVisible("Test1"), Is.True);
        }

        [Test]
        [Category("Context Tests")]
        public void IsVisible_False_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.IsVisible("Test1"), Is.False);
        }

        [Test]
        [Category("Context Tests")]
        public void IsVolatile_True_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing", PropertyTypeOptions.Volatile);

            Assert.That(ctx.IsVolatile("Test1"), Is.True);
        }

        [Test]
        [Category("Context Tests")]
        public void IsVolatile_False_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.IsVolatile("Test1"), Is.False);
        }

        [Test]
        [Category("Context Tests")]
        public void RemoveProperty_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing");

            Assert.That(ctx.HasProperty("Test1"), Is.True);

            ctx.RemoveProperty("Test1");

            Assert.That(ctx.HasProperty("Test1"), Is.False);
        }

        [TestCase(PropertyTypeOptions.None, "")]
        [TestCase(PropertyTypeOptions.Persistable, "p")]
        [TestCase(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible, "pi")]
        [TestCase(PropertyTypeOptions.Persistable | PropertyTypeOptions.Visible | PropertyTypeOptions.Volatile, "pvi")]
        [Category("Context Tests")]
        public void GetPropertyBits_Test(PropertyTypeOptions options, string expectedValue)
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Nothing", options);

            Assert.That(ctx.GetPropertyBits("Test1"), Is.EqualTo(expectedValue));
        }

        [Test]
        [Category("Context Tests")]
        public void Count_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Test #1");
            ctx.SetProperty(TestEnum.Test2, "Test #2");
            
            Assert.That(ctx.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Context Tests")]
        public void PropertyKeys_Test()
        {
            var ctx = GetContext();
            ctx.SetProperty(TestEnum.Test1, "Test #1");
            ctx.SetProperty(TestEnum.Test2, "Test #2");

            var keys = ctx.PropertyKeys.ToList();

            Assert.That(keys, Is.Not.Null);
            Assert.That(keys.Count(), Is.EqualTo(2));
            Assert.That(keys.Contains("Test1"), Is.True);
            Assert.That(keys.Contains("Test2"), Is.True);
        }
    }
}
