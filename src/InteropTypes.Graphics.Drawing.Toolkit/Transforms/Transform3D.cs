using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using System.Drawing;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    public readonly struct Drawing3DTransform :
        ICanvas2D,
        IScene3D,
        IServiceProvider
    {
        #region constructors

        public static Drawing3DTransform Create(IPrimitiveScene3D target, in Matrix4x4 xform)
        {
            return new Drawing3DTransform(target, xform);
        }

        private Drawing3DTransform(IPrimitiveScene3D target, Matrix4x4 xform)
        {
            _Target = target;
            _TargetEx = target as IScene3D;
            _Transform = xform;
            _SizeScale = xform.DecomposeScale();
        }

        #endregion

        #region data

        private readonly IPrimitiveScene3D _Target;
        private readonly IScene3D _TargetEx;
        private readonly Matrix4x4 _Transform;
        private readonly float _SizeScale;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            return this.TryGetService(serviceType, _Target);
        }

        private float _GetTransformed(float size) { return size <= 0 ? size : size * _SizeScale; }

        private Point3 _GetTransformed(Point2 point) { return Point3.Transform(new Point3(point, 0), _Transform); }

        private Point3 _GetTransformed(Point3 point) { return Point3.Transform(point, _Transform); }

        private void _TransformPoints(ReadOnlySpan<Point3> src, Span<Point3> dst)
        {
            for (int i = 0; i < src.Length; ++i)
            {
                dst[i] = _GetTransformed(src[i]);
            }
        }

        #endregion

        #region 2D Drawing API

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle color)
        {
            new Decompose2D(this).DrawAsset(transform, asset, color);
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            Span<Point3> vertices = stackalloc Point3[points.Length];

            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = _GetTransformed(points[i]);
            }

            _Target.DrawConvexSurface(vertices, color);
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, in LineStyle brush)
        {
            var xbrush = brush.WithOutline(_GetTransformed(brush.Style.OutlineWidth));

            for (int i = 1; i < points.Length; ++i)
            {
                var aa = _GetTransformed(points[i - 1]);
                var bb = _GetTransformed(points[i + 0]);
                var ss = xbrush.GetLineSegmentStyle(points.Length, i);

                DrawSegment(aa, bb, diameter, ss);
            }
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, in OutlineFillStyle brush)
        {
            width = _GetTransformed(width);
            height = _GetTransformed(height);
            var ow = _GetTransformed(brush.OutlineWidth);

            DrawSphere(_GetTransformed(center), (width + height) * 0.5f, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle brush)
        {
            var xbrush = brush.WithOutline(_GetTransformed(brush.OutlineWidth));

            Span<Point3> vertices = stackalloc Point3[points.Length];

            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = _GetTransformed(points[i]);
            }

            DrawSurface(vertices, (xbrush, true));
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3D Drawing API        

        /// <inheritdoc/>
        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            if (_TargetEx != null) _TargetEx.DrawAsset(transform * _Transform, asset, brush);
            else Decompose3D.DrawAsset(_Target, transform * _Transform, asset, brush);
        }

        /// <inheritdoc/>
        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.Style.OutlineWidth);

            if (_TargetEx != null) _TargetEx.DrawSegment(_GetTransformed(a), _GetTransformed(b), diameter, brush.WithOutline(ow));
            else Decompose3D.DrawSegment(_Target, _GetTransformed(a), _GetTransformed(b), diameter, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.OutlineWidth);

            if (_TargetEx != null) _TargetEx.DrawSphere(_GetTransformed(center), diameter, brush.WithOutline(ow));
            else Decompose3D.DrawSphere(_Target, _GetTransformed(center), diameter, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            brush = brush.WithOutline(_GetTransformed(brush.Style.OutlineWidth));

            Span<Point3> vrts = stackalloc Point3[vertices.Length];
            _TransformPoints(vertices, vrts);

            if (_TargetEx != null) _TargetEx.DrawSurface(vrts, brush);
            else Decompose3D.DrawSurface(_Target, vertices, brush);
        }

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle fillColor)
        {
            Span<Point3> vrts = stackalloc Point3[vertices.Length];
            _TransformPoints(vertices, vrts);

            _Target.DrawConvexSurface(vrts, fillColor);
        }

        #endregion
    }

}
