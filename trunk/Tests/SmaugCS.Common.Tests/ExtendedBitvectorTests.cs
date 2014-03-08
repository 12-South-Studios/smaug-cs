using NUnit.Framework;

namespace SmaugCS.Common.Tests
{
    [TestFixture]
    public class ExtendedBitvectorTests
    {
        [Test]
        public void IsEmptyTest()
        {
            var xbit = new ExtendedBitvector();

            Assert.That(xbit.IsEmpty(), Is.True);
        }

        [Test]
        public void SetBitTest()
        {
            var xbit = new ExtendedBitvector();

            Assert.That(xbit.SetBit(8), Is.EqualTo(8));
        }

        [Test]
        public void IsSetTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(8);

            Assert.That(xbit.IsSet(8), Is.True);
        }

        [Test]
        public void RemoveBitTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(8);
            xbit.RemoveBit(8);

            Assert.That(xbit.IsSet(8), Is.False);
        }

        [Test]
        public void ToggleBitTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            Assert.That(xbit.IsSet(2), Is.True);
            Assert.That(xbit.ToggleBit(8), Is.EqualTo(2));
        }

        /*[Test]
        public void ClearBitsTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit<TestTypes>(2);
            xbit.SetBit<TestTypes>(8);
            xbit.ClearBits<TestTypes>();

            Assert.That(xbit.IsEmpty(), Is.True);
        }

        [Test]
        public void HasBitsTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit<TestTypes>(2);
            xbit.SetBit<TestTypes>(8);

            var ybit = new ExtendedBitvector();
            ybit.SetBit(2);
            ybit.SetBit(8);

            Assert.That(ybit.HasBits(xbit), Is.EqualTo(260));
        }

        [Test]
        public void HasBitsFalseTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            var ybit = new ExtendedBitvector();

            Assert.That(ybit.HasBits(xbit), Is.EqualTo(0));
        }

        [Test]
        public void SameBitsTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);

            var ybit = new ExtendedBitvector();
            ybit.SetBit(2);

            Assert.That(ybit.SameBits(xbit), Is.True);
        }

        [Test]
        public void SetBitsTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            var ybit = new ExtendedBitvector();
            ybit.SetBits(xbit);

            Assert.That(ybit.SameBits(xbit), Is.True);
        }

        [Test]
        public void RemoveBitsTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            var ybit = new ExtendedBitvector();
            ybit.SetBits(xbit);
            ybit.RemoveBits(xbit);

            Assert.That(ybit.IsEmpty(), Is.True);
        }

        [Test]
        public void ToggleBitsTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            var ybit = new ExtendedBitvector();
            ybit.SetBits(xbit);
            ybit.SetBit(4);
            ybit.ToggleBits(xbit);

            Assert.That(ybit.SameBits(xbit), Is.False);
            Assert.That(ybit.IsSet(4), Is.True);
            Assert.That(ybit.IsSet(8), Is.False);
        }

        [Test]
        public void ToStringTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            Assert.That(xbit.ToString(), Is.EqualTo("260&0&0&0"));
        }

        [Test]
        public void GetFlagStringTest()
        {
            var xbit = new ExtendedBitvector();
            xbit.SetBit(2);
            xbit.SetBit(8);

            Assert.That(xbit.GetFlagString(new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }), Is.EqualTo("c i "));
        }*/
    }
}
