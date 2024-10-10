using System;
using FluentAssertions;
using Library.Common.Attributes;
using Library.Common.Contexts;
using Library.Common.Objects;
using Xunit;

namespace Library.Common.Tests;

public class BitContextFacts
{
  private class FakeEntity(long id, string name) : Entity(id, name);

  [Flags]
  private enum FactEnum
  {
    [Value(1)] Fact1,

    [Value(2)] Fact2 = 2
  }

  [Fact]
  public void HasBitFact()
  {
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

    ctx.SetBit(1);

    ctx.HasBit(1).Should().BeTrue();
  }

  [Fact]
  public void HasBit_SetByEnum_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

    ctx.SetBit(FactEnum.Fact2);

    ctx.HasBit(2).Should().BeTrue();
  }

  [Fact]
  public void HasBit_Enum_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

    ctx.SetBit(FactEnum.Fact2);

    ctx.HasBit(FactEnum.Fact2).Should().BeTrue();
  }

  [Fact]
  public void GetBits_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

    ctx.SetBit(FactEnum.Fact1);
    ctx.SetBit(FactEnum.Fact2);

    ctx.GetBits.Should().Be(3);
  }

  [Fact]
  public void SetBits_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

    const int val = (int)(FactEnum.Fact1 | FactEnum.Fact2);

    ctx.SetBits(val);

    ctx.GetBits.Should().Be(val);
  }

  [Fact]
  public void UnsetBit_Integer_Fact()
  {
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

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
    FakeEntity fake = new(1, "Fact");

    BitContext ctx = new(fake);

    ctx.SetBit(FactEnum.Fact1);
    ctx.SetBit(FactEnum.Fact2);

    ctx.HasBit(FactEnum.Fact2).Should().BeTrue();

    ctx.UnsetBit(FactEnum.Fact1);

    ctx.HasBit(FactEnum.Fact2).Should().BeTrue();
    ctx.HasBit(FactEnum.Fact1).Should().BeFalse();
  }
}