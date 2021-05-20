using System;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class RealAtomTest
    {
        [Theory]
        [InlineData(5.5D)]
        [InlineData(5.5f)]
        public void RealAtomConstructorTest(double value)
        {
            var atom = new RealAtom(value);

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.Real);
            atom.Value.Should().Be(value);
        }

        [Fact]
        public void RealAtomDumpNullParameterTest()
        {
            const double value = 5.5D;
            var atom = new RealAtom(value);

            const string prefix = "Test";

            Action act = () => atom.Dump(null, prefix);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RealAtomDumpTest()
        {
            var callback = false;

            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.InfoFormat(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
                .Invokes(() => callback = true);

            const double value = 5.5D;
            var atom = new RealAtom(value);

            const string prefix = "Test";
            atom.Dump(logger, prefix);

            callback.Should().BeTrue();
        }
    }
}
