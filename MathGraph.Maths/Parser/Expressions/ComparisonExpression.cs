using System;
using System.Collections.Generic;
using System.Text;

namespace MathGraph.Maths.Parser.Expressions
{
    public class ComparisonExpression : ChildrenMathsExpression
    {
        public ComparisonExpression(ExpressionType type) : base(type)
        {
        }

        internal override void AddFirstChild(MathsExpression child)
        {
            if(this.ChildrenCount == 2)
            {
                throw new InvalidOperationException("Can't add more than 2 children to ComparisonExpression.");
            }
            base.AddFirstChild(child);
        }

        public override int MinChildrenCount { get; } = 2;

        public override bool Equals(MathsExpression expression)
        {
            throw new NotImplementedException();
        }
    }

    public enum ComparisonType
    {
        //Indices match those of MathsTokenType
        Equals = 20,
        GreaterThan = 21,
        LessThan = 22,
        GreaterOrEqual = 23,
        LessOrEqual = 24
    }
}
