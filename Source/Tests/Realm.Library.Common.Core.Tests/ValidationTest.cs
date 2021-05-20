using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Realm.Library.Common.Test
{
    public class ValidationTest
    {
        [Theory]
        [InlineData(25, "number")]
        public void IsNotNullTest(object obj, string param)
        {
            Validation.IsNotNull(obj, param);
        }

        [Fact]
        public void IsNotNull_ThrowsException_WhenObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Validation.IsNotNull(null, "null"));
        }

        [Theory]
        [InlineData(25, typeof(int))]
        [InlineData("test", typeof(string))]
        public void IsInstanceOfTest(object obj, Type type)
        {
            Validation.IsInstanceOfType(obj, type);
        }

        [Theory]
        [InlineData("test", "number")]
        public void IsNotNullOrEmptyTest(string obj, string param)
        {
            Validation.IsNotNullOrEmpty(obj, param);
        }

        [Fact]
        public void IsNotNullOrEmpty_ThrowsException_WhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => Validation.IsNotNullOrEmpty("", null));
        }

        [Theory]
        [InlineData(new object[] { 25, 15 }, "value")]
        public void IsNotEmpty(ICollection value, string param)
        {
            Validation.IsNotEmpty(value, param);
        }

        [Fact]
        public void IsNotEmpty_THrowsException_WhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => Validation.IsNotEmpty(null, "null"));
        }

        [Fact]
        public void IsNotEmptyByTypeTest()
        {
            ICollection<string> list = new Collection<string>();
            list.Add("Test1");
            list.Add("Test2");

            Action act = () => Validation.IsNotEmpty(list, "list");
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(true, "test")]
        public void Validate(bool test, string message)
        {
            Validation.Validate(test, message);
        }

        [Fact]
        public void ValidateAction()
        {
            var callback = false;

            Validation.Validate(() => { callback = true; });

            callback.Should().BeTrue();
        }
    }
}
