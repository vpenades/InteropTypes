using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;


namespace InteropDrawing.Transforms
{
    public struct Drawing2DTransform : IDrawing2D, IDrawing3D
    {
        #region constructors

        public static Drawing2DTransform Create(IDrawing2D t, Matrix3x2 xform)
        {
            return new Drawing2DTransform(t, xform);
        }

        public static Drawing2DTransform Create(IDrawing2D t, Point2 physicalSize, Point2 virtualSize)
        {
            var xform = Matrix3x2.CreateTranslation(virtualSize.ToNumerics() / 2);

            return Create(t, physicalSize, virtualSize, xform);
        }

        public static Drawing2DTransform Create(IDrawing2D t, Point2 physicalSize, System.Drawing.RectangleF virtualBounds)
        {
            var virtualSize = virtualBounds.Size.ToVector2();
            var virtualCenter = virtualBounds.ToCenterVector2();

            var xform = Matrix3x2.CreateTranslation(virtualCenter);

            return Create(t, physicalSize, virtualSize, xform);
        }

        /*
        public static Drawing2DTransform Create(IDrawing2D t, XY physicalSize, Solids.SolidRect virtualBounds)
        {
            var virtualSize = virtualBounds.Size;
            var virtualCenter = virtualBounds.Center;

            var xform = Matrix3x2.CreateTranslation(virtualCenter);

            return Create(t, physicalSize, virtualSize, xform);
        }*/

        public static Drawing2DTransform Create(IDrawing2D t, Point2 physicalSize, Point2 virtualSize, Matrix3x2 virtualOffset)
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

        public static Drawing2DTransform Create((IDrawing2D target, float width, float height) viewport, Matrix3x2 projection, Matrix3x2 camera)
        {
            Matrix3x2.Invert(camera, out Matrix3x2 view);
            return Create(viewport, view * projection);
        }

        public static Drawing2DTransform Create((IDrawing2D target, float width, float height) viewport, Matrix3x2 projview)
        {
            var xform = projview * (viewport.width, viewport.height).CreateViewport2D();
            return new Drawing2DTransform(viewport.target, xform);
        }

        private Drawing2DTransform(IDrawing2D t, Matrix3x2 xform)
        {
            _Target = t;

            _Transform = xform;

            Matrix3x2.Invert(xform, out _TransformInverse);

            _SizeScale = xform.DecomposeScale();            
        }        

        #endregion

        #region data

        private readonly IDrawing2D _Target;
        private readonly Matrix3x2 _Transform;
        private readonly Matrix3x2 _TransformInverse;
        private readonly Single _SizeScale;

        #endregion

        #region API

        private Single _GetTransformed(Single size) { return size <= 0 ? size : size * _SizeScale; }

        private Point2 _GetTransformed(Point2 point) { return Point2.Transform(point, _Transform); }

        private Point2 _GetTransformed(Point3 point) { return Point2.Transform(point.SelectXY(), _Transform); }

        /// <summary>
        /// This matrix is used to convert a point from viewport space to scene space.
        /// </summary>
        /// <returns>A transform matrix</returns>
        public Matrix3x2 GetInverseTransform() { return _TransformInverse; }

        #endregion

        #region 2D Drawing API

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle brush)
        {
            _Target.DrawAsset(transform * _Transform, asset, brush);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float strokeWidth, LineStyle brush)
        {
            Span<Point2> pp = stackalloc Point2[points.Length];
            for (int i = 0; i < pp.Length; ++i) pp[i] = _GetTransformed(points[i]);

            strokeWidth = _GetTransformed(strokeWidth);

            _Target.DrawLines(pp, strokeWidth, brush);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            Span<Point2> pp = stackalloc Point2[points.Length];
            for (int i = 0; i < pp.Length; ++i) pp[i] = _GetTransformed(points[i]);

            var ow = _GetTransformed(brush.OutlineWidth);

            _Target.DrawPolygon(pp, brush.WithOutline(ow));
        }

        public void DrawEllipse(Point2 center, Single w, Single h, ColorStyle brush)
        {
            center = _GetTransformed(center);
            w = _GetTransformed(w);
            h = _GetTransformed(h);

            var ow = _GetTransformed(brush.OutlineWidth);

            _Target.DrawEllipse(center, w, h, brush.WithOutline(ow));
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            _Target.DrawSprite(transform * _Transform, style);
        }

