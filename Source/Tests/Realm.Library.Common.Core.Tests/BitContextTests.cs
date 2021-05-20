using System;
using FluentAssertions;
using Realm.Library.Common.Attributes;
using Realm.Library.Common.Contexts;
using Realm.Library.Common.Objects;
using Xunit;

namespace Realm.Library.Common.Fact
{
    public class BitContextFacts
    {
        private class FakeEntity : Entity
        {
            public FakeEntity(long id, string name)
                : base(id, name)
            {
            }
        }

        [Flags]
        private enum FactEnum
        {
            [Value(1)]
            Fact1,

            [Value(2)]
            Fact2 = 2
        };

        [Fact]
        public void HasBitFact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            ctx.SetBit(1);

            ctx.HasBit(1).Should().BeTrue();
        }

        [Fact]
        public void HasBit_SetByEnum_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            ctx.SetBit(FactEnum.Fact2);

            ctx.HasBit(2).Should().BeTrue();
        }

        [Fact]
        public void HasBit_Enum_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            ctx.SetBit(FactEnum.Fact2);

            ctx.HasBit(FactEnum.Fact2).Should().BeTrue();
        }

        [Fact]
        public void GetBits_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            ctx.SetBit(FactEnum.Fact1);
            ctx.SetBit(FactEnum.Fact2);

            ctx.GetBits.Should().Be(3);
        }

        [Fact]
        public void SetBits_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            const int val = (int)(FactEnum.Fact1 | FactEnum.Fact2);

            ctx.SetBits(val);

            ctx.GetBits.Should().Be(val);
        }

        [Fact]
        public void UnsetBit_Integer_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            ctx.SetBit(4);
            ctx.SetBit(2);

            ctx.HasBit(4).Should().BeTrue();

            ctx.UnsetBit(2);
            ctx.HasBit(4).Should().BeTrue();
            ctx.HasBit(2).Should().BeFalse();
        }

        [Fact]
        public void UnsetBit_Enum_Fact()
        {
            var fake = new FakeEntity(1, "Fact");

            var ctx = new BitContext(fake);

            ctx.SetBit(FactEnum.Fact1);
            ctx.SetBit(FactEnum.Fact2);

            ctx.HasBit(FactEnum.Fact2).Should().BeTrue();

            ctx.UnsetBit(FactEnum.Fact1);

            ctx.HasBit(FactEnum.Fact2).Should().BeTrue();
            ctx.HasBit(FactEnum.Fact1).Should().BeFalse();
        }
    }
}
