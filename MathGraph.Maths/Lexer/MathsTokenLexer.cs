using System.Collections.Generic;
using MathGraph.Maths.Errors;
using System.Linq;
using System.Text;
using System;

namespace MathGraph.Maths.Lexer
{
    public class MathsTokenLexer
    {
        private readonly char decimalSeparator = '.';
        
        public ErrorSink ErrorSink { get; } = new ErrorSink();

        public IEnumerable<MathsToken> LexMathematicalExpression(string expression)
        {
            expression = expression.Replace(" ", "");
            List<MathsToken> tokens = new List<MathsToken>();
            int currentIndex = 0;

            MathsToken previousToken = null;
            while (currentIndex < expression.Length)
            {
                MathsToken token = this.GetNextToken(expression, currentIndex, previousToken);
                tokens.Add(token);

                currentIndex += token.TokenSpan.Length;
                previousToken = token;
            }
            return tokens.ToArray();
        }

        private MathsToken GetNextToken(string expression, int currentIndex, MathsToken previousToken)
        {
            char currentChar = expression[currentIndex];
            MathsTokenCategory category = this.GetCharTokenCategory(currentChar);
            if (category == MathsTokenCategory.Number)
            {
                return this.GetNumberMathsToken(expression, currentIndex);
            }
            else if (category == MathsTokenCategory.Symbol)
            {
                return this.GetSymbolOrUnaryToken(currentChar, currentIndex, previousToken);
            }
            else if (category == MathsTokenCategory.Parenthesis)
            {
                MathsTokenType parenthesisType = this.GetParenthesisTokenType(currentChar);
                return new MathsToken(currentChar.ToString(), new TokenSpan(currentIndex, 1), MathsTokenCategory.Parenthesis, parenthesisType);
            }
            else if(category == MathsTokenCategory.Variable)
            {
                return this.GetVariableMathsToken(expression, currentIndex);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private MathsToken GetSymbolOrUnaryToken(char currentChar, int currentIndex, MathsToken previousToken)
        {
            MathsTokenType operatorType = this.GetOperatorTokenType(currentChar);
            MathsTokenCategory category = MathsTokenCategory.Symbol;
            if(operatorType == MathsTokenType.Minus
               && (previousToken == null || previousToken.Category == MathsTokenCategory.Symbol || previousToken.Type == MathsTokenType.OpenParenthesis))
            {
                operatorType = MathsTokenType.UnaryMinus;
                category = MathsTokenCategory.Unary;
            }
            return new MathsToken(currentChar.ToString(), new TokenSpan(currentIndex, 1), category, operatorType);
        }

        private MathsTokenCategory GetCharTokenCategory(char @char)
        {
            const string parentheses = "()[]{}";
            const string operators = "+-*/";

            if (Char.IsDigit(@char) || @char == this.decimalSeparator)
            {
                return MathsTokenCategory.Number;
            }
            else if (operators.Contains(@char))
            {
                return MathsTokenCategory.Symbol;
            }
            else if (parentheses.Contains(@char))
            {
                return MathsTokenCategory.Parenthesis;
            }
            else if(Char.IsLetter(@char))
            {
                return MathsTokenCategory.Variable;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private MathsTokenType GetOperatorTokenType(char @char)
        {
            switch (@char)
            {
                case '*': return MathsTokenType.Multiply;
                case '/': return MathsTokenType.Divide;
                case '-': return MathsTokenType.Minus;
                case '+': return MathsTokenType.Plus;
                default: throw new NotImplementedException();
            }
        }

        private MathsTokenType GetParenthesisTokenType(char @char)
        {
            switch (@char)
            {
                case '(':
                case '[':
                case '{':
                    return MathsTokenType.OpenParenthesis;
                case ')':
                case ']':
                case '}':
                    return MathsTokenType.ClosingParenthesis;
                default: throw new NotImplementedException();
            }
        }

        private MathsToken GetNumberMathsToken(string expression, int currentIndex)
        {
            StringBuilder builder = new StringBuilder();

            int index = currentIndex;
            while (index < expression.Length && (Char.IsDigit(expression[index]) || expression[index] == this.decimalSeparator))
            {
                builder.Append(expression[index]);
                index++;
            }

            string value = builder.ToString();

            if(value.Count(_char => _char == this.decimalSeparator) > 1)
            {
                this.AddDecimalPointErrors(value);
                value = this.GetValueWithoutLastDecimalPoints(value);
            }

            return new MathsToken(value, new TokenSpan(currentIndex, builder.Length), MathsTokenCategory.Number, MathsTokenType.Number);
        }

        private void AddDecimalPointErrors(string value)
        {
            int firstIndex = value.IndexOf(this.decimalSeparator);
            var errors = value.Select((_char, _index) => new KeyValuePair<char, int>(_char, _index))
                              .Where(_pair => _pair.Key == this.decimalSeparator && _pair.Value > firstIndex);
            foreach (var error in errors)
            {
                this.ErrorSink.AddError(Severety.Warning, "Duplicate decimal separator. It will be removed from the parsing result.", error.Value);
            }
        }

        private string GetValueWithoutLastDecimalPoints(string value)
        {
            string separator = this.decimalSeparator.ToString();
            string[] parts = value.Split(new char[] { this.decimalSeparator }, 2);
            return String.Join(separator, parts[0], parts[1].Replace(separator, ""));
        }

        private MathsToken GetVariableMathsToken(string expression, int currentIndex)
        {
            StringBuilder builder = new StringBuilder();

            int index = currentIndex;
            while(index < expression.Length && Char.IsLetterOrDigit(expression[index]))
            {
                builder.Append(expression[index]);
                index++;
            }

            return new MathsToken(builder.ToString(), new TokenSpan(currentIndex, builder.Length), MathsTokenCategory.Variable, MathsTokenType.Variable);
        }
    }
}
