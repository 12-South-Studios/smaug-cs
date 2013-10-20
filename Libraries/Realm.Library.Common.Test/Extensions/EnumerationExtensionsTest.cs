using System;
using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class EnumerationExtensionsTest
    {
        public enum EnumTest
        {
            [Enum("Test", 1, "Testing", "Extra,Data")]
            Test,

            Test1 = 1024,
            Test2 = 256,

            [Enum("All", 1280)]
            All
        }

        private enum NameTest
        {
            [Name("Test")]
            Test
        }

        [Test]
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
        public void HasBitTest(EnumTest value, int bit, bool expected)
        {
            Assert.That(value.HasBit(bit), Is.EqualTo(expected));
        }

        [Test]
        public void GetExtraDataTest()
        {
            Assert.That(EnumTest.Test.GetExtraData(), Is.EqualTo("Extra,Data"));
        }

        [Test]
        public void ParseExtraDataTest()
        {
            var list = EnumTest.Test.ParseExtraData(",").ToList();

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo("Extra"));
            Assert.That(list[1], Is.EqualTo("Data"));
        }

        [Test]
        public void GetEnumIntTest()
        {
            Assert.That(EnumerationExtensions.GetEnum<EnumTest>(1024), Is.EqualTo(EnumTest.Test1));
        }

        [Test]
        public void GetEnumIntInvalidTest()
        {
            Assert.Throws<ArgumentException>(() => EnumerationExtensions.GetEnum<EnumTest>((int)111),
                                             "Unit test expected an ArgumentException to be thrown");
        }

        [Test]
        public void GetEnumStringTest()
        {
            Assert.That(EnumerationExtensions.GetEnum<EnumTest>("Test1"), Is.EqualTo(EnumTest.Test1));
        }

        [Test]
        public void GetEnumStringInvalidTest()
        {
            Assert.Throws<ArgumentException>(() => EnumerationExtensions.GetEnum<EnumTest>("Blah"),
                                             "Unit test expected an ArgumentException to be thrown");
        }

        [TestCase(1024, true)]
        [TestCase(111, false)]
        public void HasBitsTest(int bit, bool expected)
        {
            Assert.That(EnumTest.All.HasBit(bit), Is.EqualTo(expected));
        }
    }
}
