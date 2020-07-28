namespace MathGraph.Maths.Errors
{
    public sealed class ErrorSinkEntry
    {
        public ErrorSinkEntry(Severety severety, string message, int index)
        {
            this.Severety = severety;
            this.Message = message;
            this.Index = index;
        }

        public Severety Severety { get; }
        public string Message { get; }
        public int Index { get; }
    }

    public enum Severety
    {
        Information,
        Warning,
        Error
    }
}
