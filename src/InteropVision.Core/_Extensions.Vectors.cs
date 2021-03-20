using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;

using RECT = System.Drawing.Rectangle;
using RECTF = System.Drawing.RectangleF;

namespace InteropVision
{
    public static partial class _Extensions
    {
        public static XY SelectXY(this XYZ v) { return new XY(v.X, v.Y); }

        public static (XYZ min, XYZ max) MinMax(this IEnumerable<XYZ> points)
        {
            var min = new XYZ(float.PositiveInfinity);
            var max = new XYZ(float.NegativeInfinity);

            foreach (var p in points)
            {
                min = XYZ.Min(min, p);
                max = XYZ.Max(max, p);
            }

            return (min, max);
        }

        public static (XYZ min, XYZ max) MinMax(this ReadOnlySpan<XYZ> points)
        {
            var min = new XYZ(float.PositiveInfinity);
            var max = new XYZ(float.NegativeInfinity);

            foreach (var p in points)
            {
                min = XYZ.Min(min, p);
                max = XYZ.Max(max, p);
            }

            return (min, max);
        }

        public static (XY min, XY max) MinMax(this IEnumerable<XY> points)
        {
            var min = new XY(float.PositiveInfinity);
            var max = new XY(float.NegativeInfinity);

            foreach (var p in points)
            {
                min = XY.Min(min, p);
                max = XY.Max(max, p);
            }

            return (min, max);
        }

        public static (XY min, XY max) MinMax(this ReadOnlySpan<XY> points)
        {
            var min = new XY(float.PositiveInfinity);
            var max = new XY(float.NegativeInfinity);

            foreach (var p in points)
            {
                min = XY.Min(min, p);
                max = XY.Max(max, p);
            }

            return (min, max);
        }

        public static (XY min, XY max) MinMax(this (XY a, XY b, XY c, XY d, XY e, XY f, XY g) points)
        {
            return points.Enumerate().MinMax();
        }

        public static (XY min, XY max) MinMax(this (XY a, XY b, XY c, XY d, XY e, XY f, XY g, XY h) points)
        {
            return points.Enumerate().MinMax();
        }

        public static RECTF FitOuterSquare(this RECTF rect)
        {
            if (rect.Width > rect.Height)
            {
                rect.Y -= (rect.Width - rect.Height) * 0.5f;
                rect.Height = rect.Width;
            }

            if (rect.Width < rect.Height)
            {
                rect.X -= (rect.Height - rect.Width) * 0.5f;
                rect.Width = rect.Height;
            }

            return rect;
        }

        public static XY CentralVector2(this RECTF rect)
        {
            return new XY(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.5f);
        }

        public static Matrix3x2 ToMatrix3x2(this RECT rect)
        {
            var xform = Matrix3x2.CreateScale(rect.Width, rect.Height);
            xform.Translation = new XY(rect.X, rect.Y);
            return xform;
        }

        public static Matrix4x4 ToMatrix4x4(this RECT rect)
        {
            var xform = Matrix4x4.CreateScale(rect.Width, rect.Height, (rect.Width + rect.Height) / 2);
            xform.Translation = new XYZ(rect.X, rect.Y, 0);
            return xform;
        }

        public static Matrix4x4 ToMatrix4x4(this RECTF rect)
        {
            var xform = Matrix4x4.CreateScale(rect.Width, rect.Height, (rect.Width + rect.Height) / 2);
            xform.Translation = new XYZ(rect.X, rect.Y, 0);
            return xform;
        }

        public static void TransformBy(this XYZ[] points, RECTF r)
        {
            var scale = new XYZ(r.Width, r.Height, (r.Width + r.Height) / 2);
            var offset = new XYZ(r.X, r.Y, 0);

            for (int i = 0; i < points.Length; ++i)
            {
                points[i] *= scale;
                points[i] += offset;
            }
        }

        public static void TransformBy(this XYZ[] points, Matrix4x4 xform)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = XYZ.Transform(points[i], xform);
            }
        }

        public static void TransformBy(this XYZ[] points, Matrix3x2 xform)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                var p = points[i];

                points[i] = new XYZ(XY.Transform( new XY(p.X,p.Y) , xform), p.Z);
            }
        }

        public static RECTF BoundingRect(this IEnumerable<XY> points)
        {
            var (min, max) = points.MinMax();
            var o = min.ToPoint();
            var s = (max - min).ToSize();
            return new RECTF(o, s);
        }

        public static RECTF BoundingRect(this (XY A, XY B, XY C, XY D, XY E, XY F, XY G) points)
        {
            var (min, max) = points.MinMax();
            var o = min.ToPoint();
            var s = (max - min).ToSize();
            return new RECTF(o, s);
        }

    }
}
