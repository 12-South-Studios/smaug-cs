using System;
using System.Text.RegularExpressions;
using NCalc;

namespace Realm.Library.NCalcExt
{
    /// <summary>
    /// Class for setting up custom functions for the NCalc Expression Parser
    /// </summary>
    public class CustomExpression
    {
        /// <summary>
        /// Name of the expression
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Regular expression match pattern
        /// </summary>
        public string RegexPattern { get; set; }

        /// <summary>
        /// Gets the regular expression object using the established pattern
        /// </summary>
        public Regex Regex
        {
            get { return new Regex(RegexPattern); }
        }

        /// <summary>
        /// Function call if a regex match is found (after replacement)
        /// </summary>
        public Func<FunctionArgs, int> ExpressionFunction { get; set; }

        /// <summary>
        /// Function that performs the proper replacement of the string matched using 
        /// the Regex pattern with a syntax that NCalc can understand
        /// </summary>
        public Func<Match, string> ReplacementFunction { get; set; }
    }
}
