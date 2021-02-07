namespace MathGraph.Maths.Parser.Expressions
{
    public class ConstantExpression : MathsExpression
    {
        public ConstantExpression(decimal value) : base(ExpressionType.Constant)
        {
            this.Value = value;
        }

        public decimal Value { get; }

        public override bool Equals(MathsExpression expression)
        {
            if(expression is ConstantExpression constantExpression)
            {
                return this.Value == constantExpression.Value;
            }
            return false;
        }
    }
}
