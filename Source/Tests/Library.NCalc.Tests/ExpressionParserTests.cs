using FluentAssertions;
using NCalc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Library.NCalc.Tests;

public class ExpressionParserTests
{
  [Theory]
  [InlineData("4", 4)]
  [InlineData("5", 5)]
  public async Task Execute_SingleValue(string expression, int expected)
  {
    ExpressionParser parser = new();
    int result = await parser.ExecuteAsync(expression);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData("4+1", 5)]
  [InlineData("4+5+1", 10)]
  public async Task Execute_AddsValues(string expression, int expected)
  {
    ExpressionParser parser = new();
    int result = await parser.ExecuteAsync(expression);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData("4-1", 3)]
  [InlineData("4-1-2", 1)]
  public async Task Execute_SubtractsValues(string expression, int expected)
  {
    ExpressionParser parser = new();
    int result = await parser.ExecuteAsync(expression);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData("4*2", 8)]
  [InlineData("4*2*2", 16)]
  public async Task Execute_MultipliesValues(string expression, int expected)
  {
    ExpressionParser parser = new();
    int result = await parser.ExecuteAsync(expression);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData("4/2", 2)]
  [InlineData("4/2/2", 1)]
  public async Task Execute_DividesValues(string expression, int expected)
  {
    ExpressionParser parser = new();
    int result = await parser.ExecuteAsync(expression);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData("4+2-3", 3)]
  [InlineData("(4+2)/3+10", 12)]
  public async Task Execute_MixedOperations(string expression, int expected)
  {
    ExpressionParser parser = new();
    int result = await parser.ExecuteAsync(expression);
    result.Should().Be(expected);
  }

  private static int RollFunction(FunctionArgs args, IEnumerable<CustomExpression> expressions)
  {
    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
    Random random = new(Convert.ToInt32(t.TotalSeconds));

    return random.Next((int)args.Parameters[0].Evaluate(), (int)args.Parameters[1].Evaluate());
  }

