using System.Collections.Generic;
using NUnit.Framework;
using Realm.Library.Patterns.Repository;

namespace Realm.Library.Pattern.Test
{
    [TestFixture]
    public class RepositoryTest
    {
        private class TestRepository : Repository<string, object>
        {
        }

        [Test]
        public void Repository_Contains_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            Assert.That(repository.Contains("Test"), Is.True);
        }

        [Test]
        public void Repository_AddNoMatch_Test()
        {
            var repository = new TestRepository();

            Assert.That(repository.Add("Test", new object()), Is.True);
        }

        [Test]
        public void Repository_AddMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            Assert.That(repository.Add("Test", new object()), Is.False);
        }

        [Test]
        public void Repository_DeleteMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            Assert.That(repository.Delete("Test"), Is.True);
        }

        [Test]
        public void Repository_DeleteNoMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            Assert.That(repository.Delete("Tester"), Is.False);
        }

        [Test]
        public void Repository_GetMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            Assert.That(repository.Get("Test"), Is.Not.Null);
        }

        [Test]
        public void Repository_GetNoMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            Assert.That(repository.Get("tester"), Is.Null);
        }

        [Test]
        public void Repository_Clear_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Clear();

            Assert.That(repository.Count, Is.EqualTo(0));
        }

        [Test]
        public void Repository_Count_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Add("Tester", new object());

            Assert.That(repository.Count, Is.EqualTo(2));
        }

        [Test]
        public void Repository_Keys_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Add("Tester", new object());

            var keyList = new List<string>(repository.Keys);
         
            Assert.That(keyList.Count, Is.EqualTo(2));
        }

        [Test]
        public void RepositoryValuesTest()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Add("Tester", new object());

            var valueList = new List<object>(repository.Values);
            
            Assert.That(valueList.Count, Is.EqualTo(2));
        }
    }
}
