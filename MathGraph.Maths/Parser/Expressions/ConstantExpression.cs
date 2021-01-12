namespace MathGraph.Maths.Parser.Expressions
{
    public class ConstantExpression : MathsExpression
    {
        public ConstantExpression(double value) : base(ExpressionType.ConstantExpression)
        {
            this.Value = value;
        }

        public override int MaxChildrenCount { get; } = 0;
        public double Value { get; }

        protected override bool Equals(MathsExpression expression)
        {
            if(expression is ConstantExpression constantExpression)
            {
                return this.Value == constantExpression.Value;
            }
            return false;
        }
    }
}
