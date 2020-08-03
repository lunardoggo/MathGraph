using MathGraph.Maths.Parser.Expressions;
using MathGraph.Maths.Lexer;
using System;

namespace MathGraph.Maths.Parser.PostfixNotation
{
    public class OperatorElement : PostfixNotationElement
    {
        internal OperatorElement(MathsTokenType type)
        {
            this.Type = (OperationType)type;
        }

        public OperationType Type { get; }

        public override bool Equals(PostfixNotationElement other)
        {
            return other is OperatorElement @operator && @operator.Type == this.Type;
        }

        public double Operate(ValueElement firstOperand, ValueElement secondOperand)
        {
            switch(this.Type)
            {
                case OperationType.Addition: return firstOperand.Value + secondOperand.Value;
                case OperationType.Division: return firstOperand.Value / secondOperand.Value;
                case OperationType.Subtraction: return firstOperand.Value - secondOperand.Value;
                case OperationType.Multiplication: return firstOperand.Value * secondOperand.Value;
                default: throw new NotImplementedException($"Operation \"{this.Type}\" is not implemented.");
            }
        }
    }
}
