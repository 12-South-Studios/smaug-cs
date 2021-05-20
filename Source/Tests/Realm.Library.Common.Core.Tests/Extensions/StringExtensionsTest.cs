using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Realm.Library.Common.Extensions;
using Xunit;

namespace Realm.Library.Common.Test.Extensions
{
    public class StringExtensionsTest
    {
        #region ToByteArray
        [Fact]
        public void ToByteArrayValidTest()
        {
            const string value = "test";

            var actual = value.ToByteArray();

            var expected = new byte[] { 116, 101, 115, 116 };

            expected.SequenceEqual(actual).Should().BeTrue();
        }
        #endregion

        #region Contains
        [Theory]
        [InlineData("testing 1 2 3", "tester", false)]
        [InlineData("testing 1 2 3", "test", true)]
        [InlineData("testing 1 2 3", "TEST", true)]
        public void ContainsTest(string source, string target, bool expected)
        {
            var result = source.Contains(target, StringComparison.OrdinalIgnoreCase);
            result.Should().Be(expected);
        }

        [Fact]
        public void ContainsNullValueTest()
        {
            const string target = "TEST";

            Action act = () => StringExtensions.Contains(string.Empty, target, StringComparison.OrdinalIgnoreCase);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ContainsNullToCheckTest()
        {
            const string source = "TEST";

            Action act = () => source.Contains(string.Empty, StringComparison.OrdinalIgnoreCase);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region ReplaceFirst
        [Fact]
        public void ReplaceFirstTest()
        {
            const string str = "This is a test";
            const string search = "is";
            const string replace = "at";
            const string expected = "That is a test";

            str.ReplaceFirst(search, replace).Should().Be(expected);
        }

        [Fact]
        public void ReplaceFirstNullValueTest()
        {
            const string target = "TEST";
            const string replace = "testing";

            Action act = () => StringExtensions.ReplaceFirst(string.Empty, target, replace);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ReplaceFirstNullSearchTest()
        {
            const string source = "TEST";
            const string replace = "testing";

            Action act = () => source.ReplaceFirst(string.Empty, replace);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ReplaceFirstNullReplaceTest()
        {
            const string source = "TEST";
            const string target = "testing 1 2 3";

            Action act = () => source.ReplaceFirst(target, string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region CapitalizeFirst
        [Fact]
        public void CapitalizeFirstTest()
        {
            const string str = "test";
            const string expected = "Test";

            str.CapitalizeFirst().Should().Be(expected);
        }

        [Fact]
        public void CapitalizeFirstNullValueTest()
        {
            Action act = () => StringExtensions.CapitalizeFirst(string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region ParseWord
        [Fact]
        public void ParseWordTest()
        {
            const string str = "This is a test";
            const int wordNumber = 4;
            const string delimeter = " ";
            const string expected = "test";

            str.ParseWord(wordNumber, delimeter).Should().Be(expected);
        }

        [Fact]
        public void ParseWordNullValueTest()
        {
            const int wordNbr = 1;
            const string delimiter = "a";

            Action act = () => StringExtensions.ParseWord(string.Empty, wordNbr, delimiter);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ParseWordNullDelimiterTest()
        {
            const string value = "Test";
            const int wordNbr = 1;

            Action act = () => value.ParseWord(wordNbr, string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region PadString
        [Fact]
        public void PadStringToFrontTest()
        {
            const string str = "Test";
            const string padChar = "*";
            const int totalLength = 10;
            const string expected = "******Test";

            str.PadStringToFront(padChar, totalLength).Should().Be(expected);
        }

        [Fact]
        public void PadStringToBackTest()
        {
            const string str = "Test";
            const string padChar = "*";
            const int totalLength = 10;
            const string expected = "Test******";

            str.PadString(padChar, totalLength).Should().Be(expected);
        }

        [Fact]
        public void PadStringInvalidLengthTest()
        {
            const string str = "Test";
            const string padChar = "*";
            const int totalLength = 1;

            str.PadString(padChar, totalLength).Should().Be(str);
        }

        [Fact]
        public void PadStringNullValueTest()
        {
            const string padChar = "*";
            const int totalLength = 10;

            Action act = () => StringExtensions.PadString(string.Empty, padChar, totalLength);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region RemoveWord
        [Theory]
        [InlineData("This is a test", 2, "This a test")]
        [InlineData("Testing", 1, "")]
        public void RemoveWordTest(string inputString, int wordNumber, string expectedValue)
        {
            var result = inputString.RemoveWord(wordNumber);
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void RemoveWordNullValueTest()
        {
            const int wordNumber = 2;

            Action act = () => StringExtensions.RemoveWord(string.Empty, wordNumber);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Trim
        [Fact]
        public void TrimTest()
        {
            const string str = "    Test    ";
            const string delimeter = " ";
            const string expected = "Test";

            str.Trim(delimeter).Should().Be(expected);
        }

        [Fact]
        public void TrimNullValueTest()
        {
            const string delimiter = "a";

            Action act = () => StringExtensions.Trim(string.Empty, delimiter);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TrimNullDelimiterTest()
        {
            const string value = "test";

            Action act = () => value.Trim(string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Split
        [Fact]
        public void SplitNullValueTest()
        {
            var delims = new[] { 'a', 'b', 'c' };

            Action act = () => StringExtensions.Split(string.Empty, delims);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SplitTest()
        {
            var delims = new[] { ',', ':', ';' };
            const string value = "this,is;a:test";

            var expected = new List<string> { "this", "is", "a", "test" };
            var actual = value.Split(delims);

            expected.SequenceEqual(actual).Should().BeTrue();
        }
        #endregion

        #region ParseQuantity
        [Fact]
        public void ParseQuantityNullValueTest()
        {
            Action act = () => StringExtensions.ParseQuantity(string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("test", 0)]
        [InlineData("a#test", 0)]
        [InlineData("5#test", 5)]
        public void ParseQuantityTest(string value, int expected)
        {
            value.ParseQuantity().Should().Be(expected);
        }
        #endregion

        #region ReplaceAll
        [Fact]
        public void ReplaceAllNullValueTest()
        {
            var chars = new[] { 'a', 'b', 'c' };
            const char replace = 'g';

            Action act = () => StringExtensions.ReplaceAll(string.Empty, chars, replace);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ReplaceAllValidTest()
        {
            const string value = "A big bear clapped and clawed the ant hill.";
            var chars = new[] { 'a', 'b', 'c' };
            const char replace = 'g';

            const string expected = "A gig gegr glgpped gnd glgwed the gnt hill.";

            value.ReplaceAll(chars, replace).Should().Be(expected);
        }
        #endregion

        #region RemoveAll

        [Theory]
        [InlineData("A big bear clapped and clawed the ant hill.", "A ig er lpped nd lwed the nt hill.")]
        public void RemoveAllTest(string value, string expected)
        {
            var result = value.RemoveAll(new List<char> { 'a', 'b', 'c' });
            result.Should().Be(expected);
        }

        #endregion

        #region AddArticle
        [Theory]
        [InlineData("the big sword", "the big sword", false, false, false)]
        [InlineData("ancient sword", "an ancient sword", false, false, false)]
        [InlineData("big sword", "a big sword", false, false, false)]
        [InlineData("first test", "\r\na first test", true, false, false)]
        [InlineData("big sword", "the big sword", false, true, false)]
        [InlineData("big sword", "A big sword", false, false, true)]
        public void AddArticleTest(string value, string expected, bool appendNewLine, bool appendThe, bool capitalizeFirst)
        {
            var options = ArticleAppendOptions.None;
            if (appendNewLine)
                options |= ArticleAppendOptions.NewLineToEnd;
            if (appendThe)
                options |= ArticleAppendOptions.TheToFront;
            if (capitalizeFirst)
                options |= ArticleAppendOptions.CapitalizeFirstLetter;

            var result = value.AddArticle(options);
            result.Should().Be(expected);
        }

        [Fact]
        public void AddArticle_ThrowsException_WhenNoStringIsProvided()
        {
            var options = ArticleAppendOptions.None | ArticleAppendOptions.NewLineToEnd;

            Action act = () => "".AddArticle(options);
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #region CaseCompare
        [Theory]
        [InlineData("testing", "testing", CaseCompareResult.Equal)]
        [InlineData("testing", "TESTING", CaseCompareResult.Equal)]
        [InlineData("test", "testing", CaseCompareResult.LessThan)]
        [InlineData("testing", "Test", CaseCompareResult.GreaterThan)]
        public void CaseCompareTest(string value, string compare, CaseCompareResult expected)
        {
            var result = value.CaseCompare(compare);
            result.Should().Be(expected);
        }
        #endregion

        [Fact]
        public void FirstWordTest()
        {
            const string str = "This is a test";
            str.FirstWord().Should().Be("This");
        }

        [Fact]
        public void SecondWordTest()
        {
            const string str = "This is a test";
            str.SecondWord().Should().Be("is");
        }

        [Fact]
        public void ThirdWordTest()
        {
            const string str = "This is a test";
            str.ThirdWord().Should().Be("a");
        }

        [Theory]
        [InlineData("25", true)]
        [InlineData("abc", false)]
        [InlineData("1b", false)]
        public void IsNumber(string value, bool expected)
        {
            value.IsNumber().Should().Be(expected);
        }

        [Theory]
        [InlineData("test", "a test")]
        [InlineData("answer", "an answer")]
        public void AOrAn(string value, string expected)
        {
            value.AOrAn().Should().Be(expected);
        }

        [Theory]
        [InlineData("abcdef", true)]
        [InlineData("123456", true)]
        [InlineData("abc123", true)]
        [InlineData("abc#123", false)]
        [InlineData("#$&", false)]
        public void IsAlphaNum(string value, bool expected)
        {
            value.IsAlphaNum().Should().Be(expected);
        }

        [Theory]
        [InlineData("TESTING", "testing", true)]
        [InlineData("TeStInG", "TESTING", true)]
        [InlineData("1234test", "1234TEST", true)]
        [InlineData("TESTING", "BOB", false)]
        public void EqualsIgnoreCase(string value, string compareTo, bool expected)
        {
            value.EqualsIgnoreCase(compareTo).Should().Be(expected);
        }

        [Theory]
        [InlineData("TESTING", "test", true)]
        [InlineData("1234testing", "1234", true)]
        [InlineData("TESTING", "BOB", false)]
        public void StartsWithIgnoreCase(string value, string startsWith, bool expected)
        {
            value.StartsWithIgnoreCase(startsWith).Should().Be(expected);
        }

        [Theory]
        [InlineData("TESTING", "test", true)]
        [InlineData("Testing", "Bob", false)]
        [InlineData("TESTING", "TEST", true)]
        public void ContainsIgnoreCase(string value, string contains, bool expected)
        {
            value.ContainsIgnoreCase(contains).Should().Be(expected);
        }

        [Theory]
        [InlineData("123", 123)]
        [InlineData("abc", 0)]
        public void ToInt32(string value, int expected)
        {
            value.ToInt32().Should().Be(expected);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("true", true)]
        [InlineData("0", false)]
        [InlineData("false", false)]
        [InlineData("whatever", false)]
        public void ToBoolean(string value, bool expected)
        {
            value.ToBoolean().Should().Be(expected);
        }

        [Theory]
        [InlineData("Bob", "Jane Bob Joe Mary", true)]
        [InlineData("Bob", "Jane Joe Mary", false)]
        public void IsEqualTest(string value, string wordList, bool expected)
        {
            value.IsEqual(wordList).Should().Be(expected);
        }

        [Theory]
        [InlineData("Bo", "Jane Bob Joe Mary", true)]
        [InlineData("ane", "Jane Bob Joe Mary", false)]
        public void IsEqualPrefix(string value, string wordList, bool expected)
        {
            value.IsEqualPrefix(wordList).Should().Be(expected);
        }

        [Theory]
        [InlineData("Who Bob", "Jane Bob Joe Mary", true)]
        [InlineData("Who Is", "Jane Bob Joe Mary", false)]
        public void IsAnyEqual(string value, string wordList, bool expected)
        {
            value.IsAnyEqual(wordList).Should().Be(expected);
        }

        [Theory]
        [InlineData("Who Bo", "Jane Bob Joe Mary", true)]
        [InlineData("Who ob", "Jane Bob Joe Mary", false)]
        public void IsAnyEqualPrefix(string value, string wordList, bool expected)
        {
            value.IsAnyEqualPrefix(wordList).Should().Be(expected);
        }

        [Theory]
        [InlineData('a', 10, "aaaaaaaaaa")]
        [InlineData('b', 1, "b")]
        public void RepeatCharacter(char c, int times, string expected)
        {
            c.Repeat(times).Should().Be(expected);
        }

        [Theory]
        [InlineData("test", 1, "test")]
        [InlineData("is", 5, "isisisisis")]
        public void RepeatSring(string str, int times, string expected)
        {
            str.Repeat(times).Should().Be(expected);
        }

        [Theory]
        [InlineData("This is a test", false)]
        [InlineData("", true)]
        [InlineData(null, true)]
        public void IsNullOrEmptyTest(string value, bool expected)
        {
            value.IsNullOrEmpty().Should().Be(expected);
        }

        [Theory]
        [InlineData("This is a test", false)]
        [InlineData("", true)]
        [InlineData("     ", true)]
        [InlineData(null, true)]
        public void IsNullOrWhitespaceTest(string value, bool expected)
        {
            value.IsNullOrWhitespace().Should().Be(expected);
        }

        [Theory]
        [InlineData("Testing", "Testing", false)]
        [InlineData("Testing", "Test", true)]
        public void NotEqualsTest(string value, string compareTo, bool expected)
        {
            value.NotEquals(compareTo).Should().Be(expected);
        }

        [Theory]
        [InlineData("Test", 'x', 2, "Text")]
        public void ThisTest(string value, char charToSet, int index, string expected)
        {
            value.SetChar(index, charToSet).Should().Be(expected);
        }

        [Fact]
        public void ToWordsTest()
        {
            const string value = "This is a test";

            var actual = value.ToWords();

            actual.Should().NotBeNull();
            actual.Count.Should().Be(4);
            actual[0].Should().Be("This");
            actual[3].Should().Be("test");
        }
    }
}
