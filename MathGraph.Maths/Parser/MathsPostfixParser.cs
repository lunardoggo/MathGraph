using MathGraph.Maths.Parser.PostfixNotation;
using System.Collections.Generic;
using MathGraph.Maths.Errors;
using MathGraph.Maths.Lexer;
using System.Linq;
using System;

namespace MathGraph.Maths.Parser
{
    public class MathsPostfixParser
    {
        public IEnumerable<PostfixNotationElement> Parse(IEnumerable<MathsToken> tokens)
        {
            IEnumerator<MathsToken> enumerator = tokens.GetEnumerator();
            Stack<MathsToken> operators = new Stack<MathsToken>();
            Queue<MathsToken> output = new Queue<MathsToken>();

            int negateOngoingLevel = 0;
            MathsToken previous = null;
            bool lastWasUnary = false;
            while (enumerator.MoveNext())
            {
                MathsToken current = enumerator.Current;
                if (current.Category == MathsTokenCategory.Number || current.Category == MathsTokenCategory.Variable)
                {
                    output.Enqueue(this.GetMathsTokenWithSign(current, lastWasUnary, negateOngoingLevel));
                    lastWasUnary = negateOngoingLevel > 0;
                }
                else if(current.Category == MathsTokenCategory.Unary)
                {
                    lastWasUnary = true;
                }
                else if (current.Category == MathsTokenCategory.Symbol)
                {
                    //TODO: Support for power-operator (is right-assiciative -> look up behaviour)
                    while (operators.Count > 0
                           && this.ComparedOperatorHasGreaterOrSamePrecedence(current, operators.Peek())
                           && operators.Peek().Type != MathsTokenType.OpenParenthesis)
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(current);
                }
                else if (current.Type == MathsTokenType.OpenParenthesis)
                {
                    operators.Push(current);
                    if(lastWasUnary)
                    {
                        lastWasUnary = false;
                        negateOngoingLevel++;
                    }
                }
                else if (current.Type == MathsTokenType.ClosingParenthesis)
                {
                    while (operators.Peek().Type != MathsTokenType.OpenParenthesis)
                    {
                        output.Enqueue(operators.Pop());
                    }
                    if (operators.Count > 0 && operators.Peek().Type == MathsTokenType.OpenParenthesis)
                    {
                        operators.Pop(); //Discard open parenthesis
                    }
                    if(lastWasUnary || previous.Category == MathsTokenCategory.Symbol)
                    {
                        this.ErrorSink.AddError(Severety.Error, $"Unexpected \"{previous.Value}\" before closing parenthesis", previous.TokenSpan.StartIndex);
                        lastWasUnary = false;
                    }
                    if (negateOngoingLevel > 0)
                    {
                        negateOngoingLevel--;
                    }
                }
                previous = current;
            }

            while (operators.Count > 0)
            {
                output.Enqueue(operators.Pop());
            }
            return output.Select(_token => PostfixNotationElementFactory.Instance.Produce(_token));
        }

        private bool ComparedOperatorHasGreaterOrSamePrecedence(MathsToken current, MathsToken comparedToken)
        {
            return this.GetOperatorPrecedence(current) <= this.GetOperatorPrecedence(comparedToken);
        }

        private int GetOperatorPrecedence(MathsToken token)
        {
            if (token.Category == MathsTokenCategory.Symbol)
            {
                switch (token.Type)
                {
                    case MathsTokenType.Minus:
                    case MathsTokenType.Plus:
                        return 0;
                    case MathsTokenType.Multiply:
                    case MathsTokenType.Divide:
                        return 1;
                    default:
                        throw new NotImplementedException($"Precendence of operator \"{token.Value}\" (type: {token.Type}) is not implemented.");
                }
            }
            return -1;
        }

        private MathsToken GetMathsTokenWithSign(MathsToken token, bool lastWasUnary, int negateOngoingLevel)
        {
            TokenSpan tokenSpan = token.TokenSpan;
            string value = token.Value;
            if(lastWasUnary || negateOngoingLevel > 0)
            {
                tokenSpan = new TokenSpan(token.TokenSpan.StartIndex - 1, token.TokenSpan.Length + 1);
                value = "-" + token.Value;
            }
            return new MathsToken(value, tokenSpan, token.Category, token.Type);

        }

        public ErrorSink ErrorSink { get; } = new ErrorSink();
    }
}
