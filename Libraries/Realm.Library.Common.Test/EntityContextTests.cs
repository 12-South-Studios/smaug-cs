using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class EntityContextTests
    {
        private class FakeObject : Entity 
        {
            public FakeObject(long id, string name) : base(id, name)
            {
            }
        }

        private class FakeContext : EntityContext<FakeObject>
        {
            public FakeContext(FakeObject parent) : base(parent)
            {
            }
        }

        [Test]
        public void Contains_Object_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            Assert.That(ctx.Contains(child), Is.True);
        }

        [Test]
        public void Contains_Id_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            Assert.That(ctx.Contains(2), Is.True);
        }

        [Test]
        public void Remove_Object_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            Assert.That(ctx.Contains(2), Is.True);

            ctx.RemoveEntity(child);

            Assert.That(ctx.Contains(2), Is.False);
        }

        [Test]
        public void GetEntity_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            var result = ctx.GetEntity(2);

            Assert.That(result, Is.EqualTo(child));
        }

        [Test]
        public void Count_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            Assert.That(ctx.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddEntities_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child1 = new FakeObject(2, "Child1");
            var child2 = new FakeObject(3, "Child2");
            var child3 = new FakeObject(4, "Child3");

            var list = new List<FakeObject>() {child1, child2, child3};

            var ctx = new FakeContext(parent);
            ctx.AddEntities(list);

            Assert.That(ctx.Count, Is.EqualTo(3));
        }

        [Test]
        public void Entities_Test()
        {
            var parent = new FakeObject(1, "Parent");

            var child1 = new FakeObject(2, "Child1");
            var child2 = new FakeObject(3, "Child2");
            var child3 = new FakeObject(4, "Child3");

            var list = new List<FakeObject>() { child1, child2, child3 };

            var ctx = new FakeContext(parent);
            ctx.AddEntities(list);

            var results = ctx.Entities;

            CollectionAssert.AreEquivalent(list, results);
        }
    }
}
