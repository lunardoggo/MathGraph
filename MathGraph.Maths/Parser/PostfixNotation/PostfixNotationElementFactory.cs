using MathGraph.Maths.Lexer;
using System.Globalization;
using System;

namespace MathGraph.Maths.Parser.PostfixNotation
{
    public class PostfixNotationElementFactory
    {
        public static PostfixNotationElementFactory Instance { get; } = new PostfixNotationElementFactory();

        private readonly CultureInfo culture;

        private PostfixNotationElementFactory()
        {
            this.culture = CultureInfo.GetCultureInfo("en-US"); //TODO: move to more central place
        }

        public PostfixNotationElement Produce(MathsToken token)
        {
            switch(token.Category)
            {
                case MathsTokenCategory.Number:
                    return this.ProduceValueElement(token);
                case MathsTokenCategory.Symbol:
                    return this.ProduceOperatorElement(token);
                case MathsTokenCategory.Variable:
                    return this.ProduceVariable(token);
                default:
                    throw new NotImplementedException($"Can't produce a postfix notation element of category \"{token.Category}\"");
            }
        }

        private PostfixNotationElement ProduceValueElement(MathsToken token)
        {
            return new ValueElement(Decimal.Parse(token.Value, this.culture));
        }

        private PostfixNotationElement ProduceOperatorElement(MathsToken token)
        {
            return new OperatorElement(token.Type);
        }

        private PostfixNotationElement ProduceVariable(MathsToken token)
        {
            return new VariableElement(token.Value.StartsWith("-"), token.Value.TrimStart('-'));
        }
    }
}
