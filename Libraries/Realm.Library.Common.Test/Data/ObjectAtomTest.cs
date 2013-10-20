using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Data
{
    [TestFixture]
    public class ObjectAtomTest
    {
        [Test]
        public void ObjectAtomDumpNullParameterTest()
        {
            const int value = 5;
            var atom = new ObjectAtom(value);

            const string prefix = "Test";

            Assert.Throws<ArgumentNullException>(() => atom.Dump(null, prefix),
                                                 "Unit Test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ObjectAtomDumpTest()
        {
            var callback = false;

            var mockLog = new Mock<ILogWrapper>();
            mockLog.Setup(s => s.InfoFormat(It.IsAny<string>(), It.IsAny<object>(),
                                            It.IsAny<object>())).Callback(() => { callback = true; });

            const int value = 5;
            var atom = new ObjectAtom(value);

            const string prefix = "Test";

            atom.Dump(mockLog.Object, prefix);

            Assert.That(callback, Is.True);
        }
    }
}
