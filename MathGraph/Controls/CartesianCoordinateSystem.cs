using System.Collections.Generic;
using MathGraph.Maths.Functions;
using System.Windows.Controls;
using MathGraph.Maths.Graphs;
using System.Globalization;
using System.Windows.Media;
using MathGraph.Utility;
using MathGraph.Maths;
using System.Windows;
using System;

namespace MathGraph.Controls
{
    public class CartesianCoordinateSystem : Control
    {
        private readonly Vector2 centerOffset = new Vector2(0, 0);

        protected override void OnRender(DrawingContext context)
        {
            base.OnRender(context);
            CartesianCoordinateSystemDrawingHelper helper = new CartesianCoordinateSystemDrawingHelper(context, new Rect(0, 0, this.ActualWidth, this.ActualHeight), centerOffset, 1.0d);

            helper.DrawAxis();
            helper.DrawPoint("A", -5.0d, 0.5d);
            helper.DrawFunction(new LinearFunction(0.5d, 2.0d));
            helper.DrawFunction(new QuadraticFunction(1.0d, new Vector2()));
        }
    }

    public class CartesianCoordinateSystemDrawingHelper
    {
        private readonly double unscaledStepSize = 50.0d;

        private readonly Pen defaultPen = new Pen(Brushes.Black, 2.0d);
        private readonly Pen thinPen = new Pen(Brushes.Black, 1.0d);

        private readonly Vector2 bottomRightCoordinate;
        private readonly Vector2 topLeftCoordinate;
        private readonly DrawingContext context;
        private readonly double stepSize;
        private readonly Vector2 center;
        private readonly double scale;
        private readonly Vector2 offset;
        private readonly Rect bounds;

        public CartesianCoordinateSystemDrawingHelper(DrawingContext context, Rect bounds, Vector2 offset, double scale)
        {
            this.context = context;
            this.offset = offset;
            this.bounds = bounds;
            this.scale = scale;
            
            this.stepSize = this.unscaledStepSize * this.scale;

            this.context.PushClip(new RectangleGeometry(bounds));

            double centerY = this.bounds.Height / 2 + this.offset.Y;
            double centerX = this.bounds.Width / 2 + this.offset.X;

            this.center = new Vector2(centerX, centerY);
            this.bottomRightCoordinate = this.GetBottomRightCoordinate();
            this.topLeftCoordinate = this.GetTopLeftCoordinate();
        }

        public void DrawPoint(string name, double x, double y)
        {
            Point point = this.GetCoordinatePoint(x, y);
            Point bottomRight = new Point(-3.0d, -3.0d);
            Point topLeft = new Point(3.0d, 3.0d);
            
            this.context.DrawLine(this.defaultPen, new Point(point.X + topLeft.X, point.Y + topLeft.Y), new Point(point.X + bottomRight.X, point.Y + bottomRight.Y));
            this.context.DrawLine(this.defaultPen, new Point(point.X + bottomRight.X, point.Y + topLeft.Y), new Point(point.X + topLeft.X, point.Y + bottomRight.Y));
            this.DrawText(new Point(point.X + 5, point.Y), name);
        }

        public void DrawFunction(IFunction function)
        {
            List<Vector2> coordinates = this.GetFunctionCoordinates(function, 0.25d);
            Vector2[] requiredCoordinates = this.GetRequiredGraphCoordinates(coordinates);

            //TODO: Name-Text
            int index = 1;
            while(index < requiredCoordinates.Length)
            {
                Point start = this.GetCoordinatePoint(requiredCoordinates[index - 1]);
                Point end = this.GetCoordinatePoint(requiredCoordinates[index]);
                this.context.DrawLine(this.thinPen, start, end);
                index++;
            }
        }

        private List<Vector2> GetFunctionCoordinates(IFunction function, double spacingX)
        {
            List<Vector2> output = new List<Vector2>();

            double right = this.bottomRightCoordinate.X;
            double left = this.topLeftCoordinate.X;
            double current = left;

            while (current < right)
            {
                output.Add(new Vector2(current, function.GetY(current)));
                current += spacingX - current % spacingX;
            }

            return output;
        }

