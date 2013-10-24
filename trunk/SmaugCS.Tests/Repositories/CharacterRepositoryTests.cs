using System;
using System.IO;
using NUnit.Framework;
using SmaugCS.Database;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class CharacterRepositoryTests
    {
        [Test]
        public void Create()
        {
            var mobRepo = new MobileRepository();
            var mob = mobRepo.Create(1, "TestMob");

            var repo = new CharacterRepository();
            var actual = repo.Create(mob);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.ID, Is.EqualTo(1));
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Create_InvalidParent()
        {
            var repo = new CharacterRepository();
            repo.Create(null);
        }
    }
}
