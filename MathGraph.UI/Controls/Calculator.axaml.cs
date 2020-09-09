using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MathGraph.UI.Controls
{
    public class Calculator : UserControl
    {
        public Calculator()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
