using MathGraph.Maths.Parser.Expressions;
using System.Linq;
using System;

namespace MathGraph.Maths.Calculator
{
    public class MathsExpressionTreeCalculator
    {
        public decimal Evaluate(MathsExpression expression)
        {
            if(expression is ConstantExpression constant)
            {
                return constant.Value;
            }
            else if(expression is OperatorExpression @operator)
            {
                decimal firstChild = this.Evaluate(@operator.Children.ElementAt(0));
                decimal secondChild = this.Evaluate(@operator.Children.ElementAt(1));

                return this.Evaluate(@operator.Operator, firstChild, secondChild);
            }
            else
            {
                throw new InvalidOperationException($"Can't evaluate expression nodes of type {expression.Type}");
            }
        }

        private decimal Evaluate(OperationType operation, decimal firstChild, decimal secondChild)
        {
            switch(operation)
            {
                case OperationType.Addition:
                    return secondChild + firstChild;
                case OperationType.Subtraction:
                    return secondChild - firstChild;
                case OperationType.Multiplication:
                    return secondChild * firstChild;
                case OperationType.Division:
                    return secondChild / firstChild;
                default:
                    throw new NotImplementedException($"Operation {operation} isn't implemented for evaluation");
            }
        }
    }
}
