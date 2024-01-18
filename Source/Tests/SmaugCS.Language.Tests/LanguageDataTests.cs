using FluentAssertions;
using Xunit;

namespace SmaugCS.Language.Tests
{

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

        [Fact]
        public void Translate_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.Translate(100, "The stars and moon are up.");

            result.Should().Be("The stars and moon are up.");
        }

        [Fact]
        public void Translate_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.Translate(0, "The star and moon are up.");

            result.Should().Be("Sta elen ihd isin ira yp.");
        }

        [Fact]
        public void DoPreConversion_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPreConversion(0, "star and moon");

            result.Should().Be("elen and isin");
        }

        [Fact]
        public void DoPreConversion_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPreConversion(100, "star and moon");

            result.Should().Be("star and moon");
        }

        [InlineData('a', 'i')]
        [InlineData('z', 'z')]
        [InlineData('C', 'Q')]
        public void GetAlphabetEquivalentTest(char originalChar, char expectedChar)
        {
            var lang = GetLanguageData();
            lang.GetAlphabetEquivalent(originalChar).Should().Be(expectedChar);
        }

        [Fact]
        public void DoPostConversion_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPostConversion(0, "array");

            result.Should().Be("aray");
        }

        [Fact]
        public void DoPostConversion_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoPostConversion(100, "array");

            result.Should().Be("array");
        }

        [Fact]
        public void DoCharacterConversion_ZeroPercent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoCharacterConversion(0, "abcd");

            result.Should().Be("iqqd");
        }

        [Fact]
        public void DoCharacterConversion_100Percent_Test()
        {
            var lang = GetLanguageData();

            var result = lang.DoCharacterConversion(100, "abcd");

            result.Should().Be("abcd");
        }
    }
}
