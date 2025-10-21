using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NCalc;
using NCalc.Handlers;

namespace Library.NCalc;

/// <summary>
/// Expression parser that utilizes the NCalc library (http://ncalc.codeplex.com/)
/// to parse mathematical expressions in addition to custom-defined functions.
/// </summary>
/// <remarks>Examples of use can be found in the Realm.Library.NCalcExt.Tests project</remarks>
public class ExpressionParser
{
  private readonly ExpressionTable _expressionTable;

  public ExpressionParser() { }

  public ExpressionParser(ExpressionTable exprTable)
  {
    _expressionTable = exprTable;
  }

  public IEnumerable<CustomExpression> Expressions { get; private set; }

  /// <summary>
  /// Executes the given expression through the NCalc engine
  /// </summary>
  /// <param name="expr">The string expression (un-parsed) to execute</param>
  /// <param name="targetObject">(Optional) The target object on which to perform replacement checks</param>
  /// <param name="expressions">(Optional) A custom list of expressions</param>
  /// <returns></returns>
  public async Task<int> ExecuteAsync(string expr, object targetObject = null,
    IEnumerable<CustomExpression> expressions = null)
  {
    try
    {
      Expressions = expressions;

      return await Task.Run(() =>
      {
        string newExpr = ReplaceExpressionMatches(expr, targetObject);

        Expression exp = new(newExpr);
        exp.EvaluateFunction += ExpOnEvaluateFunction;

        object result = exp.Evaluate();

        _ = int.TryParse(result.ToString(), out int outResult);
        return outResult;
      });
    }
    finally
    {
      Expressions = null;
    }
  }

  private void ExpOnEvaluateFunction(string name, FunctionArgs args)
  {
    CustomExpression customExpr = GetCustomExpression(name);
    if (customExpr?.ExpressionFunction == null)
      throw new InvalidOperationException();
    args.Result = customExpr.ExpressionFunction.Invoke(args, Expressions);
  }

  private CustomExpression GetCustomExpression(string name)
  {
    CustomExpression found = _expressionTable?.Get(name);
    return found ?? Expressions.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
  }

  private string ReplaceExpressionMatches(string expr, object targetObject)
  {
    List<CustomExpression> combined = [];
    if (_expressionTable != null && _expressionTable.Values.Any())
      combined.AddRange(_expressionTable.Values);
    if (Expressions != null && Expressions.Any())
      combined.AddRange(Expressions);
    if (combined.Count == 0) return expr;

    string newStr = expr;
    foreach (CustomExpression customExpr in combined)
    {
      Regex regex = customExpr.Regex;
      int originalLength = newStr.Length;
      int lengthOffset = 0;

      foreach (Match match in regex.Matches(newStr).Cast<Match>())
      {
        Tuple<string, int> result = ProcessRegexMatch(targetObject, newStr, match, customExpr, originalLength, lengthOffset);
        newStr = result.Item1;
        lengthOffset = result.Item2;
      }
    }

    return newStr;
  }

  private static Tuple<string, int> ProcessRegexMatch(object targetObject, string newStr, Match match,
    CustomExpression customExpr,
    int originalLength, int lengthOffset)
  {
    int len = match.Index + lengthOffset + match.Length;
    string firstPart = newStr[..(match.Index + lengthOffset)];
    string secondPart = newStr.Substring(len, newStr.Length - len);

    string updatedStr = string.Empty;
    if (customExpr.ReplacementFunction != null)
      updatedStr += firstPart + customExpr.ReplacementFunction.Invoke(match) + secondPart;

    if (customExpr.ReplacementReference == null || targetObject == null)
      return new Tuple<string, int>(updatedStr, updatedStr.Length - originalLength);
    
    MethodInfo method = targetObject.GetType().GetMethod(customExpr.ReplacementReference.Item1);
    PropertyInfo prop = targetObject.GetType().GetProperty(customExpr.ReplacementReference.Item1);

    if (method != null)
    {
      if (prop == null)
      {
        updatedStr += firstPart + method.Invoke(targetObject,
          customExpr.ReplacementReference.Item2 != null
            ? customExpr.ReplacementReference.Item2.ToArray()
            : []) + secondPart;
      }
    }
    else
    {
      if (prop == null)
        throw new ArgumentNullException(
          $"{customExpr.Name} could not be found on object {targetObject.GetType().FullName}");

      updatedStr += firstPart + prop.GetValue(targetObject) + secondPart;
    }

    return new Tuple<string, int>(updatedStr, updatedStr.Length - originalLength);
  }
}