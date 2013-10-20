using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Realm.Library.Common.Data;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AtomExtensionsTest
    {
        [Test]
        public void AtomExtensionToBoolAtomTest()
        {
            const bool value = true;

            var actual = value.ToAtom<BoolAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToIntAtomTest()
        {
            const int value = 5;

            var actual = value.ToAtom<IntAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToRealAtomInt64Test()
        {
            const long value = 50;

            var actual = value.ToAtom<RealAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToRealAtomDoubleTest()
        {
            const double value = 5.5D;

            var actual = value.ToAtom<RealAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToRealAtomSingleTest()
        {
            const float value = 5.5f;

            var actual = value.ToAtom<RealAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToAtomNullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => AtomExtensions.ToAtom<IntAtom>(string.Empty),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [TestCase("true", true)]
        [TestCase("blah", false)]
        public void AtomExtensionToAtomStringBoolAtomTest(string value, bool expected)
        {
            var actual = value.ToAtom<BoolAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(expected));
        }

        [TestCase("5", 5)]
        [TestCase("ten", -1)]
        public void AtomExtensionToAtomStringIntAtomTest(string value, int expected)
        {
            var actual = value.ToAtom<IntAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(expected));
        }

        [TestCase("25.5", 25.5f)]
        [TestCase("twenty", 0.0f)]
        public void AtomExtensionToAtomStringRealAtomTest(string value, double expected)
        {
            var actual = value.ToAtom<RealAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(expected));
        }

        [Test]
        public void AtomExtensionToAtomStringStringAtomTest()
        {
            const string value = "test";

            var actual = value.ToAtom<StringAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToAtomStringObjectAtomTest()
        {
            const String value = "test";

            var actual = value.ToAtom<ObjectAtom>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(value));
        }

        [Test]
        public void AtomExtensionToAtomStringInvalidTypeTest()
        {
            const String value = "test";

            var actual = value.ToAtom<ListAtom>();

            Assert.IsNull(actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AtomExtensionToAtomListNullTest()
        {
            AtomExtensions.ToAtom(null);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        /*[Test]
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
