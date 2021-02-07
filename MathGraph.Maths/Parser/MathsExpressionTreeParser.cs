using MathGraph.Maths.Parser.PostfixNotation;
using MathGraph.Maths.Parser.Expressions;
using System.Collections.Generic;
using MathGraph.Maths.Lexer;
using System;

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
            if (node is ChildrenMathsExpression expression && node.Type == ExpressionType.Operator)
            {
                for (int i = 0; i < expression.MinChildrenCount; i++)
                {
                    expression.AddFirstChild(expressionStack.Pop());
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
