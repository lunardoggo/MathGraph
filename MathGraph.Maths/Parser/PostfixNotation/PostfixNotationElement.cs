using System;

namespace MathGraph.Maths.Parser.PostfixNotation
{
    public abstract class PostfixNotationElement : IEquatable<PostfixNotationElement>
    {
        public abstract bool Equals(PostfixNotationElement other);
    }
}
