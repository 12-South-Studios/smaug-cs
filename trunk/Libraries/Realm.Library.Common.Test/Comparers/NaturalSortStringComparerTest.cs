using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Comparers
{
    [TestFixture]
    public class NaturalSortStringComparerTest
    {
        [TestCase(new object[] { "Test", "Test" }, new object[] { "Test", "Test" }, true)]
        [TestCase(new object[] { "Tester", "Test", "Testing" }, new object[] { "Test", "Tester", "Testing" }, true)]
        [TestCase(new object[] { "25", "05", "15" }, new object[] { "05", "15", "25" }, true)]
        [TestCase(new object[] { "Test", "5", "Tester15", "Tester25", "04", "15Test" }, new object[] { "04", "5", "15Test", "Test", "Tester15", "Tester25" }, true)]
        [TestCase(new object[] { "Tester", "Test", "Testing" }, new object[] { "Testing", "Tester", "Test" }, false)]
        [TestCase(new object[] { "25", "05", "15" }, new object[] { "25", "15", "05" }, false)]
        [TestCase(new object[] { "Test", "5", "Tester15", "Tester25", "04", "15Test" }, new object[] { "Tester25", "Tester15", "Test", "15Test", "5", "04" }, false)]
        public void NaturalSortStringComparerStandardTest(object[] values, object[] expectedList, bool ascending)
        {
            var sortedList =
                values.Cast<string>().ToList().OrderBy(x => x, new NaturalSortStringComparer(ascending)).ToList();

            Assert.That(expectedList.SequenceEqual(sortedList), Is.True);
        }
    }
}
