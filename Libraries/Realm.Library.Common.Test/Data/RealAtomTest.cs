using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class RealAtomTest
    {
        [TestCase(5.5D)]
        [TestCase(5.5f)]
        public void RealAtomConstructorTest(double value)
        {
            var atom = new RealAtom(value);

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.Real));
            Assert.That(atom.Value, Is.EqualTo(value));
        }

        [Test]
        public void RealAtomDumpNullParameterTest()
        {
            const double value = 5.5D;
            var atom = new RealAtom(value);

            const string prefix = "Test";

            Assert.Throws<ArgumentNullException>(() => atom.Dump(null, prefix),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void RealAtomDumpTest()
        {
            var callback = false;

            var mockLog = new Mock<ILogWrapper>();
            mockLog.Setup(s => s.InfoFormat(It.IsAny<string>(), It.IsAny<object>(),
                It.IsAny<object>())).Callback(() => { callback = true; });

            const double value = 5.5D;
            var atom = new RealAtom(value);

            const string prefix = "Test";
            atom.Dump(mockLog.Object, prefix);

            Assert.That(callback, Is.True);
        }
    }
}
