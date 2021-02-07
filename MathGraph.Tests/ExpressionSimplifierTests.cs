using MathGraph.Maths.Parser.Expressions;
using MathGraph.Maths.Solver;
using MathGraph.Maths;
using System.Linq;
using Xunit;

namespace MathGraph.Tests
{
    public class ExpressionSimplifierTests
    {
        private readonly ExpressionSimplifier simplifier = new ExpressionSimplifier();
        private readonly MathsExpressionParser parser = new MathsExpressionParser();

        [Fact]
        public void TestRewriteMinus()
        {
            //Minus should only appear in constants => -x -> -1 * x; 3 - 4 -> 3 + (-4)
            MathsExpression input = this.parser.Parse("-x - 10"); // -> -1 * x + (-10)

            MathsExpression simplified = this.simplifier.Simplify(input);

            Assert.IsType<OperatorExpression>(simplified);
            OperatorExpression operatorExpression = simplified as OperatorExpression;

            Assert.Equal(2, operatorExpression.ChildrenCount);
            Assert.Equal(OperationType.Addition, operatorExpression.Operator);
            Assert.IsType<OperatorExpression>(operatorExpression.Children.First());
            
            OperatorExpression variableOperator = operatorExpression.Children.First() as OperatorExpression;
            Assert.Equal(2, variableOperator.ChildrenCount);
            Assert.IsType<VariableExpression>(variableOperator.Children.Skip(1).First());
            Assert.IsType<ConstantExpression>(variableOperator.Children.First());
            Assert.Equal(-1, (variableOperator.Children.First() as ConstantExpression).Value);

            Assert.IsType<ConstantExpression>(operatorExpression.Children.Skip(1).First());
            Assert.Equal(-10, (operatorExpression.Children.Skip(1).First() as ConstantExpression).Value);
        }

        [Fact]
        public void TestRewriteDivision()
        {
            MathsExpression input = this.parser.Parse("x/3");

            MathsExpression simplified = this.simplifier.Simplify(input);

            Assert.IsType<OperatorExpression>(simplified);
            OperatorExpression multiplication = simplified as OperatorExpression;
            Assert.Equal(OperationType.Multiplication, multiplication.Operator);

            Assert.IsType<VariableExpression>(multiplication.Children.Skip(1).First());
            Assert.IsType<FractionExpression>(multiplication.Children.First());

            FractionExpression fraction = multiplication.Children.First() as FractionExpression;
            Assert.True(fraction.Children.All(_child => _child is ConstantExpression));
            Assert.Equal(1, (fraction.Children.First() as ConstantExpression).Value);
            Assert.Equal(3, (fraction.Children.Skip(1).First() as ConstantExpression).Value);
        }
    }
}
