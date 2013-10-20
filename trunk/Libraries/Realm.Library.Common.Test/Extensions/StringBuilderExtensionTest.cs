using System;
using System.Text;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common.Test
// ReSharper restore CheckNamespace
{
    /*[TestClass]
    public class StringBuilderExtensionTest
    {
        #region CapitalizeFirst
        [TestMethod]
        public void CapitalizeFirstTest()
        {
            var sb = new StringBuilder("this is a test");
            var expected = new StringBuilder("This is a test");
            var actual = sb.CapitalizeFirst();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CapitalizeFirstEmptyStringTest()
        {
            var sb = new StringBuilder(string.Empty);
            sb.CapitalizeFirst();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CapitalizeFirstNullValueTest()
        {
            StringBuilderExtensions.CapitalizeFirst(null);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOfAny
        [TestMethod]
        public void IndexOfAnyTest()
        {
            var sb = new StringBuilder("This is a test");
            var charArray = new[] { 'h', 'a', 'e' };
            const int startIndex = 0;
            const int expected = 1;
            var actual = sb.IndexOfAny(charArray, startIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfAnyNullValueTest()
        {
            var charArray = new[] { 'h', 'a', 'e' };
            const int startIndex = 0;

            StringBuilderExtensions.IndexOfAny(null, charArray, startIndex);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        public void IndexOfAnyNoCharsTest()
        {
            var sb = new StringBuilder("This is a test");
            var charArray = new[] { 'g', 'z', 'w' };
            const int startIndex = 0;
            const int expected = -1;
            var actual = sb.IndexOfAny(charArray, startIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexOfAnyNegativeStartIndexTest()
        {
            var sb = new StringBuilder("This is a test");
            var charArray = new[] { 'g', 'z', 'w' };
            const int startIndex = -1;

            sb.IndexOfAny(charArray, startIndex);
        }
        #endregion

        #region Substring
        [TestMethod]
        public void SubstringTest()
        {
            var sb = new StringBuilder("This is a test");
            const int startIndex = 5;
            const int length = 4;
            const string expected = "is a";
            var actual = sb.Substring(startIndex, length);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubstringNullValueTest()
        {
            const int startIndex = 0;
            const int length = 5;

            StringBuilderExtensions.Substring(null, startIndex, length);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region Remove
        [TestMethod]
        public void RemoveTest()
        {
            var sb = new StringBuilder("This is a test");
            const char ch = 't';
            var expected = new StringBuilder("This is a es");
            var actual = sb.Remove(ch);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullValueTest()
        {
            const char value = 'c';

            StringBuilderExtensions.Remove(null, value);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region RemoveFromEnd
        [TestMethod]
        public void RemoveFromEndTest()
        {
            var sb = new StringBuilder("This is a test");
            const int num = 5;
            var expected = new StringBuilder("This is a");
            var actual = sb.RemoveFromEnd(num);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveFromEndNullValueTest()
        {
            const int value = 5;

            StringBuilderExtensions.RemoveFromEnd(null, value);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region Clear
        [TestMethod]
        public void ClearTest()
        {
            var sb = new StringBuilder("Test");
            
            var actual = sb.Clear();

            const int expected = 0;

            Assert.AreEqual(expected, actual.Length);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ToString()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearNullValueTest()
        {
            StringBuilderExtensions.Clear(null);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region LTrim
        [TestMethod]
        public void LTrimTest()
        {
            var sb = new StringBuilder("    Test");
            var expected = new StringBuilder("Test");
            var actual = sb.LTrim();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LTrimNullValueTest()
        {
            StringBuilderExtensions.LTrim(null);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region RTrim
        [TestMethod]
        public void RTrimTest()
        {
            var sb = new StringBuilder("Test    ");
            var expected = new StringBuilder("Test");
            var actual = sb.RTrim();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RTrimNullValueTest()
        {
            StringBuilderExtension.RTrim(null);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region Trim
        [TestMethod]
        public void TrimTest()
        {
            var sb = new StringBuilder("    Test    ");
            var expected = new StringBuilder("Test");
            var actual = sb.Trim();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TrimNullValueTest()
        {
            StringBuilderExtension.Trim(null);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOf Char
        [TestMethod]
        public void IndexOfCharTest()
        {
            var sb = new StringBuilder("This is a test");
            const char value = 'i';
            const int expected = 2;
            var actual = sb.IndexOf(value);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfCharNullValueTest()
        {
            const char value = 'i';

            StringBuilderExtension.IndexOf(null, value);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOf Char StartIndex
        [TestMethod]
        public void IndexOfCharStartIndexTest()
        {
            var sb = new StringBuilder("This is a test");
            const char value = 'i';
            const int startIndex = 5;
            const int expected = 5;
            var actual = sb.IndexOf(value, startIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfCharStartIndexNullValueTest()
        {
            const char value = 'i';
            const int startIndex = 5;

            StringBuilderExtension.IndexOf(null, value, startIndex);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOf String
        [TestMethod]
        public void IndexOfStringTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "is";
            const int expected = 2;
            var actual = sb.IndexOf(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringNullValueTest()
        {
            const string value = "is";

            StringBuilderExtension.IndexOf(null, value);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringEmptyValueTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "";

            sb.IndexOf(value);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOf String StartIndex IgnoreCase
        [TestMethod]
        public void IndexOfStringStartIndexIgnoreCaseTest()
        {
            var sb = new StringBuilder("This Is a test");
            const string value = "is";
            const int startIndex = 5;
            const bool ignoreCase = true;
            const int expected = 5;
            var actual = sb.IndexOf(value, startIndex, ignoreCase);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringStartIndexIgnoreCaseNullValueTest()
        {
            const string value = "is";
            const int startIndex = 5;
            const bool ignoreCase = true;

            StringBuilderExtension.IndexOf(null, value, startIndex, ignoreCase);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringStartIndexIgnoreCasemptyValueTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "";
            const int startIndex = 5;
            const bool ignoreCase = true;

            sb.IndexOf(value, startIndex, ignoreCase);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOf String IgnoreCase
        [TestMethod]
        public void IndexOfStringIgnoreCaseTest()
        {
            var sb = new StringBuilder("This Is a test");
            const string value = "is";
            const bool ignoreCase = true;
            const int expected = 2;
            var actual = sb.IndexOf(value, ignoreCase);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfstringIgnoreCaseNullValueTest()
        {
            const string value = "is";
            const bool ignoreCase = true;

            StringBuilderExtension.IndexOf(null, value, ignoreCase);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringIgnoreCaseEmptyValueTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "";
            const bool ignoreCase = true;

            sb.IndexOf(value, ignoreCase);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region IndexOf String StartIndex
        [TestMethod]
        public void IndexOfStringStartIndexTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "is";
            const int startIndex = 5;
            const int expected = 5;
            var actual = sb.IndexOf(value, startIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringStartIndexNullValueTest()
        {
            const string value = "is";
            const int startIndex = 5;

            StringBuilderExtensions.IndexOf(null, value, startIndex);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexOfStringStartIndexEmptyValueTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "";
            const int startIndex = 5;

            sb.IndexOf(value, startIndex);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region ReplaceFirst
        [TestMethod]
        public void ReplaceFirstTest()
        {
            var sb = new StringBuilder("This is a test");
            const string oldValue = "is";
            const string newValue = "at";
            var expected = new StringBuilder("That is a test");
            var actual = sb.ReplaceFirst(oldValue, newValue);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceFirstNullValueTest()
        {
            const string oldValue = "is";
            const string newValue = "at";

            StringBuilderExtensions.ReplaceFirst(null, oldValue, newValue);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceFirstEmptyOldValueTest()
        {
            var sb = new StringBuilder("This is a test");
            const string oldValue = "";
            const string newValue = "at";

            sb.ReplaceFirst(oldValue, newValue);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceFirstEmptyNewValueTest()
        {
            var sb = new StringBuilder("This is a test");
            const string oldValue = "is";
            const string newValue = "";

            sb.ReplaceFirst(oldValue, newValue);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region StartsWith IgnoreCase
        [TestMethod]
        public void StartsWithIgnoreCaseTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "this";
            const bool ignoreCase = true;
            const bool expected = true;
            var actual = sb.StartsWith(value, ignoreCase);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartsWithIgnoreCaseNullValueTest()
        {
            const string value = "this";
            const bool ignoreCase = true;

            StringBuilderExtensions.StartsWith(null, value, ignoreCase);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region StartsWith
        [TestMethod]
        public void StartsWithTest()
        {
            var sb = new StringBuilder("This is a test");
            const string value = "This";
            const bool expected = true;
            var actual = sb.StartsWith(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartsWithNullValueTest()
        {
            const string value = "this";

            StringBuilderExtensions.StartsWith(null, value);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion

        #region StartsWith StartIndex IgnoreCase
        [TestMethod]
        public void StartsWithStartIndexIgnoreCaseTest()
        {
            var sb = new StringBuilder("This Is a test");
            const string value = "is";
            const bool ignoreCase = true;
            const bool expected = true;
            const int startIndex = 5;
            var actual = sb.StartsWithAndIgnoreCase(value, startIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartsWithStartIndexIgnoreCaseNullValueTest()
        {
            const string value = "this";
            const int startIndex = 5;
            const bool ignoreCase = true;

            StringBuilderExtensions.StartsWithAndIgnoreCase(null, value, startIndex);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown");
        }
        #endregion
    }*/
}
