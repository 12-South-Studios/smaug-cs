using System;
using System.Text.RegularExpressions;
using NCalc;
using Realm.Library.Common;

namespace Realm.Library.NCalcExt
{
    /// <summary>
    /// Dice object that utilizes the magnificent NCalc utility at http://ncalc.codeplex.com/
    /// Extended to allow dice functions to be evaluated as well
    /// </summary>
    /// <remarks>Also use http://regexlib.com/</remarks>
    public class DiceParser
    {
        private readonly ExpressionTable _expressionTable;

        /// <summary>
        /// Constructor
        /// </summary>
        public DiceParser() { }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="exprTable"></param>
        public DiceParser(ExpressionTable exprTable)
        {
            _expressionTable = exprTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public int Roll(string expr)
        {
            string newExpr = ReplaceExpressionMatches(expr);

            Expression exp = new Expression(newExpr);
            exp.EvaluateFunction += delegate(string name, FunctionArgs args)
                                        {
                                            if (_expressionTable == null)
                                                return;

                                            CustomExpression customExpr = _expressionTable.Get(name);
                                            if (customExpr != null && customExpr.Function != null)
                                                args.Result = customExpr.Function.Invoke(args);
                                        };

            object result = exp.Evaluate();

            Int32 outResult;
            Int32.TryParse(result.ToString(), out outResult);
            return outResult;
        }

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
                    newStr = firstPart + customExpr.RegexReplaceFunction.Invoke(match) + secondPart;
                    lengthOffset = newStr.Length - originalLength;
                }
            }

            return newStr;
        }
    }
}
