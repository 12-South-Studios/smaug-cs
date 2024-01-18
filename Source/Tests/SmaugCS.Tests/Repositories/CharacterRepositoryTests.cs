using FluentAssertions;
using SmaugCS.Repository;
using System;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories
{
    [Collection(CollectionDefinitions.NonParallelCollection)]
    public class CharacterRepositoryTests
    {
        //[Fact]
        public void Create()
        {
            var mobRepo = new MobileRepository();
            var mob = mobRepo.Create(1, "TestMob");

            var repo = new CharacterRepository();
            var actual = repo.Create(mob);

            actual.Should().NotBeNull();
            actual.ID.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void Create_InvalidParent()
        {
            var repo = new CharacterRepository();

            Action act = () => repo.Create(null);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
