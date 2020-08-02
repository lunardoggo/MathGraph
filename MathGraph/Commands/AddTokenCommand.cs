using MathGraph.Interfaces;

namespace MathGraph.Commands
{
    public class AddTokenCommand : MathsExpressionContainerCommand
    {
        public AddTokenCommand(IMathsExpressionContainer container) : base(container, false)
        {
        }

        public override void Execute(object parameter)
        {
            base.container.Expression += parameter.ToString();
        }
    }
}
