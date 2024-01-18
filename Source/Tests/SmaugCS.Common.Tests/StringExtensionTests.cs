using FluentAssertions;
using System;
using Xunit;

namespace SmaugCS.Common.Tests
{

    public class StringExtensionTests
    {
        [Theory]
        [InlineData("Tests", 1, "Tests")]
        [InlineData("2.Tests", 2, "Tests")]
        [InlineData("Tests.3", 1, "Tests.3")]
        [InlineData("25", 1, "25")]
        public void NumberArgument(string value, int expectedNumber, string expectedOut)
        {
            Tuple<int, string> returnVal = value.NumberArgument();

            returnVal.Should().NotBeNull();
            returnVal.Item1.Should().Be(expectedNumber);
            returnVal.Item2.Should().Be(expectedOut);
        }

        [Theory]
        [InlineData("This is a test", "This", "is a test")]
        [InlineData("Testing", "Testing", "")]
        public void FirstArgument(string value, string expectedArg, string expectedRemainder)
        {
            Tuple<string, string> returnVal = value.FirstArgument();

            returnVal.Should().NotBeNull();
            returnVal.Item1.Should().Be(expectedArg);
            returnVal.Item2.Should().Be(expectedRemainder);
        }

        [Theory]
        [InlineData("This is a test", "This", "is a test")]
        [InlineData("\"This is\" a test", "\"This is\"", " a test")]
        public void FirstArgumentWithQuotes(string value, string expectedArg, string expectedRemainder)
        {
            Tuple<string, string> returnVal = value.FirstArgumentWithQuotes();

            returnVal.Should().NotBeNull();
            returnVal.Item1.Should().Be(expectedArg);
            returnVal.Item2.Should().Be(expectedRemainder);
        }

        [Theory]
        [InlineData("This is a test~", "This is a test")]
        [InlineData("This is a test", "This is a test")]
        public void TrimHash(string value, string expectedValue)
        {
            value.TrimHash().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("This is~a test", "This is-a test", null)]
        [InlineData("This is~a test", "This is#a test", "#")]
        public void SmashTilde(string value, string expected, string defaultChar)
        {
            var result = defaultChar == null ? value.SmashTilde() : value.SmashTilde(defaultChar);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("This is~a test", "This is*a test", null)]
        [InlineData("This is~a test", "This is#a test", "#")]
        public void HideTilde(string value, string expected, string hiddenChar)
        {
            var result = hiddenChar == null ? value.HideTilde() : value.HideTilde(hiddenChar);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("This is*a test", "This is~a test", null)]
        [InlineData("This is#a test", "This is~a test", "#")]
        public void UnhideTilde(string value, string expected, string hiddenChar)
        {
            var result = hiddenChar == null ? value.UnhideTilde() : value.UnhideTilde(hiddenChar);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("THIS IS A TEST", true)]
        [InlineData("This is a test", false)]
        public void IsAllUpper(string value, bool expected)
        {
            value.IsAllUpper().Should().Be(expected);
        }
    }
}
