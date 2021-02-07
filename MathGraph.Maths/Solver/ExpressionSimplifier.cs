using MathGraph.Maths.Parser.Expressions;
using MathGraph.Maths.Solver.Rewriting;
using System;
using System.Linq;

namespace MathGraph.Maths.Solver
{
    public class ExpressionSimplifier
    {
        public MathsExpression Simplify(MathsExpression expression)
        {
            MathsExpression simplified = this.ApplyRewriteRules(expression);
            MathsExpression evaluated = this.RecursivelyMaxEvaluate(simplified);
            return evaluated;
        }

        private MathsExpression RecursivelyMaxEvaluate(MathsExpression expression)
        {
            return expression;
        }

        private MathsExpression ApplyRewriteRules(MathsExpression expression)
        {
            RewriteRule[] rules = new RewriteRule[]
            {
                RewriteRules.SubtractionToAddition,
                RewriteRules.UnaryMinusToMultiplication,
                RewriteRules.DivisionToFraction
            };

            return this.RecursivelyRewriteExpression(expression, rules);
        }

        private MathsExpression RecursivelyRewriteExpression(MathsExpression expression, params RewriteRule[] rules)
        {
            MathsExpression rewritten = expression;
            foreach(RewriteRule rule in rules)
            {
                rewritten = this.TryRecursivelyRewriteExpression(rewritten, rule);
            }
            return rewritten;
        }

        private MathsExpression TryRecursivelyRewriteExpression(MathsExpression expression, RewriteRule rule)
        {
            if (expression is ChildrenMathsExpression childrenExpression)
            {
                for (int i = 0; i <  childrenExpression.Children.Count(); i++)
                {
                    MathsExpression child = childrenExpression.Children.ElementAt(i);
                    MathsExpression rewritten = this.TryRecursivelyRewriteExpression(child, rule);
                    childrenExpression.ReplaceChild(child, rewritten);
                }
            }
            return rule.Rewrite(expression);
        }
    }
}
