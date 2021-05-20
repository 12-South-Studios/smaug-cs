using System;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class IntAtomTest
    {
        [Fact]
        public void IntAtomIntegerTest()
        {
            const int value = 5;

            var atom = new IntAtom(value);

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.Integer);
            atom.Value.Should().Be(value);
        }

        [Fact]
        public void IntAtomLongTest()
        {
            const long value = 50001;

            var atom = new IntAtom(value);

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.Integer);
            atom.Value.Should().Be((int)value);
        }

        [Fact]
        public void IntAtomDumpNullParameterTest()
        {
            const int value = 5;
            var atom = new IntAtom(value);

            const string prefix = "Test";

            Action act = () => atom.Dump(null, prefix);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void IntAtomDumpTest()
        {
            var callback = false;

            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.InfoFormat(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
                .Invokes(() => callback = true);

            const int value = 5;
            var atom = new IntAtom(value);

            const string prefix = "Test";

            atom.Dump(logger, prefix);

            callback.Should().BeTrue();
        }

        [Theory]
        [InlineData(5, 10, false)]
        [InlineData(5, 5, true)]
        [InlineData(5, null, false)]
        public void IntAtomEqualsTest(int firstValue, int secondValue, bool expected)
        {
            var atom = new IntAtom(firstValue);
            var compareAtom = new IntAtom(secondValue);

            atom.Equals(compareAtom).Should().Be(expected);

        }

        [Fact]
        public void IntAtomGetHashCodeTest()
        {
            const int value = 5;
            var atom = new IntAtom(value);

            atom.GetHashCode().Should().Be(value);
        }
    }
}
