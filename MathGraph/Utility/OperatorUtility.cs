using MathGraph.Maths;
using System.Windows;

namespace MathGraph.Utility
{
    public static class OperatorUtility
    {
        public static Point ToPoint(this Vector2 vector)
        {
            return new Point(vector.X, vector.Y);
        }
    }
}
