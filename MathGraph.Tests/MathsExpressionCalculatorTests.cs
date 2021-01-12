using MathGraph.Maths.Parser.Expressions;
using System.Collections.Generic;
using MathGraph.Maths.Calculator;
using MathGraph.Maths.Parser;
using MathGraph.Maths.Lexer;
using System;
using Xunit;

namespace MathGraph.Tests
{
    public class MathsExpressionCalculatorTests
    {
        [Fact]
        public void TestEvaluate()
        {
            MathsExpressionTreeCalculator calculator = new MathsExpressionTreeCalculator();

            Assert.Equal(1, calculator.Evaluate(this.GetMathsExpression("5 - 4")));
            Assert.Equal(10, calculator.Evaluate(this.GetMathsExpression("4 + 3 * 2")));
            Assert.Equal(14, calculator.Evaluate(this.GetMathsExpression("(4 + 3) * 2")));
            Assert.Equal(2, calculator.Evaluate(this.GetMathsExpression("(6 + 2) / (6 - 2)")));
            Assert.Equal(8, calculator.Evaluate(this.GetMathsExpression("3 * 4 / 2 + (7 - 2) / 2.5")));

            Assert.Throws<InvalidOperationException>(() => calculator.Evaluate(this.GetMathsExpression("3 * x - 34")));
        }

        private MathsExpression GetMathsExpression(string expressionString)
        {
            MathsTokenLexer lexer = new MathsTokenLexer();
            IEnumerable<MathsToken> tokens = lexer.LexMathematicalExpression(expressionString);
            MathsExpressionTreeParser parser = new MathsExpressionTreeParser();
            return parser.Parse(tokens);
        }
    }
}
