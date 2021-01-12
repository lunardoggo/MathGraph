using MathGraph.Maths.Lexer;
using MathGraph.Maths.Parser.Expressions;
using MathGraph.Maths.Parser.PostfixNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathGraph.Maths.Parser
{
    public class MathsExpressionTreeParser
    {
        public MathsExpression Parse(IEnumerable<MathsToken> tokens)
        {
            MathsPostfixParser postfixParser = new MathsPostfixParser();
            IEnumerable<PostfixNotationElement> elements = postfixParser.Parse(tokens);
            Queue<PostfixNotationElement> postfix = new Queue<PostfixNotationElement>(elements);
            Stack<MathsExpression> expression = new Stack<MathsExpression>();

            this.ParseRecursive(postfix, expression);

            return expression.Pop();
        }

        private void ParseRecursive(Queue<PostfixNotationElement> postfix, Stack<MathsExpression> expressionStack)
        {
            PostfixNotationElement nodeElement = postfix.Dequeue();
            MathsExpression node = this.GetExpressionFromPostfixElement(nodeElement);
            if (node.Type == ExpressionType.OperatorExpression)
            {
                for (int i = 0; i < node.MaxChildrenCount; i++)
                {
                    node.AddChild(expressionStack.Pop());
                }
            }
            expressionStack.Push(node);

            if (postfix.Count > 0)
            {
                this.ParseRecursive(postfix, expressionStack);
            }
        }

        private MathsExpression GetExpressionFromPostfixElement(PostfixNotationElement element)
        {
            if (element is OperatorElement operatorElement)
            {
                return new OperatorExpression(operatorElement.Type);
            }
            else if (element is VariableElement variableElement)
            {
                return new VariableExpression(variableElement.Name, variableElement.IsNegative);
            }
            else if (element is ValueElement valueElement)
            {
                return new ConstantExpression(valueElement.Value);
            }
            throw new NotImplementedException($"Unknown postfix notation element type {nameof(element)}");
        }
    }
}
