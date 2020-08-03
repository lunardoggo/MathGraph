using MathGraph.Maths.Parser.PostfixNotation;
using MathGraph.Maths.Calculator;
using MathGraph.Maths.Lexer;
using Xunit;

namespace MathGraph.Tests
{
    public class PostfixCalculatorTests
    {
        [Fact]
        public void TestCalculateSimpleCalculation()
        {
            PostfixNotationElement[] elements = this.GetSimpleCalculation();
            double result = new PostfixCalculator().Calculate(elements);

            Assert.Equal(9.0d, result);
        }

        [Fact]
        public void TestCalculateMoreComplexCalculation()
        {
            PostfixNotationElement[] elements = this.GetMoreComplexCalculation();
            double result = new PostfixCalculator().Calculate(elements);

            Assert.Equal(2.0d, result);
        }

        private PostfixNotationElement[] GetSimpleCalculation()
        {
            //result: 4 2.5 4 * + 5 - = 9
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

        private PostfixNotationElement[] GetMoreComplexCalculation()
        {
            //result: -7 14 / 3 + 0.5 - = 2
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
