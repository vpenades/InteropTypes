using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using System.Drawing;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    readonly partial struct Scene3DTransform :
        IEquatable<Scene3DTransform>,
        ICanvas2D,
        IScene3D,
        IServiceProvider,
        ColorStyle.IBackendDefaultValue
    {
        #region constructors

        public static Scene3DTransform Create(ICoreScene3D target, in Matrix4x4 xform)
        {
            return new Scene3DTransform(target, xform);
        }

        private Scene3DTransform(ICoreScene3D target, Matrix4x4 xform)
        {
            _Target = target;
            _TargetEx = target as IScene3D;
            _Transform = xform;
            _SizeScale = _DecomposeScale(xform);
        }

        private static Single _DecomposeScale(in Matrix4x4 matrix)
        {
            var det = matrix.GetDeterminant();
            var volume = Math.Abs(det);

            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Pow(volume, (float)1 / 3);
            #else
            return (float)Math.Pow(volume, (double)1 / 3);
            #endif
        }

        #endregion

        #region data

        private readonly ICoreScene3D _Target;
        private readonly Matrix4x4 _Transform;

        private readonly IScene3D _TargetEx;        
        private readonly float _SizeScale;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _Target.GetHashCode() ^ _Transform.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is Scene3DTransform other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(Scene3DTransform other)
        {
            if (this._Target != other._Target) return false;
            if (this._Transform != other._Transform) return false;
            return true;
        }

        #endregion

        #region properties

        /// <inheritdoc/>        
        public ColorStyle DefaultColorStyle
        {
            get => ColorStyle.TryGetDefaultFrom(_Target);
            set => value.TrySetDefaultTo(_Target);
        }

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(Matrix4x4)) return _Transform;

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
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
        {
            brush = brush.WithOutline(_GetTransformed(brush.Style.OutlineWidth));
            diameter = _GetTransformed(diameter);

            Span<Point3> vertices = stackalloc Point3[points.Length];
            for (int i = 0; i < vertices.Length; ++i) { vertices[i] = _GetTransformed(points[i]); }

            DrawSegments(vertices,diameter, brush);
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle brush)
        {
            width = _GetTransformed(width);
            height = _GetTransformed(height);
            var ow = _GetTransformed(brush.OutlineWidth);

            DrawSphere(_GetTransformed(center), (width + height) * 0.5f, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle brush)
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
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3D Drawing API        

        /// <inheritdoc/>
        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            if (_TargetEx != null) _TargetEx.DrawAsset(transform * _Transform, asset);
            else Decompose3D.DrawAsset(_Target, transform * _Transform, asset);
        }

        /// <inheritdoc/>
        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            Span<Point3> vrts = stackalloc Point3[vertices.Length];
            _TransformPoints(vertices, vrts);

            if (_TargetEx != null) _TargetEx.DrawSegments(vrts, diameter, style);
            else Decompose3D.DrawSegment(_Target, vrts, diameter, style);
        }

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.OutlineWidth);
            brush = brush.WithOutline(ow);
            var p = _GetTransformed(center);

            if (_TargetEx != null) _TargetEx.DrawSphere(p, diameter, brush);
            else Decompose3D.DrawSphere(_Target, p, diameter, brush);
        }

        /// <inheritdoc/>
        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            brush = brush.WithOutline(_GetTransformed(brush.Style.OutlineWidth));

            Span<Point3> vrts = stackalloc Point3[vertices.Length];
            _TransformPoints(vertices, vrts);

            if (_TargetEx != null) _TargetEx.DrawSurface(vrts, brush);
            else Decompose3D.DrawSurface(_Target, vrts, brush);
        }

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle fillColor)
        {
            Span<Point3> vrts = stackalloc Point3[vertices.Length];
            _TransformPoints(vertices, vrts);

            _Target.DrawConvexSurface(vrts, fillColor);
        }        

        #endregion
    }

}
