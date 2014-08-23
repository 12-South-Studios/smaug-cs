using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class EntityTest
    {
        private static FakeEntity GetEntity()
        {
            return new FakeEntity(1, "Test");
        }

        [Test]
        [Category("Object Tests")]
        public void EntityEqualsObjectNullTest()
        {
            var entity = GetEntity();

            Assert.That(entity.Equals(null), Is.False);
        }

        [Test]
        [Category("Object Tests")]

        public void EntityEqualsNotSameTypeTest()
        {
            var entity = GetEntity();
            var testObj = new object();

            Assert.That(entity.Equals(testObj), Is.False);
        }

        [Test]
        [Category("Object Tests")]
        public void EntityEqualsNotSameTest()
        {
            var entity1 = GetEntity();
            var entity2 = new FakeEntity(2, "tester");

            Assert.That(entity1.Equals(entity2), Is.False);
        }

        [Test]
        [Category("Object Tests")]
        public void EntityEqualsSameTest()
        {
            var entity1 = GetEntity();
            var entity2 = new FakeEntity(1, "Test");

            Assert.That(entity1.Equals(entity2), Is.True);
        }

        [Test]
        [Category("Object Tests")]
        public void EntityGetHashCodeTest()
        {
            var entity = GetEntity();

            Assert.That(entity.GetHashCode(), Is.EqualTo(-354185486));
        }
    }
}
