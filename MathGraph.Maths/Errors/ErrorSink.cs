using System.Collections.Immutable;
using System.Collections.Generic;

namespace MathGraph.Maths.Errors
{
    public class ErrorSink
    {
        private readonly List<ErrorSinkEntry> entries = new List<ErrorSinkEntry>();

        public void AddError(Severety severety, string message, int index)
        {
            this.entries.Add(new ErrorSinkEntry(severety, message, index));
        }

        public void AddError(ErrorSinkEntry entry)
        {
            this.entries.Add(entry);
        }

        public IEnumerable<ErrorSinkEntry> Entries
        {
            get { return this.entries.ToImmutableArray(); }
        }

        public void Clear()
        {
            this.entries.Clear();
        }
    }
}
