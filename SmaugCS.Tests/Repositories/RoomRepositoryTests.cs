using System;
using NUnit.Framework;
using SmaugCS.Database;
using SmaugCS.Exceptions;
using SmaugCS.Objects;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class RoomRepositoryTests
    {
        [Test]
        public void Create()
        {
            var area = new AreaData();

            var repo = new RoomRepository();

            var actual = repo.Create(1, area);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Vnum, Is.EqualTo(1));
            Assert.That(actual.Area, Is.EqualTo(area));
            Assert.That(repo.Contains(1), Is.True);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_ThrowsException()
        {
            var repo = new RoomRepository();

            repo.Create(1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new RoomRepository();

            repo.Create(0, null);
        }

        [Test]
        [ExpectedException(typeof (DuplicateIndexException))]
        public void Create_DuplicateVnum_Test()
        {
            var area = new AreaData();

            var repo = new RoomRepository();

            repo.Create(1, area);
            repo.Create(1, area);
        }
    }
}
