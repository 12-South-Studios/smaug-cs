using NUnit.Framework;
using SmaugCS.Repository;
using System;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class ObjInstanceRepositoryTests
    {
        [Test]
        public void Create()
        {
            var objRepo = new ObjectRepository();
            var obj = objRepo.Create(1, "TestObj");

            var repo = new ObjInstanceRepository();
            var actual = repo.Create(obj, 5);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.ID, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Create_InvalidParent()
        {
            var repo = new ObjInstanceRepository();
            Assert.Throws<ArgumentNullException>(() => repo.Create(null, null));
        }
    }
}
