using FluentAssertions;
using SmaugCS.Repository;
using System;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories;

[Collection(CollectionDefinitions.NonParallelCollection)]
public class ObjInstanceRepositoryTests
{
  [Fact]
  public void Create()
  {
    ObjectRepository objRepo = new();
    ObjectTemplate obj = objRepo.Create(1, "TestObj");

    ObjInstanceRepository repo = new();
    ObjectInstance actual = repo.Create(obj, 5);

    actual.Should().NotBeNull();
    actual.Id.Should().BeGreaterThanOrEqualTo(1);
  }

  [Fact]
  public void Create_InvalidParent()
  {
    ObjInstanceRepository repo = new();
    Assert.Throws<ArgumentNullException>(() => repo.Create(null, null));
  }
}