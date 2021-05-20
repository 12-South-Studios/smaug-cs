using FluentAssertions;
using Realm.Library.Common.Extensions;
using Xunit;

namespace Realm.Library.Common.Test.Extensions
{
    public class CharExtensionsTests
    {
        [Theory]
        [InlineData('a', true)]
        [InlineData('e', true)]
        [InlineData('i', true)]
        [InlineData('o', true)]
        [InlineData('u', true)]
        [InlineData('g', false)]
        public void IsVowelTest(char letter, bool expected)
        {
            letter.IsVowel().Should().Be(expected);
        }

        [Theory]
        [InlineData('a', false)]
        [InlineData('1', true)]
        public void IsDigitTest(char value, bool expected)
        {
            value.IsDigit().Should().Be(expected);
        }

        [Theory]
        [InlineData('\0', true)]
        [InlineData('a', false)]
        public void IsSpaceTest(char value, bool expected)
        {
            value.IsSpace().Should().Be(expected);
        }
    }
}