        #endregion

        #region 3D Drawing API

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            throw new NotImplementedException();
            // this.DrawAssetAsSurfaces(transform, asset, brush);            
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.Style.OutlineWidth);

            this.DrawLine(_GetTransformed(a), _GetTransformed(b), diameter, brush.WithOutline(ow));
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.OutlineWidth);

            this.DrawCircle(_GetTransformed(center), diameter, brush.WithOutline(ow));
        }

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

        #endregion
    }    

    /*
    public struct SpriteDrawing2DTransform : ISpritesDrawing2D
    {
        #region constructors

        public static SpriteDrawing2DTransform Create(ISpritesDrawing2D t, Matrix3x2 xform)
        {
            return new SpriteDrawing2DTransform(t, xform);
        }

        private SpriteDrawing2DTransform(ISpritesDrawing2D t, Matrix3x2 xform)
        {
            _Target = t;
            _Transform = xform;            
        }

        #endregion

        #region data

        private readonly ISpritesDrawing2D _Target;
        private Matrix3x2 _Transform;

        #endregion

        #region API

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            _Target.DrawSprite(transform * _Transform, style);
        }

        #endregion
    }*/

    public struct Drawing3DTransform : IDrawing2D, IDrawing3D
    {
        #region constructors

        public static Drawing3DTransform Create(IDrawing3D target, Matrix4x4 xform)
        {
            return new Drawing3DTransform(target, xform);
        }

        private Drawing3DTransform(IDrawing3D target, Matrix4x4 xform)
        {
            _Target = target;
            _Transform = xform;
            _SizeScale = xform.DecomposeScale();
        }

        #endregion

        #region data

        private readonly IDrawing3D _Target;
        private readonly Matrix4x4 _Transform;
        private readonly Single _SizeScale;

        #endregion

        #region API

        private Single _GetTransformed(Single size) { return size <= 0 ? size : size * _SizeScale; }

        private Point3 _GetTransformed(Point2 point) { return Point3.Transform(new Point3(point, 0), _Transform); }

        private Point3 _GetTransformed(Point3 point) { return Point3.Transform(point, _Transform); }

        #endregion

        #region 2D Drawing API

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle brush)
        {
            this.DrawAssetAsPolygons(transform, asset, brush);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
        {
            brush = brush.WithOutline(_GetTransformed(brush.Style.OutlineWidth));

            for (int i = 1; i < points.Length; ++i)
            {
                var aa = _GetTransformed(points[i - 1]);
                var bb = _GetTransformed(points[i + 0]);
                var ss = brush.GetLineSegmentStyle(points.Length, i);

                _Target.DrawSegment(aa, bb, diameter, ss);
            }
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle brush)
        {
            width = _GetTransformed(width);
            height = _GetTransformed(height);
            var ow = _GetTransformed(brush.OutlineWidth);

            _Target.DrawSphere(_GetTransformed(center), (width + height) * 0.5f, brush.WithOutline(ow));
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            brush = brush.WithOutline(_GetTransformed(brush.OutlineWidth));

            Span<Point3> vertices = stackalloc Point3[points.Length];

            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = _GetTransformed(points[i]);
            }

            _Target.DrawSurface(vertices, (brush, true));
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3D Drawing API        

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {
            _Target.DrawAsset(transform * _Transform, asset, brush);
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.Style.OutlineWidth);

            _Target.DrawSegment(_GetTransformed(a), _GetTransformed(b), diameter, brush.WithOutline(ow));
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle brush)
        {
            diameter = _GetTransformed(diameter);
            var ow = _GetTransformed(brush.OutlineWidth);

            _Target.DrawSphere(_GetTransformed(center), diameter, brush.WithOutline(ow));
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            brush = brush.WithOutline(_GetTransformed(brush.Style.OutlineWidth));

            Span<Point3> vrts = stackalloc Point3[vertices.Length];

            for (int i = 0; i < vertices.Length; ++i)
            {
                vrts[i] = _GetTransformed(vertices[i]);
            }

            _Target.DrawSurface(vrts, brush);
        }        

        #endregion
    }

}
