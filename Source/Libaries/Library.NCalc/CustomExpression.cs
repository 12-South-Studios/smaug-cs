using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NCalc;
using NCalc.Handlers;

namespace Library.NCalc;

public class CustomExpression
{
  public CustomExpression() { }

  public CustomExpression(string name, string regexPattern)
  {
    Name = name;
    RegexPattern = regexPattern;
  }

  public string Name { get; set; }
  public string RegexPattern { get; set; }
  public Regex Regex => new(RegexPattern);

  /// <summary>
  /// Function call if a regex match is found (after replacement)
  /// </summary>
  public Func<FunctionArgs, IEnumerable<CustomExpression>, int> ExpressionFunction { get; set; }

  /// <summary>
  /// Function that performs the proper replacement of the string matched using 
  /// the Regex pattern with a syntax that NCalc can understand
  /// </summary>
  public Func<Match, string> ReplacementFunction { get; set; }

  /// <summary>
  /// Function that performs the proper replacement of the string matched using
  /// the Regex pattern with a syntax that NCalc can understand.  Invokes
  /// the method or property info reference.  Can be passed a list of arguments
  /// </summary>
  public Tuple<string, List<object>> ReplacementReference { get; set; }
}