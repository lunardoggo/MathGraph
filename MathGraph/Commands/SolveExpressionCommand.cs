using System.Collections.Generic;
using MathGraph.Maths.Parser;
using MathGraph.Maths.Lexer;
using MathGraph.Interfaces;
using System.Linq;
using System;
using MathGraph.Maths.Parser.PostfixNotation;

namespace MathGraph.Commands
{
    public class SolveExpressionCommand : MathsExpressionContainerCommand
    {
        public SolveExpressionCommand(IMathsExpressionContainer container) : base(container, true)
        { }

        public override void Execute(object parameter)
        {
            MathsTokenLexer lexer = new MathsTokenLexer();
            IEnumerable<MathsToken> tokens = lexer.LexMathematicalExpression(base.container.Expression);

            MathsPostfixParser parser = new MathsPostfixParser();
            IEnumerable<PostfixNotationElement> postfix = parser.ParseTokens(tokens);

            this.container.ErrorSinkEntries = lexer.ErrorSink.Entries.Concat(parser.ErrorSink.Entries);
            //base.container.PostfixNotation = String.Join(" ", postfix.Select(_token => _token.Value));
            //TODO: calculate result
        }
    }
}
