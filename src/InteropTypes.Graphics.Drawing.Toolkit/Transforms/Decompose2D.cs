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
    /// <see cref="IVectorsDrawing2D"/> 🡆 <see cref="IPolygonDrawing2D"/>
    /// </remarks>
    public readonly partial struct Decompose2D :
        IDrawing2D,        
        ITransformer2D,
        IServiceProvider
    {
        #region lifecycle

        public Decompose2D(IPolygonDrawing2D renderTarget)
        {
            _RenderTarget = renderTarget;            
        }

        #endregion

        #region data

        private readonly IPolygonDrawing2D _RenderTarget;        

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
        public void FillConvexPolygon(ReadOnlySpan<POINT2> points, Color color)
        {
            _RenderTarget.FillConvexPolygon(points, color);
        }

        #endregion

        #region API - IDrawing2D        

        /// <inheritdoc />
        public void DrawAsset(in Matrix3x2 transform, ASSET asset, in ColorStyle style)
        {
            if (asset is IDrawingBrush<IDrawing2D> drawable)
            {
                var dc = this.CreateTransformed2D(transform);
                drawable.DrawTo(dc);
                return;
            }

            // if (asset is string text) { this.DrawFont(transform, text, style); return; }
            // if (asset is StringBuilder text) { this.DrawFont(transform, text.ToString(), style); return; }

            // fallback
            if (_RenderTarget is IDrawing2D rt) { rt.DrawAsset(transform, asset, style); return; }
        }

        /// <inheritdoc />
        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            if (_RenderTarget is IImageDrawing2D rt) rt.DrawImage(transform, style);
        }

        /// <inheritdoc />
        public void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            DrawLines(_RenderTarget, points, diameter, style);
        }

        /// <inheritdoc />
        public void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, in ColorStyle style)
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
