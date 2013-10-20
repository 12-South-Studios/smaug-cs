using System;
using System.Text.RegularExpressions;
using NCalc;
using NUnit.Framework;

namespace Realm.Library.NCalcExt.Test
{
    [TestFixture]
    public class DiceTests
    {
        [TestCase("4", 4)]
        [TestCase("5", 5)]
        [TestCase("a", ExpectedException = typeof(ArgumentException))]
        [TestCase(null, ExpectedException = typeof(ArgumentException))]
        public void RollTests_SingleValue(string expression, int expected)
        {
            DiceParser parser = new DiceParser();
            Assert.That(parser.Roll(expression), Is.EqualTo(expected));
        }

        [TestCase("4+1", 5)]
        [TestCase("4+5+1", 10)]
        public void RollTests_AddsValues(string expression, int expected)
        {
            DiceParser parser = new DiceParser();
            Assert.That(parser.Roll(expression), Is.EqualTo(expected));
        }

        [TestCase("4-1", 3)]
        [TestCase("4-1-2", 1)]
        public void RollTests_SubtractsValues(string expression, int expected)
        {
            DiceParser parser = new DiceParser();
            Assert.That(parser.Roll(expression), Is.EqualTo(expected));
        }

        [TestCase("4*2", 8)]
        [TestCase("4*2*2", 16)]
        public void RollTests_MultipliesValues(string expression, int expected)
        {
            DiceParser parser = new DiceParser();
            Assert.That(parser.Roll(expression), Is.EqualTo(expected));
        }

        [TestCase("4/2", 2)]
        [TestCase("4/2/2", 1)]
        public void RollTests_DividesValues(string expression, int expected)
        {
            DiceParser parser = new DiceParser();
            Assert.That(parser.Roll(expression), Is.EqualTo(expected));
        }

        [TestCase("4+2-3", 3)]
        [TestCase("(4+2)/3+10", 12)]
        public void RollTests_MixedOperations(string expression, int expected)
        {
            DiceParser parser = new DiceParser();
            Assert.That(parser.Roll(expression), Is.EqualTo(expected));
        }

        private static int RollFunction(FunctionArgs args)
        {
            return Common.Random.Between((int)args.Parameters[0].Evaluate(), (int)args.Parameters[1].Evaluate());
        }

        private static string ReplaceDiceCall(Match regexMatch)
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
        public void RollTests_Dice(string expression, int minExpected, int maxExpected)
        {
            ExpressionTable table = new ExpressionTable();
            table.Add(new CustomExpression
                          {
                              FunctionName = "Roll",
                              FunctionKeyword = "d",
                              RegularExpressionPattern = @"([0-9]+)?(d|D)([0-9]+)",
                              Function = RollFunction,
                              RegexReplaceFunction = ReplaceDiceCall
                          });

            DiceParser parser = new DiceParser(table);

            int result = parser.Roll(expression);
            Assert.That(result, Is.AtLeast(minExpected));
            Assert.That(result, Is.AtMost(maxExpected));
        }

        private static int StrengthFunction(FunctionArgs args)
        {
            return 18;
        }
        private static string ReplaceStrengthCall(Match regexMatch)
        {
            return "Strength()";
        }

        [TestCase("S", 18, 18)]
        [TestCase("S+5", 23, 23)]
        [TestCase("5+S+S+25", 66, 66)]
        [TestCase("2*S", 36, 36)]
        public void RollTests_CustomFunc(string expression, int minExpected, int maxExpected)
        {
            ExpressionTable table = new ExpressionTable();
            table.Add(new CustomExpression
                          {
                              FunctionName = "Strength",
                              FunctionKeyword = "S",
                              RegularExpressionPattern = @"^?\b\w*[s|S]\w*\b$?",
                              Function = StrengthFunction,
                              RegexReplaceFunction = ReplaceStrengthCall
                          });

            DiceParser parser = new DiceParser(table);

            int result = parser.Roll(expression);
            Assert.That(result, Is.AtLeast(minExpected));
            Assert.That(result, Is.AtMost(maxExpected));
        }

        [TestCase("2d6+S", 20, 30)]
        [TestCase("S+2d6+Strength", 38, 48)]
        [TestCase("2d6+S+(2d4*10)", 40, 70)]
        public void RollTests_MixedFunctions(string expression, int minExpected, int maxExpected)
        {
            ExpressionTable table = new ExpressionTable();
            table.Add(new CustomExpression
                          {
                              FunctionName = "Roll",
                              FunctionKeyword = "d",
                              RegularExpressionPattern = @"([0-9]+)?(d|D)([0-9]+)",
                              Function = RollFunction,
                              RegexReplaceFunction = ReplaceDiceCall
                          });
            table.Add(new CustomExpression
                          {
                              FunctionName = "Strength",
                              FunctionKeyword = "S",
                              RegularExpressionPattern = @"^?\b\w*[s|S]\w*\b$?",
                              Function = StrengthFunction,
                              RegexReplaceFunction = ReplaceStrengthCall
                          });

            DiceParser parser = new DiceParser(table);

            int result = parser.Roll(expression);
            Assert.That(result, Is.AtLeast(minExpected));
            Assert.That(result, Is.AtMost(maxExpected));
        }
    }
}
