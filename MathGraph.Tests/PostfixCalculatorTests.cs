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
            decimal result = new PostfixCalculator().Calculate(elements);

            Assert.Equal(9.0m, result);
        }

        [Fact]
        public void TestCalculateMoreComplexCalculation()
        {
            PostfixNotationElement[] elements = this.GetMoreComplexCalculation();
            decimal result = new PostfixCalculator().Calculate(elements);

            Assert.Equal(2.0m, result);
        }

        [Fact]
        public void TestCalculateWithPower()
        {
            PostfixNotationElement[] elements = new PostfixNotationElement[]
            {
                new ValueElement(4),
                new ValueElement(8),
                new ValueElement(3),
                new OperatorElement(MathsTokenType.Power),
                new OperatorElement(MathsTokenType.Plus)
            };
            decimal result = new PostfixCalculator().Calculate(elements);

            Assert.Equal(516.0m, result);
        }

        [Fact]
        public void TestCalculateWithNegativePower()
        {
            PostfixNotationElement[] elements = new PostfixNotationElement[]
            {
                new ValueElement(4),
                new ValueElement(8),
                new ValueElement(-1),
                new OperatorElement(MathsTokenType.Power),
                new OperatorElement(MathsTokenType.Plus)
            };
            decimal result = new PostfixCalculator().Calculate(elements);

            Assert.Equal(4.125m, result);
        }

        private PostfixNotationElement[] GetSimpleCalculation()
        {
            //result: 4 2.5 4 * + 5 - = 9
            return new PostfixNotationElement[]
            {
                new ValueElement(4.0m),
                new ValueElement(2.5m),
                new ValueElement(4.0m),
                new OperatorElement(MathsTokenType.Multiply),
                new OperatorElement(MathsTokenType.Plus),
                new ValueElement(5.0m),
                new OperatorElement(MathsTokenType.Minus)
            };
        }

        private PostfixNotationElement[] GetMoreComplexCalculation()
        {
            //result: -7 14 / 3 + 0.5 - = 2
            return new PostfixNotationElement[]
            {
                new ValueElement(-7.0m),
                new ValueElement(14.0m),
                new OperatorElement(MathsTokenType.Divide),
                new ValueElement(3.0m),
                new OperatorElement(MathsTokenType.Plus),
                new ValueElement(0.5m),
                new OperatorElement(MathsTokenType.Minus)
            };
        }
    }
}
