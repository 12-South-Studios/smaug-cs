using FluentAssertions;
using SmaugCS.Repository;
using System;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories;

[Collection(CollectionDefinitions.NonParallelCollection)]
public class CharacterRepositoryTests
{
  //[Fact]
  public void Create()
  {
    MobileRepository mobRepo = new();
    MobileTemplate mob = mobRepo.Create(1, "TestMob");

    CharacterRepository repo = new();
    CharacterInstance actual = repo.Create(mob);

    actual.Should().NotBeNull();
    actual.Id.Should().BeGreaterThanOrEqualTo(1);
  }

  [Fact]
  public void Create_InvalidParent()
  {
    CharacterRepository repo = new();

    Action act = () => repo.Create(null);
    act.Should().Throw<ArgumentNullException>();
  }
}