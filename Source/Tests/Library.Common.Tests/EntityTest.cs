using FluentAssertions;
using Library.Common.Objects;
using Xunit;

namespace Library.Common.Tests;

public class EntityFact
{
  private class FakeEntity(long id, string name) : Entity(id, name);

  [Fact]
  public void EntityEqualsObjectNullFact()
  {
    FakeEntity entity = new(1, "Fact");

    entity.Equals(null).Should().BeFalse();
  }

  [Fact]
  public void EntityEqualsNotSameTypeFact()
  {
    FakeEntity entity = new(1, "Fact");
    object factObj = new();

    entity.Equals(factObj).Should().BeFalse();
  }

  [Fact]
  public void EntityEqualsNotSameFact()
  {
    FakeEntity entity1 = new(1, "Fact");
    FakeEntity entity2 = new(2, "Facter");

    entity1.Equals(entity2).Should().BeFalse();
  }

  [Fact]
  public void EntityEqualsSameFact()
  {
    FakeEntity entity1 = new(1, "Fact");
    FakeEntity entity2 = new(1, "Fact");

    entity1.Equals(entity2).Should().BeTrue();
  }
}