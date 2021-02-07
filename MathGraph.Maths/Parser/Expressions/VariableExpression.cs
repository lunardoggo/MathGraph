namespace MathGraph.Maths.Parser.Expressions
{
    public class VariableExpression : MathsExpression
    {
        public VariableExpression(string variableName, bool isNegative) : base(ExpressionType.Variable)
        {
            this.VariableName = variableName;
            this.IsNegative = isNegative;
        }

        public string VariableName { get; }
        public bool IsNegative { get; }

        public override bool Equals(MathsExpression expression)
        {
            if(expression is VariableExpression variableExpression)
            {
                return    this.VariableName.Equals(variableExpression.VariableName)
                       && this.IsNegative == variableExpression.IsNegative;
            }
            return false;
        }
    }
}
