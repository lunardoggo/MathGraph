using MathGraph.Maths.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathGraph
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public IEnumerable<ErrorSinkEntry> ErrorSink
        {
            get { return (IEnumerable<ErrorSinkEntry>)GetValue(ErrorSinkProperty); }
            set { SetValue(ErrorSinkProperty, value); }
        }

        public static readonly DependencyProperty ErrorSinkProperty =
            DependencyProperty.Register("ErrorSink", typeof(IEnumerable<ErrorSinkEntry>), typeof(MainWindow), new UIPropertyMetadata(null));
    }
}
