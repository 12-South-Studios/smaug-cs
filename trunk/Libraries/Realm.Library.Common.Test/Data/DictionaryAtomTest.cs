using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Realm.Library.Common.Data;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class DictionaryAtomTest
    {
        [Test]
        public void ConstructorTest()
        {
            var atom = new DictionaryAtom();

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.Dictionary));
        }

        [Test]
        public void CopyConstructorNullParameterTest()
        {
            Assert.Throws<ArgumentNullException>(() => new DictionaryAtom(null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void CopyConstructorTest()
        {
            const string key = "test";
            const int value = 25;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            var newAtom = new DictionaryAtom(atom);

            Assert.That(newAtom.Count, Is.EqualTo(1));
            Assert.That(newAtom.GetInt(key), Is.EqualTo(value));
        }

        [Test]
        public void IsEmptyTest()
        {
            var atom = new DictionaryAtom();

            Assert.That(atom.IsEmpty(), Is.True);
        }

        [Test]
        public void CountTest()
        {
            const string key = "test";
            const int value = 25;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            Assert.That(atom.Count, Is.EqualTo(1));
        }

        [Test]
        public void ContainsKeyTest()
        {
            const string key = "test";
            const int value = 5000;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            Assert.That(atom.ContainsKey(key), Is.True);
        }

        [Test]
        public void GetValueTest()
        {
            const string key = "test";
            const int value = 5000;

            var atom = new DictionaryAtom();
            atom.Set(key, value);

            var actual = atom.GetAtom<IntAtom>("test");

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [TestCase("BoolKey", true)]
        [TestCase("IntKey", 25)]
        [TestCase("DoubleKey", 25.05D)]
        [TestCase("StringKey", "Testing 1 2 3")]
        [TestCase("LongKey", 9223372036854775806)]
        [TestCase("FloatKey", 25.05f)]
        public void SetTest<T>(string key, T value)
        {
            var atom = new DictionaryAtom();
            atom.Set(key, value);

            Assert.That(atom.ContainsKey(key), Is.True);
        }

        [Test]
        public void SetDictionaryAtom()
        {
            var setAtom = new DictionaryAtom();
            setAtom.Set("Test", "1 2 3 4 5");

            var atom = new DictionaryAtom();
            atom.Set("TestDictionary", setAtom);

            Assert.That(atom.ContainsKey("TestDictionary"), Is.True);

            var foundAtom = atom.GetAtom<DictionaryAtom>("TestDictionary");
            
            Assert.That(foundAtom, Is.Not.Null);
            Assert.That(foundAtom, Is.InstanceOf<DictionaryAtom>());
            Assert.That(foundAtom.ContainsKey("Test"), Is.True);
        }

        [Test]
        public void SetListAtom()
        {
            var listAtom = new ListAtom {"1 2 3 4 5"};

            var atom = new DictionaryAtom();
            atom.Set("TestList", listAtom);

            Assert.That(atom.ContainsKey("TestList"), Is.True);

            var foundAtom = atom.GetAtom<ListAtom>("TestList");

            Assert.That(foundAtom, Is.Not.Null);
            Assert.That(foundAtom, Is.InstanceOf<ListAtom>());
            Assert.That(foundAtom.GetString(0), Is.EqualTo("1 2 3 4 5"));
        }
    }
}
