using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class EntityTest
    {
        private class FakeEntity : Entity
        {
            public FakeEntity(long id, string name)
                : base(id, name)
            {
            }
        }

        [Test]
        public void EntityEqualsObjectNullTest()
        {
            var entity = new FakeEntity(1, "Test");

            Assert.That(entity.Equals(null), Is.False);
        }

        [Test]
        public void EntityEqualsNotSameTypeTest()
        {
            var entity = new FakeEntity(1, "test");
            var testObj = new object();

            Assert.That(entity.Equals(testObj), Is.False);
        }

        [Test]
        public void EntityEqualsNotSameTest()
        {
            var entity1 = new FakeEntity(1, "Test");
            var entity2 = new FakeEntity(2, "tester");

            Assert.That(entity1.Equals(entity2), Is.False);
        }

        [Test]
        public void EntityEqualsSameTest()
        {
            var entity1 = new FakeEntity(1, "Test");
            var entity2 = new FakeEntity(1, "Test");

            Assert.That(entity1.Equals(entity2), Is.True);
        }

        [Test]
        public void EntityGetHashCodeTest()
        {
            var entity = new FakeEntity(1, "test");

            Assert.That(entity.GetHashCode(), Is.EqualTo(-354185222));
        }
    }
}
