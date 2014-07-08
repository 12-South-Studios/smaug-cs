using System;
using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class FuncExtensionsTests
    {
        [Test]
        public void TryCatch_NoReturnValue_NoException_Test()
        {
            var callback = false;

            Action<object[]> func = x => callback = true;

            func.TryCatch();

            Assert.That(callback, Is.True); 
        }

        [Test]
        public void TryCatch_NoReturnValue_WithException_Test()
        {
            var callback = false;

            Action<object[]> func = x => { throw new Exception("Test Exception"); };

            Action<Exception> showError = exception => callback = true;

            func.TryCatch(showError);

            Assert.That(callback, Is.True);
        }

        [Test]
        public void TryCatch_NoException_NoFinally_Test()
        {
            var callback = false;

            Func<object[], bool> func = x =>
                {
                    callback = true;
                    return true;
                };
            Func<Exception, bool> showError = exception => false;

            var result = func.TryCatch(showError);

            Assert.That(result, Is.True);
            Assert.That(callback, Is.True);
        }

        [Test]
        public void TryCatch_WithException_Test()
        {
            var callback = false;

            Func<object[], bool> func = x => { throw new Exception("Test Exception"); };

            Func<Exception, bool> showError = exception =>
                {
                    callback = true;
                    return false;
                };

            var result = func.TryCatch(showError);

            Assert.That(result, Is.False);
            Assert.That(callback, Is.True);
        }

        [Test]
        public void TryCatch_DifferentType_Test()
        {
            var callback = false;
            var finalCallback = false;

            Func<int[], int> func = ints =>
                {
                    callback = true;
                    return ints.Sum();
                };
            Func<Exception, int> showError = exception => 0;
            Action<int[]> final = ints => { finalCallback = true; };

            var result = func.TryCatch(showError, final, 2, 5, 8);

            Assert.That(result, Is.EqualTo(15));
            Assert.That(callback, Is.True);
            Assert.That(finalCallback, Is.True);
        }
    }
}
