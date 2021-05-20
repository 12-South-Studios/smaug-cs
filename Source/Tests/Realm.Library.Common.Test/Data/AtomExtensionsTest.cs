using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Realm.Library.Common.Data;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    [ExcludeFromCodeCoverage]
    public class AtomExtensionsTest
    {
        [Fact]
        public void AtomExtensionToBoolAtomTest()
        {
            const bool value = true;

            var actual = value.ToAtom<BoolAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToIntAtomTest()
        {
            const int value = 5;

            var actual = value.ToAtom<IntAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToRealAtomInt64Test()
        {
            const long value = 50;

            var actual = value.ToAtom<RealAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToRealAtomDoubleTest()
        {
            const double value = 5.5D;

            var actual = value.ToAtom<RealAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToRealAtomSingleTest()
        {
            const float value = 5.5f;

            var actual = value.ToAtom<RealAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToAtomNullStringTest()
        {
            Action act = () => AtomExtensions.ToAtom<IntAtom>(string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("blah", false)]
        public void AtomExtensionToAtomStringBoolAtomTest(string value, bool expected)
        {
            var actual = value.ToAtom<BoolAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData("5", 5)]
        [InlineData("ten", -1)]
        public void AtomExtensionToAtomStringIntAtomTest(string value, int expected)
        {
            var actual = value.ToAtom<IntAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData("25.5", 25.5f)]
        [InlineData("twenty", 0.0f)]
        public void AtomExtensionToAtomStringRealAtomTest(string value, double expected)
        {
            var actual = value.ToAtom<RealAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(expected);
        }

        [Fact]
        public void AtomExtensionToAtomStringStringAtomTest()
        {
            const string value = "test";

            var actual = value.ToAtom<StringAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToAtomStringObjectAtomTest()
        {
            const string value = "test";

            var actual = value.ToAtom<ObjectAtom>();

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Fact]
        public void AtomExtensionToAtomStringInvalidTypeTest()
        {
            const string value = "test";

            var actual = value.ToAtom<ListAtom>();
     
            actual.Should().BeNull();
        }

        [Fact]
        public void ToAtomList_ThrowsException_WhenNullIsPassed()
        {
            Assert.Throws<ArgumentNullException>(() => AtomExtensions.ToAtom(null));
        }

        /*[Fact]
        public void AtomExtension_ToAtom_List_Test()
        {
            var list = new List<object> {25, "Testing", true, 1.52f};

            var actual = list.ToAtom();

            Assert.IsNotNull(actual);
            Assert.AreEqual(4, actual.Count);
            Assert.IsTrue(actual.GetType() == typeof(ListAtom));

            var atom = actual.Get(0);
            Assert.IsTrue(atom.GetType() == typeof(IntAtom));
            Assert.AreEqual(((IntAtom)atom).Value, 25);

            atom = actual.Get(1);
            Assert.IsTrue(atom.GetType() == typeof(StringAtom));
            Assert.AreEqual(((StringAtom)atom).Value, "Testing");


            atom = actual.Get(2);
            Assert.IsTrue(atom.GetType() == typeof(BoolAtom));
            Assert.AreEqual(((BoolAtom)atom).Value, true);

            atom = actual.Get(3);
            Assert.IsTrue(atom.GetType() == typeof(RealAtom));
            Assert.AreEqual(((RealAtom)atom).Value, 1.52f);
        }*/
    }
}
