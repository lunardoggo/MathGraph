using MathGraph.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGraph.Commands
{
    public class ClearExpressionCommand : MathsExpressionContainerCommand
    {
        public ClearExpressionCommand(IMathsExpressionContainer container) : base(container, true)
        { }

        public override void Execute(object parameter)
        {
            base.container.Expression = "";
        }
    }
}
