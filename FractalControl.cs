using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Media;
using System;
using System.Collections.Generic;

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
            // var timer = new DispatcherTimer();
            // timer.Interval = TimeSpan.FromSeconds(1 / 60.0);
            // timer.Tick += (sender, e) => Angle += Math.PI / 360;
            // timer.Start();
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

        public Algorithm Algorithm {
            get => GetValue(AlgorithmProperty);
            set => SetValue(AlgorithmProperty, value);
        }

        public override void Render(DrawingContext context)
        {
            switch (this.Algorithm) {
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

        private void DrawSierpinskiTriangle(DrawingContext context) {
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

            var pen = new Avalonia.Media.Pen(Avalonia.Media.Brushes.Black);
            context.DrawLine(pen, p1, p2);
            context.DrawLine(pen, p2, p3);
            context.DrawLine(pen, p3, p1);

            // TODO
        }

        private void DrawKochSnowflake(DrawingContext context) {
            // TODO
        }

        private void DrawHilbertCurve(DrawingContext context) {
            // TODO
        }
    }