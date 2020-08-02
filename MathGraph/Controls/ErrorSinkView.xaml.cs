using System.Collections.Generic;
using System.Windows.Controls;
using MathGraph.Maths.Errors;
using System.Windows;

namespace MathGraph.Controls
{
    public partial class ErrorSinkView : UserControl
    {
        public ErrorSinkView()
        {
            InitializeComponent();
        }

        public IEnumerable<ErrorSinkEntry> ItemsSource
        {
            get { return (IEnumerable<ErrorSinkEntry>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ErrorSinkEntry>), typeof(ErrorSinkView), new UIPropertyMetadata(null));
    }
}
