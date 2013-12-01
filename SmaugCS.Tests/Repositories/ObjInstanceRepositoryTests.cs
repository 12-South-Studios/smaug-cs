using System;
using NUnit.Framework;
using SmaugCS.Database;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_InvalidParent()
        {
            var repo = new ObjInstanceRepository();
            repo.Create(null, null);
        }
    }
}
