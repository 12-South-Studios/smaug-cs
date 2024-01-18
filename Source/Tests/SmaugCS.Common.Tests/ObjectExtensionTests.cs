using FluentAssertions;
using System;
using Xunit;

namespace SmaugCS.Common.Tests
{

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

        [Fact]
        public void GetAttribute_NoMethodFound()
        {
            var obj = new FakeObject();

            obj.GetAttribute<FakeAttribute>("IncorrectMethod").Should().BeNull();
        }

        [Fact]
        public void GetAttribute_NoAttributeFound()
        {
            var obj = new FakeObject();

            obj.GetAttribute<Fake2Attribute>("TestMethod").Should().BeNull();
        }

        [Fact]
        public void GetAttribute_AttributeFound()
        {
            var obj = new FakeObject();

            obj.GetAttribute<FakeAttribute>("TestMethod").Should().NotBeNull();
        }

        [Fact]
        public void HasAttribute_NoMethodFound()
        {
            var obj = new FakeObject();

            obj.HasAttribute<FakeAttribute>("IncorrectMethod").Should().BeFalse();
        }

        [Fact]
        public void HasAttribute_NoAttributeFound()
        {
            var obj = new FakeObject();

            obj.HasAttribute<Fake2Attribute>("TestMethod").Should().BeFalse();
        }

        [Fact]
        public void HasAttribute_AttributeFound()
        {
            var obj = new FakeObject();

            obj.HasAttribute<FakeAttribute>("TestMethod").Should().BeTrue();
        }
    }
}
