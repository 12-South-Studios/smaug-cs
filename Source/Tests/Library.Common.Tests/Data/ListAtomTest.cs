using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Data;

public class ListAtomTest
{
  [Fact]
  public void ListAtomConstructorTest()
  {
    ListAtom atom = new();

    atom.Should().NotBeNull();
    atom.Type.Should().Be(AtomType.List);
  }

  [Fact]
  public void ListAtomCountTest()
  {
    ListAtom atom = new() { 15, 25 };

    atom.Count.Should().Be(2);
  }

  [Fact]
  public void ListAtomClearTest()
  {
    ListAtom atom = new() { 15, 25 };

    atom.Count.Should().Be(2);

    atom.Clear();

    atom.Count.Should().Be(0);
  }

  [Fact]
  public void ListAtomDumpNullParameterTest()
  {
    ListAtom atom = new();

    const string prefix = "Test";

    Action act = () => atom.Dump(null, prefix);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ListAtomDumpTest()
  {
    bool callback = false;

    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Info(A<string>.Ignored, A<object>.Ignored))
      .Invokes(() => callback = true);

    ListAtom atom = new();

    const string prefix = "Test";
    atom.Dump(logger, prefix);

    callback.Should().BeTrue();
  }

  [Fact]
  public void ListAtomGetEnumeratorTest()
  {
    ListAtom listAtom = new() { 15, 25, 35 };

    IEnumerator<Atom> enumerator = listAtom.GetEnumerator();
    enumerator.Should().NotBeNull();

    int runningTotal = 0;
    while (enumerator.MoveNext())
    {
      IntAtom valueAtom = (IntAtom)enumerator.Current;
      runningTotal += valueAtom.Value;
    }

    runningTotal.Should().Be(75);
  }

  [Fact]
  public void ListAtomGetAtomTest()
  {
    IntAtom atom1 = new(15);
    IntAtom atom2 = new(25);
    IntAtom atom3 = new(35);
    ListAtom listAtom = new() { atom1, atom2, atom3 };

    listAtom.Count.Should().Be(3);

    Atom actual = listAtom.Get(1);
    actual.Should().NotBeNull();
    actual.Should().Be(atom2);
  }

  [Fact]
  public void ListAtomGetStringTest()
  {
    ListAtom listAtom = new() { "test1", "test2", "test3" };

    listAtom.Count.Should().Be(3);
    listAtom.GetString(1).Should().Be("test2");
  }

  [Fact]
  public void ListAtomGetStringInvalidTest()
  {
    ListAtom listAtom = new() { "test1", 25, "test3" };

    listAtom.Count.Should().Be(3);
    string.IsNullOrEmpty(listAtom.GetString(1)).Should().BeTrue();
  }

  [Fact]
  public void ListAtomGetIntTest()
  {
    ListAtom listAtom = new() { 15, 25, 35 };

    listAtom.Count.Should().Be(3);
    listAtom.GetInt(1).Should().Be(25);
  }

  [Fact]
  public void ListAtomSetLongTest()
  {
    const long value = 2500;
    ListAtom listAtom = new() { value };

    listAtom.GetInt(0).Should().Be((int)value);
  }

  [Fact]
  public void ListAtomGetIntInvalidTest()
  {
    ListAtom listAtom = new() { 15, "test", 35 };

    listAtom.GetInt(1).Should().Be(0);
  }

  [Fact]
  public void ListAtomGetObjectTest()
  {
    object obj1 = new();
    object obj2 = new();
    object obj3 = new();
    ListAtom listAtom = new() { obj1, obj2, obj3 };

    listAtom.GetObject(1).Should().Be(obj2);
  }

  [Fact]
  public void ListAtomGetObjectInvalidTest()
  {
    object obj1 = new();
    object obj2 = new();
    ListAtom listAtom = new() { obj1, "test", obj2 };

    listAtom.GetObject(1).Should().BeNull();
  }

  [Fact]
  public void ListAtomGetBoolTest()
  {
    ListAtom listAtom = new() { false, true, false };

    listAtom.GetBool(1).Should().BeTrue();
  }

  [Fact]
  public void ListAtomGetBoolInvalidTest()
  {
    ListAtom listAtom = new() { false, "test", false };

    listAtom.GetBool(1).Should().BeFalse();
  }

  [Fact]
  public void ListAtomGetRealTest()
  {
    ListAtom listAtom = new() { 12.5f, 25.0f, 37.5f };

    listAtom.GetReal(1).Should().Be(25.0f);
  }

  [Fact]
  public void ListAtomSetDoubleTest()
  {
    const double value = 250.52D;
    ListAtom listAtom = new() { value };

    listAtom.GetReal(0).Should().Be(value);
  }

  [Fact]
  public void ListAtomGetRealInvalidTest()
  {
    ListAtom listAtom = new() { 12.5f, "test", 37.5f };

    listAtom.GetReal(1).Should().Be(0.0f);
  }

  [Fact]
  public void ListAtomGetDictionaryTest()
  {
    DictionaryAtom atom1 = new();
    atom1.Set("Test1", "Tester tester 1 2 3");

    DictionaryAtom atom2 = new();
    atom2.Set("Test2", "This is a big test");

    ListAtom listAtom = new() { atom1, atom2 };

    listAtom.GetDictionary(1).Should().Be(atom2);
  }

  [Fact]
  public void ListAtomGetDictionaryInvalidTest()
  {
    DictionaryAtom atom1 = new();
    atom1.Set("Test1", "Tester tester 1 2 3");

    ListAtom listAtom = new() { atom1, "test" };

    listAtom.GetDictionary(1).Should().BeNull();
  }

  [Fact]
  public void ListAtomGetListTest()
  {
    ListAtom atom1 = new() { "Tester tester 1 2 3" };
    ListAtom atom2 = new() { "This is a big test" };
    ListAtom listAtom = new() { atom1, atom2 };

    listAtom.GetList(1).Should().BeSameAs(atom2);
  }

  [Fact]
  public void ListAtomGetListInvalidTest()
  {
    ListAtom atom1 = new() { "Tester tester 1 2 3" };
    ListAtom listAtom = new() { atom1, "test" };

    listAtom.GetList(1).Should().BeNull();
  }
}