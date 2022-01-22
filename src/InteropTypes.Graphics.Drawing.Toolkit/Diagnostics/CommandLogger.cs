using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropDrawing.Diagnostics
{
    public abstract class CommandLogger : IDrawing2D, IDrawing3D
    {
        #region lifecycle

        public static CommandLogger From(StringBuilder sb) { return new _StringBuilderWriter(sb); }

        public static CommandLogger From(IList<string> sb) { return new _LinesWriter(sb); }

        #endregion

        #region API

        protected abstract void Write(string line);

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle brush)
        {
            Write($"Asset {transform} {asset} {brush}");
        }

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            Write($"Asset {transform} {asset} {brush}");
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle brush)
        {
            Write($"Ellipse {center} {width} {height} {brush}");
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
        {
            Write($"Lines {points.Length} {diameter} {brush}");
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            Write($"Polygon {points.Length} {brush}");
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            Write($"Segment {a} {b} {diameter} {brush}");
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle brush)
        {
            Write($"Sphere {center} {diameter} {brush}");
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            Write($"Sprite {transform} {style}");
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            Write($"Surface {vertices.Length} {brush}");
        }

        #endregion

        #region nested types

        private sealed class _StringBuilderWriter : CommandLogger
        {
            public _StringBuilderWriter(StringBuilder sb) { _SB = sb; }

            private StringBuilder _SB;

            protected override void Write(string line)
            {
                _SB.AppendLine(line);
            }
        }

        private sealed class _LinesWriter : CommandLogger
        {
            public _LinesWriter(IList<string> sb) { _SB = sb; }

            private IList<string> _SB;

            protected override void Write(string line)
            {
                _SB.Add(line);
            }
        }

        #endregion
    }
}
