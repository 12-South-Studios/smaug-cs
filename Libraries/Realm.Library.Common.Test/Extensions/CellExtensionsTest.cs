using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    public class TestCell : Cell
    {
        public TestCell(long id, string name)
        {
            ID = id;
            Name = name;
        }
    }

    [TestFixture]
    public class CellExtensionsTest
    {
        private static TestCell GetTestCell() { return new TestCell(1, "test"); }

        [TestCase("test", true)]
        [TestCase("tester", false)]
        [TestCase("1", true)]
        [TestCase("2", false)]
        [TestCase("te", true)]
        public void CompareNameTest(string value, bool expected)
        {
            var cell = GetTestCell();
            Assert.That(cell.CompareName(value), Is.EqualTo(expected));
        }

        [Test]
        public void CompareNameNameNullEmptyTest()
        {
            var value = new TestCell(1, "");

            Assert.Throws<ArgumentNullException>(() => value.CompareName(string.Empty),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }
    }
}
