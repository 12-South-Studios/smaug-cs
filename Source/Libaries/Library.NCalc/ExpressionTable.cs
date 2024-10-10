using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.NCalc;

public class ExpressionTable
{
  private readonly Lazy<Dictionary<string, CustomExpression>> _expressionMap = new();

  public ExpressionTable()
  {
  }

  public IEnumerable<string> Keys => _expressionMap.Value.Keys;
  public IEnumerable<CustomExpression> Values => _expressionMap.Value.Values;

  public void Add(CustomExpression expression)
  {
    if (_expressionMap.Value.ContainsKey(expression.Name.ToLower()))
      throw new ArgumentException($"Function Name '{expression.Name}' is already present in the collection.");

    if (_expressionMap.Value.Values.Any(expr => expr.RegexPattern.Equals(expression.RegexPattern)))
      throw new ArgumentException(
        $"Regular Expression '{expression.RegexPattern}' is already present in the collection.");

    _expressionMap.Value.Add(expression.Name.ToLower(), expression);
  }

  public CustomExpression Get(string value)
  {
    if (string.IsNullOrEmpty(value))
      return null;

    return _expressionMap.Value.ContainsKey(value.ToLower())
      ? _expressionMap.Value[value.ToLower()]
      : _expressionMap.Value.Values.ToList()
        .FirstOrDefault(x => x.RegexPattern.Equals(value));
  }
}