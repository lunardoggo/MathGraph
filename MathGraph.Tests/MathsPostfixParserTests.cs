using System.Collections.Generic;
using MathGraph.Maths.Parser;
using MathGraph.Maths.Errors;
using MathGraph.Maths.Lexer;
using System.Linq;
using Xunit;

namespace MathGraph.Tests
{
    public class MathsPostfixParserTests
    {
        [Fact]
        public void TestParseSimpleCalculation()
        {
            this.AssertOutcome(this.GetSimpleCalculationInPostfixNotation(), this.GetSimpleCalculationTokens());
        }

        [Fact]
        public void TestParseCalculationWithUnary()
        {
            this.AssertOutcome(this.GetCalculationWithUnaryInPostfixNotation(), this.GetCalculationWithUnary());
        }

        [Fact]
        public void TestParseCalculationWithPrecedenceAndUnary()
        {
            this.AssertOutcome(this.GetCalculationWithPrecedenceAndUnaryInPostfixNotation(), this.GetCalculationWithPrecedenceAndUnary());
        }

        [Fact]
        public void TestParseWithWarnings()
        {
            MathsToken[] tokens = this.GetWarningTokens();

            MathsPostfixParser parser = new MathsPostfixParser();
            Queue<MathsToken> postfix = parser.ParseTokens(tokens);
            MathsToken[] expected = this.GetWarningPostfixNotation();

            this.AssertSameContent(expected, postfix);

            Assert.Single(parser.ErrorSink.Entries);
            this.AssertWarning(parser.ErrorSink, Severety.Error, "Unexpected \"-\" before closing parenthesis");
        }

        private void AssertWarning(ErrorSink errorSink, Severety severety, string message)
        {
            Assert.Contains(errorSink.Entries, _entry => _entry.Severety == severety && _entry.Message.Equals(message));
        }

        private void AssertOutcome(MathsToken[] expectedResult, MathsToken[] input)
        {
            Queue<MathsToken> postfix = new MathsPostfixParser().ParseTokens(input);
            this.AssertSameContent(expectedResult, postfix);
        }

        private void AssertSameContent(MathsToken[] expected, Queue<MathsToken> actual)
        {
            Assert.Equal(expected.Length, actual.Count);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual.Dequeue());
            }
        }

        private MathsToken[] GetSimpleCalculationTokens()
        {
            //same as: 4+2.5*4-5
            return new MathsToken[]
            {
                new MathsToken("4", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(1, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("2.5", new TokenSpan(2, 3), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("*", new TokenSpan(5, 1), MathsTokenCategory.Symbol, MathsTokenType.Multiply),
                new MathsToken("4", new TokenSpan(6, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-", new TokenSpan(7, 1), MathsTokenCategory.Symbol, MathsTokenType.Minus),
                new MathsToken("5", new TokenSpan(8, 1), MathsTokenCategory.Number, MathsTokenType.Number)
            };
        }

        private MathsToken[] GetSimpleCalculationInPostfixNotation()
        {
            //result: 4 2.5 4 * + 5 -
            return new MathsToken[]
            {
                new MathsToken("4", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("2.5", new TokenSpan(2, 3), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("4", new TokenSpan(6, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("*", new TokenSpan(5, 1), MathsTokenCategory.Symbol, MathsTokenType.Multiply),
                new MathsToken("+", new TokenSpan(1, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("5", new TokenSpan(8, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-", new TokenSpan(7, 1), MathsTokenCategory.Symbol, MathsTokenType.Minus)
            };
        }

        private MathsToken[] GetCalculationWithUnary()
        {
            //same as: 4*(-(2+x))+(-1)
            return new MathsToken[]
            {
                new MathsToken("4", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("*", new TokenSpan(1, 1), MathsTokenCategory.Symbol, MathsTokenType.Multiply),
                new MathsToken("(", new TokenSpan(2, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("-", new TokenSpan(3, 1), MathsTokenCategory.Unary, MathsTokenType.UnaryMinus),
                new MathsToken("(", new TokenSpan(4, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("2", new TokenSpan(5, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(6, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("x", new TokenSpan(7, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken(")", new TokenSpan(8, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis),
                new MathsToken(")", new TokenSpan(9, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis),
                new MathsToken("+", new TokenSpan(10, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("(", new TokenSpan(11, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("-", new TokenSpan(12, 1), MathsTokenCategory.Unary, MathsTokenType.UnaryMinus),
                new MathsToken("1", new TokenSpan(13, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken(")", new TokenSpan(14, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis)
            };
        }

        private MathsToken[] GetCalculationWithUnaryInPostfixNotation()
        {
            //result: 4 -2 -x + * -1 +
            return new MathsToken[]
            {
                new MathsToken("4", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-2", new TokenSpan(4, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-x", new TokenSpan(6, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(6, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("*", new TokenSpan(1, 1), MathsTokenCategory.Symbol, MathsTokenType.Multiply),
                new MathsToken("-1", new TokenSpan(12, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(10, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus)
            };
        }

        private MathsToken[] GetWarningTokens()
        {
            // (2 + 1 -)
            return new MathsToken[]
            {
                new MathsToken("(", new TokenSpan(0, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("2", new TokenSpan(1, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(2, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("1", new TokenSpan(3, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-", new TokenSpan(4, 1), MathsTokenCategory.Symbol, MathsTokenType.Minus),
                new MathsToken(")", new TokenSpan(5, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis)
            };
        }

        private MathsToken[] GetWarningPostfixNotation()
        {
            //2 1 + -
            //Eventhough the minus is before the closing parenthesis, add it to the output
            return new MathsToken[]
            {
                new MathsToken("2", new TokenSpan(1, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("1", new TokenSpan(3, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(2, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("-", new TokenSpan(4, 1), MathsTokenCategory.Symbol, MathsTokenType.Minus)
            };
        }

        private MathsToken[] GetCalculationWithPrecedenceAndUnary()
        {
            //same as: -7 / 14 + 3 - 0.5
            return new MathsToken[]
            {
                new MathsToken("-", new TokenSpan(0, 1), MathsTokenCategory.Unary, MathsTokenType.UnaryMinus),
                new MathsToken("7", new TokenSpan(1, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("/", new TokenSpan(2, 1), MathsTokenCategory.Symbol, MathsTokenType.Divide),
                new MathsToken("14", new TokenSpan(3, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(4, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("3", new TokenSpan(5, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-", new TokenSpan(6, 1), MathsTokenCategory.Symbol, MathsTokenType.Minus),
                new MathsToken("0.5", new TokenSpan(7, 3), MathsTokenCategory.Number, MathsTokenType.Number)
            };
        }

        private MathsToken[] GetCalculationWithPrecedenceAndUnaryInPostfixNotation()
        {
            //result: -7 14 / 3 + 0.5 -
            return new MathsToken[]
            {
                new MathsToken("-7", new TokenSpan(0, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("14", new TokenSpan(3, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("/", new TokenSpan(2, 1), MathsTokenCategory.Symbol, MathsTokenType.Divide),
                new MathsToken("3", new TokenSpan(5, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(4, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("0.5", new TokenSpan(7, 3), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("-", new TokenSpan(6, 1), MathsTokenCategory.Symbol, MathsTokenType.Minus)
            };
        }
    }
}
