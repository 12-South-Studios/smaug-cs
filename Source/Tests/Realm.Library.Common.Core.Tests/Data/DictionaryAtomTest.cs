using System;
using FluentAssertions;
using Realm.Library.Common.Data;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class DictionaryAtomTest
    {
        [Fact]
        public void ConstructorTest()
        {
            var atom = new DictionaryAtom();

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.Dictionary);
        }

        [Fact]
        public void CopyConstructorNullParameterTest()
        {
            Action act = () => new DictionaryAtom(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CopyConstructorTest()
        {
            const string key = "test";
            const int value = 25;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            var newAtom = new DictionaryAtom(atom);

            newAtom.Count.Should().Be(1);
            newAtom.GetInt(key).Should().Be(value);
        }

        [Fact]
        public void IsEmptyTest()
        {
            var atom = new DictionaryAtom();

            atom.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public void CountTest()
        {
            const string key = "test";
            const int value = 25;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            atom.Count.Should().Be(1);
        }

        [Fact]
        public void ContainsKeyTest()
        {
            const string key = "test";
            const int value = 5000;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            atom.ContainsKey(key).Should().BeTrue();
        }

        [Fact]
        public void GetValueTest()
        {
            const string key = "test";
            const int value = 5000;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            var actual = atom.GetAtom<IntAtom>("test");

            actual.Should().NotBeNull();
            actual.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("BoolKey", true)]
        [InlineData("IntKey", 25)]
        [InlineData("DoubleKey", 25.05D)]
        [InlineData("StringKey", "Testing 1 2 3")]
        [InlineData("LongKey", 9223372036854775806)]
        [InlineData("FloatKey", 25.05f)]
        public void SetTest<T>(string key, T value)
        {
            var atom = new DictionaryAtom();
            atom.Set(key, value);

            atom.ContainsKey(key).Should().BeTrue();
        }

        [Fact]
        public void SetDictionaryAtom()
        {
            var setAtom = new DictionaryAtom();
            setAtom.Set("Test", "1 2 3 4 5");

            var atom = new DictionaryAtom();
            atom.Set("TestDictionary", setAtom);

            atom.ContainsKey("TestDictionary").Should().BeTrue();

            var foundAtom = atom.GetAtom<DictionaryAtom>("TestDictionary");

            foundAtom.Should().NotBeNull();
            foundAtom.Should().BeAssignableTo<DictionaryAtom>();
            foundAtom.ContainsKey("Test").Should().BeTrue();
        }

        [Fact]
        public void SetListAtom()
        {
            var listAtom = new ListAtom {"1 2 3 4 5"};

            var atom = new DictionaryAtom();
            atom.Set("TestList", listAtom);

            atom.ContainsKey("TestList").Should().BeTrue();

            var foundAtom = atom.GetAtom<ListAtom>("TestList");

            foundAtom.Should().NotBeNull();
            foundAtom.Should().BeAssignableTo<ListAtom>();
            foundAtom.GetString(0).Should().Be("1 2 3 4 5");
        }
    }
}
