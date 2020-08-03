using System.Diagnostics;
using System;

namespace MathGraph.Maths.Parser.PostfixNotation
{
    [DebuggerDisplay("ValueElement \\{ Value = {Value} \\}")]
    public class ValueElement : PostfixNotationElement
    {
        internal ValueElement(double value)
        {
            this.Value = value;
        }

        public double Value { get; }

        public override string StringValue { get { return this.Value.ToString(); } }

        public override bool Equals(PostfixNotationElement other)
        {
            return other is ValueElement value && value.Value == this.Value;
        }
    }
}
