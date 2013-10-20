using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class IntAtomTest
    {
        [Test]
        public void IntAtomIntegerTest()
        {
            const int value = 5;

            var atom = new IntAtom(value);

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.Integer));
            Assert.That(atom.Value, Is.EqualTo(value));
        }

        [Test]
        public void IntAtomLongTest()
        {
            const long value = 50001;

            var atom = new IntAtom(value);

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.Integer));
            Assert.That(atom.Value, Is.EqualTo(value));
        }

        [Test]
        public void IntAtomDumpNullParameterTest()
        {
            const int value = 5;
            var atom = new IntAtom(value);

            const string prefix = "Test";

            Assert.Throws<ArgumentNullException>(() => atom.Dump(null, prefix),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void IntAtomDumpTest()
        {
            var callback = false;

            var mockLog = new Mock<ILogWrapper>();
            mockLog.Setup(s => s.InfoFormat(It.IsAny<string>(), It.IsAny<object>(),
                                            It.IsAny<object>())).Callback(() => { callback = true; });

            const int value = 5;
            var atom = new IntAtom(value);

            const string prefix = "Test";

            atom.Dump(mockLog.Object, prefix);

            Assert.That(callback, Is.True);
        }

        [TestCase(5, 10, false)]
        [TestCase(5, 5, true)]
        [TestCase(5, null, false)]
        public void IntAtomEqualsTest(int firstValue, int secondValue, bool expected)
        {
            var atom = new IntAtom(firstValue);
            var compareAtom = new IntAtom(secondValue);

            Assert.That(atom.Equals(compareAtom), Is.EqualTo(expected));

        }

        [Test]
        public void IntAtomGetHashCodeTest()
        {
            const int value = 5;
            var atom = new IntAtom(value);

            Assert.That(atom.GetHashCode(), Is.EqualTo(value));
        }
    }
}
