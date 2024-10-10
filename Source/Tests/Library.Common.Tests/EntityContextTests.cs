using System.Collections.Generic;
using FluentAssertions;
using Library.Common.Entities;
using Library.Common.Objects;
using Xunit;

namespace Library.Common.Tests;

public class EntityContextFacts
{
  private class FakeObject(long id, string name) : Entity(id, name);

  private class FakeContext(FakeObject parent) : EntityContext<FakeObject>(parent);

  [Fact]
  public void Contains_Object_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child = new(2, "Child");

    FakeContext ctx = new(parent);
    ctx.AddEntity(child);

    ctx.Contains(child).Should().BeTrue();
  }

  [Fact]
  public void Contains_Id_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child = new(2, "Child");

    FakeContext ctx = new(parent);
    ctx.AddEntity(child);

    ctx.Contains(2).Should().BeTrue();
  }

  [Fact]
  public void Remove_Object_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child = new(2, "Child");

    FakeContext ctx = new(parent);
    ctx.AddEntity(child);

    ctx.Contains(2).Should().BeTrue();

    ctx.RemoveEntity(child);

    ctx.Contains(2).Should().BeFalse();
  }

  [Fact]
  public void GetEntity_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child = new(2, "Child");

    FakeContext ctx = new(parent);
    ctx.AddEntity(child);

    FakeObject result = ctx.GetEntity(2);
    result.Should().Be(child);
  }

  [Fact]
  public void Count_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child = new(2, "Child");

    FakeContext ctx = new(parent);
    ctx.AddEntity(child);

    ctx.Count.Should().Be(1);
  }

  [Fact]
  public void AddEntities_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child1 = new(2, "Child1");
    FakeObject child2 = new(3, "Child2");
    FakeObject child3 = new(4, "Child3");

    List<FakeObject> list = [child1, child2, child3];

    FakeContext ctx = new(parent);
    ctx.AddEntities(list);

    ctx.Count.Should().Be(3);
  }

  [Fact]
  public void Entities_Fact()
  {
    FakeObject parent = new(1, "Parent");

    FakeObject child1 = new(2, "Child1");
    FakeObject child2 = new(3, "Child2");
    FakeObject child3 = new(4, "Child3");

    List<FakeObject> list = [child1, child2, child3];

    FakeContext ctx = new(parent);
    ctx.AddEntities(list);

    IList<FakeObject> results = ctx.Entities;

    list.Should().BeEquivalentTo(results);
  }
}