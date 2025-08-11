using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace InteropTypes.Graphics.Drawing.Parametric
{
    static class ShapeFactory2D
    {
        #region constants
        
        private const float PI = MathF.PI;        

        #endregion

        #region ellipse
        public static void FillEllipseVertices(this Span<POINT2> dstVertices, POINT2 center, float width, float height)
        {
            // radius
            width *= 0.5f;
            height *= 0.5f;

            for (int i = 0; i < dstVertices.Length; ++i)
            {
                var angle = i / (float)dstVertices.Length;                

                angle *= MathF.PI * 2;
                var x = MathF.Cos(angle) * width;
                var y = MathF.Sin(angle) * height;

                dstVertices[i] = center + new POINT2(x, y);
            }
        }

        #endregion

        #region rectangles

        public static int GetRectangleVertexCount(int arcVertexCount) => arcVertexCount * 4 + 4;

        public static void FillRectangleVertices(this Span<POINT2> vertices, XFORM2 rect, float borderRadius, int arcVertexCount = 6)
        {
            var scaleX = new XY(rect.M11, rect.M12);
            var scaleY = new XY(rect.M21, rect.M22);
            var origin = new XY(rect.M31, rect.M32);

            if (vertices.Length == 4)
            {
                vertices[0] = origin;
                vertices[1] = origin + scaleX;
                vertices[2] = origin + scaleX + scaleY;
                vertices[3] = origin + scaleY;
                return;
            }

            int idx = 0;

            var sizeX = scaleX.Length();
            var axisX = XY.Normalize(scaleX);
            var sizeY = scaleY.Length();
            var axisY = XY.Normalize(scaleY);

            throw new NotImplementedException();

            // top
            vertices[idx++] = origin + axisX * borderRadius;
            vertices[idx++] = origin + axisX * (sizeX - borderRadius);

            // top right
            var center = origin + axisX * (sizeX - borderRadius) + axisY * borderRadius;
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, PI * 0.5f, 0f))
            {
                vertices[idx++] = center + (axisX * p.X + axisY * p.Y) * borderRadius;
            }

            // right
            vertices[idx++] = origin + axisX * (sizeX - borderRadius);
            vertices[idx++] = origin + new POINT2(sizeX, sizeY - borderRadius);

            // bottom right
            center = origin + new XY(sizeX - borderRadius, sizeY - borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, 0, -PI * 0.5f))
            {
                vertices[idx++] = center + p * borderRadius;
            }

            // bottom
            vertices[idx++] = origin + new POINT2(sizeX - borderRadius, sizeY);
            vertices[idx++] = origin + new POINT2(borderRadius, sizeY);

            // bottom left
            center = origin + new XY(borderRadius, sizeY - borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, -PI * 0.5f, -PI))
            {
                vertices[idx++] = center + p * borderRadius;
            }

            // left
            vertices[idx++] = origin + new POINT2(0, sizeY - borderRadius);
            vertices[idx++] = origin + new POINT2(0, borderRadius);

            // top left
            center = origin + new XY(borderRadius, borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, -PI, -PI * 1.5f))
            {
                vertices[idx++] = center + p * borderRadius;
            }
        }

        public static void FillRectangleVertices(this Span<POINT2> vertices, POINT2 origin, POINT2 size, float borderRadius, int arcVertexCount = 6)
        {
            if (vertices.Length == 4)
            {
                vertices[0] = origin;
                vertices[1] = origin + new POINT2(size.X, 0);
                vertices[2] = origin + size;
                vertices[3] = origin + new POINT2(0, size.Y);
                return;
            }

            int idx = 0;

            // top
            vertices[idx++] = origin + new POINT2(borderRadius, 0);
            vertices[idx++] = origin + new POINT2(size.X - borderRadius, 0);

            // top right
            var center = origin + new POINT2(size.X - borderRadius, borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, PI * 0.5f, 0f))
            {
                vertices[idx++] = center + p * borderRadius;
            }

            // right
            vertices[idx++] = origin + new POINT2(size.X, borderRadius);
            vertices[idx++] = origin + new POINT2(size.X, size.Y - borderRadius);

            // bottom right
            center = origin + new POINT2(size.X - borderRadius, size.Y - borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, 0, -PI * 0.5f))
            {
                vertices[idx++] = center + p * borderRadius;
            }

            // bottom
            vertices[idx++] = origin + new POINT2(size.X - borderRadius, size.Y);
            vertices[idx++] = origin + new POINT2(borderRadius, size.Y);

            // bottom left
            center = origin + new XY(borderRadius, size.Y - borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, -PI * 0.5f, -PI))
            {
                vertices[idx++] = center + p * borderRadius;
            }

            // left
            vertices[idx++] = origin + new POINT2(0, size.Y - borderRadius);
            vertices[idx++] = origin + new POINT2(0, borderRadius);

            // top left
            center = origin + new POINT2(borderRadius, borderRadius);
            foreach (var p in _GetRectangleCornerVertices(arcVertexCount, -PI, -PI * 1.5f))
            {
                vertices[idx++] = center + p * borderRadius;
            }
        }

        private static IEnumerable<XY> _GetRectangleCornerVertices(int count, float rad0, float rad1)
        {
            for (int i = 1; i < count; ++i)
            {
                var factor = i / (float)count;
                var radians = rad0 * (1 - factor) + rad1 * factor;
                
                var x = MathF.Cos(radians);
                var y = -MathF.Sin(radians);                

                yield return new XY(x, y);
            }
        }

        #endregion

        #region multiline segments

        public static int GetLinesSegmentsVerticesCount(int pointsCount, bool closed) => 4 * pointsCount + (closed ? 0 : -4);

        public static void FillLinesSegments(this Span<POINT2> segments, ReadOnlySpan<POINT2> points, float diameter, bool closed)
        {
            System.Diagnostics.Debug.Assert(segments.Length == points.Length * 4);
            // System.Diagnostics.Debug.Assert(!closed || points[0] == points[points.Length-1]); // if closed, first and last points must be the same.

            // create segments

            var segment = segments;

            for (int i = 1; i < points.Length; ++i)
            {
                _FillSegmentVertices(segment, points[i - 1], points[i], diameter);

                segment = segment.Slice(4);
            }

            if (closed)
            {
                if (points[0] == points[points.Length - 1]) _FillSegmentVertices(segment, points[0], points[1], diameter);
                else _FillSegmentVertices(segment, points[points.Length-1], points[0], diameter);
            }

            // weld segments vertices

            for (int i = 2; i < points.Length; ++i)
            {
                _WeldSegmentsVertices(segments, i - 2, i - 1, diameter);
            }

            if (closed)
            {
                _WeldSegmentsVertices(segments, points.Length - 2, points.Length - 1, diameter);
                _WeldSegmentsVertices(segments, points.Length - 1, 0, diameter);
            }

            // Todo: if one segment reverses direction in one side, simply merge the two vertices.            
        }

        private static void _WeldSegmentsVertices(Span<POINT2> segments, int lidx, int ridx, float diameter)
        {
            var left = segments.Slice(lidx * 4 + 2, 2);
            var right = segments.Slice(ridx * 4, 2);

            var l = (left[0].XY + right[1].XY) * 0.5f;
            var r = (left[1].XY + right[0].XY) * 0.5f;

            var c = (r + l) * 0.5f;
            var d = new POINT2(r - l).WithLength(diameter) * 0.5f;

            left[0] = right[1] = c - d;
            right[0] = left[1] = c + d;
        }

        private static void _FillSegmentVertices(Span<POINT2> dst, POINT2 a, POINT2 b, float diameter)
        {
            var aa = a.XY;
            var bb = b.XY;

            var axisX = XY.Normalize(bb - aa) * diameter * 0.25f;
            var axisY = new XY(axisX.Y, -axisX.X);

            dst[0] = aa - axisY;
            dst[1] = aa + axisY;
            dst[2] = bb + axisY;
            dst[3] = bb - axisY;
        }

        #endregion

        #region line cap geometry        

        public static int GetLineCapVertexCount(LineCapStyle style)
        {
            switch (style)
            {
                case LineCapStyle.Square: return 2;
                case LineCapStyle.Triangle: return 3;
                case LineCapStyle.Round: return 5;
                default: return 2;
            }
        }

        public static void FillLineCapVertices(Span<POINT2> dst, int dstIdx, XY p, XY axisX, float diameter, LineCapStyle style)
        {
            var axisY = new XY(axisX.Y, -axisX.X);

            var radius = diameter * 0.5f;

            switch (style)
            {
                case LineCapStyle.Flat:
                    {
                        dst[dstIdx + 0] = p - axisY * radius;
                        dst[dstIdx + 1] = p + axisY * radius;
                        break;
                    }

                case LineCapStyle.Square:
                    {
                        dst[dstIdx + 0] = p + (-axisX - axisY) * radius;
                        dst[dstIdx + 1] = p + (-axisX + axisY) * radius;
                        break;
                    }

                case LineCapStyle.Triangle:
                    {
                        dst[dstIdx + 0] = p - axisY * radius;
                        dst[dstIdx + 1] = p - axisX * radius;
                        dst[dstIdx + 2] = p + axisY * radius;
                        break;
                    }

                case LineCapStyle.Round:
                    {
                        var a = -axisY * radius;
                        var b = -axisX * radius;
                        var c = axisY * radius;

                        dst[dstIdx + 0] = p + a;
                        dst[dstIdx + 1] = p + (a + b) * 0.7f;
                        dst[dstIdx + 2] = p + b;
                        dst[dstIdx + 3] = p + (b + c) * 0.7f;
                        dst[dstIdx + 4] = p + c;

                        break;
                    }
                default: throw new NotImplementedException();
            }

        }

        #endregion
    }
}
