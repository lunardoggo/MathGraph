using MathGraph.Maths.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MathGraph.Tests
{
    public class TokenCompletionHelperTests
    {
        [Fact]
        public void TestCompleteMultiplication()
        {
            List<MathsToken> tokens = new List<MathsToken>
            {
                new MathsToken("8", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("(", new TokenSpan(1, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("3", new TokenSpan(2, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken(")", new TokenSpan(3, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis),
                new MathsToken("2", new TokenSpan(4, 1), MathsTokenCategory.Number, MathsTokenType.Number)
            };

            MathsToken[] completed = MathsTokenCompletionHelper.Instance.Complement(tokens).ToArray();

            Dictionary<int, MathsToken> inserts = new Dictionary<int, MathsToken>()
            {
                { 1,  new MathsToken("*", new TokenSpan(0, 0), MathsTokenCategory.Symbol, MathsTokenType.Multiply) },
                { 5,  new MathsToken("*", new TokenSpan(0, 0), MathsTokenCategory.Symbol, MathsTokenType.Multiply) }
            };
            IEnumerable<MathsToken> expected = this.GetCompletedTokens(tokens, inserts);

            this.AssertTokens(expected, completed);
        }

        [Fact]
        public void TestCompleteMultiplyWithMinusOne()
        {
            List<MathsToken> tokens = new List<MathsToken>
            {
                new MathsToken("-", new TokenSpan(0, 1), MathsTokenCategory.Symbol, MathsTokenType.UnaryMinus),
                new MathsToken("(", new TokenSpan(1, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("3", new TokenSpan(2, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(3, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("2", new TokenSpan(4, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken(")", new TokenSpan(5, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis)
            };

            MathsToken[] completed = MathsTokenCompletionHelper.Instance.Complement(tokens).ToArray();

            tokens.RemoveAt(0);
            Dictionary<int, MathsToken> inserts = new Dictionary<int, MathsToken>()
            {
                { 0,  new MathsToken("-1", new TokenSpan(0, 0), MathsTokenCategory.Number, MathsTokenType.Number) },
                { 1,  new MathsToken("*", new TokenSpan(0, 0), MathsTokenCategory.Symbol, MathsTokenType.Multiply) }
            };
            IEnumerable<MathsToken> expected = this.GetCompletedTokens(tokens, inserts);

            this.AssertTokens(expected, completed);
        }

        private IEnumerable<MathsToken> GetCompletedTokens(List<MathsToken> original, Dictionary<int, MathsToken> inserts)
        {
            List<MathsToken> tmpTokens = new List<MathsToken>(original);

            foreach (var insert in inserts)
            {
                tmpTokens.Insert(insert.Key, insert.Value);
            }

            int currentIndex = 0;

            List<MathsToken> output = new List<MathsToken>();

            foreach (MathsToken token in tmpTokens)
            {
                TokenSpan span = new TokenSpan(currentIndex, token.Value.Length);
                output.Add(new MathsToken(token.Value, span, token.Category, token.Type));
                currentIndex += token.Value.Length;
            }

            return output;
        }

        private void AssertTokens(IEnumerable<MathsToken> expected, IEnumerable<MathsToken> actual)
        {
            int expectedCount = expected.Count();
            Assert.Equal(expectedCount, actual.Count());
            for (int i = 0; i < expectedCount; i++)
            {
                MathsToken expectedToken = expected.ElementAt(i);
                MathsToken actualToken = actual.ElementAt(i);

                Assert.Equal(expectedToken.Value, actualToken.Value);
                Assert.Equal(expectedToken.Type, actualToken.Type);
                Assert.Equal(expectedToken.TokenSpan, actualToken.TokenSpan);
                Assert.Equal(expectedToken.Category, actualToken.Category);
            }
        }
    }
}
