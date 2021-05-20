using FluentAssertions;
using Realm.Library.Common.Objects;
using Xunit;

namespace Realm.Library.Common.Fact
{
    public class EntityFact
    {
        private class FakeEntity : Entity
        {
            public FakeEntity(long id, string name)
                : base(id, name)
            {
            }
        }

        [Fact]
        public void EntityEqualsObjectNullFact()
        {
            var entity = new FakeEntity(1, "Fact");

            entity.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void EntityEqualsNotSameTypeFact()
        {
            var entity = new FakeEntity(1, "Fact");
            var FactObj = new object();

            entity.Equals(FactObj).Should().BeFalse();
        }

        [Fact]
        public void EntityEqualsNotSameFact()
        {
            var entity1 = new FakeEntity(1, "Fact");
            var entity2 = new FakeEntity(2, "Facter");

            entity1.Equals(entity2).Should().BeFalse();
        }

        [Fact]
        public void EntityEqualsSameFact()
        {
            var entity1 = new FakeEntity(1, "Fact");
            var entity2 = new FakeEntity(1, "Fact");

            entity1.Equals(entity2).Should().BeTrue();
        }

        [Fact]
        public void EntityGetHashCodeFact()
        {
            var entity = new FakeEntity(1, "Fact");

            entity.GetHashCode().Should().Be(358537656);
        }
    }
}