  private static string ReplaceRollCall(Match regexMatch)
  {
    return $"Roll({(string.IsNullOrEmpty(regexMatch.Groups[1].Value) 
        ? "1" : regexMatch.Groups[1].Value)}, {regexMatch.Groups[3].Value})";
  }

  [Theory]
  [InlineData("2d6", 2, 12)]
  [InlineData("Roll(2, 6)", 2, 12)]
  [InlineData("2d6+4", 6, 16)]
  [InlineData("2d6+4+1d10", 7, 26)]
  [InlineData("d10", 1, 10)]
  public async Task Execute_StandardDiceFormat(string expression, int minExpected, int maxExpected)
  {
    ExpressionTable table = new();
    table.Add(new CustomExpression("Roll", @"([0-9]+)?(d|D)([0-9]+)")
    {
      ExpressionFunction = RollFunction,
      ReplacementFunction = ReplaceRollCall
    });

    ExpressionParser parser = new(table);

    int result = await parser.ExecuteAsync(expression);
    result.Should().BeGreaterOrEqualTo(minExpected);
    result.Should().BeLessOrEqualTo(maxExpected);
  }

  private static int NumberFunction(FunctionArgs args, IEnumerable<CustomExpression> expressions)
  {
    return 18;
  }

  private static string ReplaceNumberCall(Match regexMatch)
  {
    return "Number()";
  }

  [Theory]
  [InlineData("N", 18, 18)]
  [InlineData("N+5", 23, 23)]
  [InlineData("5+N+N+25", 66, 66)]
  [InlineData("2*N", 36, 36)]
  public async Task Execute_CustomFunction(string expression, int minExpected, int maxExpected)
  {
    ExpressionTable table = new();
    table.Add(new CustomExpression("Number", @"^?\b\w*[n|N]\w*\b$?")
    {
      ExpressionFunction = NumberFunction,
      ReplacementFunction = ReplaceNumberCall
    });

    ExpressionParser parser = new(table);

    int result = await parser.ExecuteAsync(expression);
    result.Should().BeGreaterOrEqualTo(minExpected);
    result.Should().BeLessOrEqualTo(maxExpected);
  }

  [Theory]
  [InlineData("2d6+N", 20, 30)]
  [InlineData("N+2d6+Number", 38, 48)]
  [InlineData("2d6+N+(2d4*10)", 40, 70)]
  public async Task Execute_MixedFunctions(string expression, int minExpected, int maxExpected)
  {
    ExpressionTable table = new();
    table.Add(new CustomExpression("Roll", @"([0-9]+)?(d|D)([0-9]+)")
    {
      ExpressionFunction = RollFunction,
      ReplacementFunction = ReplaceRollCall
    });
    table.Add(new CustomExpression("Number", @"^?\b\w*[n|N]\w*\b$?")
    {
      ExpressionFunction = NumberFunction,
      ReplacementFunction = ReplaceNumberCall
    });

    ExpressionParser parser = new(table);

    int result = await parser.ExecuteAsync(expression);
    result.Should().BeGreaterOrEqualTo(minExpected);
    result.Should().BeLessOrEqualTo(maxExpected);
  }

  public class FakeTester
  {
    public int Value
    {
      get { return 10; }
    }

    public int GetValue()
    {
      return 20;
    }

    public int GetValueWithParams(int value)
    {
      return 30 + value;
    }
  }

  [Theory]
  [InlineData("[Value]", 10, 10)]
  [InlineData("[value]", 10, 10)]
  [InlineData("2d6+[Value]", 12, 22)]
  public async Task Execute_ClassProperty(string expression, int minExpected, int maxExpected)
  {
    FakeTester tester = new();

    List<CustomExpression> expressions =
    [
      new CustomExpression("Value", @"\[[v|V](alue)\]")
      {
        ReplacementReference = new Tuple<string, List<object>>("Value", null)
      }
    ];

    ExpressionTable table = new();
    table.Add(new CustomExpression("Roll", @"([0-9]+)?(d|D)([0-9]+)")
    {
      ExpressionFunction = RollFunction,
      ReplacementFunction = ReplaceRollCall
    });

    ExpressionParser parser = new(table);

    int result = await parser.ExecuteAsync(expression, tester, expressions);
    result.Should().BeGreaterOrEqualTo(minExpected);
    result.Should().BeLessOrEqualTo(maxExpected);
  }

  [Theory]
  [InlineData("[Value]", 20, 20)]
  [InlineData("[value]", 20, 20)]
  [InlineData("2d6+[Value]", 22, 32)]
  public async Task Execute_ClassMethod_NoArgs(string expression, int minExpected, int maxExpected)
  {
    FakeTester tester = new();

    List<CustomExpression> expressions =
    [
      new CustomExpression("Value", @"\[[v|V](alue)\]")
      {
        ReplacementReference = new Tuple<string, List<object>>("GetValue", [])
      }
    ];

    ExpressionTable table = new();
    table.Add(new CustomExpression("Roll", @"([0-9]+)?(d|D)([0-9]+)")
    {
      ExpressionFunction = RollFunction,
      ReplacementFunction = ReplaceRollCall
    });

    ExpressionParser parser = new(table);

    int result = await parser.ExecuteAsync(expression, tester, expressions);
    result.Should().BeGreaterOrEqualTo(minExpected);
    result.Should().BeLessOrEqualTo(maxExpected);
  }

  [Theory]
  [InlineData("[Value]", 35, 35)]
  [InlineData("[value]", 35, 35)]
  [InlineData("2d6+[Value]", 37, 47)]
  public async Task Execute_ClassMethod_WithArgs(string expression, int minExpected, int maxExpected)
  {
    FakeTester tester = new();

    List<CustomExpression> expressions =
    [
      new CustomExpression("Value", @"\[[v|V](alue)\]")
      {
        ReplacementReference = new Tuple<string, List<object>>("GetValueWithParams", [5])
      }
    ];

    ExpressionTable table = new();
    table.Add(new CustomExpression("Roll", @"([0-9]+)?(d|D)([0-9]+)")
    {
      ExpressionFunction = RollFunction,
      ReplacementFunction = ReplaceRollCall
    });

    ExpressionParser parser = new(table);

    int result = await parser.ExecuteAsync(expression, tester, expressions);
    result.Should().BeGreaterOrEqualTo(minExpected);
    result.Should().BeLessOrEqualTo(maxExpected);
  }

  [Fact]
  public async Task Execute_BlanksExpressions_WhenExceptionOccurs()
  {
    FakeTester tester = new();

    List<CustomExpression> expressions =
    [
      new CustomExpression("Value", @"\[[v|V](alue)\]")
      {
        ReplacementReference = new Tuple<string, List<object>>("GetValueWithParams", [5])
      }
    ];

    ExpressionTable table = new();
    table.Add(new CustomExpression("Roll", @"([0-9]+)?(d|D)([0-9]+)")
    {
      ExpressionFunction = RollFunction,
      ReplacementFunction = null
    });

    ExpressionParser parser = new(table);

    await parser.ExecuteAsync("[Value]", tester, expressions);
    parser.Expressions.Should().BeNull();
  }
}