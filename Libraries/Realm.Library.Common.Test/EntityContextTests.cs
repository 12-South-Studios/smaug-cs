using System.Collections.Generic;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class EntityContextTests
    {
        private static FakeEntity GetParent()
        {
            return new FakeEntity(1, "Parent");
        }

        private static FakeEntity GetChild()
        {
            return new FakeEntity(2, "Child");
        }

        private static FakeContext GetContext()
        {
            return new FakeContext(GetParent());
        }

        [Test]
        [Category("Context Tests")]
        public void Contains_Object_Test()
        {
            var ctx = GetContext();
            var child = GetChild();
            ctx.AddEntity(child);

            Assert.That(ctx.Contains(child), Is.True);
        }

        [Test]
        [Category("Context Tests")]
        public void Contains_Id_Test()
        {
            var ctx = GetContext();
            var child = GetChild();
            ctx.AddEntity(child);

            Assert.That(ctx.Contains(2), Is.True);
        }

        [Test]
        [Category("Context Tests")]
        public void Remove_Object_Test()
        {
            var ctx = GetContext();
            var child = GetChild();
            ctx.AddEntity(child);
            ctx.RemoveEntity(child);

            Assert.That(ctx.Contains(2), Is.False);
        }

        [Test]
        [Category("Context Tests")]
        public void GetEntity_Test()
        {
            var ctx = GetContext();
            var child = GetChild();
            ctx.AddEntity(child);

            var result = ctx.GetEntity(2);

            Assert.That(result, Is.EqualTo(child));
        }

        [Test]
        [Category("Context Tests")]
        public void Count_Test()
        {
            var ctx = GetContext();
            var child = GetChild();
            ctx.AddEntity(child);

            Assert.That(ctx.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Context Tests")]
        public void AddEntities_Test()
        {
            var child1 = GetChild();
            var child2 = new FakeEntity(3, "Child2");
            var child3 = new FakeEntity(4, "Child3");

            var list = new List<FakeEntity> {child1, child2, child3};

            var ctx = GetContext();
            ctx.AddEntities(list);

            Assert.That(ctx.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("Context Tests")]
        public void Entities_Test()
        {
            var child1 = GetChild();
            var child2 = new FakeEntity(3, "Child2");
            var child3 = new FakeEntity(4, "Child3");

            var list = new List<FakeEntity> { child1, child2, child3 };

            var ctx = GetContext();
            ctx.AddEntities(list);

            var results = ctx.Entities;

            CollectionAssert.AreEquivalent(list, results);
        }
    }
}
