using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Realm.Library.NCalc
{
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

        private IEnumerable<CustomExpression> _expressions;

        /// <summary>
        /// Executes the given expression through the NCalc engine
        /// </summary>
        /// <param name="expr">The string expression (un-parsed) to execute</param>
        /// <param name="targetObject">(Optional) The target object on which to perform replacement checks</param>
        /// <param name="expressions">(Optional) A custom list of expressions</param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string expr, object targetObject = null, IEnumerable<CustomExpression> expressions = null)
        {
            try
            {
                _expressions = expressions;

                return await Task.Run(() =>
                {
                    var newExpr = ReplaceExpressionMatches(expr, targetObject);

                    var exp = new global::NCalc.Expression(newExpr);
                    exp.EvaluateFunction += ExpOnEvaluateFunction;

                    var result = exp.Evaluate();

                    int.TryParse(result.ToString(), out var outResult);
                    return outResult;
                });
            }
            finally
            {
                _expressions = null;
            }
        }

        private void ExpOnEvaluateFunction(string name, global::NCalc.FunctionArgs args)
        {
            var customExpr = _expressionTable?.Get(name);
            if (customExpr == null)
                customExpr = _expressions.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (customExpr?.ExpressionFunction != null)
                args.Result = customExpr.ExpressionFunction.Invoke(args, _expressions);
        }

        private string ReplaceExpressionMatches(string expr, object targetObject)
        {
            var combined = new List<CustomExpression>();
            if (_expressionTable != null && _expressionTable.Values.Any())
                combined.AddRange(_expressionTable.Values);
            if (_expressions != null && _expressions.Any())
                combined.AddRange(_expressions);
            if (!combined.Any()) return expr;

            var newStr = expr;
            foreach (var customExpr in combined)
            {
                var regex = customExpr.Regex;
                var originalLength = newStr.Length;
                var lengthOffset = 0;

                foreach (Match match in regex.Matches(newStr))
                    newStr = ProcessRegexMatch(targetObject, newStr, match, customExpr, originalLength, ref lengthOffset);
            }

            return newStr;
        }

        private static string ProcessRegexMatch(object targetObject, string newStr, Match match, CustomExpression customExpr, int originalLength,
            ref int lengthOffset)
        {
            var len = match.Index + lengthOffset + match.Length;
            var firstPart = newStr.Substring(0, match.Index + lengthOffset);
            var secondPart = newStr.Substring(len, newStr.Length - len);

            string updatedStr = string.Empty;
            if (customExpr.ReplacementFunction != null)
                updatedStr += firstPart + customExpr.ReplacementFunction.Invoke(match) + secondPart;
            if (customExpr.ReplacementReference != null && targetObject != null)
            {
                var method = targetObject.GetType().GetMethod(customExpr.ReplacementReference.Item1);
                var prop = targetObject.GetType().GetProperty(customExpr.ReplacementReference.Item1);

                if (method != null && prop == null)
                {
                    if (customExpr.ReplacementReference.Item2 != null)
                        updatedStr += firstPart + method.Invoke(targetObject, customExpr.ReplacementReference.Item2.ToArray()) + secondPart;
                    else
                        updatedStr += firstPart + method.Invoke(targetObject, new object[] { }) + secondPart;
                }

                if (method == null && prop != null)
                    updatedStr += firstPart + prop.GetValue(targetObject) + secondPart;

                if (method == null && prop == null)
                    throw new ArgumentNullException($"{customExpr.Name} could not be found on object {targetObject.GetType().FullName}");
            }

            lengthOffset = updatedStr.Length - originalLength;
            return updatedStr;
        }
    }
}
