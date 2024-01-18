using FluentAssertions;
using System;
using Xunit;

namespace SmaugCS.Common.Tests
{

    public class EnumerationExtensionTests
    {
        private class FakeAttribute : Attribute { }

        private class Fake2Attribute : Attribute { }

        private enum FakeEnum
        {
            [Fake]
            Value1,

            [Fake2]
            Value2
        }

        [Fact]
        public void GetAttribute_NotFound()
        {
            FakeEnum.Value1.GetAttribute<Fake2Attribute>().Should().BeNull();
        }

        [Fact]
        public void GetAttribute_Found()
        {
            FakeEnum.Value1.GetAttribute<FakeAttribute>().Should().NotBeNull();
        }

        [Fact]
        public void HasAttribute_NotFound()
        {
            FakeEnum.Value1.HasAttribute<Fake2Attribute>().Should().BeFalse();
        }

        [Fact]
        public void HasAttribute_Found()
        {
            FakeEnum.Value1.HasAttribute<FakeAttribute>().Should().BeTrue();
        }
    }
}
