using System;
using NUnit.Framework;
namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class BitContextTests
    {
        private class FakeEntity : Entity
        {
            public FakeEntity(long id, string name)
                : base(id, name)
            {
            }
        }

        [Flags]
        private enum TestEnum
        {
            [Value(1)]
            Test1,

            [Value(2)]
            Test2 = 2
        };

        [Test]
        public void HasBitTest()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            ctx.SetBit(1);

            Assert.That(ctx.HasBit(1), Is.True);
        }

        [Test]
        public void HasBit_SetByEnum_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            ctx.SetBit(TestEnum.Test2);

            Assert.That(ctx.HasBit(2), Is.True);
        }

        [Test]
        public void HasBit_Enum_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            ctx.SetBit(TestEnum.Test2);

            Assert.That(ctx.HasBit(TestEnum.Test2), Is.True);
        }

        [Test]
        public void GetBits_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            ctx.SetBit(TestEnum.Test1);
            ctx.SetBit(TestEnum.Test2);

            Assert.That(ctx.GetBits, Is.EqualTo(3));
        }

        [Test]
        public void SetBits_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            const int val = (int)(TestEnum.Test1 | TestEnum.Test2);

            ctx.SetBits(val);

            Assert.That(ctx.GetBits, Is.EqualTo(val));
        }

        [Test]
        public void UnsetBit_Integer_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            ctx.SetBit(4);
            ctx.SetBit(2);

            Assert.That(ctx.HasBit(4), Is.True);

            ctx.UnsetBit(2);

            Assert.That(ctx.HasBit(4), Is.True);
            Assert.That(ctx.HasBit(2), Is.False);
        }

        [Test]
        public void UnsetBit_Enum_Test()
        {
            var fake = new FakeEntity(1, "Test");

            var ctx = new BitContext(fake);

            ctx.SetBit(TestEnum.Test1);
            ctx.SetBit(TestEnum.Test2);

            Assert.That(ctx.HasBit(TestEnum.Test2), Is.True);

            ctx.UnsetBit(TestEnum.Test1);

            Assert.That(ctx.HasBit(TestEnum.Test2), Is.True);
            Assert.That(ctx.HasBit(TestEnum.Test1), Is.False);
        }
    }
}
