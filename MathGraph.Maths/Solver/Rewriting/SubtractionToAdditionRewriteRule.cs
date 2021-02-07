using MathGraph.Maths.Parser.Expressions;
using System.Linq;

namespace MathGraph.Maths.Solver.Rewriting
{
    public class SubtractionToAdditionRewriteRule : RewriteRule
    {
        protected override bool CanBeApplied(MathsExpression expression)
        {
            return expression is OperatorExpression operatorExpression && operatorExpression.Operator == OperationType.Subtraction
                   && operatorExpression.ChildrenCount == 2;
        }

        protected override MathsExpression RewriteCore(MathsExpression expression)
        {
            OperatorExpression operatorExpression = expression as OperatorExpression;
            MathsExpression rightOperand = this.GetRightOperand(operatorExpression.Children.ElementAt(1));

            OperatorExpression output = new OperatorExpression(OperationType.Addition);
            output.AddFirstChild(rightOperand);
            output.AddFirstChild(operatorExpression.Children.First());

            return output;
        }

        private MathsExpression GetRightOperand(MathsExpression right)
        {
            if(right is ConstantExpression constant)
            {
                return new ConstantExpression(-constant.Value);
            }
            else if(right is VariableExpression variable)
            {
                if(variable.IsNegative)
                {
                    return new VariableExpression(variable.VariableName, false);
                }
            }

            OperatorExpression multiplication = new OperatorExpression(OperationType.Multiplication);
            multiplication.AddFirstChild(new ConstantExpression(-1));
            multiplication.AddFirstChild(right);

            return multiplication;
        }
    }
}
