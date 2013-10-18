using System;
using NUnit.Framework;
using SmaugCS.Common;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class CharExtensionTests
    {
        [TestCase('a', true)]
        [TestCase('1', false)]
        public void IsDigitTest(char value, bool expected)
        {
            Assert.That(value.IsDigit(), Is.EqualTo(expected));
        }

        [TestCase(' ', true)]
        [TestCase('a', false)]
        public void IsSpaceTest(char value, bool expected)
        {
            Assert.That(value.IsSpace(), Is.EqualTo(expected));
        }
    }
}
