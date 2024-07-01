using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;


namespace Fractals;

public class FractalControl : Control
{
    static FractalControl()
    {
        AffectsRender<FractalControl>(DepthProperty);
        AffectsRender<FractalControl>(AlgorithmProperty);
    }

    public FractalControl()
    {
    }

    public static readonly StyledProperty<int> DepthProperty =
        AvaloniaProperty.Register<FractalControl, int>(nameof(Depth), defaultValue: 0);

    public static readonly StyledProperty<Algorithm> AlgorithmProperty =
        AvaloniaProperty.Register<FractalControl, Algorithm>(nameof(Algorithm), defaultValue: Algorithm.SierpinskiTriangle);

    public int Depth
    {
        get => GetValue(DepthProperty);
        set => SetValue(DepthProperty, value);
    }

    public Algorithm Algorithm
    {
        get => GetValue(AlgorithmProperty);
        set => SetValue(AlgorithmProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        switch (this.Algorithm)
        {
            case Algorithm.SierpinskiTriangle:
                DrawSierpinskiTriangle(context);
                break;
            case Algorithm.KochSnowflake:
                DrawKochSnowflake(context);
                break;
            case Algorithm.HilbertCurve:
                DrawHilbertCurve(context);
                break;
        }
    }

    private void DrawSierpinskiTriangle(DrawingContext context)
    {
        double margin = 20;
        double triangleHeightPerSide = Math.Sqrt(3.0) / 2.0;
        double width = this.Bounds.Width;
        double height = this.Bounds.Height;
        double triangleSide = Math.Min(width - 2 * margin, (height - 2 * margin) / triangleHeightPerSide);
        double triangleHeight = triangleHeightPerSide * triangleSide;
        double x1 = width / 2;
        double x2 = x1 - triangleSide / 2;
        double x3 = x1 + triangleSide / 2;
        double y1 = height / 2 - triangleHeight / 2;
        double y2 = y1 + triangleHeight;
        double y3 = y2;
        Point p1 = new Point(x1, y1);
        Point p2 = new Point(x2, y2);
        Point p3 = new Point(x3, y3);

        var pen = new Pen(Brushes.Black);
        context.DrawLine(pen, p1, p2);
        context.DrawLine(pen, p2, p3);
        context.DrawLine(pen, p3, p1);

        DrawSierpinskiImpl(context, pen, this.Depth, p1, p2, p3);

    }
    private void DrawSierpinskiImpl(DrawingContext context, Pen pen, int depth, Point a, Point b, Point c)
    {
        if (depth == 0)
        {
            return;
        }
        Point ab = Lerp(a, b, t: 0.5);
        Point bc = Lerp(b, c, t: 0.5);
        Point ac = Lerp(a, c, t: 0.5);
        context.DrawLine(pen, ab, bc);
        context.DrawLine(pen, bc, ac);
        context.DrawLine(pen, ab, ac);
        DrawSierpinskiImpl(context, pen, depth - 1, a, ac, ab);
        DrawSierpinskiImpl(context, pen, depth - 1, b, ab, bc);
        DrawSierpinskiImpl(context, pen, depth - 1, c, ac, bc);
    }
    Point Lerp(Point a, Point b, double t)
    {
        double X = b.X * t + a.X * (1 - t);
        double Y = b.Y * t + a.Y * (1 - t);

        return new Point(X, Y);
    }

    private void DrawKochSnowflake(DrawingContext context)
    {
        double margin = 20;
        double triangleHeightPerSide = Math.Sqrt(3.0) / 2.0;
        double width = this.Bounds.Width;
        double height = this.Bounds.Height;
        double triangleSide = Math.Min(width - 2 * margin, (height - 2 * margin) / (4.0 / 3.0 * triangleHeightPerSide));
        double triangleHeight = triangleHeightPerSide * triangleSide;
        double x1 = width / 2;
        double x2 = x1 - triangleSide / 2;
        double x3 = x1 + triangleSide / 2;
        double y1 = height / 2 - 2.0 / 3.0 * triangleHeight;
        double y2 = y1 + triangleHeight;
        double y3 = y2;
        Point p1 = new Point(x1, y1);
        Point p2 = new Point(x2, y2);
        Point p3 = new Point(x3, y3);

        var pen = new Pen(Brushes.Black);
        DrawKochSnowflakeImpl(context, pen, this.Depth, p1, p2);
        DrawKochSnowflakeImpl(context, pen, this.Depth, p2, p3);
        DrawKochSnowflakeImpl(context, pen, this.Depth, p3, p1);
    }
    private void DrawKochSnowflakeImpl(DrawingContext context, Pen pen, int depth, Point a, Point b)
    {
        if (depth == 0)
        {
            context.DrawLine(pen, a, b);
            return;
        }
        Point p1 = Lerp(a, b, t: 1.0 / 3.0);
        Point c = Lerp(a, b, t: 1.0 / 2.0);
        Point p3 = Lerp(a, b, t: 2.0 / 3.0); ;
        double dx = (b.X - a.X) / (2 * Math.Sqrt(3));
        double dy = (b.Y - a.Y) / (2 * Math.Sqrt(3));
        Point p2 = new Point(c.X - dy, c.Y + dx);
        DrawKochSnowflakeImpl(context, pen, depth - 1, a, p1);
        DrawKochSnowflakeImpl(context, pen, depth - 1, p1, p2);
        DrawKochSnowflakeImpl(context, pen, depth - 1, p2, p3);
        DrawKochSnowflakeImpl(context, pen, depth - 1, p3, b);
    }
    private void DrawHilbertCurve(DrawingContext context)
    {
        double margin = 20;
        double width = this.Bounds.Width;
        double height = this.Bounds.Height;
        double side = Math.Min(width - 2 * margin, height - 2 * margin);
        double Xmin = width / 2 - side / 2;
        double Xmax = width / 2 + side / 2;
        double Ymin = height / 2 - side / 2;
        double Ymax = height / 2 + side / 2;
        Point lastPoint = new Point(0, 0);
        var pen = new Pen(Brushes.Red);
        DrawHilbertCurveImpl(context, pen, this.Depth, Xmin, Xmax, Ymin, Ymax, ref lastPoint, Direction.Up);
    }
    void DrawHilbertCurveImpl(DrawingContext context, Pen pen, int depth, double Xmin, double Xmax, double Ymin, double Ymax, ref Point lastPoint, Direction direction)
    {
        var centerPoint = new Point((Xmax + Xmin) / 2, (Ymax + Ymin) / 2);
        if (depth == 0)
        {
            if (lastPoint.X != 0 || lastPoint.Y != 0)
            {
                context.DrawLine(pen, lastPoint, centerPoint);
            }
            lastPoint = centerPoint;
            return;
        }
        switch (direction)
        {
            case Direction.Up:
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, centerPoint.Y, Ymax, ref lastPoint, Direction.Right);
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, Ymin, centerPoint.Y, ref lastPoint, Direction.Up);
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, Ymin, centerPoint.Y, ref lastPoint, Direction.Up);
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, centerPoint.Y, Ymax, ref lastPoint, Direction.Left);
                break;
            case Direction.Down:
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, Ymin, centerPoint.Y, ref lastPoint, Direction.Left);
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, centerPoint.Y, Ymax, ref lastPoint, Direction.Down);
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, centerPoint.Y, Ymax, ref lastPoint, Direction.Down);
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, Ymin, centerPoint.Y, ref lastPoint, Direction.Right);
                break;
            case Direction.Left:
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, Ymin, centerPoint.Y, ref lastPoint, Direction.Down);
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, Ymin, centerPoint.Y, ref lastPoint, Direction.Left);
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, centerPoint.Y, Ymax, ref lastPoint, Direction.Left);
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, centerPoint.Y, Ymax, ref lastPoint, Direction.Up);
                break;
            case Direction.Right:
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, centerPoint.Y, Ymax, ref lastPoint, Direction.Up);
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, centerPoint.Y, Ymax, ref lastPoint, Direction.Right);
                DrawHilbertCurveImpl(context, pen, depth - 1, centerPoint.X, Xmax, Ymin, centerPoint.Y, ref lastPoint, Direction.Right);
                DrawHilbertCurveImpl(context, pen, depth - 1, Xmin, centerPoint.X, Ymin, centerPoint.Y, ref lastPoint, Direction.Down);
                break;
        }
    }
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}