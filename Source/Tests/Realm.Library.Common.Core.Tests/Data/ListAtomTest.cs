using System;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class ListAtomTest
    {
        [Fact]
        public void ListAtomConstructorTest()
        {
            var atom = new ListAtom();

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.List);
        }

        [Fact]
        public void ListAtomCountTest()
        {
            var atom = new ListAtom { 15, 25 };

            atom.Count.Should().Be(2);
        }

        [Fact]
        public void ListAtomClearTest()
        {
            var atom = new ListAtom { 15, 25 };

            atom.Count.Should().Be(2);

            atom.Clear();

            atom.Count.Should().Be(0);
        }

        [Fact]
        public void ListAtomDumpNullParameterTest()
        {
            var atom = new ListAtom();

            const string prefix = "Test";

            Action act = () => atom.Dump(null, prefix);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ListAtomDumpTest()
        {
            var callback = false;

            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.InfoFormat(A<string>.Ignored, A<object>.Ignored))
                .Invokes(() => callback = true);

            var atom = new ListAtom();

            const string prefix = "Test";
            atom.Dump(logger, prefix);

            callback.Should().BeTrue();
        }

        [Fact]
        public void ListAtomGetEnumeratorTest()
        {
            var listAtom = new ListAtom { 15, 25, 35 };

            var enumerator = listAtom.GetEnumerator();
            enumerator.Should().NotBeNull();

            var runningTotal = 0;
            while (enumerator.MoveNext())
            {
                var valueAtom = (IntAtom)enumerator.Current;
                runningTotal += valueAtom.Value;
            }

            runningTotal.Should().Be(75);
        }

        [Fact]
        public void ListAtomGetAtomTest()
        {
            var atom1 = new IntAtom(15);
            var atom2 = new IntAtom(25);
            var atom3 = new IntAtom(35);
            var listAtom = new ListAtom { atom1, atom2, atom3 };

            listAtom.Count.Should().Be(3);

            var actual = listAtom.Get(1);
            actual.Should().NotBeNull();
            actual.Should().Be(atom2);
        }

        [Fact]
        public void ListAtomGetStringTest()
        {
            var listAtom = new ListAtom { "test1", "test2", "test3" };

            listAtom.Count.Should().Be(3);
            listAtom.GetString(1).Should().Be("test2");
        }

        [Fact]
        public void ListAtomGetStringInvalidTest()
        {
            var listAtom = new ListAtom { "test1", 25, "test3" };

            listAtom.Count.Should().Be(3);
            string.IsNullOrEmpty(listAtom.GetString(1)).Should().BeTrue();
        }

        [Fact]
        public void ListAtomGetIntTest()
        {
            var listAtom = new ListAtom { 15, 25, 35 };

            listAtom.Count.Should().Be(3);
            listAtom.GetInt(1).Should().Be(25);
        }

        [Fact]
        public void ListAtomSetLongTest()
        {
            const long value = 2500;
            var listAtom = new ListAtom { value };

            listAtom.GetInt(0).Should().Be((int)value);
        }

        [Fact]
        public void ListAtomGetIntInvalidTest()
        {
            var listAtom = new ListAtom { 15, "test", 35 };

            listAtom.GetInt(1).Should().Be(0);
        }

        [Fact]
        public void ListAtomGetObjectTest()
        {
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = new object();
            var listAtom = new ListAtom { obj1, obj2, obj3 };

            listAtom.GetObject(1).Should().Be(obj2);
        }

        [Fact]
        public void ListAtomGetObjectInvalidTest()
        {
            var obj1 = new object();
            var obj2 = new object();
            var listAtom = new ListAtom { obj1, "test", obj2 };

            listAtom.GetObject(1).Should().BeNull();
        }

        [Fact]
        public void ListAtomGetBoolTest()
        {
            var listAtom = new ListAtom { false, true, false };

            listAtom.GetBool(1).Should().BeTrue();
        }

        [Fact]
        public void ListAtomGetBoolInvalidTest()
        {
            var listAtom = new ListAtom { false, "test", false };

            listAtom.GetBool(1).Should().BeFalse();
        }

        [Fact]
        public void ListAtomGetRealTest()
        {
            var listAtom = new ListAtom { 12.5f, 25.0f, 37.5f };

            listAtom.GetReal(1).Should().Be(25.0f);
        }

        [Fact]
        public void ListAtomSetDoubleTest()
        {
            const double value = 250.52D;
            var listAtom = new ListAtom { value };

            listAtom.GetReal(0).Should().Be(value);
        }

        [Fact]
        public void ListAtomGetRealInvalidTest()
        {
            var listAtom = new ListAtom { 12.5f, "test", 37.5f };

            listAtom.GetReal(1).Should().Be(0.0f);
        }

        [Fact]
        public void ListAtomGetDictionaryTest()
        {
            var atom1 = new DictionaryAtom();
            atom1.Set("Test1", "Tester tester 1 2 3");

            var atom2 = new DictionaryAtom();
            atom2.Set("Test2", "This is a big test");

            var listAtom = new ListAtom { atom1, atom2 };

            listAtom.GetDictionary(1).Should().Be(atom2);
        }

        [Fact]
        public void ListAtomGetDictionaryInvalidTest()
        {
            var atom1 = new DictionaryAtom();
            atom1.Set("Test1", "Tester tester 1 2 3");

            var listAtom = new ListAtom { atom1, "test" };

            listAtom.GetDictionary(1).Should().BeNull();
        }

        [Fact]
        public void ListAtomGetListTest()
        {
            var atom1 = new ListAtom { "Tester tester 1 2 3" };
            var atom2 = new ListAtom { "This is a big test" };
            var listAtom = new ListAtom { atom1, atom2 };

            listAtom.GetList(1).Should().BeSameAs(atom2);
        }

        [Fact]
        public void ListAtomGetListInvalidTest()
        {
            var atom1 = new ListAtom { "Tester tester 1 2 3" };
            var listAtom = new ListAtom { atom1, "test" };

            listAtom.GetList(1).Should().BeNull();
        }
    }
}
