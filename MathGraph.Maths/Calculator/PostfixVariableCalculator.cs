using MathGraph.Maths.Parser.PostfixNotation;
using System.Collections.Generic;
using MathGraph.Maths.Errors;
using System.Linq;

namespace MathGraph.Maths.Calculator
{
    public class PostfixVariableCalculator : PostfixCalculator
    {
        public double Calculate(IEnumerable<PostfixNotationElement> elements, params VariableValue[] variableValues)
        {
            var duplicateValues = variableValues.GroupBy(_var => _var.VariableName)
                                                .Where(_grp => _grp.Count() > 1)
                                                .Select(_grp => _grp.Key);
            if(duplicateValues.Any())
            {
                foreach(string duplicate in duplicateValues)
                {
                    this.ErrorSink.AddError(Severety.Error, $"Duplicate variable value assignment for: \"{duplicate}\"", -1);
                }
                return double.NaN;
            }

            PostfixNotationElement[] elementArray = this.GetReplacedElements(elements, variableValues);
            var leftOverVariables = elementArray.OfType<VariableElement>();
            if (leftOverVariables.Any())
            {
                foreach(var variable in leftOverVariables)
                {
                    this.ErrorSink.AddError(Severety.Error, $"Missing variable value assignment for: \"{variable.Name}\"", -1);
                }
                return double.NaN;
            }

            return base.Calculate(elementArray);
        }

        private PostfixNotationElement[] GetReplacedElements(IEnumerable<PostfixNotationElement> elements, VariableValue[] values)
        {
            PostfixNotationElement[] output = elements.ToArray();
            for (int i = 0; i < output.Length; i++)
            {
                if (output[i] is VariableElement variable)
                {
                    VariableValue value = values.SingleOrDefault(_val => _val.VariableName.Equals(variable.Name));

                    if (value != null)
                    {
                        output[i] = new ValueElement(value.Value);
                    }
                }
            }
            return output;
        }
    }

    public class VariableValue
    {
        public VariableValue(string variableName, double value)
        {
            this.VariableName = variableName;
            this.Value = value;
        }

        public string VariableName { get; }
        public double Value { get; }
    }
}
