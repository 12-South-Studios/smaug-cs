using FluentAssertions;
using NCalc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace Library.NCalc.Tests;

public class ExpressionTableTests
{
  private static int FakeFunc(FunctionArgs args, IEnumerable<CustomExpression> expressions)
  {
    return 0;
  }

  private static string FakeReplaceFunc(Match regexMatch)
  {
    return string.Empty;
  }

  [Fact]
  public void Add_HasNoConflicts()
  {
    ExpressionTable table = new();
    CustomExpression expr = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    Action add = () => table.Add(expr);
    add.Should().NotThrow<Exception>();
  }

  [Fact]
  public void Add_HasANameConflict_ThrowsException()
  {
    ExpressionTable table = new();
    CustomExpression expr1 = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    table.Add(expr1);

    CustomExpression expr2 = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    Action add = () => table.Add(expr2);
    add.Should().Throw<ArgumentException>("Regular Expression '[0-9]' is already present in the collection.");
  }

  [Fact]
  public void Add_HasARegexConflict_ThrowsException()
  {
    ExpressionTable table = new();
    CustomExpression expr1 = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    table.Add(expr1);

    CustomExpression expr2 = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    Action add = () => table.Add(expr2);
    add.Should().Throw<ArgumentException>().WithMessage("Function Name 'Test' is already present in the collection.");
  }

  [Fact]
  public void Get_HasANameMatch_ReturnsValidResult()
  {
    ExpressionTable table = new();
    CustomExpression expr1 = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    table.Add(expr1);

    table.Get("Test").Should().Be(expr1);
  }

  [Fact]
  public void Get_HasARegexMatch_ReturnsValidResult()
  {
    ExpressionTable table = new();
    CustomExpression expr1 = new("Test", "[0-9]")
    {
      ExpressionFunction = FakeFunc,
      ReplacementFunction = FakeReplaceFunc
    };

    table.Add(expr1);

    table.Get("[0-9]").Should().Be(expr1);
  }
}