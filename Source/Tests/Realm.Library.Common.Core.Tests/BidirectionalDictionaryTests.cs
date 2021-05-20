using System.Linq;
using FluentAssertions;
using Realm.Library.Common.Collections;
using Xunit;

namespace Realm.Library.Common.Fact
{
    public class BidirectionalDictionaryFacts
    {
        private static BidirectionalDictionary<string, string> _dictionary;
            
        public BidirectionalDictionaryFacts()
        {
            _dictionary = new BidirectionalDictionary<string, string>();
            _dictionary.Add("FirstValue", "FirstLookupValue");
            _dictionary.Add("SecondValue", "SecondLookupValue");
        }

        ~BidirectionalDictionaryFacts()
        {
            _dictionary = null;
        }

        [Theory]
        [InlineData("FirstValue", "FirstLookupValue", true)]
        [InlineData("SecondValue", "FirstLookupValue", false)]
        public void GetByFirstFact(string firstValue, string secondValue, bool expectedResult)
        {
            var results = _dictionary.GetByFirst(firstValue);

            var result = results.ToList().Contains(secondValue);
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("SecondValue", "FirstLookupValue", true)]
        [InlineData("SecondLookupValue", "FirstLookupValue", false)]
        public void GetBySecondFact(string secondValue, string firstValue, bool expectedResult)
        {
            var results = _dictionary.GetBySecond(secondValue);

            var result = results.ToList().Contains(firstValue);
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void Remove_Fact()
        {
            _dictionary.Remove("FirstValue", "FirstLookupValue");

            var results = _dictionary.GetByFirst("FirstValue");
            var result = results.ToList().Contains("FirstValue");
            result.Should().BeFalse();

            results = _dictionary.GetBySecond("FirstLookupValue");
            result = results.ToList().Contains("FirstLookupValue");
            result.Should().BeFalse();
        }
    }
}
