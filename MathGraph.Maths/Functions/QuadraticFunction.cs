using MathGraph.Maths.Graphs;
using System;

namespace MathGraph.Maths.Functions
{
    public class QuadraticFunction : IFunction
    {
        private readonly double compression;
        private readonly Vector2 vertex;

        public QuadraticFunction(double compression, Vector2 vertex)
        {
            this.compression = compression;
            this.vertex = vertex;
        }

        public double GetY(double x)
        {
            return this.compression * Math.Pow(x - this.vertex.X, 2) + this.vertex.Y;
        }
    }
}
