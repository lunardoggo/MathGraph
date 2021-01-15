using System.Collections.Generic;
using System.Windows.Controls;
using MathGraph.Maths.Errors;
using System.Windows.Input;
using MathGraph.Interfaces;
using MathGraph.Commands;
using System.Windows;

namespace MathGraph.Controls
{
    /// <summary>
    /// Interaktionslogik für CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl, IMathsExpressionContainer
    {
        public CalculatorView()
        {
            InitializeComponent();

            this.RemoveLastTokenCommand = new RemoveLastTokenCommand(this);
            this.ClearExpressionCommand = new ClearExpressionCommand(this);
            this.SolveExpressionCommand = new SolveExpressionCommand(this);
            this.AddTokenCommand = new AddTokenCommand(this);
        }

        public ICommand RemoveLastTokenCommand { get; }
        public ICommand ClearExpressionCommand { get; }
        public ICommand SolveExpressionCommand { get; }
        public ICommand AddTokenCommand { get; }

        public IEnumerable<ErrorSinkEntry> ErrorSinkEntries
        {
            get { return (IEnumerable<ErrorSinkEntry>)GetValue(ErrorSinkEntriesProperty); }
            set { SetValue(ErrorSinkEntriesProperty, value); }
        }

        public string PostfixNotation
        {
            get { return (string )GetValue(PostfixNotationProperty); }
            set { SetValue(PostfixNotationProperty, value); }
        }

        public string Expression
        {
            get { return (string)GetValue(ExpressionProperty); }
            set { SetValue(ExpressionProperty, value); }
        }

        public decimal Result
        {
            get { return (decimal)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(decimal), typeof(CalculatorView), new PropertyMetadata(0.0m));

        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register("Expression", typeof(string), typeof(CalculatorView), new PropertyMetadata(""));

        public static readonly DependencyProperty PostfixNotationProperty =
            DependencyProperty.Register("PostfixNotation", typeof(string ), typeof(CalculatorView), new PropertyMetadata(""));

        public static readonly DependencyProperty ErrorSinkEntriesProperty =
            DependencyProperty.Register("ErrorSinkEntries", typeof(IEnumerable<ErrorSinkEntry>), typeof(CalculatorView), new UIPropertyMetadata(null));
    }
}
