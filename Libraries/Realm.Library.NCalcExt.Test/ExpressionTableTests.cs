using System;
using System.Text.RegularExpressions;
using NCalc;
using NUnit.Framework;

namespace Realm.Library.NCalcExt.Test
{
    [TestFixture]
    public class ExpressionTableTests
    {
        private static int FakeFunc(FunctionArgs args)
        {
            return 0;
        }
        private static string FakeReplaceFunc(Match regexMatch)
        {
            return string.Empty;
        }

        [Test]
        public void Add_NoConflicts_Test()
        {
            var table = new ExpressionTable();
            var expr = new CustomExpression()
                           {
                               FunctionName = "Test",
                               FunctionKeyword = "T",
                               RegularExpressionPattern = "[0-9]",
                               Function = FakeFunc,
                               RegexReplaceFunction = FakeReplaceFunc
                           };

            table.Add(expr);

            Assert.Pass("No exceptions were thrown");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_NameConflict_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var expr2 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_KeywordConflict_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var expr2 = new CustomExpression()
                            {
                                FunctionName = "Test1",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_RegexConflict_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var expr2 = new CustomExpression()
                            {
                                FunctionName = "Test1",
                                FunctionKeyword = "s",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr2);
        }

        [Test]
        public void Get_NameMatch_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var foundExpr = table.Get("Test");

            Assert.That(foundExpr, Is.EqualTo(expr1));
        }

        [Test]
        public void Get_KeywordMatch_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var foundExpr = table.Get("T");

            Assert.That(foundExpr, Is.EqualTo(expr1));
        }

        [Test]
        public void Get_RegexMatch_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                FunctionName = "Test",
                                FunctionKeyword = "T",
                                RegularExpressionPattern = "[0-9]",
                                Function = FakeFunc,
                                RegexReplaceFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var foundExpr = table.Get("[0-9]");

            Assert.That(foundExpr, Is.EqualTo(expr1));
        }
    }
}
