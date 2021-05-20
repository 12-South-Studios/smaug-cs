using System.Linq;
using FluentAssertions;
using Realm.Library.Common.Comparers;
using Xunit;

namespace Realm.Library.Common.Test.Comparers
{
    public class NaturalSortStringComparerTest
    {
        [Theory]
        [InlineData(new object[] { "Test", "Test" }, new object[] { "Test", "Test" }, true)]
        [InlineData(new object[] { "Tester", "Test", "Testing" }, new object[] { "Test", "Tester", "Testing" }, true)]
        [InlineData(new object[] { "25", "05", "15" }, new object[] { "05", "15", "25" }, true)]
        [InlineData(new object[] { "Test", "5", "Tester15", "Tester25", "04", "15Test" }, new object[] { "04", "5", "15Test", "Test", "Tester15", "Tester25" }, true)]
        [InlineData(new object[] { "Tester", "Test", "Testing" }, new object[] { "Testing", "Tester", "Test" }, false)]
        [InlineData(new object[] { "25", "05", "15" }, new object[] { "25", "15", "05" }, false)]
        [InlineData(new object[] { "Test", "5", "Tester15", "Tester25", "04", "15Test" }, new object[] { "Tester25", "Tester15", "Test", "15Test", "5", "04" }, false)]
        public void NaturalSortStringComparerStandardTest(object[] values, object[] expectedList, bool ascending)
        {
            var sortedList =
                values.Cast<string>().ToList().OrderBy(x => x, new NaturalSortStringComparer(ascending)).ToList();

            var result = expectedList.SequenceEqual(sortedList);
            result.Should().BeTrue();
        }
    }
}