        private Vector2[] GetRequiredGraphCoordinates(List<Vector2> coordinates)
        {
            List<Vector2> output = new List<Vector2>();
            Vector2? previous = null;
            for (int i = 0; i < coordinates.Count; i++)
            {
                Vector2 current = coordinates[i];

                if (previous == null)
                {
                    output.Add(current);
                }
                else
                {
                    bool previousInBounds = previous.HasValue && this.IsInBounds(previous.Value);
                    bool nextInBounds = i < coordinates.Count - 2 && this.IsInBounds(coordinates[i + 1]);

                    if (nextInBounds)
                    {
                        Vector2 previousDelta = previous.Value - current;
                        Vector2 nextDelta = current - coordinates[i + 1];

                        if (previousDelta != nextDelta)
                        {
                            output.Add(current);
                        }
                    }
                    else if(previousInBounds)
                    {
                        output.Add(current);
                    }
                }
                previous = current;
            }

            return output.ToArray();
        }

        private bool IsInBounds(Vector2 coordinate)
        {
            Point point = this.GetCoordinatePoint(coordinate);

            return point.X >= 0 && point.Y >= 0 && point.X <= this.bounds.Width && point.Y <= this.bounds.Height;
        }

        public void DrawAxis()
        {
            context.DrawLine(this.defaultPen, new Point(0, this.center.Y), new Point(this.bounds.Width, this.center.Y));
            context.DrawLine(this.defaultPen, new Point(this.center.X, 0), new Point(this.center.X, this.bounds.Height));

            this.DrawHorizontalAxisSteps(this.center);
            this.DrawVerticalAxisSteps(this.center);
        }

        private void DrawHorizontalAxisSteps(Vector2 center)
        {
            Vector2 left = this.topLeftCoordinate;
            double start = this.GetCoordinatePoint((int)left.X, 0).X;
            int step = (int)left.X;

            while ((start += this.stepSize) <= this.bounds.Width)
            {
                step++;
                this.DrawHorizontalStep(step, start, center.Y);
            }
        }

        private void DrawHorizontalStep(int step, double x, double y)
        {
            bool important = this.IsImportantStep(step);
            Pen pen = important ? this.defaultPen : this.thinPen;

            if (step != 0)
            {
                this.context.DrawLine(pen, new Point(x, y - 5), new Point(x, y + 5));
            }
            if (important)
            {
                this.DrawText(new Point(x + 5, y), step.ToString());
            }
        }

        private void DrawVerticalAxisSteps(Vector2 center)
        {
            Vector2 top = this.topLeftCoordinate;
            double start = this.GetCoordinatePoint(0, (int)top.Y).Y;
            int step = (int)top.Y + 1;

            while (start <= this.bounds.Height)
            {
                step--;
                this.DrawVerticalStep(step, center.X, start);
                start += this.stepSize;
            }
        }

        private void DrawVerticalStep(int step, double x, double y)
        {
            if (step != 0)
            {
                bool important = this.IsImportantStep(step);
                Pen pen = important ? this.defaultPen : this.thinPen;

                this.context.DrawLine(pen, new Point(x - 5, y), new Point(x + 5, y));
                if (important)
                {
                    this.DrawText(new Point(x + 5, y + 4), step.ToString());
                }
            }
        }

        private bool IsImportantStep(int step)
        {
            return Math.Abs(step) == 1 || step % 5 == 0;
        }

        private void DrawText(Point location, string text)
        {
            FormattedText formatted = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12.0d, Brushes.Black);
            this.context.DrawText(formatted, location);
        }

        private Point GetCoordinatePoint(double x, double y)
        {
            //The WPF-Coordinatesystem starts with 0 on the top, therefore, y must be inverted
            return (this.center + new Vector2(x, -y) * this.stepSize).ToPoint();
        }

        private Point GetCoordinatePoint(Vector2 coordinate)
        {
            return this.GetCoordinatePoint(coordinate.X, coordinate.Y);
        }

        private Vector2 GetPointCoordinate(Point point)
        {
            return new Vector2((point.X - this.center.X) / this.stepSize, (point.Y + this.center.Y) / this.stepSize);
        }

        private Vector2 GetTopLeftCoordinate()
        {
            return this.GetPointCoordinate(new Point());
        }

        private Vector2 GetBottomRightCoordinate()
        {
            return this.GetPointCoordinate(new Point(this.bounds.Width, -this.bounds.Height));
        }
    }
}
