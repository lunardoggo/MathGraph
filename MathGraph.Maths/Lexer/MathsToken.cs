using System.Diagnostics;
using System;

namespace MathGraph.Maths.Lexer
{
    [DebuggerDisplay("\\{ Type = {Type}, Value = \"{Value}\" \\}")]
    public class MathsToken : IEquatable<MathsToken>
    {
        public MathsToken(string value, TokenSpan tokenSpan, MathsTokenCategory category, MathsTokenType type)
        {
            this.TokenSpan = tokenSpan;
            this.Category = category;
            this.Value = value;
            this.Type = type;
        }

        public MathsTokenCategory Category { get; }
        public MathsTokenType Type { get; }
        public TokenSpan TokenSpan { get; }
        public string Value { get; }

        public bool Equals(MathsToken other)
        {
            return other != null
                && (this.TokenSpan == null && other.TokenSpan == null || this.TokenSpan.Equals(other.TokenSpan))
                && this.Category == other.Category
                && this.Value == other.Value
                && this.Type == other.Type;
        }
    }

    public enum MathsTokenCategory
    {
        Undefined,

        Parenthesis,
        Symbol,
        Variable,
        Number,
        Unary
    }

    public enum MathsTokenType
    {
        Undefined,

        Number,

        Multiply = 2,
        Divide = 3,
        Minus = 4,
        Plus = 5,
        Power = 6,

        ClosingParenthesis,
        OpenParenthesis,

        Variable,

        UnaryMinus
    }
}
