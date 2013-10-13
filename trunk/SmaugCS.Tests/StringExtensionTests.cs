using System;
using NUnit.Framework;
using SmaugCS.Common;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class StringExtensionTests
    {
        [TestCase("25", true)]
        [TestCase("abc", false)]
        [TestCase("1b", false)]
        public void IsNumber(string value, bool expected)
        {
            Assert.That(value.IsNumber(), Is.EqualTo(expected));
        }

        [TestCase("test", "a test")]
        [TestCase("answer", "an answer")]
        public void AOrAn(string value, string expected)
        {
            Assert.That(value.AOrAn(), Is.EqualTo(expected));
        }

        [TestCase("abcdef", true)]
        [TestCase("123456", true)]
        [TestCase("abc123", true)]
        [TestCase("abc#123", false)]
        [TestCase("#$&", false)]
        public void IsAlphaNum(string value, bool expected)
        {
            Assert.That(value.IsAlphaNum(), Is.EqualTo(expected));
        }

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

        [TestCase("TESTING", "testing", true)]
        [TestCase("TeStInG", "TESTING", true)]
        [TestCase("1234test", "1234TEST", true)]
        [TestCase("TESTING", "BOB", false)]
        public void EqualsIgnoreCase(string value, string compareTo, bool expected)
        {
            Assert.That(value.EqualsIgnoreCase(compareTo), Is.EqualTo(expected));
        }

        [TestCase("TESTING", "test", true)]
        [TestCase("1234testing", "1234", true)]
        [TestCase("TESTING", "BOB", false)]
        public void StartsWithIgnoreCase(string value, string startsWith, bool expected)
        {
            Assert.That(value.StartsWithIgnoreCase(startsWith), Is.EqualTo(expected));
        }

        [TestCase("TESTING", "test", true)]
        [TestCase("Testing", "Bob", false)]
        [TestCase("TESTING", "TEST", true)]
        public void ContainsIgnoreCase(string value, string contains, bool expected)
        {
            Assert.That(value.ContainsIgnoreCase(contains), Is.EqualTo(expected));
        }

        [TestCase("123", 123)]
        [TestCase("abc", 0)]
        public void ToInt32(string value, int expected)
        {
            Assert.That(value.ToInt32(), Is.EqualTo(expected));
        }

        [TestCase("1", true)]
        [TestCase("true", true)]
        [TestCase("0", false)]
        [TestCase("false", false)]
        [TestCase("whatever", false)]
        public void ToBoolean(string value, bool expected)
        {
            Assert.That(value.ToBoolean(), Is.EqualTo(expected));
        }

        [TestCase("Bob", "Jane Bob Joe Mary", true)]
        [TestCase("Bob", "Jane Joe Mary", false)]
        public void IsEqualTest(string value, string wordList, bool expectedValue)
        {
            Assert.That(value.IsEqual(wordList), Is.EqualTo(expectedValue));
        }

        [TestCase("Bo", "Jane Bob Joe Mary", true)]
        [TestCase("ane", "Jane Bob Joe Mary", false)]
        public void IsEqualPrefix(string value, string wordList, bool expectedValue)
        {
            Assert.That(value.IsEqualPrefix(wordList), Is.EqualTo(expectedValue));
        }

        [TestCase("Who Bob", "Jane Bob Joe Mary", true)]
        [TestCase("Who Is", "Jane Bob Joe Mary", false)]
        public void IsAnyEqual(string value, string wordList, bool expectedValue)
        {
            Assert.That(value.IsAnyEqual(wordList), Is.EqualTo(expectedValue));
        }

        [TestCase("Who Bo", "Jane Bob Joe Mary", true)]
        [TestCase("Who ob", "Jane Bob Joe Mary", false)]
        public void IsAnyEqualPrefix(string value, string wordList, bool expectedValue)
        {
            Assert.That(value.IsAnyEqualPrefix(wordList), Is.EqualTo(expectedValue));
        }

        [TestCase("This is a test~", "This is a test")]
        [TestCase("This is a test", "This is a test")]
        public void TrimHash(string value, string expectedValue)
        {
            Assert.That(value.TrimHash(), Is.EqualTo(expectedValue));
        }

        [TestCase('a', 10, "aaaaaaaaaa")]
        [TestCase('b', 1, "b")]
        public void RepeatCharacter(char c, int times, string expected)
        {
            Assert.That(c.Repeat(times), Is.EqualTo(expected));
        }

        [TestCase("test", 1, "test")]
        [TestCase("is", 5, "isisisisis")]
        public void RepeatSring(string str, int times, string expected)
        {
            Assert.That(str.Repeat(times), Is.EqualTo(expected));
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
