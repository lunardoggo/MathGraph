namespace MathGraph.Maths
{
    public struct Vector2
    {
        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public static bool operator ==(Vector2 first, Vector2 second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator !=(Vector2 first, Vector2 second)
        {
            return !(first == second);
        }

        public static Vector2 operator +(Vector2 first, Vector2 second)
        {
            return new Vector2(first.X + second.X, first.Y + second.Y);
        }

        public static Vector2 operator -(Vector2 first, Vector2 second)
        {
            return first + (second * -1.0d);
        }

        public static Vector2 operator *(Vector2 vector, double factor)
        {
            return new Vector2(vector.X * factor, vector.Y * factor);
        }
    }
}
