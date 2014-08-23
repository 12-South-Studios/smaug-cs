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
        public void OnSetup()
        {
            _dictionary = new BidirectionalDictionary<string, string>();
            _dictionary.Add("FirstValue", "FirstLookupValue");
        }

        [TearDown]
        public void OnTeardown()
        {
            _dictionary = null;
        }

        [TestCase("FirstValue", "FirstLookupValue", true)]
        [TestCase("SecondValue", "FirstLookupValue", false)]
        [Category("Dictionary Tests")]
        public void GetByFirstTest(string firstValue, string secondValue, bool expectedResult)
        {
            var results = _dictionary.GetByFirst(firstValue);

            Assert.That(results.ToList().Contains(secondValue), Is.EqualTo(expectedResult));
        }

        [TestCase("FirstLookupValue", "FirstValue", true)]
        [TestCase("SecondLookupValue", "FirstValue", false)]
        [Category("Dictionary Tests")]
        public void GetBySecondTest(string secondValue, string firstValue, bool expectedResult)
        {
            var results = _dictionary.GetBySecond(secondValue);

            Assert.That(results.ToList().Contains(firstValue), Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("Dictionary Tests")]
        public void Remove_RemovesFirstValue_Test()
        {
            _dictionary.Remove("FirstValue", "FirstLookupValue");

            var results = _dictionary.GetByFirst("FirstValue");
            Assert.That(results.ToList().Contains("FirstValue"), Is.False);
        }

        [Test]
        [Category("Dictionary Tests")]
        public void Remove_RemovesSecondValue_Test()
        {
            _dictionary.Remove("FirstValue", "FirstLookupValue");

            var results = _dictionary.GetBySecond("FirstLookupValue");
            Assert.That(results.ToList().Contains("FirstLookupValue"), Is.False);
        }
    }
}
