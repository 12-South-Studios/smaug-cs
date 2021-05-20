using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Realm.Library.Common.Attributes;
using Xunit;

namespace Realm.Library.Common.Fact
{
    public class EnumerationFunctionFacts
    {
        private enum Fact
        {
            [Enum("Fact One")]
            Fact1 = 1,

            [Enum("Fact Two")]
            Fact2
        }

        [Theory]
        [InlineData("Fact One", Fact.Fact1)]
        [InlineData("Fact Two", Fact.Fact2)]
        public void GetEnumByNameFact(string name, Enum expectedVal)
        {
            var result = EnumerationFunctions.GetEnumByName<Fact>(name);
            result.Should().Be(expectedVal);
        }

        [Fact]
        public void GetValuesFact()
        {
            IEnumerable<Fact> list = EnumerationFunctions.GetAllEnumValues<Fact>();
            list.Should().NotBeNull();

            List<Fact> enumList = list.ToList();
            enumList.Should().Contain(Fact.Fact1);
            enumList.Should().Contain(Fact.Fact2);
        }

        [Fact]
        public void MaxFact()
        {
            var result = EnumerationFunctions.MaximumEnumValue<Fact>();
            result.Should().Be(2);
        }

        [Fact]
        public void MinFact()
        {
            var result = EnumerationFunctions.MinimumEnumValue<Fact>();
            result.Should().Be(1);
        }
    }
}
