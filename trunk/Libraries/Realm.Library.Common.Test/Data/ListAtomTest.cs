using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class ListAtomTest
    {
        [Test]
        public void ListAtomConstructorTest()
        {
            var atom = new ListAtom();

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.List));
        }

        [Test]
        public void ListAtomCountTest()
        {
            var atom = new ListAtom { 15, 25 };

            Assert.That(atom.Count, Is.EqualTo(2));
        }

        [Test]
        public void ListAtomClearTest()
        {
            var atom = new ListAtom { 15, 25 };

            Assert.That(atom.Count, Is.EqualTo(2));

            atom.Clear();

            Assert.That(atom.Count, Is.EqualTo(0));
        }

        [Test]
        public void ListAtomDumpNullParameterTest()
        {
            var atom = new ListAtom();

            const string prefix = "Test";
            Assert.Throws<ArgumentNullException>(() => atom.Dump(null, prefix),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ListAtomDumpTest()
        {
            var callback = false;

            var mockLog = new Mock<ILogWrapper>();
            mockLog.Setup(s => s.InfoFormat(It.IsAny<string>(), It.IsAny<object>()))
                   .Callback(() => { callback = true; });

            var atom = new ListAtom();

            const string prefix = "Test";
            atom.Dump(mockLog.Object, prefix);

            Assert.That(callback, Is.True.After(250));
        }

        [Test]
        public void ListAtomGetEnumeratorTest()
        {
            var listAtom = new ListAtom { 15, 25, 35 };

            var enumerator = listAtom.GetEnumerator();
            Assert.That(enumerator, Is.Not.Null);

            var runningTotal = 0;
            while (enumerator.MoveNext())
            {
                var valueAtom = (IntAtom)enumerator.Current;
                runningTotal += valueAtom.Value;
            }

            Assert.That(runningTotal, Is.EqualTo(75));
        }

        [Test]
        public void ListAtomGetAtomTest()
        {
            var atom1 = new IntAtom(15);
            var atom2 = new IntAtom(25);
            var atom3 = new IntAtom(35);
            var listAtom = new ListAtom { atom1, atom2, atom3 };

            Assert.That(listAtom.Count, Is.EqualTo(3));

            var actual = listAtom.Get(1);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(atom2));
        }

        [Test]
        public void ListAtomGetStringTest()
        {
            var listAtom = new ListAtom { "test1", "test2", "test3" };

            Assert.That(listAtom.Count, Is.EqualTo(3));
            Assert.That(listAtom.GetString(1), Is.EqualTo("test2"));
        }

        [Test]
        public void ListAtomGetStringInvalidTest()
        {
            var listAtom = new ListAtom { "test1", 25, "test3" };

            Assert.That(listAtom.Count, Is.EqualTo(3));
            Assert.That(string.IsNullOrEmpty(listAtom.GetString(1)), Is.True);
        }

        [Test]
        public void ListAtomGetIntTest()
        {
            var listAtom = new ListAtom { 15, 25, 35 };

            Assert.That(listAtom.Count, Is.EqualTo(3));
            Assert.That(listAtom.GetInt(1), Is.EqualTo(25));
        }

        [Test]
        public void ListAtomSetLongTest()
        {
            const long value = 2500;
            var listAtom = new ListAtom { value };

            Assert.That(listAtom.GetInt(0), Is.EqualTo(value));
        }

        [Test]
        public void ListAtomGetIntInvalidTest()
        {
            var listAtom = new ListAtom { 15, "test", 35 };

            Assert.That(listAtom.GetInt(1), Is.EqualTo(0));
        }

        [Test]
        public void ListAtomGetObjectTest()
        {
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = new object();
            var listAtom = new ListAtom { obj1, obj2, obj3 };

            Assert.That(listAtom.GetObject(1), Is.EqualTo(obj2));
        }

        [Test]
        public void ListAtomGetObjectInvalidTest()
        {
            var obj1 = new object();
            var obj2 = new object();
            var listAtom = new ListAtom { obj1, "test", obj2 };

            Assert.That(listAtom.GetObject(1), Is.Null);
        }

        [Test]
        public void ListAtomGetBoolTest()
        {
            var listAtom = new ListAtom { false, true, false };

            Assert.That(listAtom.GetBool(1), Is.True);
        }

        [Test]
        public void ListAtomGetBoolInvalidTest()
        {
            var listAtom = new ListAtom { false, "test", false };

            Assert.That(listAtom.GetBool(1), Is.False);
        }

        [Test]
        public void ListAtomGetRealTest()
        {
            var listAtom = new ListAtom { 12.5f, 25.0f, 37.5f };

            Assert.That(listAtom.GetReal(1), Is.EqualTo(25.0f));
        }

        [Test]
        public void ListAtomSetDoubleTest()
        {
            const double value = 250.52D;
            var listAtom = new ListAtom { value };

            Assert.That(listAtom.GetReal(0), Is.EqualTo(value));
        }

        [Test]
        public void ListAtomGetRealInvalidTest()
        {
            var listAtom = new ListAtom { 12.5f, "test", 37.5f };

            Assert.That(listAtom.GetReal(1), Is.EqualTo(0.0f));
        }

        [Test]
        public void ListAtomGetDictionaryTest()
        {
            var atom1 = new DictionaryAtom();
            atom1.Set("Test1", "Tester tester 1 2 3");

            var atom2 = new DictionaryAtom();
            atom2.Set("Test2", "This is a big test");

            var listAtom = new ListAtom { atom1, atom2 };

            Assert.That(listAtom.GetDictionary(1), Is.EqualTo(atom2));
        }

        [Test]
        public void ListAtomGetDictionaryInvalidTest()
        {
            var atom1 = new DictionaryAtom();
            atom1.Set("Test1", "Tester tester 1 2 3");

            var listAtom = new ListAtom { atom1, "test" };

            Assert.That(listAtom.GetDictionary(1), Is.Null);
        }

        [Test]
        public void ListAtomGetListTest()
        {
            var atom1 = new ListAtom { "Tester tester 1 2 3" };
            var atom2 = new ListAtom { "This is a big test" };
            var listAtom = new ListAtom { atom1, atom2 };

            Assert.That(listAtom.GetList(1), Is.EqualTo(atom2));
        }

        [Test]
        public void ListAtomGetListInvalidTest()
        {
            var atom1 = new ListAtom { "Tester tester 1 2 3" };
            var listAtom = new ListAtom { atom1, "test" };

            Assert.That(listAtom.GetList(1), Is.Null);
        }
    }
}
