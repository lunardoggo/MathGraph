using MathGraph.Maths.Parser.Expressions;
using MathGraph.Maths.Parser;
using MathGraph.Maths.Lexer;
using Xunit;
using System.Linq;

namespace MathGraph.Tests
{
    public class MathsExpressionTreeParserTests
    {
        [Fact]
        public void ParseSimpleAdditionTree()
        {
            MathsToken[] tokens = new MathsToken[]
            {
                new MathsToken("10", new TokenSpan(0, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(2, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("5", new TokenSpan(3, 1), MathsTokenCategory.Number, MathsTokenType.Number),
            };

            MathsExpression expression = new MathsExpressionTreeParser().Parse(tokens);

            this.AssertOperatorExpression(expression, OperationType.Addition, out var firstChild, out var secondChild);
            Assert.IsType<ConstantExpression>(firstChild);
            Assert.Equal(10, (firstChild as ConstantExpression).Value);
            Assert.IsType<ConstantExpression>(secondChild);
            Assert.Equal(5, (secondChild as ConstantExpression).Value);
        }

        [Fact]
        public void ParseSimpleWithOrderOfComputation()
        {
            MathsToken[] tokens = new MathsToken[]
            {
                new MathsToken("5", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(1, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("10", new TokenSpan(2, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("/", new TokenSpan(4, 1), MathsTokenCategory.Symbol, MathsTokenType.Divide),
                new MathsToken("2", new TokenSpan(5, 1), MathsTokenCategory.Number, MathsTokenType.Number),
            };

            MathsExpression expression = new MathsExpressionTreeParser().Parse(tokens);

            this.AssertOperatorExpression(expression, OperationType.Addition, out var fiveConstant, out var division);
            this.AssertOperatorExpression(division, OperationType.Division, out var tenConstant, out var twoConstant);
            this.AssertConstantExpression(twoConstant, 2);
            this.AssertConstantExpression(fiveConstant, 5);
            this.AssertConstantExpression(tenConstant, 10);
        }

        private void AssertOperatorExpression(MathsExpression expression, OperationType type, out MathsExpression firstChild, out MathsExpression secondChild)
        {
            Assert.IsType<OperatorExpression>(expression);
            OperatorExpression operatorExpression = expression as OperatorExpression;
            Assert.Equal(ExpressionType.Operator, operatorExpression.Type);
            Assert.Equal(type, operatorExpression.Operator);
            Assert.Equal(2, operatorExpression.Children.Count());
            firstChild = operatorExpression.Children.First();
            secondChild = operatorExpression.Children.Skip(1).First();
        }

        private void AssertConstantExpression(MathsExpression expression, decimal value)
        {
            Assert.IsType<ConstantExpression>(expression);
            ConstantExpression constant = expression as ConstantExpression;
            Assert.Equal(ExpressionType.Constant, constant.Type);
            Assert.Equal(value, constant.Value);
        }
    }
}
