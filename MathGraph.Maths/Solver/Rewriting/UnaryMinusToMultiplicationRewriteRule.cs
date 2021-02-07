using MathGraph.Maths.Parser.Expressions;
using System;

namespace MathGraph.Maths.Solver.Rewriting
{
    public class UnaryMinusToMultiplicationRewriteRule : RewriteRule
    {
        protected override bool CanBeApplied(MathsExpression expression)
        {
            return expression is VariableExpression variable && variable.IsNegative;
        }

        protected override MathsExpression RewriteCore(MathsExpression expression)
        {
            if (this.CanBeApplied(expression))
            {
                VariableExpression variable = expression as VariableExpression;

                OperatorExpression operatorExpression = new OperatorExpression(OperationType.Multiplication);
                operatorExpression.AddRightChild(new ConstantExpression(-1));
                operatorExpression.AddRightChild(new VariableExpression(variable.VariableName, false));

                return operatorExpression;
            }
            throw new NotSupportedException($"{typeof(UnaryMinusToMultiplicationRewriteRule).Name} doesn't support rewriting the given {nameof(expression)}");
        }
    }
}
