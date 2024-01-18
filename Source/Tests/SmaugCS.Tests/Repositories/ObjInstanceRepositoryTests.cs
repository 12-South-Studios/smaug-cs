using FluentAssertions;
using SmaugCS.Repository;
using System;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories
{
    [Collection(CollectionDefinitions.NonParallelCollection)]
    public class ObjInstanceRepositoryTests
    {
        [Fact]
        public void Create()
        {
            var objRepo = new ObjectRepository();
            var obj = objRepo.Create(1, "TestObj");

            var repo = new ObjInstanceRepository();
            var actual = repo.Create(obj, 5);

            actual.Should().NotBeNull();
            actual.ID.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void Create_InvalidParent()
        {
            var repo = new ObjInstanceRepository();
            Assert.Throws<ArgumentNullException>(() => repo.Create(null, null));
        }
    }
}
