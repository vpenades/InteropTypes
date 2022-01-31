using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using System.Drawing;

namespace InteropDrawing.Transforms
{
    public readonly struct Drawing2DTransform :
        ICanvas2D,
        IScene3D,
        ITransformer2D,
        IServiceProvider        
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
            var virtualSize = virtualBounds.Size.ToVector2();
            var virtualCenter = virtualBounds.ToCenterVector2();

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

            _TransformForward = xform;
            Matrix3x2.Invert(xform, out _TransformInverse);
            _TransformScaleForward = xform.DecomposeScale();
            _TransformScaleInverse = 1f / _TransformScaleForward;
        }

        #endregion

        #region data

        private readonly IPrimitiveCanvas2D _Target;        
        private readonly ICanvas2D _TargetEx;        

        // two way transform

        private readonly Matrix3x2 _TransformForward;
        private readonly Matrix3x2 _TransformInverse;
        private readonly Single _TransformScaleForward;
        private readonly Single _TransformScaleInverse;

        #endregion

        #region API

        /// <inheritdoc/>        
        public object GetService(Type serviceType)
        {
            return this.TryGetService(serviceType, _Target);
        }

        private Single _GetTransformed(Single size) { return size <= 0 ? size : size * _TransformScaleForward; }

        private Point2 _GetTransformed(Point2 point)
        {
            return Point2.Transform(point, _TransformForward);
        }

        private Point2 _GetTransformed(Point3 point)
        {
            return Point2.Transform(point.SelectXY(), _TransformForward);
        }

        /// <summary>
        /// This matrix is used to convert a point from viewport space to scene space.
        /// </summary>
        /// <returns>A transform matrix</returns>
        public Matrix3x2 GetInverseTransform() { return _TransformInverse; }

        #endregion

        #region ITransformer2D API

        /// <inheritdoc />
        public void TransformForward(Span<Point2> points)
        {
            Point2.Transform(points, _TransformForward);
            if (_Target is ITransformer2D xform) xform.TransformForward(points);
        }

        /// <inheritdoc />
        public void TransformInverse(Span<Point2> points)
        {
            if (_Target is ITransformer2D xform) xform.TransformInverse(points);
            Point2.Transform(points, _TransformInverse);
        }

        /// <inheritdoc />
        public void TransformNormalsForward(Span<Point2> vectors)
        {
            Point2.TransformNormals(vectors, _TransformForward);
            if (_Target is ITransformer2D xform) xform.TransformNormalsForward(vectors);
        }

        /// <inheritdoc />
        public void TransformNormalsInverse(Span<Point2> vectors)
        {
            if (_Target is ITransformer2D xform) xform.TransformNormalsInverse(vectors);
            Point2.Transform(vectors, _TransformInverse);
        }

        /// <inheritdoc />
        public void TransformScalarsForward(Span<Single> scalars)
        {
            for (int i = 0; i < scalars.Length; i++) scalars[i] *= _TransformScaleForward;
            if (_Target is ITransformer2D xform) xform.TransformScalarsForward(scalars);
        }

        /// <inheritdoc />
        public void TransformScalarsInverse(Span<Single> scalars)
        {
            if (_Target is ITransformer2D xform) xform.TransformScalarsInverse(scalars);
            for (int i = 0; i < scalars.Length; i++) scalars[i] *= _TransformScaleInverse;
        }

        #endregion

        #region 2D Drawing API

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle color)
        {
            if (_TargetEx != null)
            {
                _TargetEx.DrawAsset(transform * _TransformForward, asset, color);
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
        public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle brush)
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
                Decompose2D.DrawPolygon(_Target, pp, brush.WithOutline(ow) );
            }
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float strokeWidth, in LineStyle brush)
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
        public void DrawEllipse(Point2 center, Single w, Single h, in OutlineFillStyle brush)
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
        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            _Target.DrawImage(transform * _TransformForward, style);
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
        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.Style.OutlineWidth);

            this.DrawLine(_GetTransformed(a), _GetTransformed(b), diameter, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.OutlineWidth);

            this.DrawCircle(_GetTransformed(center), diameter, brush.WithOutline(ow));
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

            this.DrawPolygon(vvv, brush.Style);
        }

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            Span<Point2> vvv = stackalloc Point2[vertices.Length];

            for (int i = 0; i < vvv.Length; ++i) vvv[i] = _GetTransformed(vertices[i]);           

            // todo: reverse winding if needed

            this.DrawConvexPolygon(vvv, style);
        }

        #endregion
    } 
}
