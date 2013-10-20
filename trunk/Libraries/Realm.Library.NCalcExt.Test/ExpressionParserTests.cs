using System;
using System.Text.RegularExpressions;
using NCalc;
using NUnit.Framework;

namespace Realm.Library.NCalcExt.Test
{
    [TestFixture]
    public class ExpressionParserTests
    {
        [TestCase("4", 4)]
        [TestCase("5", 5)]
        [TestCase("a", ExpectedException = typeof(ArgumentException))]
        [TestCase(null, ExpectedException = typeof(ArgumentException))]
        public void ExecuteTest_SingleValue(string expression, int expected)
        {
            ExpressionParser parser = new ExpressionParser();
            Assert.That(parser.Execute(expression), Is.EqualTo(expected));
        }

        [TestCase("4+1", 5)]
        [TestCase("4+5+1", 10)]
        public void ExecuteTest_AddsValues(string expression, int expected)
        {
            ExpressionParser parser = new ExpressionParser();
            Assert.That(parser.Execute(expression), Is.EqualTo(expected));
        }

        [TestCase("4-1", 3)]
        [TestCase("4-1-2", 1)]
        public void ExecuteTest_SubtractsValues(string expression, int expected)
        {
            ExpressionParser parser = new ExpressionParser();
            Assert.That(parser.Execute(expression), Is.EqualTo(expected));
        }

        [TestCase("4*2", 8)]
        [TestCase("4*2*2", 16)]
        public void ExecuteTest_MultipliesValues(string expression, int expected)
        {
            ExpressionParser parser = new ExpressionParser();
            Assert.That(parser.Execute(expression), Is.EqualTo(expected));
        }

        [TestCase("4/2", 2)]
        [TestCase("4/2/2", 1)]
        public void ExecuteTest_DividesValues(string expression, int expected)
        {
            ExpressionParser parser = new ExpressionParser();
            Assert.That(parser.Execute(expression), Is.EqualTo(expected));
        }

        [TestCase("4+2-3", 3)]
        [TestCase("(4+2)/3+10", 12)]
        public void ExecuteTest_MixedOperations(string expression, int expected)
        {
            ExpressionParser parser = new ExpressionParser();
            Assert.That(parser.Execute(expression), Is.EqualTo(expected));
        }

        private static int RollFunction(FunctionArgs args)
        {
            return Common.Random.Between((int)args.Parameters[0].Evaluate(), (int)args.Parameters[1].Evaluate());
        }

        private static string ReplaceRollCall(Match regexMatch)
        {
            return string.Format("Roll({0}, {1})",
                                 string.IsNullOrEmpty(regexMatch.Groups[1].Value) ? "1" : regexMatch.Groups[1].Value,
                                 regexMatch.Groups[3].Value);
        }

        [TestCase("2d6", 2, 12)]
        [TestCase("Roll(2, 6)", 12)]
        [TestCase("2d6+4", 6, 16)]
        [TestCase("2d6+4+1d10", 7, 26)]
        [TestCase("d10", 1, 10)]
        public void ExecuteTest_StandardDiceFormat(string expression, int minExpected, int maxExpected)
        {
            ExpressionTable table = new ExpressionTable();
            table.Add(new CustomExpression
                          {
                              Name = "Roll",
                              RegexPattern = @"([0-9]+)?(d|D)([0-9]+)",
                              ExpressionFunction = RollFunction,
                              ReplacementFunction = ReplaceRollCall
                          });

            ExpressionParser parser = new ExpressionParser(table);

            Assert.That(parser.Execute(expression),
                        Is.GreaterThanOrEqualTo(minExpected) & Is.LessThanOrEqualTo(maxExpected));
        }

        private static int NumberFunction(FunctionArgs args)
        {
            return 18;
        }
        private static string ReplaceNumberCall(Match regexMatch)
        {
            return "Number()";
        }

        [TestCase("N", 18, 18)]
        [TestCase("N+5", 23, 23)]
        [TestCase("5+N+N+25", 66, 66)]
        [TestCase("2*N", 36, 36)]
        public void ExecuteTest_CustomFunction(string expression, int minExpected, int maxExpected)
        {
            ExpressionTable table = new ExpressionTable();
            table.Add(new CustomExpression
                          {
                              Name = "Number",
                              RegexPattern = @"^?\b\w*[n|N]\w*\b$?",
                              ExpressionFunction = NumberFunction,
                              ReplacementFunction = ReplaceNumberCall
                          });

            ExpressionParser parser = new ExpressionParser(table);

            Assert.That(parser.Execute(expression),
                        Is.GreaterThanOrEqualTo(minExpected) & Is.LessThanOrEqualTo(maxExpected));
        }

        [TestCase("2d6+N", 20, 30)]
        [TestCase("N+2d6+Number", 38, 48)]
        [TestCase("2d6+N+(2d4*10)", 40, 70)]
        public void ExecuteTest_MixedFunctions(string expression, int minExpected, int maxExpected)
        {
            ExpressionTable table = new ExpressionTable();
            table.Add(new CustomExpression
                          {
                              Name = "Roll",
                              RegexPattern = @"([0-9]+)?(d|D)([0-9]+)",
                              ExpressionFunction = RollFunction,
                              ReplacementFunction = ReplaceRollCall
                          });
            table.Add(new CustomExpression
                          {
                              Name = "Number",
                              RegexPattern = @"^?\b\w*[n|N]\w*\b$?",
                              ExpressionFunction = NumberFunction,
                              ReplacementFunction = ReplaceNumberCall
                          });

            ExpressionParser parser = new ExpressionParser(table);

            Assert.That(parser.Execute(expression),
                        Is.GreaterThanOrEqualTo(minExpected) & Is.LessThanOrEqualTo(maxExpected));
        }
    }
}
