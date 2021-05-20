using System;
using FakeItEasy;
using FluentAssertions;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using Xunit;

namespace Realm.Library.Common.Test.Data
{
    public class StringAtomTest
    {
        [Fact]
        public void StringAtomConstructorTest()
        {
            const string value = "Test";

            var atom = new StringAtom(value);

            atom.Should().NotBeNull();
            atom.Type.Should().Be(AtomType.String);
            atom.Value.Should().Be(value);
        }

        [Fact]
        public void StringAtomDumpNullParameterTest()
        {
            const string value = "test";
            var atom = new StringAtom(value);

            const string prefix = "Test";

            Action act = () => atom.Dump(null, prefix);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StringAtomDumpTest()
        {
            var callback = false;

            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.InfoFormat(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
                .Invokes(() => callback = true);

            const string value = "Test";
            var atom = new StringAtom(value);

            const string prefix = "Test";
            atom.Dump(logger, prefix);

            callback.Should().BeTrue();
        }

        [Theory]
        [InlineData("Test", "Tester", false)]
        [InlineData("Test", "Test", true)]
        public void StringAtomEqualsTest(string firstValue, string secondValue, bool expected)
        {
            var atom = new StringAtom(firstValue);
            var compareAtom = new StringAtom(secondValue);

            atom.Equals(compareAtom).Should().Be(expected);
        }

        [Fact]
        public void StringAtomEqualsNullParameterTest()
        {
            const string value = "test";
            var atom = new StringAtom(value);

            atom.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void StringAtomGetHashCodeTest()
        {
            const string value = "test";
            var atom = new StringAtom(value);

            atom.GetHashCode().Should().Be(-354185609);
        }
    }
}
