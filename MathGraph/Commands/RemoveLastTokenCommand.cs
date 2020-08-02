using MathGraph.Interfaces;
using System;

namespace MathGraph.Commands
{
    public class RemoveLastTokenCommand : MathsExpressionContainerCommand
    {

        public RemoveLastTokenCommand(IMathsExpressionContainer container) : base(container, true)
        { }

        public override void Execute(object parameter)
        {
            int newLength = Math.Max(0, base.container.Expression.Length - 1);
            base.container.Expression = base.container.Expression.Substring(0, newLength);
        }
    }
}
