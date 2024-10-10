using System;
using FluentAssertions;
using Library.Common.Data;
using Xunit;

namespace Library.Common.Tests.Data;

public class DictionaryAtomTest
{
  [Fact]
  public void ConstructorTest()
  {
    DictionaryAtom atom = new();

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

    DictionaryAtom atom = new();
    atom.Set(key, value);

    DictionaryAtom newAtom = new(atom);

    newAtom.Count.Should().Be(1);
    newAtom.GetInt(key).Should().Be(value);
  }

  [Fact]
  public void IsEmptyTest()
  {
    DictionaryAtom atom = new();

    atom.IsEmpty().Should().BeTrue();
  }

  [Fact]
  public void CountTest()
  {
    const string key = "test";
    const int value = 25;

    DictionaryAtom atom = new();
    atom.Set(key, value);

    atom.Count.Should().Be(1);
  }

  [Fact]
  public void ContainsKeyTest()
  {
    const string key = "test";
    const int value = 5000;

    DictionaryAtom atom = new();
    atom.Set(key, value);

    atom.ContainsKey(key).Should().BeTrue();
  }

  [Fact]
  public void GetValueTest()
  {
    const string key = "test";
    const int value = 5000;

    DictionaryAtom atom = new();
    atom.Set(key, value);

    IntAtom actual = atom.GetAtom<IntAtom>("test");

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
    DictionaryAtom atom = new();
    atom.Set(key, value);

    atom.ContainsKey(key).Should().BeTrue();
  }

  [Fact]
  public void SetDictionaryAtom()
  {
    DictionaryAtom setAtom = new();
    setAtom.Set("Test", "1 2 3 4 5");

    DictionaryAtom atom = new();
    atom.Set("TestDictionary", setAtom);

    atom.ContainsKey("TestDictionary").Should().BeTrue();

    DictionaryAtom foundAtom = atom.GetAtom<DictionaryAtom>("TestDictionary");

    foundAtom.Should().NotBeNull();
    foundAtom.Should().BeAssignableTo<DictionaryAtom>();
    foundAtom.ContainsKey("Test").Should().BeTrue();
  }

  [Fact]
  public void SetListAtom()
  {
    ListAtom listAtom = new() { "1 2 3 4 5" };

    DictionaryAtom atom = new();
    atom.Set("TestList", listAtom);

    atom.ContainsKey("TestList").Should().BeTrue();

    ListAtom foundAtom = atom.GetAtom<ListAtom>("TestList");

    foundAtom.Should().NotBeNull();
    foundAtom.Should().BeAssignableTo<ListAtom>();
    foundAtom.GetString(0).Should().Be("1 2 3 4 5");
  }
}