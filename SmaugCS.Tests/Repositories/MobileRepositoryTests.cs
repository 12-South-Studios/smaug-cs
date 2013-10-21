using System;
using System.IO;
using NUnit.Framework;
using SmaugCS.Database;
using SmaugCS.Exceptions;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class MobileRepositoryTests
    {
        [Test]
        public void Create()
        {
            var repo = new MobileRepository();

            var actual = repo.Create(1, "Test");

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Vnum, Is.EqualTo(1));
            Assert.That(actual.Name, Is.EqualTo("A newly created Test"));
            Assert.That(repo.Contains(1), Is.True);
        }

        [Test]
        public void Create_CloneObject()
        {
            var repo = new MobileRepository();

            var source = repo.Create(1, "Test");
            source.ShortDescription = "This is a test";

            var cloned = repo.Create(2, 1, "Test2");

            Assert.That(cloned, Is.Not.Null);
            Assert.That(cloned.Vnum, Is.EqualTo(2));
            Assert.That(cloned.Name, Is.EqualTo("A newly created Test2"));
            Assert.That(cloned.ShortDescription, Is.EqualTo(source.ShortDescription));
            Assert.That(repo.Contains(2), Is.True);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Create_ThrowsException()
        {
            var repo = new MobileRepository();

            repo.Create(1, "");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new MobileRepository();

            repo.Create(0, "Test");
        }

        [Test]
        [ExpectedException(typeof(DuplicateIndexException))]
        public void Create_DuplicateVnum()
        {
            var repo = new MobileRepository();

            repo.Create(1, "Test");
            repo.Create(1, "Test2");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Create_Clone_InvalidCloneVnum()
        {
            var repo = new MobileRepository();

            repo.Create(1, 1, "Test");
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void Create_Clone_MissingCloneMob()
        {
            var repo = new MobileRepository();

            repo.Create(1, "Test");
            repo.Create(2, 5, "Test2");
        }
    }
}
