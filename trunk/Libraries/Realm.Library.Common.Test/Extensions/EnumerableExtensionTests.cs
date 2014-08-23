using System.Collections.Generic;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class EnumerableExtensionTests
    {
        private static IEnumerable<int> GetEnumerableIntegerList()
        {
            return new List<int> { 5, 10, 15, 20, 25 };
        }

        private static IEnumerable<FakeEntity> GetEnumerableFakeEntityList()
        {
            return new List<FakeEntity>
                       {
                           new FakeEntity(1, "Test1"),
                           new FakeEntity(2, "Test2"),
                           new FakeEntity(3, "Test3")
                       };
        }

        [Test]
        [Category("Extension Tests")]
        public void IndexOfTest()
        {
            const int expected = 2;

            Assert.That(GetEnumerableIntegerList().IndexOf(15), Is.EqualTo(expected));
        }

        [Test]
        [Category("Extension Tests")]
        public void IndexOfWithComparerTest()
        {
            const int expected = 1;
            var comparer = new GenericEqualityComparer<FakeEntity>(Equals);

            Assert.That(GetEnumerableFakeEntityList().IndexOf(new FakeEntity(2, "Test2"), comparer),
                Is.EqualTo(expected));
        }

        [TestCase(10, 15)]
        [TestCase(50, 5)]
        [TestCase(25, 25)]
        [Category("Extension Tests")]
        public void PeekWithValidValueTest(int peekValue, int expectedValue)
        {
            Assert.That(GetEnumerableIntegerList().Peek(peekValue), Is.EqualTo(expectedValue));
        }
    }
}
