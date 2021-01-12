using MathGraph.Maths.Parser.PostfixNotation;
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
            PostfixNotationElement[] postfix = parser.Parse(tokens).ToArray();
            PostfixNotationElement[] expected = this.GetWarningPostfixNotation();

            this.AssertSameContent(expected, postfix);

            Assert.Single(parser.ErrorSink.Entries);
            this.AssertWarning(parser.ErrorSink, Severety.Error, "Unexpected \"-\" before closing parenthesis");
        }

        [Fact]
        public void TestParseOrderOfOperations()
        {
            MathsToken[] tokens = new MathsToken[]
            {
                new MathsToken("5", new TokenSpan(0, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("+", new TokenSpan(1, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("10", new TokenSpan(2, 2), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken("/", new TokenSpan(4, 1), MathsTokenCategory.Symbol, MathsTokenType.Divide),
                new MathsToken("2", new TokenSpan(5, 1), MathsTokenCategory.Number, MathsTokenType.Number),
            };

            PostfixNotationElement[] elements = new PostfixNotationElement[]
            {
                new ValueElement(5),
                new ValueElement(10),
                new ValueElement(2),
                new OperatorElement(MathsTokenType.Divide),
                new OperatorElement(MathsTokenType.Plus)
            };

            this.AssertOutcome(elements, tokens);
        }

        private void AssertWarning(ErrorSink errorSink, Severety severety, string message)
        {
            Assert.Contains(errorSink.Entries, _entry => _entry.Severety == severety && _entry.Message.Equals(message));
        }

        private void AssertOutcome(PostfixNotationElement[] expectedResult, MathsToken[] input)
        {
            PostfixNotationElement[] postfix = new MathsPostfixParser().Parse(input).ToArray();
            this.AssertSameContent(expectedResult, postfix);
        }

        private void AssertSameContent(PostfixNotationElement[] expected, PostfixNotationElement[] actual)
        {
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
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

        private PostfixNotationElement[] GetSimpleCalculationInPostfixNotation()
        {
            //result: 4 2.5 4 * + 5 -
            return new PostfixNotationElement[]
            {
                new ValueElement(4.0d),
                new ValueElement(2.5d),
                new ValueElement(4.0d),
                new OperatorElement(MathsTokenType.Multiply),
                new OperatorElement(MathsTokenType.Plus),
                new ValueElement(5.0d),
                new OperatorElement(MathsTokenType.Minus)
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
                new MathsToken("x", new TokenSpan(7, 1), MathsTokenCategory.Variable, MathsTokenType.Variable),
                new MathsToken(")", new TokenSpan(8, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis),
                new MathsToken(")", new TokenSpan(9, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis),
                new MathsToken("+", new TokenSpan(10, 1), MathsTokenCategory.Symbol, MathsTokenType.Plus),
                new MathsToken("(", new TokenSpan(11, 1), MathsTokenCategory.Parenthesis, MathsTokenType.OpenParenthesis),
                new MathsToken("-", new TokenSpan(12, 1), MathsTokenCategory.Unary, MathsTokenType.UnaryMinus),
                new MathsToken("1", new TokenSpan(13, 1), MathsTokenCategory.Number, MathsTokenType.Number),
                new MathsToken(")", new TokenSpan(14, 1), MathsTokenCategory.Parenthesis, MathsTokenType.ClosingParenthesis)
            };
        }

        private PostfixNotationElement[] GetCalculationWithUnaryInPostfixNotation()
        {
            //result: 4 -2 -x + * -1 +
            return new PostfixNotationElement[]
            {
                new ValueElement(4.0d),
                new ValueElement(-2.0d),
                new VariableElement(true, "x"),
                new OperatorElement(MathsTokenType.Plus),
                new OperatorElement(MathsTokenType.Multiply),
                new ValueElement(-1.0d),
                new OperatorElement(MathsTokenType.Plus)
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

        private PostfixNotationElement[] GetWarningPostfixNotation()
        {
            //2 1 + -
            //Eventhough the minus is before the closing parenthesis, add it to the output
            return new PostfixNotationElement[]
            {
                new ValueElement(2.0d),
                new ValueElement(1.0d),
                new OperatorElement(MathsTokenType.Plus),
                new OperatorElement(MathsTokenType.Minus)
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

        private PostfixNotationElement[] GetCalculationWithPrecedenceAndUnaryInPostfixNotation()
        {
            //result: -7 14 / 3 + 0.5 -
            return new PostfixNotationElement[]
            {
                new ValueElement(-7.0d),
                new ValueElement(14.0d),
                new OperatorElement(MathsTokenType.Divide),
                new ValueElement(3.0d),
                new OperatorElement(MathsTokenType.Plus),
                new ValueElement(0.5d),
                new OperatorElement(MathsTokenType.Minus)
            };
        }
    }
}
