using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text;

namespace MathGraph.UI.Controls
{
    public class CartesianCoordinateSystem : Control
    {
        private readonly Point centerOffset = new Point(0, 0);

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            CartesianCoordinateSystemDrawingHelper helper = new CartesianCoordinateSystemDrawingHelper(context, this.Bounds, centerOffset, 1.0d);

            helper.DrawAxis();
            helper.DrawPoint("A", -5.0d, 0.5d);
            helper.DrawLinearFunction("f1", 0.5d, 1.0d);
            helper.DrawParabula("f2", 1, new Point(), 0.25d);
            helper.DrawParabula("f3", -0.25, new Point(-2, -1), 0.25d);
        }
    }

    public class CartesianCoordinateSystemDrawingHelper
    {
        private readonly double unscaledStepSize = 50.0d;

        private readonly Pen defaultPen = new Pen(Brushes.Black, 2.0d);
        private readonly Pen smallPen = new Pen(Brushes.Black, 1.0d);

        private readonly Point bottomRightCoordinate;
        private readonly Point topLeftCoordinate;
        private readonly DrawingContext context;
        private readonly double stepSize;
        private readonly Point center;
        private readonly double scale;
        private readonly Point offset;
        private readonly Rect bounds;

        public CartesianCoordinateSystemDrawingHelper(DrawingContext context, Rect bounds, Point offset, double scale)
        {
            this.context = context;
            this.offset = offset;
            this.bounds = bounds;
            this.scale = scale;

            this.stepSize = this.unscaledStepSize * this.scale;

            double centerY = this.bounds.Height / 2 + this.offset.Y;
            double centerX = this.bounds.Width / 2 + this.offset.X;

            this.center = new Point(centerX, centerY);

            this.bottomRightCoordinate = this.GetBottomRightCoordinate();
            this.topLeftCoordinate = this.GetTopLeftCoordinate();
        }

        public void DrawPoint(string name, double x, double y)
        {
            Point point = this.GetCoordinatePoint(x, y);
            Point bottomRight = new Point(-3, 3);
            Point topLeft = new Point(-3, -3);

            context.DrawLine(this.defaultPen, point + topLeft, point - topLeft);
            context.DrawLine(this.defaultPen, point + bottomRight, point - bottomRight);
            this.DrawText(point + new Point(5, 0), name);
        }

        public void DrawLinearFunction(string name, double slope, double yAxisIntersect)
        {
            Point end = this.GetCoordinatePoint(this.bottomRightCoordinate.X, this.bottomRightCoordinate.X * slope + yAxisIntersect);
            Point start = this.GetCoordinatePoint(this.topLeftCoordinate.X, this.topLeftCoordinate.X * slope + yAxisIntersect);
            Point yIntersect = this.GetCoordinatePoint(0, yAxisIntersect);

            this.context.DrawLine(this.smallPen, start, end);
            this.DrawText(yIntersect + new Point(10, -30), name);
        }

        public void DrawParabula(string name, double compression, Point vertex, double stepSize)
        {
            double right = this.bottomRightCoordinate.X;
            double current = this.topLeftCoordinate.X;
            double detail = stepSize / this.scale;

            Point last = new Point(current, this.GetParabolaY(compression, vertex, current));
            do
            {
                current += detail;
                Point next = new Point(current, this.GetParabolaY(compression, vertex, current));

                if(Math.Max(last.Y, next.Y) >= this.bottomRightCoordinate.Y && Math.Min(last.Y, next.Y) <= this.topLeftCoordinate.Y)
                {
                    this.context.DrawLine(this.smallPen, this.GetCoordinatePoint(last.X, last.Y), this.GetCoordinatePoint(next.X, next.Y));
                }

                last = next;
            }
            while (current <= right);

            this.DrawText(this.GetCoordinatePoint(vertex.X, vertex.Y) + new Point(5, (compression > 0 ? 1 : -3) * 5), name);
        }

        private double GetParabolaY(double compression, Point vertex, double x)
        {
            return compression * Math.Pow(x - vertex.X, 2) + vertex.Y;
        }

        public void DrawAxis()
        {
            context.DrawLine(this.defaultPen, new Point(0, this.center.Y), new Point(this.bounds.Width, this.center.Y));
            context.DrawLine(this.defaultPen, new Point(this.center.X, 0), new Point(this.center.X, this.bounds.Height));

            this.DrawHorizontalAxisSteps(this.center);
            this.DrawVerticalAxisSteps(this.center);
        }

        private void DrawHorizontalAxisSteps(Point center)
        {
            Point left = this.topLeftCoordinate;
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
            Pen pen = important ? this.defaultPen : this.smallPen;

            if (step != 0)
            {
                this.context.DrawLine(pen, new Point(x, y - 5), new Point(x, y + 5));
            }
            if (important)
            {
                this.DrawText(new Point(x + 5, y), step.ToString());
            }
        }

        private void DrawVerticalAxisSteps(Point center)
        {
            Point top = this.topLeftCoordinate;
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
                Pen pen = important ? this.defaultPen : this.smallPen;

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
            this.context.DrawText(Brushes.Black, location, this.CreateFormattedText(new Size(10 * text.Length, 10), text));
        }

        private FormattedText CreateFormattedText(Size constraint, string text)
        {
            return new FormattedText()
            {
                Constraint = constraint,
                Typeface = new Typeface("Verdana"),
                Text = text,
                Wrapping = TextWrapping.NoWrap,
                TextAlignment = TextAlignment.Left
            };
        }

        private Point GetCoordinatePoint(double x, double y)
        {
            return this.center + new Point(x, -y) * this.stepSize;
        }

        private Point GetPointCoordinate(Point point)
        {
            return new Point((point.X - this.center.X) / this.stepSize, (point.Y + this.center.Y) / this.stepSize);
        }

        private Point GetTopLeftCoordinate()
        {
            return this.GetPointCoordinate(new Point());
        }

        private Point GetBottomRightCoordinate()
        {
            return this.GetPointCoordinate(new Point(this.bounds.Width, -this.bounds.Height));
        }
    }
}
