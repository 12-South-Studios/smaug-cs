using System.Collections.Generic;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class EnumerableExtensionTests
    {
        private static IEnumerable<int> GetEnumerableIntegerList()
        {
            return new List<int>() { 5, 10, 15, 20, 25 };
        }

        private class Fake
        {
            private string Name { get; set; }

            public Fake(string name)
            {
                Name = name;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;

                Fake fake = obj as Fake;
                return fake != null && fake.Name == Name;
            }

            public override int GetHashCode()
            {
                int hash = 13;
                hash = (hash * 7) + Name.GetHashCode();
                return hash;
            }

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        private static IEnumerable<Fake> GetEnumerableFakeList()
        {
            return new List<Fake>()
                       {
                           new Fake("Test1"),
                           new Fake("Test2"),
                           new Fake("Test3")
                       };
        }

        [Test]
        public void IndexOfTest()
        {
            const int expected = 2;

            Assert.That(GetEnumerableIntegerList().IndexOf(15), Is.EqualTo(expected));
        }

        [Test]
        public void IndexOfWithComparerTest()
        {
            const int expected = 1;
            var comparer = new GenericEqualityComparer<Fake>(Equals);

            Assert.That(GetEnumerableFakeList().IndexOf(new Fake("Test2"), comparer), Is.EqualTo(expected));
        }

        [TestCase(10, 15)]
        [TestCase(50, 5)]
        [TestCase(25, 25)]
        public void PeekWithValidValueTest(int peekValue, int expectedValue)
        {
            Assert.That(GetEnumerableIntegerList().Peek(peekValue), Is.EqualTo(expectedValue));
        }
    }
}
