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
            Validation.IsNotNullOrEmpty(expression.Name, "expression.Name");
            Validation.IsNotNullOrEmpty(expression.RegexPattern, "expression.RegexPattern");
            Validation.IsNotNull(expression.ExpressionFunction, "expression.ExpressionFunction");
            Validation.IsNotNull(expression.ReplacementFunction, "expression.ReplacementFunction");

            if (_expressionMap.ContainsKey(expression.Name.ToLower()))
                throw new ArgumentException(string.Format("Function Name '{0}' is already present in the collection.",
                                                          expression.Name));

            if (_expressionMap.Values.Any(expr => expr.RegexPattern.Equals(expression.RegexPattern)))
                throw new ArgumentException(
                    string.Format("Regular Expression '{0}' is already present in the collection.",
                                  expression.RegexPattern));

            _expressionMap.Add(expression.Name.ToLower(), expression);
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

            return (_expressionMap.ContainsKey(value.ToLower()))
                       ? _expressionMap[value.ToLower()]
                       : _expressionMap.Values.ToList()
                                       .FirstOrDefault(x => x.RegexPattern.Equals(value));
        }
    }
}
