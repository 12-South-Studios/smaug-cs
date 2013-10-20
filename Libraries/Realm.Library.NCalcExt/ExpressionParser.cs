using System;
using System.Text.RegularExpressions;
using NCalc;
using Realm.Library.Common;

namespace Realm.Library.NCalcExt
{
    /// <summary>
    /// Expression parser that utilizes the NCalc library (http://ncalc.codeplex.com/)
    /// to parse mathematical expressions in addition to custom-defined functions.
    /// </summary>
    /// <remarks>Examples of use can be found in the Realm.Library.NCalcExt.Tests project</remarks>
    public class ExpressionParser
    {
        private readonly ExpressionTable _expressionTable;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExpressionParser() { }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="exprTable"></param>
        public ExpressionParser(ExpressionTable exprTable)
        {
            _expressionTable = exprTable;
        }

        /// <summary>
        /// Performs the actual execution of the expression parser
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public int Execute(string expr)
        {
            Validation.IsNotNullOrEmpty(expr, "expr");

            string newExpr = ReplaceExpressionMatches(expr);

            Expression exp = new Expression(newExpr);
            exp.EvaluateFunction += delegate(string name, FunctionArgs args)
                                        {
                                            if (_expressionTable == null)
                                                return;

                                            CustomExpression customExpr = _expressionTable.Get(name);
                                            if (customExpr != null && customExpr.ExpressionFunction != null)
                                                args.Result = customExpr.ExpressionFunction.Invoke(args);
                                        };

            object result = exp.Evaluate();

            Int32 outResult;
            Int32.TryParse(result.ToString(), out outResult);
            return outResult;
        }

        /// <summary>
        /// Performs a regex match and replace for any custom expressions
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private string ReplaceExpressionMatches(string expr)
        {
            if (_expressionTable == null || _expressionTable.Keys.IsEmpty())
                return expr;

            string newStr = expr;
            foreach (CustomExpression customExpr in _expressionTable.Values)
            {
                Regex regex = customExpr.Regex;
                int originalLength = newStr.Length;
                int lengthOffset = 0;

                foreach (Match match in regex.Matches(newStr))
                {
                    string firstPart = newStr.Substring(0, match.Index + lengthOffset);
                    string secondPart = newStr.Substring(match.Index + lengthOffset + match.Length,
                                                         newStr.Length - (match.Index + lengthOffset + match.Length));
                    newStr = firstPart + customExpr.ReplacementFunction.Invoke(match) + secondPart;
                    lengthOffset = newStr.Length - originalLength;
                }
            }

            return newStr;
        }
    }
}
