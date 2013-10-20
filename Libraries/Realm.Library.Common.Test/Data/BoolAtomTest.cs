using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class BoolAtomTest
    {
        [Test]
        public void ConstructorTest()
        {
            const bool value = true;

            var atom = new BoolAtom(value);

            Assert.That(atom, Is.Not.Null);
            Assert.That(atom.Type, Is.EqualTo(AtomType.Boolean));
            Assert.That(atom.Value, Is.EqualTo(value));
        }

        [Test]
        public void BoolAtomDumpNullParameterTest()
        {
            const bool value = true;
            var atom = new BoolAtom(value);

            const string prefix = "Test";

            Assert.Throws<ArgumentNullException>(() => atom.Dump(null, prefix),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void BoolAtomDumpTest()
        {
            var callback = false;

            var mockLog = new Mock<ILogWrapper>();
            mockLog.Setup(s => s.InfoFormat(It.IsAny<string>(), It.IsAny<object>(),
                                            It.IsAny<object>())).Callback(() => { callback = true; });

            const bool value = true;
            var atom = new BoolAtom(value);

            const string prefix = "Test";
            atom.Dump(mockLog.Object, prefix);

            Assert.That(callback, Is.True.After(250));
        }
    }
}
