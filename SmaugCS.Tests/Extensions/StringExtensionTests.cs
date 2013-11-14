using System;
using NUnit.Framework;
using SmaugCS.Common;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionTests
    {
        [TestCase("Tests", 1, "Tests")]
        [TestCase("2.Tests", 2, "Tests")]
        [TestCase("Tests.3", 1, "Tests.3")]
        [TestCase("25", 1, "25")]
        public void NumberArgument(string value, int expectedNumber, string expectedOut)
        {
            Tuple<int, string> returnVal = value.NumberArgument();

            Assert.That(returnVal, Is.Not.Null);
            Assert.That(returnVal.Item1, Is.EqualTo(expectedNumber));
            Assert.That(returnVal.Item2, Is.EqualTo(expectedOut));
        }

        [TestCase("This is a test", "This", "is a test")]
        [TestCase("Testing", "Testing", "")]
        public void FirstArgument(string value, string expectedArg, string expectedRemainder)
        {
            Tuple<string, string> returnVal = value.FirstArgument();

            Assert.That(returnVal, Is.Not.Null);
            Assert.That(returnVal.Item1, Is.EqualTo(expectedArg));
            Assert.That(returnVal.Item2, Is.EqualTo(expectedRemainder));
        }

        [TestCase("This is a test", "This", "is a test")]
        [TestCase("\"This is\" a test", "\"This is\"", " a test")]
        public void FirstArgumentWithQuotes(string value, string expectedArg, string expectedRemainder)
        {
            Tuple<string, string> returnVal = value.FirstArgumentWithQuotes();

            Assert.That(returnVal, Is.Not.Null);
            Assert.That(returnVal.Item1, Is.EqualTo(expectedArg));
            Assert.That(returnVal.Item2, Is.EqualTo(expectedRemainder));
        }

        [TestCase("This is a test~", "This is a test")]
        [TestCase("This is a test", "This is a test")]
        public void TrimHash(string value, string expectedValue)
        {
            Assert.That(value.TrimHash(), Is.EqualTo(expectedValue));
        }

        [TestCase("This is~a test", "This is-a test", null)]
        [TestCase("This is~a test", "This is#a test", "#")]
        public void SmashTilde(string value, string expected, string defaultChar)
        {
            Assert.That(defaultChar == null ? value.SmashTilde() : value.SmashTilde(defaultChar), Is.EqualTo(expected));
        }

        [TestCase("This is~a test", "This is*a test", null)]
        [TestCase("This is~a test", "This is#a test", "#")]
        public void HideTilde(string value, string expected, string hiddenChar)
        {
            Assert.That(hiddenChar == null ? value.HideTilde() : value.HideTilde(hiddenChar), Is.EqualTo(expected));
        }

        [TestCase("This is*a test", "This is~a test", null)]
        [TestCase("This is#a test", "This is~a test", "#")]
        public void UnhideTilde(string value, string expected, string hiddenChar)
        {
            Assert.That(hiddenChar == null ? value.UnhideTilde() : value.UnhideTilde(hiddenChar), Is.EqualTo(expected));
        }
    }
}
