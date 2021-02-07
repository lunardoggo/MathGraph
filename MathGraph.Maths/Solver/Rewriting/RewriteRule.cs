using MathGraph.Maths.Parser.Expressions;

namespace MathGraph.Maths.Solver.Rewriting
{
    public static class RewriteRules
    {
        public static readonly RewriteRule UnaryMinusToMultiplication = new UnaryMinusToMultiplicationRewriteRule();
        public static readonly RewriteRule SubtractionToAddition = new SubtractionToAdditionRewriteRule();
        public static readonly RewriteRule DivisionToFraction = new DivisionToFractionRewriteRule();
    }

    public abstract class RewriteRule
    {
        public MathsExpression Rewrite(MathsExpression expression)
        {
            if(this.CanBeApplied(expression))
            {
                return this.RewriteCore(expression);
            }
            return expression;
        }

        protected abstract MathsExpression RewriteCore(MathsExpression expression);
        protected abstract bool CanBeApplied(MathsExpression expression);
    }
}
