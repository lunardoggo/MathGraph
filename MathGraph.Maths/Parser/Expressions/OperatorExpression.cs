using System.Linq;

namespace MathGraph.Maths.Parser.Expressions
{
    public class OperatorExpression : MathsExpression
    {
        public OperatorExpression(OperationType @operator) 
            : base(ExpressionType.OperatorExpression)
        {
            this.Operator = @operator;
        }

        public override int MaxChildrenCount { get; } = 2;
        public OperationType Operator { get; }

        protected override bool Equals(MathsExpression expression)
        {
            if(expression is OperatorExpression operatorExpression)
            {
                return    this.Operator == operatorExpression.Operator
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
        Subtraction = 4,
        Division = 3,
        Addition = 5,
    }
}
