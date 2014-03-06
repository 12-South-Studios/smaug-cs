using System;
using NUnit.Framework;

namespace SmaugCS.Language.Tests
{
    [TestFixture]
    public class LanguageDataTests
    {
        private static LanguageData GetLanguageData()
        {
            var lang = new LanguageData(1, "elvish", LanguageTypes.Elven) { Alphabet = "iqqdakvtujfwghepcrslybszoz" };
            lang.AddPreConversion("star", "elen");
            lang.AddPreConversion("moon", "isin");
            lang.AddPostConversion("rr", "r");
            lang.AddPostConversion("qq", "q");
            return lang;
        }

        [Test]
        public void Translate_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.Translate(100, "The stars and moon are up.");

            Assert.That(result, Is.EqualTo("The stars and moon are up."));
        }

        [Test]
        public void Translate_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.Translate(0, "The star and moon are up.");
  
            Assert.That(result, Is.EqualTo("Sta elen ihd isin ira yp."));
        }

        [Test]
        public void DoPreConversion_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPreConversion(0, "star and moon");

            Assert.That(result, Is.EqualTo("elen and isin"));
        }

        [Test]
        public void DoPreConversion_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPreConversion(100, "star and moon");

            Assert.That(result, Is.EqualTo("star and moon"));
        }

        [TestCase('a', 'i')]
        [TestCase('z', 'z')]
        [TestCase('C', 'Q')]
        public void GetAlphabetEquivalentTest(char originalChar, char expectedChar)
        {
            var lang = GetLanguageData();
            Assert.That(lang.GetAlphabetEquivalent(originalChar), Is.EqualTo(expectedChar));
        }

        [Test]
        public void DoPostConversion_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPostConversion(0, "array");

            Assert.That(result, Is.EqualTo("aray"));
        }

        [Test]
        public void DoPostConversion_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPostConversion(100, "array");

            Assert.That(result, Is.EqualTo("array"));
        }

        [Test]
        public void DoCharacterConversion_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoCharacterConversion(0, "abcd");

            Assert.That(result, Is.EqualTo("iqqd"));
        }

        [Test]
        public void DoCharacterConversion_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoCharacterConversion(100, "abcd");

            Assert.That(result, Is.EqualTo("abcd"));
        }
    }
}
