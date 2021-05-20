using System;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class BoolAtomTest
    {
        [Fact]
        public void ConstructorTest()
        {
            const bool value = true;

            var atom = new BoolAtom(value);

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.Boolean);
            atom.Value.Should().Be(value);
        }

        [Fact]
        public void BoolAtomDumpNullParameterTest()
        {
            const bool value = true;
            var atom = new BoolAtom(value);

            const string prefix = "Test";

            Action act = () => atom.Dump(null, prefix);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void BoolAtomDumpTest()
        {
            var callback = false;

            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.InfoFormat(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
                .Invokes(() => callback = true);

            const bool value = true;
            var atom = new BoolAtom(value);

            const string prefix = "Test";
            atom.Dump(logger, prefix);

            callback.Should().BeTrue();
        }
    }
}
