using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;
using POINT2 = InteropDrawing.Point2;

namespace InteropDrawing
{
    

    /// <summary>
    /// Represents an object that can be drawn to a <see cref="IDrawing2D"/>.
    /// </summary>
    public interface IDrawable2D
    {
        /// <summary>
        /// Draws this object into the drawing context.
        /// </summary>
        /// <param name="dc">The drawing context.</param>
        void DrawTo(IDrawing2D dc);
    }

    /// <summary>
    /// Represents a render target context where we can draw 2D polygons.
    /// </summary>
    public interface IPolygonDrawing2D
    {
        /// <summary>
        /// Draws a closed polygon.
        /// </summary>
        /// <param name="points">the vertices of the polygon.</param>
        /// <param name="style">The outline and fill options.</param>
        /// <remarks>
        /// Some implementations might be able to handle complex shapes,
        /// while others might only be able to draw convex shapes.
        /// </remarks>
        void DrawPolygon(ReadOnlySpan<POINT2> points, ColorStyle style);
    }

    /// <summary>
    /// Represents a render target context where we can draw 2D shapes.
    /// </summary>
    public interface IDrawing2D : IPolygonDrawing2D
    {
        // methods could return a value:
        // - a boolean, indicating success or failure/unsupported
        // - or an object that could be used to fill additional metadata.

        // metadata could be set with Push/Pop methods

        /// <summary>
        /// Draws an asset.
        /// </summary>
        /// <param name="transform">The transform to apply to the asset.</param>
        /// <param name="asset">The asset to draw.</param>
        /// <param name="style">The style to apply to the asset.</param>
        /// <remarks>
        /// Assets are dependant on the implementation, but at the most basic level,
        /// <see cref="Model2D"/> is supported as an asset.
        /// </remarks>
        void DrawAsset(in XFORM2 transform, ASSET asset, ColorStyle style);

        void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, LineStyle style);

        void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, ColorStyle style);

        void DrawSprite(in XFORM2 transform, in SpriteStyle style);
    }

    public interface ICanvas2D : IDrawable2D { }

    public interface IDrawingContext2D : IDrawing2D, IDisposable { }
    
}
