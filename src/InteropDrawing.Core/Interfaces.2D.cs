using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;
using POINT2 = InteropDrawing.Point2;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    public interface IBaseDrawing2D { }

    public interface IAssetDrawing2D : IBaseDrawing2D
    {
        /// <summary>
        /// Draws an asset.
        /// </summary>
        /// <param name="transform">The transform to apply to the asset.</param>
        /// <param name="asset">The asset to draw.</param>
        /// <param name="style">The style to apply to the asset.</param>
        /// <remarks>
        /// Assets are dependant on the implementation, but at the most basic level,
        /// <see Model2D is supported as an asset.
        /// </remarks>
        void DrawAsset(in XFORM2 transform, ASSET asset, in ColorStyle style);
    }
    

    /// <summary>
    /// Represents a drawing canvas where we can draw 2D polygons.
    /// </summary>
    public interface IPolygonDrawing2D : IBaseDrawing2D
    {
        /// <summary>
        /// Fills a closed polygon.
        /// </summary>
        /// <param name="points">The vertices of the polygon.</param>
        /// <param name="color">The color of the polygon</param>
        /// <remarks>
        /// The caller must ensure the points represent a convex polygon.
        /// </remarks>
        void FillConvexPolygon(ReadOnlySpan<POINT2> points, COLOR color);
    }

    /// <summary>
    /// Represents a drawing canvas where we can draw vector graphics.
    /// </summary>
    public interface IVectorsDrawing2D : IPolygonDrawing2D
    {
        /// <summary>
        /// Draws a closed polygon.
        /// </summary>
        /// <param name="points">The vertices of the polygon.</param>
        /// <param name="style">The outline and fill style.</param>
        /// <remarks>
        /// Some implementations might be able to handle complex shapes,
        /// while others might only be able to draw convex shapes.
        /// </remarks>
        void DrawPolygon(ReadOnlySpan<POINT2> points, in PolygonStyle style);

        void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style);

        void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, in ColorStyle style);
    }

    /// <summary>
    /// Represents a drawing canvas where we can draw images.
    /// </summary>
    public interface IImageDrawing2D : IBaseDrawing2D
    {
        /// <summary>
        /// Draws an image at the location given by <paramref name="transform"/>.
        /// </summary>
        /// <param name="transform">The location where to draw the image.</param>
        /// <param name="style">The image definition.</param>
        void DrawImage(in XFORM2 transform, in ImageStyle style);        
    }

    /// <summary>
    /// Represents a drawing canvas where we can draw vector graphics and images.
    /// </summary>
    public interface IDrawing2D : IAssetDrawing2D, IPolygonDrawing2D, IVectorsDrawing2D, IImageDrawing2D { }
}
