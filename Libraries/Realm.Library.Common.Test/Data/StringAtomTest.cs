using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class StringAtomTest
    {
        [Test]
        [Category("Object Tests")]
        public void StringAtomConstructorTest()
        {
            const string value = "Test";

            var atom = new StringAtom(value);

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.String));
            Assert.That(atom.Value, Is.EqualTo(value));
        }

        [Test]
        [Category("Object Tests")]
        public void StringAtomDumpNullParameterTest()
        {
            const string value = "test";
            var atom = new StringAtom(value);

            const string prefix = "Test";
            Assert.Throws<ArgumentNullException>(() => atom.Dump(null, prefix),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Object Tests")]
        public void StringAtomDumpTest()
        {
            var callback = false;

            var mockLog = new Mock<ILogWrapper>();
            mockLog.Setup(s => s.InfoFormat(It.IsAny<string>(), It.IsAny<object>(),
                                            It.IsAny<object>())).Callback(() => { callback = true; });

            const string value = "Test";
            var atom = new StringAtom(value);

            const string prefix = "Test";
            atom.Dump(mockLog.Object, prefix);

            Assert.That(callback, Is.True);
        }

        [TestCase("Test", "Tester", false)]
        [TestCase("Test", "Test", true)]
        [Category("Object Tests")]
        public void StringAtomEqualsTest(string firstValue, string secondValue, bool expected)
        {
            var atom = new StringAtom(firstValue);
            var compareAtom = new StringAtom(secondValue);

            Assert.That(atom.Equals(compareAtom), Is.EqualTo(expected));
        }

        [Test]
        [Category("Object Tests")]
        public void StringAtomEqualsNullParameterTest()
        {
            const string value = "test";
            var atom = new StringAtom(value);

            Assert.That(atom.Equals(null), Is.False);
        }

        [Test]
        [Category("Object Tests")]
        public void StringAtomGetHashCodeTest()
        {
            const string value = "test";
            var atom = new StringAtom(value);

            Assert.That(atom.GetHashCode(), Is.EqualTo(-354185609));
        }
    }
}
