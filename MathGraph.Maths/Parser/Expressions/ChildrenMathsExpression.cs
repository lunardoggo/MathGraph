using System.Collections.Generic;
using System;

namespace MathGraph.Maths.Parser.Expressions
{
    public abstract class ChildrenMathsExpression : MathsExpression
    {
        protected List<MathsExpression> children = new List<MathsExpression>();

        public ChildrenMathsExpression(ExpressionType type) : base(type)
        { }

        internal virtual void AddFirstChild(MathsExpression child)
        {
            this.children.Insert(0, child);
        }

        internal virtual void AddRightChild(MathsExpression child)
        {
            this.children.Add(child);
        }

        internal virtual void ReplaceChild(MathsExpression original, MathsExpression replacement)
        {
            int index = this.children.IndexOf(original);
            if (index != -1)
            {
                this.children[index] = replacement;
            }
            else
            {
                throw new IndexOutOfRangeException($"Original MathsExpression for replacement could not be found.");
            }
        }

        public IEnumerable<MathsExpression> Children
        {
            get { return children; }
        }

        public int ChildrenCount { get { return this.children.Count; } }
        public abstract int MinChildrenCount { get; }

    }
}
