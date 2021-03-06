﻿using System;

namespace MathGraph.Maths.Parser.Expressions
{
    public abstract class MathsExpression : IEquatable<MathsExpression>
    {
        public MathsExpression(ExpressionType type)
        {
            this.Type = type;
        }

        public abstract bool Equals(MathsExpression expression);

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
        Operator,
        Constant,
        Variable,
        Fraction
    }
}
