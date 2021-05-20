using System;
using System.Collections.Generic;
using System.Linq;

namespace Realm.Library.NCalc
{
    public class ExpressionTable
    {
        private readonly Dictionary<string, CustomExpression> _expressionMap;

        public ExpressionTable()
        {
            _expressionMap = new Dictionary<string, CustomExpression>();
        }

        public IEnumerable<string> Keys => _expressionMap.Keys;
        public IEnumerable<CustomExpression> Values => _expressionMap.Values;

        public void Add(CustomExpression expression)
        {
            if (_expressionMap.ContainsKey(expression.Name.ToLower()))
                throw new ArgumentException($"Function Name '{expression.Name}' is already present in the collection.");

            if (_expressionMap.Values.Any(expr => expr.RegexPattern.Equals(expression.RegexPattern)))
                throw new ArgumentException(
                    $"Regular Expression '{expression.RegexPattern}' is already present in the collection.");

            _expressionMap.Add(expression.Name.ToLower(), expression);
        }

        public CustomExpression Get(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return _expressionMap.ContainsKey(value.ToLower())
                ? _expressionMap[value.ToLower()]
                : _expressionMap.Values.ToList()
                    .FirstOrDefault(x => x.RegexPattern.Equals(value));
        }
    }
}
