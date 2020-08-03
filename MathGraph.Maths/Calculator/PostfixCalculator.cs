using MathGraph.Maths.Parser.PostfixNotation;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using MathGraph.Maths.Errors;
using System.Linq;
using System;

[assembly:InternalsVisibleTo("MathGraph.Tests")]
namespace MathGraph.Maths.Calculator
{
    public class PostfixCalculator
    {
        public ErrorSink ErrorSink { get; }

        public PostfixCalculator()
        {
            this.ErrorSink = new ErrorSink();
        }

        public double Calculate(IEnumerable<PostfixNotationElement> elements)
        {
            IEnumerator<PostfixNotationElement> enumerator = elements.GetEnumerator();
            Stack<ValueElement> operands = new Stack<ValueElement>();

            while(enumerator.MoveNext())
            {
                if (enumerator.Current is ValueElement value)
                {
                    operands.Push(value);
                }
                else if (enumerator.Current is OperatorElement @operator)
                {
                    if (operands.Count < 2)
                    {
                        this.ErrorSink.AddError(Severety.Error, "An operator must have two values to operate on", -1);
                        return 0.0d;
                    }

                    //First operand popped of must be the right operand in order to calculate divisions and subtractions correctly
                    ValueElement second = operands.Pop(); 
                    ValueElement first = operands.Pop();

                    try
                    {
                        double result = @operator.Operate(first, second);
                        operands.Push(new ValueElement(result));
                    }
                    catch (Exception ex)
                    {
                        this.ErrorSink.AddError(Severety.Error, $"Could not operate on values \"{first.Value}\" and \"{second.Value}\" with operation type \"{@operator.Type}\": {ex.Message}", -1);
                        return 0.0d;
                    }
                }
                else
                {
                    this.ErrorSink.AddError(Severety.Error, $"This calculator doesn't support elements of type \"{enumerator.Current.GetType()}\"", -1);
                }
            }

            if(operands.Count > 1)
            {
                this.ErrorSink.AddError(Severety.Error, $"Could not operate on {operands.Count} operands because of missing operators", -1);
                return 0.0d;
            }

            return operands.Single().Value;
        }
    }
}
