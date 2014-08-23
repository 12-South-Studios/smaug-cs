using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class EnumerationExtensionsTest
    {
        public enum EnumTest
        {
            [Enum("Test", Value = 1, ShortName = "Testing", ExtraData = "Extra,Data")]
            Test,

            Test1 = 1024,

            [Enum("Test Two", Value = 5)]
            Test2 = 256,

            [Enum("All", Value = 1280)]
            All
        }

        private enum NameTest
        {
            [Name("Test")]
            Test
        }

        private enum RangeTest
        {
            [Range(Minimum = 5, Maximum = 10)]
            Test1,

            [Range(Maximum = 5)]
            Test2,

            [Range(Minimum = 11, Maximum = 20)]
            Test3
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetValueInRangeMatchTest()
        {
            var value = EnumerationExtensions.GetValueInRange(18, RangeTest.Test2);
            Assert.That(value, Is.EqualTo(RangeTest.Test3));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetValueInRangeNoMatchTest()
        {
            var value = EnumerationExtensions.GetValueInRange(-2, RangeTest.Test2);
            Assert.That(value, Is.EqualTo(RangeTest.Test2)); 
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetMinimumRangeTest()
        {
            var value = RangeTest.Test1.GetMinimum();
            Assert.That(value, Is.EqualTo(5));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetMaximumRangeTest()
        {
            var value = RangeTest.Test1.GetMaximum();
            Assert.That(value, Is.EqualTo(10));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetMinimumRangeWhenNotSpecifiedTest()
        {
            var value = RangeTest.Test2.GetMinimum();
            Assert.That(value, Is.EqualTo(Int32.MinValue));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumIgnoreCaseTest()
        {
            var value = EnumerationExtensions.GetEnumIgnoreCase<EnumTest>("test");

            Assert.That(value, Is.EqualTo(EnumTest.Test));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumIgnoreCaseInvalidTest()
        {
            Assert.Throws<InvalidEnumArgumentException>(
                () => EnumerationExtensions.GetEnumIgnoreCase<EnumTest>("tester"),
                "Unit test expected an InvalidEnumArgumentException to be thrown");
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumTest()
        {
            Assert.That(EnumerationExtensions.GetEnum<EnumTest>("Test"), Is.EqualTo(EnumTest.Test));
            Assert.That(EnumTest.Test.GetName(), Is.EqualTo("Test"));
            Assert.That(EnumTest.Test.GetValue(), Is.EqualTo(1));
            Assert.That(EnumTest.Test.GetShortName(), Is.EqualTo("Testing"));
            Assert.That(NameTest.Test.GetName(), Is.EqualTo("Test"));
        }

        [TestCase(EnumTest.Test, 2, false)]
        [TestCase(EnumTest.Test, 3, true)]
        [NUnit.Framework.Category("Extension Tests")]
        public void HasBitTest(EnumTest value, int bit, bool expected)
        {
            Assert.That(value.HasBit(bit), Is.EqualTo(expected));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetExtraDataTest()
        {
            Assert.That(EnumTest.Test.GetExtraData(), Is.EqualTo("Extra,Data"));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void ParseExtraDataTest()
        {
            var list = EnumTest.Test.ParseExtraData(",").ToList();

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo("Extra"));
            Assert.That(list[1], Is.EqualTo("Data"));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumIntTest()
        {
            Assert.That(EnumerationExtensions.GetEnum<EnumTest>(1024), Is.EqualTo(EnumTest.Test1));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumIntInvalidTest()
        {
            Assert.Throws<ArgumentException>(() => EnumerationExtensions.GetEnum<EnumTest>(111),
                                             "Unit test expected an ArgumentException to be thrown");
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumStringTest()
        {
            Assert.That(EnumerationExtensions.GetEnum<EnumTest>("Test1"), Is.EqualTo(EnumTest.Test1));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumStringInvalidTest()
        {
            Assert.Throws<ArgumentException>(() => EnumerationExtensions.GetEnum<EnumTest>("Blah"),
                                             "Unit test expected an ArgumentException to be thrown");
        }

        [TestCase(1024, true)]
        [TestCase(111, false)]
        [NUnit.Framework.Category("Extension Tests")]
        public void HasBitsTest(int bit, bool expected)
        {
            Assert.That(EnumTest.All.HasBit(bit), Is.EqualTo(expected));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetValuesTest()
        {
            var expectedList = new List<EnumTest> { EnumTest.All, EnumTest.Test, EnumTest.Test1, EnumTest.Test2 };

            var values = EnumerationExtensions.GetValues<EnumTest>().ToList();

            Assert.That(values.Count(), Is.EqualTo(4));
            CollectionAssert.AreEquivalent(values, expectedList);
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumByNameTest()
        {
            var value = EnumerationExtensions.GetEnumByName<EnumTest>("Test Two");

            Assert.That(value, Is.EqualTo(EnumTest.Test2));
        }

        [Test]
        [NUnit.Framework.Category("Extension Tests")]
        public void GetEnumByNameInvalidTest()
        {
            Assert.Throws<InvalidEnumArgumentException>(
                () => EnumerationExtensions.GetEnumByName<EnumTest>("Invalid Name"),
                "Unit test expected an InvalidEnumArgumentException to be thrown");
        }
    }
}
