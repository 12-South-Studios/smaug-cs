using System;
using NUnit.Framework;

namespace SmaugCS.Common.Tests
{
    [TestFixture]
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

        [Test]
        public void GetAttribute_NotFound()
        {
            Assert.That(FakeEnum.Value1.GetAttribute<Fake2Attribute>(), Is.Null);
        }

        [Test]
        public void GetAttribute_Found()
        {
            Assert.That(FakeEnum.Value1.GetAttribute<FakeAttribute>(), Is.Not.Null);
        }

        [Test]
        public void HasAttribute_NotFound()
        {
            Assert.That(FakeEnum.Value1.HasAttribute<Fake2Attribute>(), Is.False);
        }

        [Test]
        public void HasAttribute_Found()
        {
            Assert.That(FakeEnum.Value1.HasAttribute<FakeAttribute>(), Is.True);
        }
    }
}
