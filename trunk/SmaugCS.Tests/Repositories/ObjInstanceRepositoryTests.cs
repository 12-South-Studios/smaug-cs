using System;
using System.IO;
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

            var repo = new ObjInstanceRepository(5, 5);
            var actual = repo.Create(obj, 5);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.ID, Is.EqualTo(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void Create_InvalidParent()
        {
            var repo = new ObjInstanceRepository(5, 5);
            repo.Create(null, 0);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void Create_InvalidLevel()
        {
            var objRepo = new ObjectRepository();
            var obj = objRepo.Create(1, "TestObj");

            var repo = new ObjInstanceRepository(5, 5);
            repo.Create(obj, 0);
        }
    }
}
