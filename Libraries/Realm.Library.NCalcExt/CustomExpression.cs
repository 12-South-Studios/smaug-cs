using System;
using System.Text.RegularExpressions;
using NCalc;

namespace Realm.Library.NCalcExt
{
    /// <summary>
    /// Class for setting up custom functions for the Dice Parser
    /// </summary>
    public class CustomExpression
    {
        /// <summary>
        /// Name of the function
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Keyword that is found within the expression
        /// </summary>
        public string FunctionKeyword { get; set; }

        /// <summary>
        /// Regular expression match pattern
        /// </summary>
        public string RegularExpressionPattern { get; set; }

        /// <summary>
        /// Gets the regular expression object using the established pattern
        /// </summary>
        public Regex Regex
        {
            get { return new Regex(RegularExpressionPattern); }
        }

        /// <summary>
        /// Function call if a match is found
        /// </summary>
        public Func<FunctionArgs, int> Function { get; set; }

        /// <summary>
        /// The function to call to do a replacement 
        /// </summary>
        public Func<Match, string> RegexReplaceFunction { get; set; }
    }
}
