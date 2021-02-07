using System.Linq;
using System;

namespace MathGraph.Maths.Parser.Expressions
{
    public class FractionExpression : ChildrenMathsExpression
    {
        public FractionExpression() : base(ExpressionType.Fraction)
        { }

        public override int MinChildrenCount { get; } = 2;

        internal override void AddFirstChild(MathsExpression child)
        {
            if(this.children.Count == 2)
            {
                throw new InvalidOperationException("Can't add more than 2 children to FractionExpression.");
            }
            base.AddFirstChild(child);
        }

        internal override void AddRightChild(MathsExpression child)
        {
            if (this.children.Count == 2)
            {
                throw new InvalidOperationException("Can't add more than 2 children to FractionExpression.");
            }
            base.AddRightChild(child);
        }

        public override bool Equals(MathsExpression expression)
        {
            if(expression is FractionExpression fraction)
            {
                return this.children.Except(fraction.Children).Count() == 0;
            }
            return false;
        }
    }
}
