using MathGraph.Maths.Errors;
using MathGraph.Maths.Lexer;
using System.Linq;
using Xunit;

namespace MathGraph.Tests
{
    public class MathsTokenLexerTests
    {
        [Fact]
        public void TestLexSimpleExpression()
        {
            string expression = "10 + (3 - 6)";

            MathsToken[] tokens = this.GetExpressionTokens(expression);
            Assert.Equal(7, tokens.Length);

            this.AssertToken("10", MathsTokenCategory.Number, MathsTokenType.Number, tokens[0]);
            this.AssertToken("+", MathsTokenCategory.Operator, MathsTokenType.Plus, tokens[1]);
            this.AssertToken("(", MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis, tokens[2]);
            this.AssertToken("3", MathsTokenCategory.Number, MathsTokenType.Number, tokens[3]);
            this.AssertToken("-", MathsTokenCategory.Operator, MathsTokenType.Minus, tokens[4]);
            this.AssertToken("6", MathsTokenCategory.Number, MathsTokenType.Number, tokens[5]);
            this.AssertToken(")", MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis, tokens[6]);
        }

        [Fact]
        public void TestLexExpressionWithFloatingPointNumbers()
        {
            string expression = "4.5 / 0.5 + 2";

            MathsToken[] tokens = this.GetExpressionTokens(expression);
            Assert.Equal(5, tokens.Length);

            this.AssertToken("4.5", MathsTokenCategory.Number, MathsTokenType.Number, tokens[0]);
            this.AssertToken("/", MathsTokenCategory.Operator, MathsTokenType.Divide, tokens[1]);
            this.AssertToken("0.5", MathsTokenCategory.Number, MathsTokenType.Number, tokens[2]);
            this.AssertToken("+", MathsTokenCategory.Operator, MathsTokenType.Plus, tokens[3]);
            this.AssertToken("2", MathsTokenCategory.Number, MathsTokenType.Number, tokens[4]);
        }

        [Fact]
        public void TestErrorSinkMultipleDecimalPoints()
        {
            MathsTokenLexer lexer = new MathsTokenLexer();
            string expression = "10.0.4.100";

            MathsToken[] tokens = lexer.LexMathematicalExpression(expression).ToArray();

            Assert.Single(tokens);
            this.AssertToken("10.04100", MathsTokenCategory.Number, MathsTokenType.Number, tokens.Single());

            Assert.Equal(2, lexer.ErrorSink.Entries.Count());
            Assert.True(lexer.ErrorSink.Entries.All(_entry => _entry.Severety == Severety.Warning));
        }

        [Fact]
        private void TestLexExpressionWithVariable()
        {
            string expression = "2 * x1 + 4.5";

            MathsToken[] tokens = this.GetExpressionTokens(expression);

            Assert.Equal(5, tokens.Length);

            this.AssertToken("2", MathsTokenCategory.Number, MathsTokenType.Number, tokens[0]);
            this.AssertToken("*", MathsTokenCategory.Operator, MathsTokenType.Multiply, tokens[1]);
            this.AssertToken("x1", MathsTokenCategory.Variable, MathsTokenType.Variable, tokens[2]);
            this.AssertToken("+", MathsTokenCategory.Operator, MathsTokenType.Plus, tokens[3]);
            this.AssertToken("4.5", MathsTokenCategory.Number, MathsTokenType.Number, tokens[4]);
        }

        private MathsToken[] GetExpressionTokens(string expression)
        {
            return new MathsTokenLexer().LexMathematicalExpression(expression).ToArray();
        }

        private void AssertToken(string assertedValue, MathsTokenCategory assertedCategory, MathsTokenType assertedType, MathsToken originalToken)
        {
            Assert.Equal(assertedCategory, originalToken.Category);
            Assert.Equal(assertedValue, originalToken.Value);
            Assert.Equal(assertedType, originalToken.Type);
        }
    }
}
