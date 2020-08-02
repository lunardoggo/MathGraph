namespace MathGraph.Maths.Parser.Expressions
{
    public class OperatorExpression : MathsExpression
    {
        public OperatorExpression(MathsExpression left, OperationType @operator, MathsExpression right) 
            : base(ExpressionType.OperatorExpression)
        {
            this.Operator = @operator;
            this.Right = right;
            this.Left = left;
        }

        public OperationType Operator { get; }
        public MathsExpression Right { get; }
        public MathsExpression Left { get; }

        protected override bool Equals(MathsExpression expression)
        {
            if(expression is OperatorExpression operatorExpression)
            {
                return    this.Operator == operatorExpression.Operator
                       && this.Right.Equals(operatorExpression.Right)
                       && this.Left.Equals(operatorExpression.Left);
            }
            return false;
        }
    }

    public enum OperationType
    {
        Multiplication,
        Division,
        Addition,
        Subtraction
    }
}
