using System;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class CellExtensionsTest
    {
        private static FakeCell GetFakeCell() { return new FakeCell(1, "test"); }

        [TestCase("test", true)]
        [TestCase("tester", false)]
        [TestCase("1", true)]
        [TestCase("2", false)]
        [TestCase("te", true)]
        [Category("Extension Tests")]
        public void CompareNameTest(string value, bool expected)
        {
            var cell = GetFakeCell();
            Assert.That(cell.CompareName(value), Is.EqualTo(expected));
        }

        [Test]
        [Category("Extension Tests")]
        public void CompareNameNameNullEmptyTest()
        {
            var value = new FakeCell(1, "");

            Assert.Throws<ArgumentNullException>(() => value.CompareName(string.Empty),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }
    }
}
