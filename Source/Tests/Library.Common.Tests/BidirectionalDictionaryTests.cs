using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Common.Collections;
using Test.Common;
using Xunit;

namespace Library.Common.Tests;

[Collection(CollectionDefinitions.NonParallelCollection)]
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
    IEnumerable<string> results = _dictionary.GetByFirst(firstValue);

    bool result = results.ToList().Contains(secondValue);
    result.Should().Be(expectedResult);
  }

  [Theory]
  [InlineData("SecondLookupValue", "SecondValue", true)]
  [InlineData("SecondLookupValue", "FirstLookupValue", false)]
  public void GetBySecondFact(string secondValue, string firstValue, bool expectedResult)
  {
    IEnumerable<string> results = _dictionary.GetBySecond(secondValue);

    bool result = results.ToList().Contains(firstValue);
    result.Should().Be(expectedResult);
  }

  [Fact]
  public void Remove_Fact()
  {
    _dictionary.Remove("FirstValue", "FirstLookupValue");

    IEnumerable<string> results = _dictionary.GetByFirst("FirstValue");
    bool result = results.ToList().Contains("FirstValue");
    result.Should().BeFalse();

    results = _dictionary.GetBySecond("FirstLookupValue");
    result = results.ToList().Contains("FirstLookupValue");
    result.Should().BeFalse();
  }
}