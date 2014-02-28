using System;
using System.Linq;
using NUnit.Framework;
using Realm.Library.Common.Collections;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class BidirectionalDictionaryTests
    {
        private static BidirectionalDictionary<string, string> _dictionary;
            
        [SetUp]
        private void OnSetup()
        {
            _dictionary = new BidirectionalDictionary<string, string>();
            _dictionary.Add("FirstValue", "FirstLookupValue"); 
        }

        [TearDown]
        private void OnTeardown()
        {
            _dictionary = null;
        }

        [TestCase("FirstValue", "FirstLookupValue", true)]
        [TestCase("SecondValue", "FirstLookupValue", false)]
        public void GetByFirstTest(string firstValue, string secondValue, bool expectedResult)
        {
            var results = _dictionary.GetByFirst(firstValue);

            Assert.That(results.ToList().Contains(secondValue), Is.EqualTo(expectedResult));
        }

        [TestCase("SecondValue", "FirstLookupValue", true)]
        [TestCase("SecondLookupValue", "FirstLookupValue", false)]
        public void GetBySecondTest(string secondValue, string firstValue, bool expectedResult)
        {
            var results = _dictionary.GetBySecond(secondValue);

            Assert.That(results.ToList().Contains(firstValue), Is.EqualTo(expectedResult));
        }

        [Test]
        public void Remove_Test()
        {
            _dictionary.Remove("FirstValue", "FirstLookupValue");

            var results = _dictionary.GetByFirst("FirstValue");

            Assert.That(results.ToList().Contains("FirstValue"), Is.False);

            results = _dictionary.GetBySecond("FirstLookupValue");

            Assert.That(results.ToList().Contains("FirstLookupValue"), Is.False);
        }
    }
}
