using System;

namespace MathGraph.Maths.Lexer
{
    public class TokenSpan : IEquatable<TokenSpan>
    {
        public TokenSpan(int startIndex, int length)
        {
            this.StartIndex = startIndex;
            this.Length = length;
        }

        public int StartIndex { get; }
        public int Length { get; }

        public bool Equals(TokenSpan other)
        {
            return other != null
                && this.StartIndex == other.StartIndex
                && this.Length == other.Length;
        }
    }
}
