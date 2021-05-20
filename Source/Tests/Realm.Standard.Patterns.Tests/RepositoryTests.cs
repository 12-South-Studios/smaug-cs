using Realm.Standard.Patterns.Repository;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Realm.Standard.Patterns.Tests
{
    public class RepositoryTests
    {
        private class TestRepository : Repository<string, object>
        {
        }

        [Fact]
        public void Repository_Contains_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            var result = repository.Contains("Test");
            result.Should().BeTrue();
        }

        [Fact]
        public void Repository_AddNoMatch_Test()
        {
            var repository = new TestRepository();

            var result = repository.Add("Test", new object());
            result.Should().BeTrue();;
        }

        [Fact]
        public void Repository_AddMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            var result = repository.Add("Test", new object());
            result.Should().BeFalse();
        }

        [Fact]
        public void Repository_DeleteMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            var result = repository.Delete("Test");
            result.Should().BeTrue();
        }

        [Fact]
        public void Repository_DeleteNoMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            var result = repository.Delete("Tester");
            result.Should().BeFalse();
        }

        [Fact]
        public void Repository_GetMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            var result = repository.Get("Test");
            result.Should().NotBeNull();
        }

        [Fact]
        public void Repository_GetNoMatch_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());

            var result = repository.Get("tester");
            result.Should().BeNull();
        }

        [Fact]
        public void Repository_Clear_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Clear();

            repository.Count.Should().Be(0);
        }

        [Fact]
        public void Repository_Count_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Add("Tester", new object());

            repository.Count.Should().Be(2);
        }

        [Fact]
        public void Repository_Keys_Test()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Add("Tester", new object());

            var keyList = new List<string>(repository.Keys);
            keyList.Count.Should().Be(2);
        }

        [Fact]
        public void RepositoryValuesTest()
        {
            var repository = new TestRepository();
            repository.Add("Test", new object());
            repository.Add("Tester", new object());

            var valueList = new List<object>(repository.Values);
            valueList.Count.Should().Be(2);
        }
    }
}
