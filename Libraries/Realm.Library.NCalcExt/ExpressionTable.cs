using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;

namespace Realm.Library.NCalcExt
{
    /// <summary>
    /// Expression Table maintains a dictionary of expression objects
    /// </summary>
    public class ExpressionTable
    {
        private readonly Dictionary<string, CustomExpression> _expressionMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExpressionTable()
        {
            _expressionMap = new Dictionary<string, CustomExpression>();
        }

        /// <summary>
        /// Gets an enumerable list of the Dictionary keys
        /// </summary>
        public IEnumerable<string> Keys { get { return _expressionMap.Keys; } }

        /// <summary>
        /// Gets an enumerable list of the Dictionary values
        /// </summary>
        public IEnumerable<CustomExpression> Values { get { return _expressionMap.Values; } }

        /// <summary>
        /// Attempts to add a custom expression to the expression table.  Checks for 
        /// name, keyword and regular expression conflicts.
        /// </summary>
        /// <param name="expression"></param>
        public void Add(CustomExpression expression)
        {
            Validation.IsNotNull(expression, "expression");
            Validation.IsNotNullOrEmpty(expression.FunctionName, "expression.FunctionName");
            Validation.IsNotNullOrEmpty(expression.FunctionKeyword, "expression.FunctionKeyword");
            Validation.IsNotNullOrEmpty(expression.RegularExpressionPattern, "expression.RegularExpressionPattern");
            Validation.IsNotNull(expression.Function, "expression.Function");
            Validation.IsNotNull(expression.RegexReplaceFunction, "expression.RegexReplaceFunction");

            if (_expressionMap.ContainsKey(expression.FunctionName.ToLower()))
                throw new ArgumentException(string.Format("Function Name '{0}' is already present in the collection.",
                                                          expression.FunctionName));
            foreach (CustomExpression expr in _expressionMap.Values)
            {
                if (expr.FunctionKeyword.Equals(expression.FunctionKeyword, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException(string.Format("Keyword '{0}' is already present in the collection.",
                                                              expr.FunctionKeyword));
                if (expr.RegularExpressionPattern.Equals(expression.RegularExpressionPattern))
                    throw new ArgumentException(
                        string.Format("Regular Expression '{0}' is already present in the collection.",
                                      expr.RegularExpressionPattern));
            }

            _expressionMap.Add(expression.FunctionName.ToLower(), expression);
        }

        /// <summary>
        /// Attempts to retrieve a custom expression from the expresison table. Checks
        /// for name, keyword and regular expression matches.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CustomExpression Get(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (_expressionMap.ContainsKey(value.ToLower()))
                return _expressionMap[value.ToLower()];
            return
                _expressionMap.Values.ToList()
                              .FirstOrDefault(x => x.FunctionKeyword.Equals(value, StringComparison.OrdinalIgnoreCase)
                                                   || x.RegularExpressionPattern.Equals(value));
        }
    }
}
