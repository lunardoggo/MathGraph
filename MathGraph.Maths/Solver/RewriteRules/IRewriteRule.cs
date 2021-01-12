using System;
using System.Collections.Generic;
using System.Text;

namespace MathGraph.Maths.Solver.RewriteRules
{
    public interface IRewriteRule
    {
        bool CanBeApplied();
    }
}
