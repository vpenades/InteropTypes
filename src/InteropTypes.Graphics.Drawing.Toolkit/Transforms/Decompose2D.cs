using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;
using POINT2 = InteropDrawing.Point2;
using VECTOR2 = System.Numerics.Vector2;
using System.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Transforms
{
    /// <summary>
    /// Represents a drawing canvas filter that translates complex vector drawing calls into basic convex polygons.
    /// </summary>
    /// <remarks>
    /// <see cref="ICanvas2D"/> 🡆 <see cref="IPrimitiveCanvas2D"/>
    /// </remarks>
    public readonly partial struct Decompose2D :
        ICanvas2D,        
        ITransformer2D,
        IServiceProvider
    {
        #region lifecycle

        public Decompose2D(IPrimitiveCanvas2D renderTarget)
        {
            _RenderTarget = renderTarget;            
        }

        #endregion

        #region data

        private readonly IPrimitiveCanvas2D _RenderTarget;        

        #endregion

        #region API - IServiceProvider

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            return this.TryGetService(serviceType, _RenderTarget);
        }

        #endregion

        #region API - ITransformer2D

        /// <inheritdoc />
        public void TransformForward(Span<POINT2> points)
        {
            if (_RenderTarget is ITransformer2D xform) xform.TransformForward(points);
        }

        /// <inheritdoc />
        public void TransformInverse(Span<POINT2> points)
        {
            if (_RenderTarget is ITransformer2D xform) xform.TransformInverse(points);
        }

        /// <inheritdoc />
        public void TransformNormalsForward(Span<POINT2> vectors)
        {
            if (_RenderTarget is ITransformer2D xform) xform.TransformNormalsForward(vectors);
        }

        /// <inheritdoc />
        public void TransformNormalsInverse(Span<POINT2> vectors)
        {
            if (_RenderTarget is ITransformer2D xform) xform.TransformNormalsInverse(vectors);
        }

        /// <inheritdoc />
        public void TransformScalarsForward(Span<Single> scalars)
        {
            if (_RenderTarget is ITransformer2D xform) xform.TransformScalarsForward(scalars);
        }

        /// <inheritdoc />
        public void TransformScalarsInverse(Span<Single> scalars)
        {
            if (_RenderTarget is ITransformer2D xform) xform.TransformScalarsInverse(scalars);
        }

        #endregion

        #region API - IPolygonDrawing2D

        /// <inheritdoc />
        public void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle color)
        {
            _RenderTarget.DrawConvexPolygon(points, color);
        }

        #endregion

        #region API - IDrawing2D        

        /// <inheritdoc />
        public void DrawAsset(in Matrix3x2 transform, ASSET asset, ColorStyle color)
        {
            DrawAsset(_RenderTarget, transform, asset, color);
        }

        /// <inheritdoc />
        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            _RenderTarget.DrawImage(transform, style);
        }

        /// <inheritdoc />
        public void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            DrawLines(_RenderTarget, points, diameter, style);
        }

        /// <inheritdoc />
        public void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, in OutlineFillStyle style)
        {
            DrawEllipse(_RenderTarget, center, width, height, style);
        }       

        /// <inheritdoc />
        public void DrawPolygon(ReadOnlySpan<POINT2> points, in PolygonStyle style)
        {
            DrawPolygon(_RenderTarget, points,style);
        }

        #endregion                
    }
}
