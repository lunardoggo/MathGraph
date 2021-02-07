using MathGraph.Maths.Parser.Expressions;
using System.Linq;
using System;

namespace MathGraph.Maths.Solver.Rewriting
{
    public class DivisionToFractionRewriteRule : RewriteRule
    {
        protected override bool CanBeApplied(MathsExpression expression)
        {
            return expression is OperatorExpression operatorExpression && operatorExpression.Operator == OperationType.Division;
        }

        protected override MathsExpression RewriteCore(MathsExpression expression)
        {
            if(expression is ChildrenMathsExpression childrenExpression && childrenExpression.ChildrenCount == 2)
            {
                if(childrenExpression.Children.Any(_child => _child is VariableExpression))
                {
                    return this.RewriteWithVariable(childrenExpression);
                }
                else
                {
                    return this.RewriteWithoutVariable(childrenExpression);
                }
            }
            throw new NotSupportedException($"Couldn't rewrite {nameof(expression)} into a fraction.");
        }

        private MathsExpression RewriteWithVariable(ChildrenMathsExpression expression)
        {
            MathsExpression denominator = expression.Children.Skip(1).First();
            MathsExpression numerator = expression.Children.First();

            if(denominator is VariableExpression denominatorVariable) // left/right -> left * right^-1
            {
                denominator = this.RewriteVarialbeDenominator(denominatorVariable);

                OperatorExpression output = new OperatorExpression(OperationType.Multiplication);
                output.AddFirstChild(numerator);
                output.AddRightChild(denominator);
                return output;
            }
            else if(numerator is VariableExpression numeratorVariable)
            {
                OperatorExpression output = new OperatorExpression(OperationType.Multiplication);
                FractionExpression fraction = new FractionExpression();
                fraction.AddFirstChild(new ConstantExpression(1));
                fraction.AddRightChild(denominator);

                output.AddFirstChild(fraction);
                output.AddRightChild(numeratorVariable);
                return output;
            }
            throw new InvalidOperationException($"VariableExpression in fraction could not be found.");
        }

        private MathsExpression RewriteVarialbeDenominator(VariableExpression expression)
        {
            OperatorExpression reciprocal = new OperatorExpression(OperationType.Power);
            reciprocal.AddFirstChild(expression);
            reciprocal.AddRightChild(new ConstantExpression(-1));

            return reciprocal;
        }

        private MathsExpression RewriteWithoutVariable(ChildrenMathsExpression expression)
        {
            FractionExpression fraction = new FractionExpression();
            fraction.AddFirstChild(expression.Children.First());
            fraction.AddRightChild(expression.Children.Skip(1).First());
            return fraction;
        }
    }
}
