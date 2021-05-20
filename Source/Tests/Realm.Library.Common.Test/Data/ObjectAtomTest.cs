using System;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class ObjectAtomTest
    {
        [Fact]
        public void ObjectAtomDumpNullParameterTest()
        {
            const int value = 5;
            var atom = new ObjectAtom(value);

            const string prefix = "Test";

            Action act = () => atom.Dump(null, prefix);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ObjectAtomDumpTest()
        {
            var callback = false;

            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.InfoFormat(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
                .Invokes(() => callback = true);

            const int value = 5;
            var atom = new ObjectAtom(value);

            const string prefix = "Test";

            atom.Dump(logger, prefix);

            callback.Should().BeTrue();
        }
    }
}
