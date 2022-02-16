using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using System.Drawing;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    readonly partial struct Drawing2DTransform :
        ICanvas2D,
        IScene3D,
        ITransformer2D,
        IServiceProvider,
        ColorStyle.IDefaultValue
    {
        #region constructors

        public static Drawing2DTransform Create(IPrimitiveCanvas2D t, Matrix3x2 xform)
        {
            return new Drawing2DTransform(t, xform);
        }

        public static Drawing2DTransform Create(IPrimitiveCanvas2D t, Point2 physicalSize, Point2 virtualSize)
        {
            var xform = Matrix3x2.CreateTranslation(virtualSize.ToNumerics() / 2);

            return Create(t, physicalSize, virtualSize, xform);
        }

        public static Drawing2DTransform Create(IPrimitiveCanvas2D t, Point2 physicalSize, RectangleF virtualBounds)
        {
            Point2 virtualOrig = virtualBounds.Location;
            Point2 virtualSize = virtualBounds.Size;
            var virtualCenter = virtualOrig + virtualSize * 0.5f;

            var xform = Matrix3x2.CreateTranslation(virtualCenter);

            return Create(t, physicalSize, virtualSize, xform);
        }

        public static Drawing2DTransform Create(IPrimitiveCanvas2D t, Point2 physicalSize, Point2 virtualSize, Matrix3x2 virtualOffset)
        {
            var xform = Matrix3x2.Identity;

            var scaleX = physicalSize.X / virtualSize.X;
            var scaleY = physicalSize.Y / virtualSize.Y;
            var scale = Math.Min(scaleX, scaleY);

            xform.M11 = +scale;
            xform.M22 = -scale;

            xform.M31 = physicalSize.X * 0.5f;
            xform.M32 = physicalSize.Y * 0.5f;

            Matrix3x2.Invert(virtualOffset, out Matrix3x2 invVirtOffset);

            xform = Matrix3x2.Multiply(invVirtOffset, xform);

            return new Drawing2DTransform(t, xform);
        }

        public static Drawing2DTransform Create((IPrimitiveCanvas2D target, float width, float height) viewport, Matrix3x2 projection, Matrix3x2 camera)
        {
            Matrix3x2.Invert(camera, out Matrix3x2 view);
            return Create(viewport, view * projection);
        }

        public static Drawing2DTransform Create((IPrimitiveCanvas2D target, float width, float height) viewport, Matrix3x2 projview)
        {
            var xform = projview * (viewport.width, viewport.height).CreateViewport2D();
            return new Drawing2DTransform(viewport.target, xform);
        }

        private Drawing2DTransform(IPrimitiveCanvas2D t, Matrix3x2 xform)
        {
            _Target = t;
            _TargetEx = t as ICanvas2D;

            _Transform = new TwoWayTransform2D(xform);
        }        

        #endregion

        #region data

        private readonly IPrimitiveCanvas2D _Target;
        private readonly ICanvas2D _TargetEx;

        // two way transform

        private readonly TwoWayTransform2D _Transform;

        #endregion

        #region API

        /// <inheritdoc/>
        public ColorStyle DefaultColorStyle
        {
            get => ColorStyle.TryGetDefaultFrom(_Target);
            set => value.TrySetDefaultTo(_Target);
        }

        /// <inheritdoc/>        
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMeshCanvas2D))
            {
                if (_Target is IMeshCanvas2D meshTarget) return new MeshCanvas2DTransform(meshTarget, _Transform);
            }

            return this.TryGetService(serviceType, _Target);
        }

        private float _GetTransformed(float size) { return size <= 0 ? size : size * _Transform.ScaleForward; }

        private Point2 _GetTransformed(Point2 point) { return Point2.Transform(point, _Transform.Forward); }

        private Point2 _GetTransformed(Point3 point) { return Point2.Transform(point.XY, _Transform.Forward); }

        /// <summary>
        /// This matrix is used to convert a point from viewport space to scene space.
        /// </summary>
        /// <returns>A transform matrix</returns>
        public Matrix3x2 GetInverseTransform() { return _Transform.Inverse; }

        #endregion

        #region ITransformer2D API

        /// <inheritdoc />
        public void TransformForward(Span<Point2> points)
        {
            Point2.Transform(points, _Transform.Forward);
            if (_Target is ITransformer2D xform) xform.TransformForward(points);
        }

        /// <inheritdoc />
        public void TransformInverse(Span<Point2> points)
        {
            if (_Target is ITransformer2D xform) xform.TransformInverse(points);
            Point2.Transform(points, _Transform.Inverse);
        }

        /// <inheritdoc />
        public void TransformNormalsForward(Span<Point2> vectors)
        {
            Point2.TransformNormals(vectors, _Transform.Forward);
            if (_Target is ITransformer2D xform) xform.TransformNormalsForward(vectors);
        }

        /// <inheritdoc />
        public void TransformNormalsInverse(Span<Point2> vectors)
        {
            if (_Target is ITransformer2D xform) xform.TransformNormalsInverse(vectors);
            Point2.Transform(vectors, _Transform.Inverse);
        }

        /// <inheritdoc />
        public void TransformScalarsForward(Span<float> scalars)
        {
            for (int i = 0; i < scalars.Length; i++) scalars[i] *= _Transform.ScaleForward;
            if (_Target is ITransformer2D xform) xform.TransformScalarsForward(scalars);
        }

        /// <inheritdoc />
        public void TransformScalarsInverse(Span<float> scalars)
        {
            if (_Target is ITransformer2D xform) xform.TransformScalarsInverse(scalars);
            for (int i = 0; i < scalars.Length; i++) scalars[i] *= _Transform.ScaleInverse;
        }

        #endregion

        #region 2D Drawing API

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle color)
        {
            if (_TargetEx != null)
            {
                _TargetEx.DrawAsset(transform * _Transform.Forward, asset, color);
            }
            else
            {
                // Decompose2D.DrawAsset(_Polygons, transform * _TransformForward, asset, brush);
            }
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            Span<Point2> pp = stackalloc Point2[points.Length];
            for (int i = 0; i < pp.Length; ++i) pp[i] = _GetTransformed(points[i]);

            _TargetEx.DrawConvexPolygon(pp, color);
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle brush)
        {
            Span<Point2> pp = stackalloc Point2[points.Length];
            for (int i = 0; i < pp.Length; ++i) pp[i] = _GetTransformed(points[i]);

            var ow = _GetTransformed(brush.OutlineWidth);

            if (_TargetEx != null)
            {
                _TargetEx.DrawPolygon(pp, brush.WithOutline(ow));
            }
            else
            {
                Decompose2D.DrawPolygon(_Target, pp, brush.WithOutline(ow));
            }
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float strokeWidth, LineStyle brush)
        {
            Span<Point2> pp = stackalloc Point2[points.Length];
            for (int i = 0; i < pp.Length; ++i) pp[i] = _GetTransformed(points[i]);

            strokeWidth = _GetTransformed(strokeWidth);

            if (_TargetEx != null)
            {
                _TargetEx.DrawLines(pp, strokeWidth, brush);
            }
            else
            {
                Decompose2D.DrawLines(_Target, pp, strokeWidth, brush);
            }
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float w, float h, OutlineFillStyle brush)
        {
            center = _GetTransformed(center);
            w = _GetTransformed(w);
            h = _GetTransformed(h);

            var ow = _GetTransformed(brush.OutlineWidth);

            if (_TargetEx != null)
            {
                _TargetEx.DrawEllipse(center, w, h, brush.WithOutline(ow));
            }
            else
            {
                Decompose2D.DrawEllipse(_Target, center, w, h, brush.WithOutline(ow));
            }
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            _Target.DrawImage(transform * _Transform.Forward, style);
        }

        #endregion

        #region 3D Drawing API

        /// <inheritdoc/>
        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            throw new NotImplementedException();
            // this.DrawAssetAsSurfaces(transform, asset, brush);            
        }

        /// <inheritdoc/>
        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            Span<Point2> vvv = stackalloc Point2[vertices.Length];
            for (int i = 0; i < vvv.Length; ++i) vvv[i] = _GetTransformed(vertices[i]);

            this.DrawLines(vvv, diameter, style);
        }

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.OutlineWidth);

            this.DrawEllipse(_GetTransformed(center), diameter, diameter, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            Span<Point2> vvv = stackalloc Point2[vertices.Length];
            for (int i = 0; i < vvv.Length; ++i) vvv[i] = _GetTransformed(vertices[i]);

            if (!brush.DoubleSided)
            {
                // TODO: check winding
            }

            DrawPolygon(vvv, brush.Style);
        }

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            Span<Point2> vvv = stackalloc Point2[vertices.Length];

            for (int i = 0; i < vvv.Length; ++i) vvv[i] = _GetTransformed(vertices[i]);

            // todo: reverse winding if needed

            DrawConvexPolygon(vvv, style);
        }        

        #endregion
    }

    readonly struct TwoWayTransform2D
    {
        public TwoWayTransform2D(in Matrix3x2 xform)
        {
            Forward = xform;
            Matrix3x2.Invert(xform, out Inverse);
            ScaleForward = _DecomposeScale(xform);
            ScaleInverse = 1f / ScaleForward;
        }

        private static float _DecomposeScale(in Matrix3x2 xform)
        {
            var det = xform.GetDeterminant();
            var area = Math.Abs(det);

            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Sqrt(area);
            #else
            return (float)Math.Sqrt(area);
            #endif
        }

        public readonly Matrix3x2 Forward;
        public readonly Matrix3x2 Inverse;
        public readonly float ScaleForward;
        public readonly float ScaleInverse;
    }


    readonly partial struct MeshCanvas2DTransform :
        IMeshCanvas2D,
        ITransformer2D,
        IServiceProvider
    {
        #region constructor
        public MeshCanvas2DTransform(IMeshCanvas2D target, in TwoWayTransform2D ft)
        {
            _Target = target;
            _Transform = ft;
        }

        #endregion

        #region data

        private readonly IMeshCanvas2D _Target;
        private readonly TwoWayTransform2D _Transform;

        #endregion

        #region API

        /// <inheritdoc/>        
        public object GetService(Type serviceType)
        {
            return this.TryGetService(serviceType, _Target);
        }

        #endregion

        #region ITransformer2D API

        /// <inheritdoc />
        public void TransformForward(Span<Point2> points)
        {
            Point2.Transform(points, _Transform.Forward);
            if (_Target is ITransformer2D xform) xform.TransformForward(points);
        }

        /// <inheritdoc />
        public void TransformInverse(Span<Point2> points)
        {
            if (_Target is ITransformer2D xform) xform.TransformInverse(points);
            Point2.Transform(points, _Transform.Inverse);
        }

        /// <inheritdoc />
        public void TransformNormalsForward(Span<Point2> vectors)
        {
            Point2.TransformNormals(vectors, _Transform.Forward);
            if (_Target is ITransformer2D xform) xform.TransformNormalsForward(vectors);
        }

        /// <inheritdoc />
        public void TransformNormalsInverse(Span<Point2> vectors)
        {
            if (_Target is ITransformer2D xform) xform.TransformNormalsInverse(vectors);
            Point2.Transform(vectors, _Transform.Inverse);
        }

        /// <inheritdoc />
        public void TransformScalarsForward(Span<float> scalars)
        {
            for (int i = 0; i < scalars.Length; i++) scalars[i] *= _Transform.ScaleForward;
            if (_Target is ITransformer2D xform) xform.TransformScalarsForward(scalars);
        }

        /// <inheritdoc />
        public void TransformScalarsInverse(Span<float> scalars)
        {
            if (_Target is ITransformer2D xform) xform.TransformScalarsInverse(scalars);
            for (int i = 0; i < scalars.Length; i++) scalars[i] *= _Transform.ScaleInverse;
        }

        #endregion

        #region API

        public void DrawMesh(ReadOnlySpan<Vertex2> vertices, ReadOnlySpan<int> indices, object texture)
        {
            Span<Vertex2> ps = stackalloc Vertex2[vertices.Length];

            vertices.CopyTo(ps);

            for(int i=0; i < ps.Length; ++i)
            {
                ps[i].Position = Vector2.Transform(ps[i].Position, _Transform.Forward);
            }

            _Target.DrawMesh(ps,indices,texture);
        }

        #endregion
    }
}
