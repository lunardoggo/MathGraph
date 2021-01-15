using System.Collections.Generic;
using System.Linq;

namespace MathGraph.Maths.Lexer
{
    public class MathsTokenCompletionHelper
    {
        private static readonly MathsToken Multiply = new MathsToken("*", new TokenSpan(0, 0), MathsTokenCategory.Symbol, MathsTokenType.Multiply);

        public static MathsTokenCompletionHelper Instance { get; } = new MathsTokenCompletionHelper();
        private MathsTokenCompletionHelper()
        { }

        public IEnumerable<MathsToken> Complement(IEnumerable<MathsToken> tokens)
        {
            List<MathsToken> tmpTokens = new List<MathsToken>(tokens);
            this.InsertMissingMultiplications(tmpTokens);
            return this.GetUpdatedMathsTokens(tmpTokens);
        }

        private void InsertMissingMultiplications(List<MathsToken> tokens)
        {
            for (int i = 0; i < tokens.Count - 1; i++)
            {
                MathsToken current = tokens[i];
                MathsToken next = tokens[i + 1];
                if ((this.IsVariableOrNumber(current) && this.IsOpenParenthesis(next))
                    || (this.IsClosedParenthesis(current) && this.IsVariableOrNumber(next)))
                {
                    tokens.Insert(i + 1, MathsTokenCompletionHelper.Multiply);
                    i++;
                }
                else if(this.IsMinusSymbol(current) && this.IsOpenParenthesis(next))
                {
                    tokens[i] = new MathsToken("-1", new TokenSpan(0, 0), MathsTokenCategory.Number, MathsTokenType.Number);
                    tokens.Insert(i + 1, MathsTokenCompletionHelper.Multiply);
                    i += 2;
                }
            }
        }

        private bool IsOpenParenthesis(MathsToken token)
        {
            return token.Category == MathsTokenCategory.Parenthesis && token.Type == MathsTokenType.OpenParenthesis;
        }

        private bool IsClosedParenthesis(MathsToken token)
        {
            return token.Category == MathsTokenCategory.Parenthesis && token.Type == MathsTokenType.ClosingParenthesis;
        }

        private bool IsVariableOrNumber(MathsToken token)
        {
            return (token.Category == MathsTokenCategory.Number && token.Type == MathsTokenType.Number)
                   || (token.Category == MathsTokenCategory.Variable && token.Type == MathsTokenType.Variable);
        }

        private bool IsMinusSymbol(MathsToken token)
        {
            return token.Category == MathsTokenCategory.Symbol && token.Type == MathsTokenType.UnaryMinus;
        }

        private IEnumerable<MathsToken> GetUpdatedMathsTokens(IEnumerable<MathsToken> tokens)
        {
            int tokenCount = tokens.Count();
            int currentIndex = 0;

            List<MathsToken> output = new List<MathsToken>();
            for (int i = 0; i < tokenCount; i++)
            {
                MathsToken token = tokens.ElementAt(i);
                TokenSpan span = new TokenSpan(currentIndex, token.Value.Length);
                output.Add(new MathsToken(token.Value, span, token.Category, token.Type));
                currentIndex += token.Value.Length;
            }
            return output;
        }
    }
}
