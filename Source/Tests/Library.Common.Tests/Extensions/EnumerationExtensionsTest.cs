﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using Library.Common.Attributes;
using Library.Common.Extensions;
using Xunit;

namespace Library.Common.Tests.Extensions;

public class EnumerationExtensionsTest
{
  public enum EnumTest
  {
    [Enum("Test", Value = 1, ShortName = "Testing", ExtraData = "Extra,Data")]
    Test,

    Test1 = 1024,

    [Enum("Test Two", Value = 5)] Test2 = 256,

    [Enum("All", Value = 1280)] All
  }

  private enum NameTest
  {
    [Name("Test")] Test
  }

  private enum RangeTest
  {
    [Range(Minimum = 5, Maximum = 10)]
    Test1,

    [Range(Maximum = 5)] Test2,

    [Range(Minimum = 11, Maximum = 20)]
    Test3
  }

  [Fact]
  public void GetValueInRangeMatchTest()
  {
    RangeTest value = EnumerationExtensions.GetValueInRange(18, RangeTest.Test2);
    value.Should().Be(RangeTest.Test3);
  }

  [Fact]
  public void GetValueInRangeNoMatchTest()
  {
    RangeTest value = EnumerationExtensions.GetValueInRange(-2, RangeTest.Test2);
    value.Should().Be(RangeTest.Test2);
  }

  [Fact]
  public void GetMinimumRangeTest()
  {
    int value = RangeTest.Test1.GetMinimum();
    value.Should().Be(5);
  }

  [Fact]
  public void GetMaximumRangeTest()
  {
    int value = RangeTest.Test1.GetMaximum();
    value.Should().Be(10);
  }

  [Fact]
  public void GetMinimumRangeWhenNotSpecifiedTest()
  {
    int value = RangeTest.Test2.GetMinimum();
    value.Should().Be(int.MinValue);
  }

  [Fact]
  public void GetEnumIgnoreCaseTest()
  {
    EnumTest value = EnumerationExtensions.GetEnumIgnoreCase<EnumTest>("test");
    value.Should().Be(EnumTest.Test);
  }

  [Fact]
  public void GetEnumIgnoreCaseInvalidTest()
  {
    Action act = () => EnumerationExtensions.GetEnumIgnoreCase<EnumTest>("tester");
    act.Should().Throw<InvalidEnumArgumentException>();
  }

  [Fact]
  public void GetEnumTest()
  {
    EnumTest result = EnumerationExtensions.GetEnum<EnumTest>("Test");
    result.Should().Be(EnumTest.Test);

    EnumTest.Test.GetName().Should().Be("Test");
    EnumTest.Test.GetValue().Should().Be(1);
    EnumTest.Test.GetShortName().Should().Be("Testing");
    EnumTest.Test.GetName().Should().Be("Test");
  }

  [Theory]
  [InlineData(EnumTest.Test, 2, false)]
  [InlineData(EnumTest.Test, 3, true)]
  public void HasBitTest(EnumTest value, int bit, bool expected)
  {
    value.HasBit(bit).Should().Be(expected);
  }

  [Fact]
  public void GetExtraDataTest()
  {
    string result = EnumTest.Test.GetExtraData();
    result.Should().Be("Extra,Data");
  }

  [Fact]
  public void ParseExtraDataTest()
  {
    List<string> list = EnumTest.Test.ParseExtraData(",").ToList();

    list.Count.Should().Be(2);
    list[0].Should().Be("Extra");
    list[1].Should().Be("Data");
  }

  [Fact]
  public void GetEnumIntTest()
  {
    EnumTest result = EnumerationExtensions.GetEnum<EnumTest>(1024);
    result.Should().Be(EnumTest.Test1);
  }

  [Fact]
  public void GetEnumIntInvalidTest()
  {
    Action act = () => EnumerationExtensions.GetEnum<EnumTest>(111);
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void GetEnumStringTest()
  {
    EnumTest result = EnumerationExtensions.GetEnum<EnumTest>("Test1");
    result.Should().Be(EnumTest.Test1);
  }

  [Fact]
  public void GetEnumStringInvalidTest()
  {
    Action act = () => EnumerationExtensions.GetEnum<EnumTest>("Blah");
    act.Should().Throw<ArgumentException>();
  }

  [Theory]
  [InlineData(1024, true)]
  [InlineData(111, false)]
  public void HasBitsTest(int bit, bool expected)
  {
    EnumTest.All.HasBit(bit).Should().Be(expected);
  }

  [Fact]
  public void GetValuesTest()
  {
    List<EnumTest> expectedList = [EnumTest.All, EnumTest.Test, EnumTest.Test1, EnumTest.Test2];

    List<EnumTest> values = EnumerationExtensions.GetValues<EnumTest>().ToList();

    values.Count().Should().Be(4);
    values.Should().BeEquivalentTo(expectedList);
  }

  [Fact]
  public void GetEnumByNameTest()
  {
    EnumTest value = EnumerationExtensions.GetEnumByName<EnumTest>("Test Two");

    value.Should().Be(EnumTest.Test2);
  }

  [Fact]
  public void GetEnumByNameInvalidTest()
  {
    Action act = () => EnumerationExtensions.GetEnumByName<EnumTest>("Invalid Name");
    act.Should().Throw<InvalidEnumArgumentException>();
  }
}