using MathGraph.Maths.Calculator;
using MathGraph.Maths.Errors;
using MathGraph.Maths.Graphs;
using MathGraph.Maths.Lexer;
using MathGraph.Maths.Parser;
using MathGraph.Maths.Parser.PostfixNotation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MathGraph.Maths.Functions
{
    public class LinearFunction : IFunction
    {
        private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("en-US"); //TODO: move to more central place
        private const string VariableName = "x";

        private readonly IEnumerable<PostfixNotationElement> equation;
        private readonly PostfixVariableCalculator calculator;

        public LinearFunction(double slope, double yAxisIntercept)
        {
            this.equation = this.GetEquation(slope, yAxisIntercept);
            this.calculator = new PostfixVariableCalculator();
        }

        private IEnumerable<PostfixNotationElement> GetEquation(double slope, double yAxisIntercept)
        {
            MathsTokenLexer lexer = new MathsTokenLexer();
            IEnumerable<MathsToken> tokens = lexer.LexMathematicalExpression($"{slope.ToString(LinearFunction.Culture)} * {LinearFunction.VariableName} + {yAxisIntercept.ToString(LinearFunction.Culture)}");
            this.AddErrors(lexer.ErrorSink);

            MathsPostfixParser parser = new MathsPostfixParser();
            IEnumerable<PostfixNotationElement> output = parser.ParseTokens(tokens);
            this.AddErrors(parser.ErrorSink);

            return output;
        }

        private void AddErrors(ErrorSink source)
        {
            foreach(ErrorSinkEntry entry in source.Entries)
            {
                this.ErrorSink.AddError(entry);
            }
        }

        public double GetY(double x)
        {
            return this.calculator.Calculate(this.equation, new VariableValue(LinearFunction.VariableName, x));
        }

        public ErrorSink ErrorSink { get; }
    }
}
