using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Realm.Library.Common.Extensions;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        #region ToByteArray
        [Test]
        public void ToByteArrayValidTest()
        {
            const string value = "test";

            var actual = value.ToByteArray();

            var expected = new byte[] { 116, 101, 115, 116 };

            Assert.That(expected.SequenceEqual(actual), Is.True);
        }
        #endregion

        #region Contains
        [TestCase("testing 1 2 3", "tester", false)]
        [TestCase("testing 1 2 3", "test", true)]
        [TestCase("testing 1 2 3", "TEST", true)]
        public void ContainsTest(string source, string target, bool expected)
        {
            Assert.That(source.Contains(target, StringComparison.OrdinalIgnoreCase), Is.EqualTo(expected));
        }

        [Test]
        public void ContainsNullValueTest()
        {
            const string target = "TEST";

            Assert.Throws<ArgumentNullException>(
                () => StringExtensions.Contains(string.Empty, target, StringComparison.OrdinalIgnoreCase),
                "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ContainsNullToCheckTest()
        {
            const string source = "TEST";

            Assert.Throws<ArgumentNullException>(
                () => source.Contains(string.Empty, StringComparison.OrdinalIgnoreCase),
                "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region ReplaceFirst
        [Test]
        public void ReplaceFirstTest()
        {
            const string str = "This is a test";
            const string search = "is";
            const string replace = "at";
            const string expected = "That is a test";

            Assert.That(str.ReplaceFirst(search, replace), Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceFirstNullValueTest()
        {
            const string target = "TEST";
            const string replace = "testing";

            Assert.Throws<ArgumentNullException>(() => StringExtensions.ReplaceFirst(string.Empty, target, replace),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ReplaceFirstNullSearchTest()
        {
            const string source = "TEST";
            const string replace = "testing";

            Assert.Throws<ArgumentNullException>(() => source.ReplaceFirst(string.Empty, replace),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ReplaceFirstNullReplaceTest()
        {
            const string source = "TEST";
            const string target = "testing 1 2 3";

            Assert.Throws<ArgumentNullException>(() => source.ReplaceFirst(target, string.Empty),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region CapitalizeFirst
        [Test]
        public void CapitalizeFirstTest()
        {
            const string str = "test";
            const string expected = "Test";

            Assert.That(str.CapitalizeFirst(), Is.EqualTo(expected));
        }

        [Test]
        public void CapitalizeFirstNullValueTest()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.CapitalizeFirst(string.Empty),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region ParseWord
        [Test]
        public void ParseWordTest()
        {
            const string str = "This is a test";
            const int wordNumber = 4;
            const string delimeter = " ";
            const string expected = "test";

            Assert.That(str.ParseWord(wordNumber, delimeter), Is.EqualTo(expected));
        }

        [Test]
        public void ParseWordNullValueTest()
        {
            const int wordNbr = 1;
            const string delimiter = "a";

            Assert.Throws<ArgumentNullException>(() => StringExtensions.ParseWord(string.Empty, wordNbr, delimiter),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ParseWordNullDelimiterTest()
        {
            const string value = "Test";
            const int wordNbr = 1;

            Assert.Throws<ArgumentNullException>(() => value.ParseWord(wordNbr, string.Empty),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region PadString
        [Test]
        public void PadStringToFrontTest()
        {
            const string str = "Test";
            const string padChar = "*";
            const int totalLength = 10;
            const string expected = "******Test";

            Assert.That(str.PadStringToFront(padChar, totalLength), Is.EqualTo(expected));
        }

        [Test]
        public void PadStringToBackTest()
        {
            const string str = "Test";
            const string padChar = "*";
            const int totalLength = 10;
            const string expected = "Test******";

            Assert.That(str.PadString(padChar, totalLength), Is.EqualTo(expected));
        }

        [Test]
        public void PadStringInvalidLengthTest()
        {
            const string str = "Test";
            const string padChar = "*";
            const int totalLength = 1;

            Assert.That(str.PadString(padChar, totalLength), Is.EqualTo(str));
        }

        [Test]
        public void PadStringNullValueTest()
        {
            const string padChar = "*";
            const int totalLength = 10;

            Assert.Throws<ArgumentNullException>(() => StringExtensions.PadString(string.Empty, padChar, totalLength),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region RemoveWord
        [TestCase("This is a test", 2, "This a test")]
        [TestCase("Testing", 1, "")]
        public void RemoveWordTest(string inputString, int wordNumber, string expectedValue)
        {
            Assert.That(inputString.RemoveWord(wordNumber), Is.EqualTo(expectedValue));
        }

        [Test]
        public void RemoveWordNullValueTest()
        {
            const int wordNumber = 2;

            Assert.Throws<ArgumentNullException>(() => StringExtensions.RemoveWord(string.Empty, wordNumber),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region Trim
        [Test]
        public void TrimTest()
        {
            const string str = "    Test    ";
            const string delimeter = " ";
            const string expected = "Test";

            Assert.That(str.Trim(delimeter), Is.EqualTo(expected));
        }

        [Test]
        public void TrimNullValueTest()
        {
            const string delimiter = "a";

            Assert.Throws<ArgumentNullException>(() => StringExtensions.Trim(string.Empty, delimiter),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void TrimNullDelimiterTest()
        {
            const string value = "test";

            Assert.Throws<ArgumentNullException>(() => value.Trim(string.Empty),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region Split
        [Test]
        public void SplitNullValueTest()
        {
            var delims = new[] { 'a', 'b', 'c' };

            Assert.Throws<ArgumentNullException>(() => StringExtensions.Split(string.Empty, delims),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void SplitTest()
        {
            var delims = new[] { ',', ':', ';' };
            const string value = "this,is;a:test";

            var expected = new List<string> { "this", "is", "a", "test" };
            var actual = value.Split(delims);

            Assert.That(expected.SequenceEqual(actual), Is.True);
        }
        #endregion

        #region ParseQuantity
        [Test]
        public void ParseQuantityNullValueTest()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ParseQuantity(string.Empty),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [TestCase("test", 0)]
        [TestCase("a#test", 0)]
        [TestCase("5#test", 5)]
        public void ParseQuantityTest(string value, int expected)
        {
            Assert.That(value.ParseQuantity(), Is.EqualTo(expected));
        }
        #endregion

        #region ReplaceAll
        [Test]
        public void ReplaceAllNullValueTest()
        {
            var chars = new[] { 'a', 'b', 'c' };
            const char replace = 'g';

            Assert.Throws<ArgumentNullException>(() => StringExtensions.ReplaceAll(string.Empty, chars, replace),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public void ReplaceAllValidTest()
        {
            const string value = "A big bear clapped and clawed the ant hill.";
            var chars = new[] { 'a', 'b', 'c' };
            const char replace = 'g';

            const string expected = "A gig gegr glgpped gnd glgwed the gnt hill.";

            Assert.That(value.ReplaceAll(chars, replace), Is.EqualTo(expected));
        }
        #endregion

        #region RemoveAll

        [TestCase("A big bear clapped and clawed the ant hill.", "A ig er lpped nd lwed the nt hill.")]
        public void RemoveAllTest(string value, string expected)
        {
            Assert.That(value.RemoveAll(new List<char> { 'a', 'b', 'c' }), Is.EqualTo(expected));
        }

        #endregion

        #region AddArticle

        [TestCase("", "", true, false, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("the big sword", "the big sword", false, false, false)]
        [TestCase("ancient sword", "an ancient sword", false, false, false)]
        [TestCase("big sword", "a big sword", false, false, false)]
        [TestCase("first test", "\r\na first test", true, false, false)]
        [TestCase("big sword", "the big sword", false, true, false)]
        [TestCase("big sword", "A big sword", false, false, true)]
        public void AddArticleTest(string value, string expected, bool appendNewLine, bool appendThe, bool capitalizeFirst)
        {
            var options = ArticleAppendOptions.None;
            if (appendNewLine)
                options |= ArticleAppendOptions.NewLineToEnd;
            if (appendThe)
                options |= ArticleAppendOptions.TheToFront;
            if (capitalizeFirst)
                options |= ArticleAppendOptions.CapitalizeFirstLetter;

            Assert.That(value.AddArticle(options), Is.EqualTo(expected));
        }

        #endregion

        #region CaseCompare

        [TestCase("testing", "testing", CaseCompareResult.Equal)]
        [TestCase("testing", "TESTING", CaseCompareResult.Equal)]
        [TestCase("test", "testing", CaseCompareResult.LessThan)]
        [TestCase("testing", "Test", CaseCompareResult.GreaterThan)]
        public void CaseCompareTest(string value, string compare, CaseCompareResult expected)
        {
            Assert.That(value.CaseCompare(compare), Is.EqualTo(expected));
        }
        #endregion

        [Test]
        public void FirstWordTest()
        {
            const string str = "This is a test";
            Assert.That(str.FirstWord(), Is.EqualTo("This"));
        }

        [Test]
        public void SecondWordTest()
        {
            const string str = "This is a test";
            Assert.That(str.SecondWord(), Is.EqualTo("is"));
        }

        [Test]
        public void ThirdWordTest()
        {
            const string str = "This is a test";
            Assert.That(str.ThirdWord(), Is.EqualTo("a"));
        }
    }
}
