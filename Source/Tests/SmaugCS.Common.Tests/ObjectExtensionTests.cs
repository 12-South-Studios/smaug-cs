using NUnit.Framework;
using System;

namespace SmaugCS.Common.Tests
{
    [TestFixture]
    public class ObjectExtensionTests
    {
        private class FakeAttribute : Attribute { }

        private class Fake2Attribute : Attribute { }

        private class FakeObject
        {
            [Fake]
            public bool TestProp { get; set; }

            [Fake]
            public bool TestMethod()
            {
                return true;
            }
        }

        [Test]
        public void GetAttribute_NoMethodFound()
        {
            var obj = new FakeObject();

            Assert.That(obj.GetAttribute<FakeAttribute>("IncorrectMethod"), Is.Null);
        }

        [Test]
        public void GetAttribute_NoAttributeFound()
        {
            var obj = new FakeObject();

            Assert.That(obj.GetAttribute<Fake2Attribute>("TestMethod"), Is.Null);
        }

        [Test]
        public void GetAttribute_AttributeFound()
        {
            var obj = new FakeObject();

            Assert.That(obj.GetAttribute<FakeAttribute>("TestMethod"), Is.Not.Null);
        }

        [Test]
        public void HasAttribute_NoMethodFound()
        {
            var obj = new FakeObject();

            Assert.That(obj.HasAttribute<FakeAttribute>("IncorrectMethod"), Is.False);
        }

        [Test]
        public void HasAttribute_NoAttributeFound()
        {
            var obj = new FakeObject();

            Assert.That(obj.HasAttribute<Fake2Attribute>("TestMethod"), Is.False);
        }

        [Test]
        public void HasAttribute_AttributeFound()
        {
            var obj = new FakeObject();

            Assert.That(obj.HasAttribute<FakeAttribute>("TestMethod"), Is.True);
        }
    }
}
