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
                               Name = "Test",
                               RegexPattern = "[0-9]",
                               ExpressionFunction = FakeFunc,
                               ReplacementFunction = FakeReplaceFunc
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
                                Name = "Test",
                                RegexPattern = "[0-9]",
                                ExpressionFunction = FakeFunc,
                                ReplacementFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var expr2 = new CustomExpression()
                            {
                                Name = "Test",
                                RegexPattern = "[0-9]",
                                ExpressionFunction = FakeFunc,
                                ReplacementFunction = FakeReplaceFunc
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
                                Name = "Test",
                                RegexPattern = "[0-9]",
                                ExpressionFunction = FakeFunc,
                                ReplacementFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            var expr2 = new CustomExpression()
                            {
                                Name = "Test1",
                                RegexPattern = "[0-9]",
                                ExpressionFunction = FakeFunc,
                                ReplacementFunction = FakeReplaceFunc
                            };

            table.Add(expr2);
        }

        [Test]
        public void Get_NameMatch_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                Name = "Test",
                                RegexPattern = "[0-9]",
                                ExpressionFunction = FakeFunc,
                                ReplacementFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            Assert.That(table.Get("Test"), Is.EqualTo(expr1));
        }

        [Test]
        public void Get_RegexMatch_Test()
        {
            var table = new ExpressionTable();
            var expr1 = new CustomExpression()
                            {
                                Name = "Test",
                                RegexPattern = "[0-9]",
                                ExpressionFunction = FakeFunc,
                                ReplacementFunction = FakeReplaceFunc
                            };

            table.Add(expr1);

            Assert.That(table.Get("[0-9]"), Is.EqualTo(expr1));
        }
    }
}
