namespace MathGraph.Maths.Lexer
{
    public class MathsToken
    {
        public MathsToken(string value, LineSpan lineSpan, MathsTokenCategory category, MathsTokenType type)
        {
            this.LineSpan = lineSpan;
            this.Category = category;
            this.Value = value;
            this.Type = type;
        }

        public MathsTokenCategory Category { get; }
        public MathsTokenType Type { get; }
        public LineSpan LineSpan { get; }
        public string Value { get; }
    }

    public enum MathsTokenCategory
    {
        Undefined,

        Parenthesis,
        Operator,
        Variable,
        Number,
    }

    public enum MathsTokenType
    {
        Undefined,

        Number,
        
        Multiply,
        Divide,
        Minus,
        Plus,

        ClosingParenthesis,
        OpenParenthesis,

        Variable
    }
}
