﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Drawing.Diagnostics
{
    public abstract class CommandLogger : ICanvas2D, IScene3D
    {
        #region lifecycle

        public static CommandLogger From(StringBuilder sb) { return new _StringBuilderWriter(sb); }

        public static CommandLogger From(IList<string> sb) { return new _LinesWriter(sb); }

        #endregion

        #region API

        protected abstract void Write(string line);

        #endregion

        #region API 2D

        /// <inheritdoc />
        public void DrawAsset(in Matrix3x2 transform, object asset)
        {
            Write($"Asset {transform} {asset}");
        }

        /// <inheritdoc />
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            Write($"Convex {points.Length} {color}");
        }

        /// <inheritdoc />
        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle brush)
        {
            Write($"Ellipse {center} {width} {height} {brush}");
        }

        /// <inheritdoc />
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
        {
            Write($"Lines {points.Length} {diameter} {brush}");
        }

        /// <inheritdoc />
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle brush)
        {
            Write($"Polygon {points.Length} {brush}");
        }

        /// <inheritdoc />
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            Write($"Sprite {transform} {style}");
        }

        public void DrawTextLine(in Matrix3x2 transform, string text, float size, FontStyle font)
        {
            Write($"Text {transform} {text}");
        }

        #endregion

        #region API 3D

        /// <inheritdoc />
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            Write($"ConvexSrf {vertices.Length} {style}");
        }

        /// <inheritdoc />
        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            Write($"Asset {transform} {asset}");
        }

        /// <inheritdoc />
        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            Write($"Segment {vertices.Length} {style}");
        }

        /// <inheritdoc />
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            Write($"Sphere {center} {diameter} {brush}");
        }

        /// <inheritdoc />
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
