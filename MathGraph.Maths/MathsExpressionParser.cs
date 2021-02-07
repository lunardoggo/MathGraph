using MathGraph.Maths.Parser.Expressions;
using System.Collections.Generic;
using MathGraph.Maths.Parser;
using MathGraph.Maths.Lexer;

namespace MathGraph.Maths
{
    public class MathsExpressionParser
    {
        private readonly MathsExpressionTreeParser parser = new MathsExpressionTreeParser();
        private readonly MathsTokenLexer lexer = new MathsTokenLexer();

        public MathsExpression Parse(string expressionString)
        {
            IEnumerable<MathsToken> tokens = lexer.LexMathematicalExpression(expressionString);
            return parser.Parse(tokens);
        }
    }
}
