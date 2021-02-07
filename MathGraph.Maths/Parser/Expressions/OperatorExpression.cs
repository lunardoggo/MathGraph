using System.Linq;

namespace MathGraph.Maths.Parser.Expressions
{
    public class OperatorExpression : ChildrenMathsExpression
    {
        public OperatorExpression(OperationType @operator)
            : base(ExpressionType.Operator)
        {
            this.Operator = @operator;
            this.MinChildrenCount = this.Operator == OperationType.Factorial ? 1 : 2;
        }

        public override int MinChildrenCount { get; }
        public OperationType Operator { get; }

        public override bool Equals(MathsExpression expression)
        {
            if (expression is OperatorExpression operatorExpression)
            {
                return this.Operator == operatorExpression.Operator
                       && this.children.Count == operatorExpression.children.Count
                       && this.children.Except(operatorExpression.Children).Count() == 0;
            }
            return false;
        }
    }

    public enum OperationType
    {
        //Indices match those of MathsTokenType
        Multiplication = 2,
        Division = 3,
        Subtraction = 4,
        Addition = 5,
        Power = 6,
        Factorial = 7
    }
}
