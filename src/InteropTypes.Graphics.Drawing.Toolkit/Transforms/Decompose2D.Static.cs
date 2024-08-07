﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Drawing;

using ASSET = System.Object;
using SCALAR = System.Single;
using POINT2 = InteropTypes.Graphics.Drawing.Point2;
using VECTOR2 = System.Numerics.Vector2;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    partial struct Decompose2D
    {
        #region API - Static ICanvas2D

        public static bool DrawAsset<TAsset>(ICoreCanvas2D dc, in Matrix3x2 transform, TAsset asset)
        {
            if (asset == null) return true; // nothing to draw  

            if (typeof(IDrawingBrush<ICanvas2D>).IsAssignableFrom(typeof(TAsset))) { ((IDrawingBrush<ICanvas2D>)asset).DrawTo(new Decompose2D(Canvas2DTransform.Create(dc, transform))); return true; }

            if (typeof(IDrawingBrush<ICoreCanvas2D>).IsAssignableFrom(typeof(TAsset))) { ((IDrawingBrush<ICoreCanvas2D>)asset).DrawTo(Canvas2DTransform.Create(dc, transform)); return true; }

            // fallback

            ASSET xasset = typeof(IPseudoImmutable).IsAssignableFrom(typeof(TAsset))
                ? ((IPseudoImmutable)asset).ImmutableKey
                : (ASSET)asset;

            return DrawAsset(dc, transform, xasset);
        }

        public static bool DrawAsset(ICoreCanvas2D dc, in Matrix3x2 transform, ASSET asset)
        {
            if (asset == null) return true; // nothing to draw            

            if (asset is IDrawingBrush<ICanvas2D> d2) { d2.DrawTo(new Decompose2D(Canvas2DTransform.Create(dc, transform))); return true; }

            if (asset is IDrawingBrush<ICoreCanvas2D> d1) { d1.DrawTo(Canvas2DTransform.Create(dc, transform)); return true; }

            // fallback

            return asset is IPseudoImmutable inmutable && DrawAsset(dc, transform, inmutable.ImmutableKey);
        }

        public static void DrawEllipse(ICoreCanvas2D dc, POINT2 center, SCALAR width, SCALAR height, in OutlineFillStyle style)
        {
            // calculate number of vertices based on dimensions
            int count = Math.Max((int)width, (int)height);
            if (count < 3) count = 3;

            Span<POINT2> points = stackalloc POINT2[count];

            Parametric.ShapeFactory2D.FillEllipseVertices(points, center, width, height);

            DrawPolygon(dc, points, style);
        }

        public static void DrawEllipse(Backends.IBackendCanvas2D dc, POINT2 center, SCALAR width, SCALAR height, in OutlineFillStyle style)
        {
            // calculate number of vertices based on dimensions
            int count = Math.Max((int)width, (int)height);
            if (count < 3) count = 3;

            Span<POINT2> points = stackalloc POINT2[count];

            Parametric.ShapeFactory2D.FillEllipseVertices(points, center, width, height);

            DrawPolygon(dc, points, style);
        }

        public static void DrawPolygon(ICoreCanvas2D dc, ReadOnlySpan<POINT2> points, in PolygonStyle style)
        {
            if (points.Length < 3) return;

            if (style.HasFill) dc.DrawConvexPolygon(points, style.FillColor); // todo: triangulate

            if (style.HasOutline) _DrawSolidLines(dc, points, style.OutlineWidth, style.OutlineColor, true);
        }

        public static void DrawPolygon(Backends.IBackendCanvas2D dc, ReadOnlySpan<POINT2> points, in PolygonStyle style)
        {
            if (points.Length < 3) return;

            if (style.HasFill) dc.DrawConvexPolygon(points, style.FillColor); // todo: triangulate

            if (style.HasOutline) _DrawSolidLines(dc, points, style.OutlineWidth, style.OutlineColor, true);
        }        

        public static void DrawLines(ICoreCanvas2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            var xstyle = style.IsSolid(ref diameter, out var solid)
                ? new LineStyle(solid)
                : style;

            if (style.Style.HasFill)
            {
                _FillLineAsPolygon(dc, points, diameter, style.FillColor, style.StartCap, style.EndCap);
            }
        }

        public static void DrawLines(Backends.IBackendCanvas2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            var xstyle = style.IsSolid(ref diameter, out var solid)
                ? new LineStyle(solid)
                : style;

            var pixelSize = dc.GetPixelsPerUnit();

            if (diameter <= pixelSize)
            {
                dc.DrawThinLines(points, diameter, style.FillColor);
                return;
            }

            if (style.Style.HasFill)
            {
                _FillLineAsPolygon(dc, points, diameter, style.FillColor, style.StartCap, style.EndCap);
            }
        }        

        #endregion

        #region core        

        private static void _DrawSolidLines(ICoreCanvas2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, ColorStyle color, bool closed)
        {
            if (points.Length < 2) return;
            if (points.Length == 2) closed = false;

            // create segments
            Span<POINT2> segments = stackalloc POINT2[Parametric.ShapeFactory2D.GetLinesSegmentsVerticesCount(points.Length, closed)];
            Parametric.ShapeFactory2D.FillLinesSegments(segments, points, diameter, closed);

            // draw segments
            var segment = segments;
            while (segment.Length >= 4)
            {
                dc.DrawConvexPolygon(segment.Slice(0, 4), color);
                segment = segment.Slice(4);
            }
        }

        private static void _FillLineAsPolygon(ICoreCanvas2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, ColorStyle color, LineCapStyle startCapStyle, LineCapStyle endCapStyle)
        {
            var a = startCapStyle;

            for (int i = 1; i < points.Length; ++i)
            {
                var b = i < points.Length - 1
                    ? LineCapStyle.Triangle
                    : endCapStyle;

                _FillLineAsPolygon(dc, points[i - 1], points[i], diameter, color, a, b);

                a = LineCapStyle.Triangle;
            }
        }

        private static void _FillLineAsPolygon(ICoreCanvas2D dc, POINT2 a, POINT2 b, SCALAR diameter, ColorStyle color, LineCapStyle startCapStyle, LineCapStyle endCapStyle)
        {
            var startCapCount = Parametric.ShapeFactory2D.GetLineCapVertexCount(startCapStyle);
            var endCapCount = Parametric.ShapeFactory2D.GetLineCapVertexCount(endCapStyle);
            Span<POINT2> vertices = stackalloc POINT2[startCapCount + endCapCount];

            var aa = a.XY;
            var bb = b.XY;

            var delta = bb - aa;

            if (delta.LengthSquared() <= 0.0000001f)
            {
                delta = delta == VECTOR2.Zero ? VECTOR2.UnitX : delta * 100000f;
            }

            delta = VECTOR2.Normalize(delta);

            Parametric.ShapeFactory2D.FillLineCapVertices(vertices, 0, aa, delta, diameter, startCapStyle);
            Parametric.ShapeFactory2D.FillLineCapVertices(vertices, startCapCount, bb, -delta, diameter, endCapStyle);

            dc.DrawConvexPolygon(vertices, color);
        }        

        #endregion
    }
}
