using System.Collections.Generic;
using FluentAssertions;
using Realm.Library.Common.Extensions;
using Xunit;

namespace Realm.Library.Common.Test.Extensions
{
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
                hash = hash * 7 + Name.GetHashCode();
                return hash;
            }

            public override string ToString()
            {
                return $"{Name}";
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

        [Fact]
        public void IndexOfTest()
        {
            const int expected = 2;

            var result = GetEnumerableIntegerList().IndexOf(15);
            result.Should().Be(expected);
        }

        [Fact]
        public void IndexOfWithComparerTest()
        {
            const int expected = 1;
            var comparer = new GenericEqualityComparer<Fake>(Equals);

            var result = GetEnumerableFakeList().IndexOf(new Fake("Test2"), comparer);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(10, 15)]
        [InlineData(50, 5)]
        [InlineData(25, 25)]
        public void PeekWithValidValueTest(int peekValue, int expectedValue)
        {
            var result = GetEnumerableIntegerList().Peek(peekValue);
            result.Should().Be(expectedValue);
        }
    }
}
