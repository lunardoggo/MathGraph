using System.Collections.Generic;
using MathGraph.Maths.Errors;

namespace MathGraph.Interfaces
{
    public interface IMathsExpressionContainer
    {
        IEnumerable<ErrorSinkEntry> ErrorSinkEntries { get; set; }
        string PostfixNotation { get; set; }
        string Expression { get; set; }
        double Result { get; set; }
    }
}
