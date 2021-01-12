using System.Collections.Generic;
using System;

namespace MathGraph.Maths.Parser.Expressions
{
    public abstract class MathsExpression
    {
        protected List<MathsExpression> children = new List<MathsExpression>();

        public MathsExpression(ExpressionType type)
        {
            this.Type = type;
        }

        protected abstract bool Equals(MathsExpression expression);

        internal void AddChild(MathsExpression child)
        {
            if(this.children.Count >= this.MaxChildrenCount)
            {
                throw new InvalidOperationException($"Can't add more than {this.MaxChildrenCount} to node of type {this.Type}");
            }
            this.children.Add(child);
        }

        public IEnumerable<MathsExpression> Children
        {
            get { return children; }
        }

        public abstract int MaxChildrenCount { get; }

        public override bool Equals(object obj)
        {
            if(obj is MathsExpression expression && expression.GetType() == this.GetType() && expression.Type == this.Type)
            {
                return this.Equals(expression);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Type.GetHashCode();
        }

        public ExpressionType Type { get; }
    }

    public enum ExpressionType
    {
        OperatorExpression,
        ConstantExpression,
        VariableExpression
    }
}
