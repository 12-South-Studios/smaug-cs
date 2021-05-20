using System.Collections.Generic;
using FluentAssertions;
using Realm.Library.Common.Entities;
using Realm.Library.Common.Objects;
using Xunit;

namespace Realm.Library.Common.Fact
{
    public class EntityContextFacts
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

        [Fact]
        public void Contains_Object_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            ctx.Contains(child).Should().BeTrue();
        }

        [Fact]
        public void Contains_Id_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            ctx.Contains(2).Should().BeTrue();
        }

        [Fact]
        public void Remove_Object_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            ctx.Contains(2).Should().BeTrue();

            ctx.RemoveEntity(child);

            ctx.Contains(2).Should().BeFalse();
        }

        [Fact]
        public void GetEntity_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            var result = ctx.GetEntity(2);
            result.Should().Be(child);
        }

        [Fact]
        public void Count_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child = new FakeObject(2, "Child");

            var ctx = new FakeContext(parent);
            ctx.AddEntity(child);

            ctx.Count.Should().Be(1);
        }

        [Fact]
        public void AddEntities_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child1 = new FakeObject(2, "Child1");
            var child2 = new FakeObject(3, "Child2");
            var child3 = new FakeObject(4, "Child3");

            var list = new List<FakeObject>() {child1, child2, child3};

            var ctx = new FakeContext(parent);
            ctx.AddEntities(list);

            ctx.Count.Should().Be(3);
        }

        [Fact]
        public void Entities_Fact()
        {
            var parent = new FakeObject(1, "Parent");

            var child1 = new FakeObject(2, "Child1");
            var child2 = new FakeObject(3, "Child2");
            var child3 = new FakeObject(4, "Child3");

            var list = new List<FakeObject>() { child1, child2, child3 };

            var ctx = new FakeContext(parent);
            ctx.AddEntities(list);

            var results = ctx.Entities;

            list.Should().BeEquivalentTo(results);
        }
    }
}
