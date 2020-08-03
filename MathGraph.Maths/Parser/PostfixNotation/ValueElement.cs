namespace MathGraph.Maths.Parser.PostfixNotation
{
    public class ValueElement : PostfixNotationElement
    {
        internal ValueElement(double value)
        {
            this.Value = value;
        }

        public double Value { get; }

        public override bool Equals(PostfixNotationElement other)
        {
            return other is ValueElement value && value.Value == this.Value;
        }
    }
}
