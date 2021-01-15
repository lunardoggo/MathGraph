using MathGraph.Maths.Parser.Expressions;
using MathGraph.Maths.Lexer;
using System.Diagnostics;
using System;

namespace MathGraph.Maths.Parser.PostfixNotation
{
    [DebuggerDisplay("OperatorElement \\{ Operation = {Type} \\}")]
    public class OperatorElement : PostfixNotationElement
    {
        internal OperatorElement(MathsTokenType type)
        {
            this.Type = (OperationType)type;
        }

        public OperationType Type { get; }

        public override string StringValue
        {
            get
            {
                switch(this.Type)
                {
                    case OperationType.Addition: return "+";
                    case OperationType.Division: return "/";
                    case OperationType.Subtraction: return "-";
                    case OperationType.Multiplication: return "*";
                    case OperationType.Power: return "^";
                    default: throw new NotImplementedException();
                }
            }
        }

        public override bool Equals(PostfixNotationElement other)
        {
            return other is OperatorElement @operator && @operator.Type == this.Type;
        }

        public decimal Operate(ValueElement firstOperand, ValueElement secondOperand)
        {
            switch (this.Type)
            {
                case OperationType.Addition: return firstOperand.Value + secondOperand.Value;
                case OperationType.Division: return firstOperand.Value / secondOperand.Value;
                case OperationType.Subtraction: return firstOperand.Value - secondOperand.Value;
                case OperationType.Multiplication: return firstOperand.Value * secondOperand.Value;
                case OperationType.Power: return this.Pow(firstOperand.Value, secondOperand.Value);
                default: throw new NotImplementedException($"Operation \"{this.Type}\" is not implemented.");
            }
        }

        private decimal Pow(decimal @base, decimal exponent)
        {
            decimal output = @base;
            if(exponent == 0)
            {
                return 1.0m;
            }
            if(exponent % 1 != 0)
            {
                throw new InvalidOperationException("Exponents must be whole numbers!");
            }
            int power = (int)Math.Abs(exponent);

            //Math-class doesn't provide Pow method for decimal, iterative solution as quick and dirty hack
            //(maybe there is something better out there)
            for(int i = 1; i < power; i++)
            {
                output *= @base;
            }

            return power == exponent ? output : 1.0m / output;
        }
    }
}
