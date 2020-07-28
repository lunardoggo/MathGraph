namespace MathGraph.Maths.Lexer
{
    public class LineSpan
    {
        public LineSpan(int startIndex, int length)
        {
            this.StartIndex = startIndex;
            this.Length = length;
        }

        public int StartIndex { get; }
        public int Length { get; }
    }
}
