namespace MathGraph.Maths.Parser.PostfixNotation
{
    public class VariableElement : PostfixNotationElement
    {
        internal VariableElement(bool negative, string name)
        {
            this.IsNegative = negative;
            this.Name = name;
        }

        public bool IsNegative { get; }
        public string Name { get; }

        public override bool Equals(PostfixNotationElement other)
        {
            return other is VariableElement variable
                && variable.IsNegative == this.IsNegative
                && variable.Name.Equals(this.Name);
        }
    }
}
