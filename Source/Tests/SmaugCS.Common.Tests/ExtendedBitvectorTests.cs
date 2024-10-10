using FluentAssertions;
using Xunit;

namespace SmaugCS.Common.Tests;

public class ExtendedBitvectorTests
{
  [Fact]
  public void IsEmptyTest()
  {
    ExtendedBitvector xbit = new();

    xbit.IsEmpty().Should().BeTrue();
  }

  [Fact]
  public void SetBitTest()
  {
    ExtendedBitvector xbit = new();
    xbit.SetBit(8);

    xbit.IsSet(8).Should().BeTrue();
  }

  [Fact]
  public void IsSetTest()
  {
    ExtendedBitvector xbit = new();
    xbit.SetBit(8);

    xbit.IsSet(8).Should().BeTrue();
  }

  [Fact]
  public void RemoveBitTest()
  {
    ExtendedBitvector xbit = new();
    xbit.SetBit(8);
    xbit.RemoveBit(8);

    xbit.IsSet(8).Should().BeFalse();
  }

  //[Fact]
  //public void ToggleBitTest()
  //{
  //    var xbit = new ExtendedBitvector();
  //    xbit.SetBit(2);
  //    xbit.SetBit(8);

  //    xbit.IsSet(2).Should().BeTrue();
  //    xbit.ToggleBit(8);

  //    xbit.IsSet(8).Should().BeTrue();
  //}

  /*[Fact]
  public void ClearBitsTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit<TestTypes>(2);
      xbit.SetBit<TestTypes>(8);
      xbit.ClearBits<TestTypes>();

      xbit.IsEmpty().Should().BeTrue();
  }

  [Fact]
  public void HasBitsTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit<TestTypes>(2);
      xbit.SetBit<TestTypes>(8);

      var ybit = new ExtendedBitvector();
      ybit.SetBit(2);
      ybit.SetBit(8);

      ybit.HasBits(xbit).Should().Be(260));
  }

  [Fact]
  public void HasBitsFalseTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);
      xbit.SetBit(8);

      var ybit = new ExtendedBitvector();

      ybit.HasBits(xbit).Should().Be(0));
  }

  [Fact]
  public void SameBitsTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);

      var ybit = new ExtendedBitvector();
      ybit.SetBit(2);

      ybit.SameBits(xbit).Should().BeTrue();
  }

  [Fact]
  public void SetBitsTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);
      xbit.SetBit(8);

      var ybit = new ExtendedBitvector();
      ybit.SetBits(xbit);

      ybit.SameBits(xbit).Should().BeTrue();
  }

  [Fact]
  public void RemoveBitsTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);
      xbit.SetBit(8);

      var ybit = new ExtendedBitvector();
      ybit.SetBits(xbit);
      ybit.RemoveBits(xbit);

      ybit.IsEmpty().Should().BeTrue();
  }

  [Fact]
  public void ToggleBitsTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);
      xbit.SetBit(8);

      var ybit = new ExtendedBitvector();
      ybit.SetBits(xbit);
      ybit.SetBit(4);
      ybit.ToggleBits(xbit);

      ybit.SameBits(xbit).Should().BeFalse();
      ybit.IsSet(4).Should().BeTrue();
      ybit.IsSet(8).Should().BeFalse();
  }

  [Fact]
  public void ToStringTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);
      xbit.SetBit(8);

      xbit.ToString().Should().Be("260&0&0&0"));
  }

  [Fact]
  public void GetFlagStringTest()
  {
      var xbit = new ExtendedBitvector();
      xbit.SetBit(2);
      xbit.SetBit(8);

      xbit.GetFlagString(new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }).Should().Be("c i "));
  }*/
}